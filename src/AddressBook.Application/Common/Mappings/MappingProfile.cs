using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Lookup.Departments.DTOs;
using AddressBook.Application.Lookup.Jobs.DTOs;
using AddressBook.Domain.Entities;
using AutoMapper;

namespace AddressBook.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddressEntry, AddressEntryDto>()
            .ForMember(d => d.JobName, o => o.MapFrom(s => s.Job.Name))
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name));

        CreateMap<AddressEntry, AddressEntryListItemDto>()
            .ForMember(d => d.JobName, o => o.MapFrom(s => s.Job.Name))
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name));

        CreateMap<Job, JobDto>();
        CreateMap<Department, DepartmentDto>();
    }
}
