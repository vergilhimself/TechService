using System.Windows;
using Практика_макет;

namespace TestProject2
{
    [TestClass]
    public class MainWindowTests
    {             
        [TestMethod]
        public void DbUse_Auth_UserFound()
        {
            // Arrange
            var loginText = "kasoo";
            var passwordText = "root";
            
            Assert.IsTrue(DbUse.Auth(loginText, passwordText));
        }

        [TestMethod]
        public void DbUse_Auth_UserNotFound()
        {
            // Arrange
            var loginText = "notkasoo";
            var passwordText = "root";

            Assert.IsFalse(DbUse.Auth(loginText, passwordText));
        }

        [TestMethod]
        public void DbUse_Auth_InorrectPassowrd()
        {
            // Arrange
            var loginText = "kasoo";
            var passwordText = "notroot";

            Assert.IsFalse(DbUse.Auth(loginText, passwordText));
        }

        [TestMethod]
        public void DbUse_GetRequestsByUserRole_WithUser()
        {
            // Arrange
            var loginText = "kasoo";
            var passwordText = "root";
            DbUse.Auth(loginText, passwordText);
            
            List<RequestDetail> requests = DbUse.GetRequestsByUserRole(EnteredUser.Instance.CurrentType);

            Assert.AreNotEqual(0, requests.Count);
        }

        [TestMethod]
        public void DbUse_GetRequestsByUserRole_WithOutUser()
        {
            // Arrange
            var loginText = "sdasdasdas";
            var passwordText = "asdasdasda";
            if (loginText != "" || passwordText != "") { 
            DbUse.Auth(loginText, passwordText);

            List<RequestDetail> requests = DbUse.GetRequestsByUserRole(EnteredUser.Instance.CurrentType);

            Assert.AreNotEqual(0, requests.Count);
            } else
            {
                Assert.Fail("Тест провален принудительно.");
            }
        }
    }
}
