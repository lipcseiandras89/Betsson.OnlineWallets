using Betsson.OnlineWallets.Web.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Betsson.OnlineWallets.Web.UnitTests.Controllers
{
    [TestFixture]
    internal class TestSystemController
    {
        private readonly static object[] _error = [null, new Exception()];


        /// <summary>
        /// Arrange
        /// 
        /// ExceptionHandlerPathFeature is null.
        /// 
        /// Act
        /// 
        /// Call Error.
        /// 
        /// Assert
        /// 
        /// Error returns the result of Problem().
        /// </summary>
        [Test]
        public void TestError_ExceptionHandlerPathFeatureIsNull()
        {
            // Arrange
            var mockSystemController = new Mock<SystemController>() { CallBase = true };
            var mockIExceptionHandlerPathFeature = new Mock<IExceptionHandlerPathFeature>();
            mockIExceptionHandlerPathFeature.SetupGet(x => x.Error).Returns(null as Exception);
            var mockHttpContext = new Mock<HttpContext>();
            var mockFeatures = new Mock<IFeatureCollection>();
            mockFeatures.Setup(x => x.Get<IExceptionHandlerPathFeature>()).Returns(mockIExceptionHandlerPathFeature.Object);
            mockHttpContext.SetupGet(x => x.Features).Returns(mockFeatures.Object);
            mockSystemController.SetupGet(x => x.HttpContextNew).Returns(mockHttpContext.Object);
            var mockObjectResult = new Mock<ObjectResult>(null);
            mockSystemController.Setup(x => x.Problem(null, null, null, null, null)).Returns(mockObjectResult.Object);

            // Act
            var result = mockSystemController.Object.Error();

            // Assert
            Assert.That(mockObjectResult.Object, Is.EqualTo(result));
        }

        /// <summary>
        /// Arrange
        /// 
        /// Case 1. Error is null.
        /// Case 2. Error is not an InsufficientBalanceException object.
        /// 
        /// Act
        /// 
        /// Call Error.
        /// 
        /// Assert
        /// 
        /// exceptionHandlerPathFeature returns a Problem, called with parameterless constructor.
        /// </summary>
        [TestCaseSource(nameof(_error))]
        public void TestError_ErrorIsNull(Exception error)
        {
            // Arrange
            var mockSystemController = new Mock<SystemController>() { CallBase = true };
            var mockIExceptionHandlerPathFeature = new Mock<IExceptionHandlerPathFeature>();
            mockIExceptionHandlerPathFeature.SetupGet(x => x.Error).Returns(error);
            var mockHttpContext = new Mock<HttpContext>();
            var mockFeatures = new Mock<IFeatureCollection>();
            mockFeatures.Setup(x => x.Get<IExceptionHandlerPathFeature>()).Returns(mockIExceptionHandlerPathFeature.Object);
            mockHttpContext.SetupGet(x => x.Features).Returns(mockFeatures.Object);
            mockSystemController.SetupGet(x => x.HttpContextNew).Returns(mockHttpContext.Object);
            var mockObjectResult = new Mock<ObjectResult>(null);
            mockSystemController.Setup(x => x.Problem(null, null, null, null, null)).Returns(mockObjectResult.Object);

            // Act
            var result = mockSystemController.Object.Error();

            // Assert
            Assert.That(result, Is.EqualTo(mockObjectResult.Object));
        }
    }
}