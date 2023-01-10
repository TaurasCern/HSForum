using HSForumAPI.Infrastructure.Repositories;
using HSForumAPI.Infrastructure.Repositories.IRepositories;

namespace HSForumAPI
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostReplyRepository, PostReplyRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();

            return services;
        }
    }
}
