using HSForumAPI.Controllers;
using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.DTOs.PostReplyDTOs;
using HSForumAPI.Domain.DTOs.PostTypeDTOs;
using HSForumAPI.Domain.DTOs.RatingDTOs;
using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HSForumAPI.Tests.ControllerTests
{
    [TestClass]
    public class UserControllerTests
    {
        private HSForumContext _context;
        Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
        
        private void ResetDatabase()
        {
            _context.Posts.RemoveRange(_context.Posts);
            _context.Users.RemoveRange(_context.Users);
            _context.PostTypes.RemoveRange(_context.PostTypes);
            _context.UserRoles.RemoveRange(_context.UserRoles);
            _context.Roles.RemoveRange(_context.Roles);

            //_context.Ratings.RemoveRange(_context.Ratings);

             _context.SaveChanges();
        }
        [TestInitialize]
        public void OnInit()
        {
            var options = new DbContextOptionsBuilder<HSForumContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            
            _context = new HSForumContext(options);


            _context.Users.AddRange(MockData.LocalUserMockData.GetData());
            _context.PostTypes.AddRange(MockData.PostTypeMockData.Data);
            _context.Roles.AddRange(MockData.RoleMockData.Data);
            _context.UserRoles.AddRange(MockData.UserRoleMockData.Data);
            //_context.SaveChanges();
            //_context.Posts.AddRange(MockData.PostMockData.Data);
            //_context.Ratings.AddRange(MockData.RatingsMockData.Data);


            _context.SaveChanges();
        
            Mock<IConfigurationSection> mockSection = new ();
            mockSection.Setup(x => x.Value).Returns(Guid.NewGuid().ToString());
            _mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(mockSection.Object);
        }
        [TestMethod]
        public async Task Login_ShouldBeOk()
        {
            var fake_username = "test";
            var fake_password = "test";
        
            var fake_user = _context.Users.Where(u => u.Username == fake_username).First();
        
            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.TryLoginAsync(fake_username, fake_password))
                .Returns(Task.FromResult(new Tuple<bool, LocalUser>(true, fake_user)));
        
            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();
            IAdapterService adapterService = new AdapterService();
            IRatingService ratingService = new RatingService();

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService, adapterService,ratingService);
            var actual = await sut.Login(new LoginRequest { Username = fake_username, Password = fake_password });
        
            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
            ResetDatabase();
        }
        [TestMethod]
        public async Task Login_ShouldBeUnauthorized()
        {
            var fake_username = "test";
            var fake_password = "test";
        
            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.TryLoginAsync(fake_username, fake_password))
                .Returns(Task.FromResult(new Tuple<bool, LocalUser>(false, new LocalUser())));

             IJwtService jwtService = new JwtService(_mockConfiguration.Object);
             IPasswordService passwordService = new PasswordService();
             IAdapterService adapterService = new AdapterService();
             IRatingService ratingService = new RatingService();

             var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService, adapterService, ratingService);
             var actual = await sut.Login(new LoginRequest { Username = fake_username, Password = fake_password });

             Assert.IsInstanceOfType(actual, typeof(UnauthorizedResult));
             ResetDatabase();
        }
        [TestMethod]
        public async Task Register_ShouldBeCreated()
        {
            var fake_username = "testUsername";
            var fake_email = "testEmail";
            var fake_password = "testPassword";

            var fake_hash = new byte[32];
            var fake_salt = new byte[64];

            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.IsRegisteredAsync(fake_username, fake_email))
                .Returns(Task.FromResult(false));
            
            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();
            IAdapterService adapterService = new AdapterService();
            IRatingService ratingService = new RatingService();

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService, adapterService, ratingService);
            var actual = await sut.Register(new RegistrationRequest
            {
                Username = fake_username,
                Email = fake_email,
                Password = fake_password
            });   
            Assert.IsInstanceOfType(actual, typeof(CreatedResult));
            ResetDatabase();
        }
        [TestMethod]
        public async Task Register_ShouldBeBadRequest()
        {
            var fake_username = "test";
            var fake_email = "test";
            var fake_password = "test";
        
            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.IsRegisteredAsync(fake_username, fake_email))
                .Returns(Task.FromResult(true));

            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();
            IAdapterService adapterService = new AdapterService();
            IRatingService ratingService = new RatingService();

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService, adapterService, ratingService);
            var actual = await sut.Register(new RegistrationRequest
            {
                Username = fake_username,
                Email = fake_email,
                Password = fake_password
            });
            Assert.IsInstanceOfType(actual, typeof(BadRequestObjectResult));
            ResetDatabase();
        }
        [TestMethod]
        public async Task Get_ShouldBeBadRequest()
        {
            Mock<IUnitOfWork> mockUnitOfWork = new();
            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();
            IAdapterService adapterService = new AdapterService();
            IRatingService ratingService = new RatingService();

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService, adapterService, ratingService);
            var actual = await sut.Get(0);
            Assert.IsInstanceOfType(actual, typeof(BadRequestResult));
            ResetDatabase();
        }
        [TestMethod]
        public async Task Get_ShouldBeNotFound()
        {
            var fake_userId = 1;

            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.ExistAsync(u => u.UserId == fake_userId))
                .Returns(Task.FromResult(false));

            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();
            IAdapterService adapterService = new AdapterService();
            IRatingService ratingService = new RatingService();

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService, adapterService, ratingService);
            var actual = await sut.Get(1);
            Assert.IsInstanceOfType(actual, typeof(NotFoundResult));
            ResetDatabase();
        }
        [TestMethod]
        public async Task Get_ShouldBeOk()
        {
            var fake_user = _context.Users.First();
            var fake_userId = fake_user.UserId;

            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.ExistAsync(u => u.UserId == fake_userId))
                .Returns(Task.FromResult(true));
            mockUnitOfWork.Setup(x => x.Users.GetAsync(u => u.UserId == fake_userId, true))
                .Returns(Task.FromResult(fake_user));
            mockUnitOfWork.Setup(x => x.Posts.GetAllWithRatingsAsync(p => p.UserId == fake_userId && p.IsActive == true, true))
                .Returns(Task.FromResult(new List<Post>()));
        
            Mock<IRatingService> mockRatingService = new();
            mockRatingService.Setup(x => x.CalculateUserReputation(It.IsAny<List<Post>>()))
                .Returns(Task.FromResult(1));

            Mock<IAdapterService> mockAdapterService = new ();
            mockAdapterService.Setup(x => x.Bind(_context.Users.First(),
                1, 1))
                 .Returns(new UserGetResponse() { Username = "", CreatedAt = DateTime.UtcNow, Reputation = 1, PostCount = 1});

            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();
        
            var sut = new UserController(
                mockUnitOfWork.Object, 
                jwtService, 
                passwordService, 
                mockAdapterService.Object, 
                mockRatingService.Object);

            //var actual = await sut.Get(fake_userId);
            //Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
            ResetDatabase();
        }
    }
}