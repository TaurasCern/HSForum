using HSForumAPI.Controllers;
using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Diagnostics;

namespace HSForumAPI.Tests.ControllerTests
{
    [TestClass]
    public class UserControllerTests
    {
        private HSForumContext _context;
        Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
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

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService);
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

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService);
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

            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Users.IsRegisteredAsync(fake_username, fake_email))
                .Returns(Task.FromResult(false));

            IJwtService jwtService = new JwtService(_mockConfiguration.Object);
            IPasswordService passwordService = new PasswordService();

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService);
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

            var sut = new UserController(mockUnitOfWork.Object, jwtService, passwordService);
            var actual = await sut.Register(new RegistrationRequest
            {
                Username = fake_username,
                Email = fake_email,
                Password = fake_password
            });

            Assert.IsInstanceOfType(actual, typeof(BadRequestObjectResult));
            ResetDatabase();
        }
    }
}