namespace Betsson.OnlineWallets.Web.UnitTests
{
    [TestFixture]
    public class TestSystemController
    {
        private readonly static object[] _error = [null, new Exception()];


        /// <summary>
        /// Arrange
        /// 
        /// exceptionHandlerPathFeature is null.
        /// 
        /// Act
        /// 
        /// Call Error.
        /// 
        /// Assert
        /// exceptionHandlerPathFeature returns a Problem, called with parameterless constructor.
        /// </summary>
        [Test]
        public void TestError_ExceptionHandlerPathFeatureIsNull()
        {
            Assert.Fail();
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
        /// exceptionHandlerPathFeature returns a Problem, called with parameterless constructor.
        /// </summary>
        [TestCaseSource(nameof(_error))]
        public void TestError_ErrorIsNull(Exception error)
        {
            Assert.Fail();
        }

    }
}