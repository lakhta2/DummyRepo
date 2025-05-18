using DummyTask.Core.ContractsAndDTO_s;
using DummyTask.Core.Models;
using System.Reflection;
using System.Xml.Linq;
using Xunit;


namespace DummyTask.Core.Tests.Unit
{
    public class UserTests
    {
        [Fact]
        public void Create_ValidDTO_UserCreated()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);

            //act
            var result = User.Create(createUserDTO);

            //assert
            Assert.Equal("Test login", result.Login);
            Assert.Equal("qwerty", result.Password);
            Assert.Equal("William", result.Name);
            Assert.Equal(2, result.Gender);
            Assert.Equal(DateTime.Parse("15.05.2025").Ticks, result.Birthday.Value.Ticks);
            Assert.False(result.Admin);
        }
        [Fact]
        public void UpdateName_ValidDTO_NameUpdated()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var modifiableUser = User.Create(createUserDTO);
            var updateName = new UpdateName(modifiableUser.Id, "Alex", "Rex");

            //act
            modifiableUser.UpdateName(updateName);

            //assert
            Assert.Equal("Alex", modifiableUser.Name);
            Assert.Equal("Rex", modifiableUser.ModifiedBy);
            Assert.Equal(DateTime.Today.Date, modifiableUser.ModifiedOn.Value.Date);
        }
        [Fact]
        public void UpdateGender_ValidDTO_GenderUpdated()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var modifiableUser = User.Create(createUserDTO);
            var updateGender = new UpdateGender(modifiableUser.Id, 0, "Rex");

            //act
            modifiableUser.UpdateGender(updateGender);

            //assert
            Assert.Equal(0, modifiableUser.Gender);
            Assert.Equal("Rex", modifiableUser.ModifiedBy);
            Assert.Equal(DateTime.Today.Date, modifiableUser.ModifiedOn.Value.Date);
        }
        [Fact]
        public void UpdateLogin_ValidDTO_LoginUpdated()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var modifiableUser = User.Create(createUserDTO);
            var updateLogin = new UpdateLogin(modifiableUser.Id, "New Login", "Rex");

            //act
            modifiableUser.UpdateLogin(updateLogin);

            //assert
            Assert.Equal("New Login", modifiableUser.Login);
            Assert.Equal("Rex", modifiableUser.ModifiedBy);
            Assert.Equal(DateTime.Today.Date, modifiableUser.ModifiedOn.Value.Date);
        }
        [Fact]
        public void UpdatePassword_ValidDTO_PasswordUpdated()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var modifiableUser = User.Create(createUserDTO);
            var updatePassword = new UpdatePassword(modifiableUser.Id, "123456", "Rex");
            
            //act
            modifiableUser.UpdatePassword(updatePassword);

            //assert
            Assert.Equal("123456", modifiableUser.Password);
            Assert.Equal("Rex", modifiableUser.ModifiedBy);
            Assert.Equal(DateTime.Today.Date, modifiableUser.ModifiedOn.Value.Date);
        }
        [Fact]
        public void UpdateBirthday_ValidDTO_BirthdayUpdated()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var modifiableUser = User.Create(createUserDTO);
            var updateBirthday = new UpdateBirthday(modifiableUser.Id, DateTime.Parse("15.05.2025"), "Rex");

            //act
            modifiableUser.UpdateBirthday(updateBirthday);

            //assert
            Assert.Equal(DateTime.Parse("15.05.2025"), modifiableUser.Birthday);
            Assert.Equal("Rex", modifiableUser.ModifiedBy);
            Assert.Equal(DateTime.Today.Date, modifiableUser.ModifiedOn.Value.Date);
        }
        [Fact]
        public void SoftDelete_ValidDTO_UserIsSoftDeleted()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var modifiableUser = User.Create(createUserDTO);
            var deleteSoft = new DeleteSoft(modifiableUser.Id, "Rex");

            //act
            modifiableUser.SoftDelete(deleteSoft);

            //assert
            Assert.NotEqual(DateTime.MinValue, modifiableUser.RevokedOn);
            Assert.Equal("Rex", modifiableUser.RevokedBy);
        }
        [Fact]
        public void Restore_ValidDTO_UserIsSoftDeleted()
        {
            //arrange
            var createUserDTO = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var modifiableUser = User.Create(createUserDTO);
            var restore = new Restore(modifiableUser.Id);
            var deleteSoft = new DeleteSoft(modifiableUser.Id, "Rex");
            modifiableUser.SoftDelete(deleteSoft);

            //act
            modifiableUser.Restore(restore);

            //assert
            Assert.Equal(DateTime.MinValue, modifiableUser.RevokedOn);
            Assert.True(string.IsNullOrEmpty(modifiableUser.RevokedBy));
        }
    }
}
