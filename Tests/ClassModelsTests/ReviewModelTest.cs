using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using Project.Models;
using Tests.TestHelpers;

namespace Tests.ClassModelsTests
{
    [TestClass]
    public class ReviewModelTest : IDisposable
    {
        private ReviewStateHelper _stateHelper;
        private ReviewModel _reviewModel;

        public ReviewModelTest()
        {
            _stateHelper = new ReviewStateHelper();
            _reviewModel = new ReviewModel();
            _stateHelper.StoreState(); // Backup the initial state
        }

        public void Dispose()
        {
            _stateHelper.RestoreState(); // Restore state after each test
        }

        [TestMethod]
        public void AddReview_ShouldInsertReviewSuccessfully()
        {
            // Arrange
            var newReview = new Review(0, 12345, "Great doctor!", 5);

            // Act
            var result = _reviewModel.AddReview(newReview);

            // Assert
            result.Should().BeTrue();

            var fetched = _reviewModel.FetchReview(12345);
            fetched.Should().NotBeNull();
            fetched!.MedicalRecordID.Should().Be(12345);
            fetched.Text.Should().Be("Great doctor!");
            fetched.NrStars.Should().Be(5);
        }

        [TestMethod]
        public void FetchReview_ShouldReturnNull_WhenReviewDoesNotExist()
        {
            // Act
            var result = _reviewModel.FetchReview(-999);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void RemoveReview_ShouldDeleteReviewSuccessfully()
        {
            // Arrange
            var review = new Review(0, 98765, "To be deleted", 2);
            _reviewModel.AddReview(review);
            var fetched = _reviewModel.FetchReview(98765);
            fetched.Should().NotBeNull();

            // Act
            var removed = _reviewModel.RemoveReview(fetched!.ReviewID);

            // Assert
            removed.Should().BeTrue();
            var resultAfterDelete = _reviewModel.FetchReview(98765);
            resultAfterDelete.Should().BeNull();
        }
    }
}
