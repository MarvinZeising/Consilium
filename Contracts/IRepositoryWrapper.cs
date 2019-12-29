namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IPersonRepository Person { get; }
        void Save();
    }
}
