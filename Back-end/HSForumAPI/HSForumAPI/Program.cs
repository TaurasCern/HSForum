using HSForumAPI.Domain.Services;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace HSForumAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<HSForumContext>(option =>
            {
                option.UseSqlite(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            });


            // Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostReplyRepository, PostReplyRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddHttpContextAccessor();


            var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });           



            builder.Services.AddControllers();



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("_allowAll",
                    policy =>
                    {
                        policy
                          .AllowAnyHeader()
                          .AllowAnyOrigin()
                          .AllowAnyMethod();
                    });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(option =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description =
                        "JWT Authorization header is using Bearer scheme. \r\n\r\n" +
                        "Enter 'Bearer' and token separated by a space. \r\n\r\n" +
                        "Example: \"Bearer d5f41g85d1f52a\"",
                    Name = "Authorization", 
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("_allowAny");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DbContext>();
            dbContext.Database.Migrate();
        }
    }
}