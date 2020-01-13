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

            CreateMap<Congregation, CongregationDto>();
            CreateMap<CreateCongregationDto, Congregation>();

            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>();

            CreateMap<Topic, TopicDto>();
            CreateMap<CreateTopicDto, Topic>();
            CreateMap<UpdateTopicDto, Topic>();

            CreateMap<Article, ArticleDto>();
            CreateMap<CreateArticleDto, Article>();

            CreateMap<CreateInvitationDto, Participation>();
            CreateMap<Participation, ParticipationDto>();

            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();
        }
    }
}
