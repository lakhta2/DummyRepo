using DummyTask.Core.Abstractions;
using DummyTask.Core.Models;
using DummyTask.Core.ContractsAndDTO_s;

namespace DummyTask.Persistence
{
    public class VirtualRepository : IVirtualRepository
    {
        public Dictionary<Guid, User> CollectionOfUsers;
        public VirtualRepository(Dictionary<Guid, User> collectionOfUsers) 
        {
            CollectionOfUsers = collectionOfUsers;
        }
        public VirtualRepository()
        {
            CollectionOfUsers = new Dictionary<Guid, User>();
        }
        public static VirtualRepository Create(Dictionary<Guid, User> collectionOfUsers)
        {
            return new VirtualRepository(collectionOfUsers);
        }

        public Task AddToSet(User user)
        {
            this.CollectionOfUsers.Add(user.Id, user);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetAllActiveUsersAsync()
        {
            var allActiveUsers = this.CollectionOfUsers
                .Values
                .Where(u => string.IsNullOrEmpty(u.RevokedBy) && u.RevokedOn == DateTime.MinValue);

            return Task.FromResult(allActiveUsers);
        }

        public Task<IEnumerable<User>> GetAllUsersOlderThanAsync(int age)
        {
            var allUsersOlderThan = this.CollectionOfUsers
                .Values
                .Where(u =>
                {
                    if (u.Birthday != null)
                    {
                        var calculatedAge = DateTime.UtcNow.Year - u.Birthday.Value.Year;
                        if (u.Birthday.Value.Date > DateTime.Today.AddYears(-calculatedAge)) calculatedAge--;
                        return calculatedAge > age;
                    }
                    return false;
                });

            return Task.FromResult(allUsersOlderThan);
        }

        public Task<User> GetUserById(Guid id)
        {
            var userById = this.CollectionOfUsers
                .Values
                .Where(u => u.Id == id)
                .FirstOrDefault();

            return Task.FromResult(userById);
        }

        public Task<IEnumerable<User>> GetUserByLoginAndPasswordAsync(string login, string password)
        {
            var getUserByLoginAndPassword = this.CollectionOfUsers
                .Values
                .Where(u => u.Login == login && u.Password == password);

            return Task.FromResult(getUserByLoginAndPassword);
        }

        public Task<IEnumerable<User>> GetUserByLoginAsync(string login)
        {
            var getUserByLogin = this.CollectionOfUsers
                 .Values
                 .Where(u => u.Login == login);

            return Task.FromResult(getUserByLogin);
        }

        public Task Restore(Restore restore)
        {
            var userToRestore = this.CollectionOfUsers
                .Values
                .Where(u => u.Id == restore.Id)
                .First();
            userToRestore.Restore(restore);

            return Task.CompletedTask;
        }
    }
}
