namespace Service.Core
{
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using Data.Core;
	using Data.Core.Models;
	using Service.Abstract;
	using Service.Extensions;
	using Service.ViewModels;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	public class UserManager : IUserManager
	{
		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _ctx;

		public UserManager(IMapper mapper, ApplicationDbContext ctx)
		{
			_mapper = mapper;
			_ctx = ctx;
		}

		#region Users
		public IQueryable<UserViewModel> GetUsers() => _ctx.Users.ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);

		public UserViewModel GetUserByLogin(string loginName)
		{
			var entity = _ctx.Users.FirstOrDefault(u => u.Email == loginName);

			return entity == null ? throw new ArgumentException("User not found") : _mapper.Map<UserViewModel>(entity);
		}
		public bool UserExist(string? loginName)
		{
			if (loginName == null)
			{
				return false;
			}

			var entity = _ctx.Users.FirstOrDefault(u => u.Email == loginName);
			return entity != null;
		}

		public async Task CreateUser(UserViewModel viewModel)
		{
			var entity = _mapper.Map<User>(viewModel);

			entity.Login = entity.Login.IsNull() ? "" : entity.Login;
			entity.PasswordHash = entity.PasswordHash.IsNull() ? "" : entity.PasswordHash;

			await _ctx.Users.AddAsync(entity);
			await _ctx.SaveChangesAsync();
			viewModel.UserId = entity.UserId;
		}


		public async Task UpdateUser(UserViewModel viewModel)
		{
			viewModel.Login = viewModel.Login.IsNull() ? "" : viewModel.Login;
			viewModel.PasswordHash = viewModel.PasswordHash.IsNull() ? "" : viewModel.PasswordHash;

			await _ctx.Users.UpdateEntity(viewModel.UserId, viewModel, _mapper);
			await _ctx.SaveChangesAsync();
		}

		public async Task RemoveUser(UserViewModel viewModel)
		{
			await _ctx.Users.RemoveById(viewModel.UserId);
			await _ctx.SaveChangesAsync();
		}
		#endregion
	}
}
