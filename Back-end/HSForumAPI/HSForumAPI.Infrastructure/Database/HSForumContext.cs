using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Database
{
    public class HSForumContext : DbContext
    {
        public HSForumContext(DbContextOptions<HSForumContext> options)
            : base(options)
        {
            
        }
        public DbSet<LocalUser> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReply> PostReplies { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //LocalUser
            modelBuilder.Entity<LocalUser>()
                .HasKey(u => u.UserId);
            //Post
            modelBuilder.Entity<Post>()
                .HasKey(p => p.PostId);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.PostType)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.PostTypeId);
            //PostReply
            modelBuilder.Entity<PostReply>()
                .HasKey(pr => pr.ReplyId);
            modelBuilder.Entity<PostReply>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.Replies)
                .HasForeignKey(pr => pr.UserId);
            modelBuilder.Entity<PostReply>()
                .HasOne(pr => pr.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(pr => pr.PostId);
            //PostType
            modelBuilder.Entity<PostType>()
                .HasKey(pt => pt.PostTypeId);
            modelBuilder.Entity<PostType>()
                .Property(pt => pt.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (EPostType)Enum.Parse(typeof(EPostType), v));
            //Rating
            modelBuilder.Entity<Rating>()
                .HasKey(r => r.RatingId);
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(pr => pr.UserId);
            modelBuilder.Entity<Rating>()
                .HasOne(pr => pr.Post)
                .WithMany(p => p.Ratings)
                .HasForeignKey(pr => pr.PostId);
            //Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasConversion(
                    v => v.ToString(),
                    v => (ERole)Enum.Parse(typeof(ERole), v));
            //UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(r => r.UserRoleId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pr => pr.RoleId);
        }
    }
}
