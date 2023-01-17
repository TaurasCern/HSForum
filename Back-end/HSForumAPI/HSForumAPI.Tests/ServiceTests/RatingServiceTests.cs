using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Tests.ServiceTests
{
    [TestClass]
    public class RatingServiceTests
    {
        private readonly List<Post> MockPosts = new List<Post>()
        {
            new Post
            {
                Ratings = new List<Rating>
                {
                    new Rating
                    {
                        UserId = 1,
                        IsPositive = true,
                    },
                    new Rating
                    {
                        UserId = 2,
                        IsPositive = false,
                    },
                    new Rating
                    {
                        UserId = 3,
                        IsPositive = true,
                    }
                }
            },
            new Post
            {
                Ratings = new List<Rating>
                {
                    new Rating
                    {
                        UserId = 1,
                        IsPositive = false,
                    },
                    new Rating
                    {
                        UserId = 2,
                        IsPositive = false,
                    },
                    new Rating
                    {
                        UserId = 3,
                        IsPositive = false,
                    }
                }
            },
        };
        [TestMethod]
        public async Task CalculateRating_GivenPost_ShouldReturnRatingScore()
        {
            var fake_post = MockPosts[0];
        
            IRatingService ratingService = new RatingService();

            var actual = await ratingService.CalculateRating(fake_post);

            var expected = 1;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public async Task CalculateRating_GivenNull_ShouldReturnZero()
        {
            var fake_postRatings = new Post()
            {
                Ratings = null
            };

            IRatingService ratingService = new RatingService();

            var actual = await ratingService.CalculateRating(fake_postRatings);

            var expected = 0;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public async Task CalculateUserReputation_GivenPosts_ShouldReturnInt()
        {
            var fake_posts = MockPosts;

            IRatingService ratingService = new RatingService();

            var actual = await ratingService.CalculateUserReputation(fake_posts);

            var expected = -2;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public async Task CalculateUserReputation_GivenEmptyPosts_ShouldReturnZero()
        {
            var fake_posts = new List<Post>();

            IRatingService ratingService = new RatingService();

            var actual = await ratingService.CalculateUserReputation(fake_posts);

            var expected = 0;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public async Task CheckIfRated_GivenPostAndUserId_ShouldReturnTrue()
        {
            var fake_userId = 1;
            var fake_postRatings = MockPosts[0];

            IRatingService ratingService = new RatingService();

            var actual = await ratingService.CheckIfRated(fake_postRatings, fake_userId);

            var expected = true;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public async Task CheckIfRated_GivenPostAndUserId_ShouldReturnFalse()
        {
            var fake_userId = 4;
            var fake_postRatings = MockPosts[0];

            IRatingService ratingService = new RatingService();

            var actual = await ratingService.CheckIfRated(fake_postRatings, fake_userId);

            var expected = false;

            Assert.AreEqual(expected, actual);
        }
    }
}
