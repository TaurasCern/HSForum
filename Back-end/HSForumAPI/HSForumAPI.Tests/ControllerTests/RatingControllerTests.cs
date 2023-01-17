using HSForumAPI.Controllers;
using HSForumAPI.Domain.DTOs.RatingDTOs;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace HSForumAPI.Tests.ControllerTests
{
    [TestClass]
    public class RatingControllerTests
    {
        [TestMethod]
        public async Task Post_ShouldBeBadRequest()
        {
            var fake_request = new RatingRequest
            {
                PostId = 1,
                IsPositive = true,
            };
            var fake_userId = 1;


            var fake_identity = new GenericIdentity(fake_userId.ToString());

            fake_identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

            var fake_contextUser = new ClaimsPrincipal(fake_identity);

            var fake_httpContext = new DefaultHttpContext()
            {
                User = fake_contextUser
            };

            var controllerContext = new ControllerContext()
            {
                HttpContext = fake_httpContext,
            };

            Mock<IUnitOfWork> mockUnitOfWork = new();
            mockUnitOfWork.Setup(x => x.Posts.GetWithRepliesAsync(p => p.PostId == fake_request.PostId, false))
                .Returns(Task.FromResult(new Post() { UserId = fake_userId}));

            mockUnitOfWork.Setup(x => x.Ratings.CreateAsync(It.IsAny<Rating>()))
                .Returns(Task.FromResult(It.IsAny<Rating>()));

            //Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            //
            //mockHttpContextAccessor.Setup(x => x.HttpContext.User.Identity.Name)
            //    .Returns($"{fake_userId}");
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            IAdapterService adapter = new AdapterService();
            IRatingService ratingService = new RatingService();

            var sut = new RatingController(mockUnitOfWork.Object, adapter, httpContextAccessor, ratingService)
            {
                ControllerContext = controllerContext
            };
            var actual = await sut.Post(fake_request);

            Assert.IsInstanceOfType(actual, typeof(CreatedAtRouteResult));
        }
    }
}
