namespace Service.Extensions
{
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Threading.Tasks;

	public static class DbSetExtention
	{
		/// <summary>
		/// Удаление записи по Id
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <param name="entityId">id записи для удаления</param>
		/// <exception cref="Exception"></exception>
		public async static Task RemoveById<T>(this DbSet<T> table, int entityId) where T : class
		{
			var entity = await table.FindAsync(entityId) ?? throw new Exception(table.EntityType.Name + "NotFound");
			table.Remove(entity);
		}

		public async static Task UpdateEntity<T>(this DbSet<T> table, int entityId, object viewModel, AutoMapper.IMapper mapper) where T : class
		{
			var entity = await table.FindAsync(entityId) ?? throw new Exception(table.EntityType.Name + "NotFound");
			mapper.Map(viewModel, entity);
		}
	}
}
