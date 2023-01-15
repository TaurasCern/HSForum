﻿// <auto-generated />
using System;
using HSForumAPI.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HSForumAPI.Infrastructure.Migrations
{
    [DbContext(typeof(HSForumContext))]
    [Migration("20230115000854_AddedIsActiveToPostReply")]
    partial class AddedIsActiveToPostReply
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.12");

            modelBuilder.Entity("HSForumAPI.Domain.Models.LocalUser", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            CreatedAt = new DateTime(2023, 1, 15, 2, 8, 54, 611, DateTimeKind.Local).AddTicks(501),
                            Email = "admin@hsforum.lt",
                            PasswordHash = new byte[] { 248, 51, 5, 11, 173, 94, 175, 198, 242, 54, 48, 190, 71, 241, 15, 30, 22, 65, 117, 8, 139, 141, 30, 152, 105, 47, 76, 215, 170, 150, 74, 231 },
                            PasswordSalt = new byte[] { 170, 94, 209, 136, 42, 86, 3, 111, 239, 132, 112, 39, 182, 160, 37, 54, 238, 230, 208, 220, 174, 241, 0, 145, 141, 22, 126, 171, 179, 48, 111, 186, 45, 244, 213, 100, 68, 180, 240, 34, 126, 61, 117, 53, 17, 252, 86, 169, 5, 22, 167, 162, 91, 226, 32, 53, 85, 171, 157, 210, 71, 64, 193, 38 },
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PostTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PostId");

                    b.HasIndex("PostTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.PostReply", b =>
                {
                    b.Property<int>("ReplyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReplyId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostReplies");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.PostType", b =>
                {
                    b.Property<int>("PostTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PostTypeId");

                    b.ToTable("PostTypes");

                    b.HasData(
                        new
                        {
                            PostTypeId = 1,
                            Type = "News"
                        },
                        new
                        {
                            PostTypeId = 2,
                            Type = "Tech_help"
                        },
                        new
                        {
                            PostTypeId = 3,
                            Type = "Intel"
                        },
                        new
                        {
                            PostTypeId = 4,
                            Type = "AMD"
                        },
                        new
                        {
                            PostTypeId = 5,
                            Type = "Nvidia"
                        });
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPositive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RatingId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            Name = "User"
                        },
                        new
                        {
                            RoleId = 2,
                            Name = "Admin"
                        },
                        new
                        {
                            RoleId = 3,
                            Name = "Moderator"
                        });
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.UserRole", b =>
                {
                    b.Property<int>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserRoleId = 1,
                            RoleId = 2,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.Post", b =>
                {
                    b.HasOne("HSForumAPI.Domain.Models.PostType", "PostType")
                        .WithMany("Posts")
                        .HasForeignKey("PostTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HSForumAPI.Domain.Models.LocalUser", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.PostReply", b =>
                {
                    b.HasOne("HSForumAPI.Domain.Models.Post", "Post")
                        .WithMany("Replies")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HSForumAPI.Domain.Models.LocalUser", "User")
                        .WithMany("Replies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.Rating", b =>
                {
                    b.HasOne("HSForumAPI.Domain.Models.Post", "Post")
                        .WithMany("Ratings")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HSForumAPI.Domain.Models.LocalUser", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.UserRole", b =>
                {
                    b.HasOne("HSForumAPI.Domain.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HSForumAPI.Domain.Models.LocalUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.LocalUser", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Ratings");

                    b.Navigation("Replies");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.Post", b =>
                {
                    b.Navigation("Ratings");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.PostType", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("HSForumAPI.Domain.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}