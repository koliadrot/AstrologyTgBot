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

                cfg.CreateMap<BasePhotoInfo, BasePhotoInfoViewModel>().ReverseMap();
                cfg.CreateMap<BaseVideoInfo, BaseVideoInfoViewModel>().ReverseMap();
                cfg.CreateMap<BaseVideoNoteInfo, BaseVideoNoteInfoViewModel>().ReverseMap();
                cfg.CreateMap<BaseClientMatch, BaseClientMatchViewModel>().ReverseMap();

                cfg.CreateMap<ClientMatchInfo, ClientMatchInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientMatchUnchecked, ClientMatchUncheckedViewModel>().ReverseMap();
                cfg.CreateMap<ClientMatchChecked, ClientMatchCheckedViewModel>().ReverseMap();
                cfg.CreateMap<ClientMatchUncheckedVideoInfo, ClientMatchUncheckedVideoInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientMatchUncheckedVideoNoteInfo, ClientMatchUncheckedVideoNoteInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientMatchCheckedVideoNoteInfo, ClientMatchCheckedVideoNoteInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientMatchCheckedVideoInfo, ClientMatchCheckedVideoInfoViewModel>().ReverseMap();

                cfg.CreateMap<ClientMediaInfo, ClientMediaInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientPhotoInfo, ClientPhotoInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientVideoInfo, ClientVideoInfoViewModel>().ReverseMap();
                cfg.CreateMap<ClientVideoNoteInfo, ClientVideoNoteInfoViewModel>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
