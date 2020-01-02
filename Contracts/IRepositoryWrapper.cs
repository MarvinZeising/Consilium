namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IPersonRepository Person { get; }
        IProjectRepository Project { get; }
        IParticipationRepository Participation { get; }
        IRoleRepository Role { get; }
        void Save();
    }
}
