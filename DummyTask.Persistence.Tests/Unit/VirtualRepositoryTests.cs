using DummyTask.Core.ContractsAndDTO_s;
using DummyTask.Core.Models;

namespace DummyTask.Persistence.Tests.Unit
{
    public class VirtualRepositoryTests
    {
        /// <summary>
        /// Because of the fact that in this case I decided to use Dictionary, instead of real db through EF, unit-tests and db tests were merged... 
        /// For testing db use Fakes
        /// According to R.Martin - "Fake" is a real implementetion of some system which, for some reasons, can't be used in production code. 
        /// Example of such system is in-memory implementation of Databases.
        /// It that case i tried to create fake database to not use the real one.
        /// </summary>
        [Fact]
        public void AddToSet_ValidData_UserAddedToSet()
        {
            //arrange
            var dtoForUserCreation = new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false);
            var userToBeAdded = User.Create(dtoForUserCreation);
            var dictionary = new Dictionary<Guid, User>();
            var repositoryExemplare = VirtualRepository.Create(dictionary);

            //act
            repositoryExemplare.AddToSet(userToBeAdded);

            //assert
            var value = Assert.Contains(userToBeAdded.Id, dictionary);
            Assert.Equal(userToBeAdded, value);
        }
        [Fact]
        public async Task GetAllActiveUsersAsync_ValidData_SubsetOfActiveUsersReturnedAsync()
        {
            //arrange
            CreateUser[] users = [ 
                new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Max    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Sam    ", 2, DateTime.Parse("15.05.2025"), false), //Revoked
                new CreateUser("Test login", "qwerty", "Ronald ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Grey   ", 2, DateTime.Parse("15.05.2025"), false)  //Revoked
                ];
            var userList = new List<User>();
            foreach (var user in users) 
            {
                userList.Add(User.Create(user));
            }
            userList[2].SoftDelete(new DeleteSoft(Guid.NewGuid(),"Me"));
            userList[4].SoftDelete(new DeleteSoft(Guid.NewGuid(), "Me"));
            var dictionary = new Dictionary<Guid, User>();
            var repositoryExemplare = VirtualRepository.Create(dictionary);
            foreach(var user in userList)
            {
                repositoryExemplare.AddToSet(user);
            }


            //act
            var activeUsers = await repositoryExemplare.GetAllActiveUsersAsync();

            //assert    
            Assert.All(activeUsers, m => {
                Assert.False(m.RevokedBy == "Me");
                Assert.False(m.RevokedOn > DateTime.MinValue);
                });
            Assert.Equal(3, activeUsers.Count());
            var expectedActiveUsersId = userList
                .Where(u => string.IsNullOrEmpty(u.RevokedBy) && u.RevokedOn == DateTime.MinValue)
                .Select(u=>u.Id)
                .ToHashSet();
            var actualId = activeUsers.Select(u => u.Id).ToHashSet();
            Assert.True(expectedActiveUsersId.SequenceEqual(actualId));
        }

        [Theory]
        [InlineData(23)]
        public async Task GetAllUsersOlderThanAsync_ValidData_SubsetOfActiveUsersReturnedAsync(int ageTreshold)
        {
            //arrange
            CreateUser[] users = [
                new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2012"), false),
                new CreateUser("Test login", "qwerty", "Max    ", 2, DateTime.Parse("15.05.2011"), false),
                new CreateUser("Test login", "qwerty", "Sam    ", 2, DateTime.Parse("15.05.1988"), false), //Older than
                new CreateUser("Test login", "qwerty", "Ronald ", 2, DateTime.Parse("15.05.2015"), false),
                new CreateUser("Test login", "qwerty", "Grey   ", 2, DateTime.Parse("15.05.2001"), false),  //Older than
                new CreateUser("Test login", "qwerty", "Grey   ", 2, null, false)  //Older than
                ];
            var userList = new List<User>();
            foreach (var user in users)
            {
                userList.Add(User.Create(user));
            }
            var dictionary = new Dictionary<Guid, User>();
            var repositoryExemplare = VirtualRepository.Create(dictionary);
            foreach (var user in userList)
            {
                repositoryExemplare.AddToSet(user);
            }

            //act
            var usersOlderThan = await repositoryExemplare.GetAllUsersOlderThanAsync(ageTreshold);

            //assert
            Assert.All(usersOlderThan, u =>
            {
                if (u.Birthday != null)
                {
                    var age = DateTime.UtcNow.Year - u.Birthday.Value.Year;
                    if (u.Birthday.Value.Date > DateTime.Today.AddYears(-age)) age--;
                    Assert.True(age > ageTreshold);
                }
            });
            Assert.Equal(2, usersOlderThan.Count());
            var expectedUsersOlderThanId = userList
                .Where(u => {
                    if (u.Birthday != null)
                    {
                        var age = DateTime.UtcNow.Year - u.Birthday.Value.Year;
                        if (u.Birthday.Value.Date > DateTime.Today.AddYears(-age)) age--;
                        return age > ageTreshold;
                    } 
                    return false;
                })
                .Select(u => u.Id)
                .ToHashSet();
            var actualId = usersOlderThan.Select(u => u.Id).ToHashSet();
            Assert.True(expectedUsersOlderThanId.SequenceEqual(actualId));
        }
        [Fact]
        public async Task GetUserByLoginAndPasswordAsync_ValidData_SubsetOfActiveUsersReturnedAsync()
        {
            //arrange
            CreateUser[] users = [
                new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("UniqueLogin", "qwerty", "Max    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Sam    ", 2, DateTime.Parse("15.05.2025"), false), 
                new CreateUser("Test login", "qwerty", "Ronald ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Grey   ", 2, DateTime.Parse("15.05.2025"), false)
                ];
            var userList = new List<User>();
            foreach (var user in users)
            {
                userList.Add(User.Create(user));
            }
            var dictionary = new Dictionary<Guid, User>();
            var repositoryExemplare = VirtualRepository.Create(dictionary);
            foreach (var user in userList)
            {
                repositoryExemplare.AddToSet(user);
            }

            //act
            var userByLoginAndPassword = await repositoryExemplare.GetUserByLoginAndPasswordAsync("UniqueLogin", "qwerty");

            //assert
            Assert.Single(userByLoginAndPassword);
            Assert.All(userByLoginAndPassword, m => {
                Assert.True(m.Login == "UniqueLogin");
                Assert.True(m.Password == "qwerty");
            });
        }
        [Fact]
        public async Task GetUserByLoginAsync_ValidData_SubsetOfActiveUsersReturnedAsync()
        {
            //arrange
            CreateUser[] users = [
                new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("UniqueLogin", "qwerty", "Max    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Sam    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Ronald ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Grey   ", 2, DateTime.Parse("15.05.2025"), false)
                ];
            var userList = new List<User>();
            foreach (var user in users)
            {
                userList.Add(User.Create(user));
            }
            var dictionary = new Dictionary<Guid, User>();
            var repositoryExemplare = VirtualRepository.Create(dictionary);
            foreach (var user in userList)
            {
                repositoryExemplare.AddToSet(user);
            }

            //act
            var userByLogin = await repositoryExemplare.GetUserByLoginAsync("UniqueLogin");

            //assert
            Assert.Single(userByLogin);
            Assert.True(userByLogin.First().Login == "UniqueLogin");
        }
        [Fact]
        public async Task Restore_ValidData_SubsetOfActiveUsersReturnedAsync()
        {
            //arrange
            CreateUser[] users = [
                new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("UniqueLogin", "qwerty", "Max    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Sam    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Ronald ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Grey   ", 2, DateTime.Parse("15.05.2025"), false)
                ];
            var userList = new List<User>();
            foreach (var user in users)
            {
                userList.Add(User.Create(user));
            }
            var dictionary = new Dictionary<Guid, User>();
            var repositoryExemplare = VirtualRepository.Create(dictionary);
            userList[2].SoftDelete(new DeleteSoft(Guid.NewGuid(), "Me"));
            var restore = new Restore(userList[2].Id);
            foreach (var user in userList)
            {
                repositoryExemplare.AddToSet(user);
            }

            //act
            await repositoryExemplare.Restore(restore);

            //assert
            Assert.True(userList[2].RevokedOn == DateTime.MinValue);
            Assert.True(string.IsNullOrEmpty(userList[2].RevokedBy));
        }
        [Fact]
        public async Task GetUserById_ValidData_SubsetOfActiveUsersReturnedAsync()
        {
            //arrange
            CreateUser[] users = [
                new CreateUser("Test login", "qwerty", "William", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("UniqueLogin", "qwerty", "Max    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Sam    ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Ronald ", 2, DateTime.Parse("15.05.2025"), false),
                new CreateUser("Test login", "qwerty", "Grey   ", 2, DateTime.Parse("15.05.2025"), false)
                ];
            var userList = new List<User>();
            foreach (var user in users)
            {
                userList.Add(User.Create(user));
            }
            var dictionary = new Dictionary<Guid, User>();
            var repositoryExemplare = VirtualRepository.Create(dictionary);
            foreach (var user in userList)
            {
                repositoryExemplare.AddToSet(user);
            }

            //act
            var userFromCollection = await repositoryExemplare.GetUserById(userList[3].Id);

            //assert
            Assert.True(userList[3] == userFromCollection);
        }
    }
}
