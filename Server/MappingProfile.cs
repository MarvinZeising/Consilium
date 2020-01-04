using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();

            CreateMap<Person, PersonDto>();
            CreateMap<CreatePersonDto, Person>();

            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>();

            CreateMap<CreateInvitationDto, Participation>();
            CreateMap<Participation, ParticipationDto>();

            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();
        }
    }
}
