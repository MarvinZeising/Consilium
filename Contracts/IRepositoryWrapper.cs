namespace Server.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IPersonRepository Person { get; }
        IProjectRepository Project { get; }
        IParticipationRepository Participation { get; }
        IRoleRepository Role { get; }
        ICategoryRepository Category { get; }
        IShiftRepository Shift { get; }
        IApplicationRepository Application { get; }
        ITeamRepository Team { get; }
        IEligibilityRepository Eligibility { get; }
        ICongregationRepository Congregation { get; }
        ITopicRepository Topic { get; }
        IArticleRepository Article { get; }
        void Save();
    }
}
