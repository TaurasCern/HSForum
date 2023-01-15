namespace HSForumAPI.Infrastructure.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IPostRepository Posts { get; }
        IPostReplyRepository PostReplies { get; }
        IRatingRepository Ratings { get; }
        IPostTypeRepository PostTypes { get; }
        Task SaveAsync();
    }
}
