using AutoMapper;
using CS_BLL.Models;
using CS_DAL.Entities;
using System.Linq;

namespace CS_BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<FileDatum, FileDatumModel>()
                .ForMember(fdm => fdm.FileName, fd => fd.MapFrom(x => x.FileName))
                .ForMember(fdm => fdm.UserName, fd => fd.MapFrom(x => x.UserName))
                .ForMember(fdm => fdm.CreationDate, fd => fd.MapFrom(x => x.CreationDate))
                .ForMember(fdm => fdm.EditDate, fd => fd.MapFrom(x => x.EditDate))
                    .ReverseMap();
            
        }
    }
}
