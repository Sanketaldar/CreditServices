using AutoMapper;
using CreditReporting.Application.DTOs;
using CreditReporting.Domain.Entities;

namespace CreditReporting.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CibilReport, CibilReportDto>().ReverseMap();
            CreateMap<CibilCheckRequest, CibilReport>();
        }
    }
}
