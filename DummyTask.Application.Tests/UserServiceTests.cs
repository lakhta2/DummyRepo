using DummyTask.Core.ContractsAndDTO_s;
using DummyTask.Core.Abstractions;
using DummyTask.Application.Services;
using DummyTask.Core.Models;
using Moq;

namespace DummyTask.Application.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void Create_ValidDTO_DataSentToCoreLevel()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);

            //act
            var createdUser = userService.Create(createUserDTO);

            //assert
            mock.Verify(
                r => r.AddToSet(It.Is<User>(u =>
                    u.Login == createUserDTO.Login &&
                    u.Password == createUserDTO.Password &&
                    u.Name == createUserDTO.Name &&
                    u.Gender == createUserDTO.Gender &&
                    u.Birthday == createUserDTO.Birthday &&
                    u.Admin == createUserDTO.Admin
                    )),
                Times.Once
                );
        }
        [Fact]
        public void GetAllActiveUsersAsync_ValidDTO_DataSentToCoreLevel()
        {
            //arrange
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);

            //act
            var createdUser = userService.GetAllActiveUsersAsync();

            //assert
            mock.Verify(r => r.GetAllActiveUsersAsync(), Times.Once());
        }
        [Fact]
        public void GetAllUsersOlderThanAsync_ValidDTO_DataSentToCoreLevel()
        {
            //arrange
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);

            //act
            var createdUser = userService.GetAllUsersOlderThanAsync(23);

            //assert
            mock.Verify(r => r.GetAllUsersOlderThanAsync(It.Is<int>(a => a == 23)), Times.Once());
        }
        [Fact]
        public void GetUserByLoginAndPasswordAsync_ValidDTO_DataSentToCoreLevel()
        {
            //arrange
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);

            //act
            var userByLoginAndPassword = userService.GetUserByLoginAndPasswordAsync("login", "password");

            //assert
            mock.Verify(r => r.GetUserByLoginAndPasswordAsync(It.Is<string>(l => l == "login"),It.Is<string>(p => p == "password")), Times.Once());
        }
        [Fact]
        public void GetUserByLoginAsync_ValidDTO_DataSentToCoreLevel()
        {
            //arrange
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);

            //act
            var userByLogin = userService.GetUserByLoginAsync("login");

            //assert
            mock.Verify(r => r.GetUserByLoginAsync(It.Is<string>(l => l == "login")), Times.Once());
        }
        [Fact]
        public void Restore_ValidDTO_DataSentToCoreLevel()
        {
            //arrange
            var userId = Guid.NewGuid();
            var restoreDTO = new Restore(userId); 
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);
            var user = new Mock<User>();
            mock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user.Object);

            //act
            var restoredUser = userService.Restore(restoreDTO);

            //assert
            mock.Verify(r => r.Restore(It.Is<Restore>(l => l.Id == restoreDTO.Id)), Times.Once());
        }
        [Fact]
        public void SoftDelete_ValidDTO_DataSentToCoreLevel()
        {
            //arrange
            var userId = Guid.NewGuid();
            var softDelete = new DeleteSoft(userId, "");
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);
            var user = new Mock<User>();
            mock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user.Object);

            //act
            var deletedUser = userService.SoftDelete(softDelete);

            //assert
            mock.Verify(r => r.GetUserById(It.Is<Guid>(l => l == softDelete.Id)), Times.Once());
        }
        [Fact]
        public void Update_UpdateBirthday_DataSentToCoreLevel() //Update Birthday
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateBirthday = new UpdateBirthday(userId,DateTime.UtcNow, "");
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);
            var user = new Mock<User>();
            mock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user.Object);

            //act
            var updatedUser = userService.Update(updateBirthday);

            //assert
            mock.Verify(r => r.GetUserById(It.Is<Guid>(l => l == updateBirthday.Id)), Times.Once());
        }
        [Fact]
        public void Update_UpdateGender_DataSentToCoreLevel() //Update Gender
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateGender = new UpdateGender(userId, 1, "");
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);
            var user = new Mock<User>();
            mock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user.Object);

            //act
            var updatedUser = userService.Update(updateGender);

            //assert
            mock.Verify(r => r.GetUserById(It.Is<Guid>(l => l == updateGender.Id)), Times.Once());
        }
        [Fact]
        public void Update_UpdateLogin_DataSentToCoreLevel() 
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateLogin = new UpdateLogin(userId, "NewOne", "");
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);
            var user = new Mock<User>();
            mock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user.Object);

            //act
            var updatedUser = userService.Update(updateLogin);

            //assert
            mock.Verify(r => r.GetUserById(It.Is<Guid>(l => l == updateLogin.Id)), Times.Once());
        }
        [Fact]
        public void Update_UpdateName_DataSentToCoreLevel() 
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateName = new UpdateName(userId, "NewName", "");
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);
            var user = new Mock<User>();
            mock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user.Object);

            //act
            var updatedUser = userService.Update(updateName);

            //assert
            mock.Verify(r => r.GetUserById(It.Is<Guid>(l => l == updateName.Id)), Times.Once());
        }
        [Fact]
        public void Update_UpdatePassword_DataSentToCoreLevel()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updatePassword = new UpdatePassword(userId, "NewPassword", "");
            var mock = new Mock<IVirtualRepository>();
            var userService = new UserService(mock.Object);
            var user = new Mock<User>();
            mock.Setup(r => r.GetUserById(userId)).ReturnsAsync(user.Object);

            //act
            var updatedUser = userService.Update(updatePassword);

            //assert
            mock.Verify(r => r.GetUserById(It.Is<Guid>(l => l == updatePassword.Id)), Times.Once());
        }
    }
}
