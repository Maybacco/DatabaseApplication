using DatabaseApplication;
using Microsoft.EntityFrameworkCore;
using UserService;

namespace TestApplication
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var service = new UserService.UserService();
            service.ClearUsers();
        }

        [Test]
        public void ShouldAddUser()
        {

            var service = new UserService.UserService();
            
            service.AddUser(new User()
            {
                FirstName = "Test",
                LastName = "TestCognome",
            });
            

            Assert.AreEqual(service.GetAllUsers().Count(), 1);

        }

        [Test]
        public void ShouldRemoveUser()
        {
            var service = new UserService.UserService();


            var lastId = service.AddUser(new User()
            {
                FirstName = "Test",
                LastName = "TestCognome",
            });

            Assert.AreEqual(service.GetAllUsers().Count(), 1);


            service.RemoveUser(lastId);
            Assert.AreEqual(service.GetAllUsers().Count(), 0);


        }

        [Test]
        public void ShouldUpdateUser()
        {
            var service = new UserService.UserService();


            var lastId = service.AddUser(new User()
            {
                FirstName = "Test",
                LastName = "TestCognome",
            });

            var checkUser = service.GetUserById(lastId);
            Assert.AreEqual(checkUser.FirstName, "Test");

            service.UpdateUser(new User()
            {
                Id = lastId,
                FirstName = "Franco",
                LastName = "Rossi"
            });

            checkUser = service.GetUserById(lastId);
            Assert.AreEqual(checkUser.FirstName, "Franco");
        }
    }
}