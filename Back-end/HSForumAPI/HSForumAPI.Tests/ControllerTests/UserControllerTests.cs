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
        Mock<IAdapterService> _mockadapterConfiguration = new();
        
        private void ResetDatabase()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.PostTypes.RemoveRange(_context.PostTypes);
            _context.UserRoles.RemoveRange(_context.UserRoles);
            _context.Roles.RemoveRange(_context.Roles);
            _context.SaveChanges();
        }
        [TestInitialize]
        public void OnInit()
        {
            var options = new DbContextOptionsBuilder<HSForumContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        
            _context = new HSForumContext(options);
        
            _context.Users.AddRange(MockData.LocalUserMockData.GetData());
            _context.PostTypes.AddRange(MockData.PostTypeMockData.Data);
            _context.Roles.AddRange(MockData.RoleMockData.Data);
            _context.UserRoles.AddRange(MockData.UserRoleMockData.Data);
        
            _context.SaveChanges();
        
            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns(Guid.NewGuid().ToString());
            _mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(mockSection.Object);

            _mockadapterConfiguration = new();
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<PostRequest>(), It.IsAny<int>()))
                 .Returns(It.IsAny<Post>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<Post>(), It.IsAny<int>()))
                 .Returns(It.IsAny<PostResponse>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<PostReply>()))
                 .Returns(It.IsAny<PostReplyResponse>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<PostReply>(), It.IsAny<string>()))
                 .Returns(It.IsAny<PostReplyResponse>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<PostReplyRequest>(), It.IsAny<int>()))
                 .Returns(It.IsAny<PostReply>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<RatingRequest>(), It.IsAny<int>()))
                 .Returns(It.IsAny<Rating>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<Rating>(), It.IsAny<bool>()))
                 .Returns(It.IsAny<RatingResponse>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<Post>()))
                 .Returns(It.IsAny<PostUpdateRequest>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<PostUpdateRequest>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                 .Returns(It.IsAny<Post>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<LocalUser>(), It.IsAny<int>(), It.IsAny<int>()))
                 .Returns(It.IsAny<UserGetResponse>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<LocalUser>()))
                 .Returns(It.IsAny<LoginResponse>());
            _mockadapterConfiguration.Setup(x => x.Bind(It.IsAny<PostType>()))
                 .Returns(It.IsAny<PostTypeResponse>());
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
                .Returns(Task.FromResult(new Tuple<bool, LocalUser>(false, _context.Users.FirstOrDefault(u => u.Username == fake_username))));

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
            var fake_userId = 1;

            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.ExistAsync(u => u.UserId == fake_userId))
                .Returns(Task.FromResult(true));
            mockUnitOfWork.Setup(x => x.Users.GetAsync(u => u.UserId == fake_userId, true))
                .Returns(Task.FromResult(It.IsAny<LocalUser>()));
            mockUnitOfWork.Setup(x => x.Posts.GetAllWithRatingsAsync(p => p.UserId == fake_userId && p.IsActive == true, true))
                .Returns(Task.FromResult(It.IsAny<List<Post>>()));

            Mock<IRatingService> mockRatingService = new();
            mockRatingService.Setup(x => x.CalculateUserReputation(It.IsAny<List<Post>>()))
                .Returns(Task.FromResult(It.IsAny<int>()));

            Mock<IAdapterService> mockAdapterService = new (_mockadapterConfiguration);

            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService, mockAdapterService.Object, mockRatingService.Object);
            var actual = await sut.Get(1);
            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
            ResetDatabase();
        }
    }
}