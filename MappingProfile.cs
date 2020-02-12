using AutoMapper;
using Server.Entities.DataTransferObjects;
using Server.Entities.Models;

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

            CreateMap<Eligibility, EligibilityDto>();
            CreateMap<CreateEligibilityDto, Eligibility>();
            CreateMap<UpdateEligibilityDto, Eligibility>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Team, TeamDto>();
            CreateMap<CreateTeamDto, Team>();

            CreateMap<Shift, ShiftDto>();
            CreateMap<CreateShiftDto, Shift>();

            CreateMap<Application, ApplicationDto>();
            CreateMap<CreateApplicationDto, Application>();

            CreateMap<Attendee, AttendeeDto>();
            CreateMap<CreateAttendeeDto, Attendee>();
        }
    }
}
