namespace HSForumAPI.Infrastructure.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        Task SaveAsync();
    }
}
