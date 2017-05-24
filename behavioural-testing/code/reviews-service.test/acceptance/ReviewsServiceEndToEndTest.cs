﻿using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test.acceptance
{
    [TestFixture]
    public class ReviewsServiceEndToEndTest
    {
        private readonly ServiceRunner _service = new ServiceRunner();
        private readonly Database _database = new Database();

        [SetUp]
        public void SetUp()
        {
            _database.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            _service.Stop();
        }

        [Test]
        public void Save_an_empty_review()
        {
            _service.ReceiveReview("empty.http");
            _database.AssertWasSaved(new ReviewDto());
        }
    }
}