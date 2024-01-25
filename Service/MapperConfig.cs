namespace Service
{
    using AutoMapper;
    using Data.Core.Models;
    using Service.ViewModels;

    public class MapperConfig
    {
        public IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, ClientViewModel>().ReverseMap();
                cfg.CreateMap<ClientMediaInfo, ClientMediaInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientPhotoInfo, ClientPhotoInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientVideoInfo, ClientVideoInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientVideoNoteInfo, ClientVideoNoteInfoViewModel>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
