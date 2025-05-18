using DummyTask.Core.Abstractions;
using DummyTask.Core.ContractsAndDTO_s;
using DummyTask.Core.Models;

namespace DummyTask.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IVirtualRepository _virtualRepository;
        public UserService(IVirtualRepository virtualRepository)
        {
            _virtualRepository = virtualRepository;
        }
        public Task Create(CreateUser user)
        {
            var userToBeAdded = User.Create(user);
            _virtualRepository.AddToSet(userToBeAdded);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetAllActiveUsersAsync()
        {
            return _virtualRepository.GetAllActiveUsersAsync();
        }

        public Task<IEnumerable<User>> GetAllUsersOlderThanAsync(int age)
        {
            return _virtualRepository.GetAllUsersOlderThanAsync(age);
        }

        public Task<IEnumerable<User>> GetUserByLoginAndPasswordAsync(string login, string password)
        {
            return _virtualRepository.GetUserByLoginAndPasswordAsync(login, password);
        }

        public Task<IEnumerable<User>> GetUserByLoginAsync(string login)
        {
            return _virtualRepository.GetUserByLoginAsync(login);
        }

        public Task<User> Restore(Restore restore)
        {
            var userToRestore = _virtualRepository.GetUserById(restore.Id).Result;
            _virtualRepository.Restore(restore);
            return Task.FromResult(userToRestore);
        }

        public Task SoftDelete(DeleteSoft user)
        {
            var userToBeSoftDeleted = _virtualRepository.GetUserById(user.Id).Result;
            if (userToBeSoftDeleted != null)
            {
                userToBeSoftDeleted.SoftDelete(user);
            }
            return Task.CompletedTask;
        }

        public Task Update(UpdateBirthday update) 
        {
            var userToBeUpdated = _virtualRepository.GetUserById(update.Id).Result;
            userToBeUpdated.UpdateBirthday(update);
            return Task.CompletedTask;
        }

        public Task Update(UpdateGender update)
        {
            var userToBeUpdated = _virtualRepository.GetUserById(update.Id).Result;
            userToBeUpdated.UpdateGender(update);
            return Task.CompletedTask;
        }

        public Task Update(UpdateLogin update) 
        {
            var userToBeUpdated = _virtualRepository.GetUserById(update.Id).Result;
            userToBeUpdated.UpdateLogin(update);
            return Task.CompletedTask;
        }

        public Task Update(UpdateName update) 
        {
            var userToBeUpdated = _virtualRepository.GetUserById(update.Id).Result;
            userToBeUpdated.UpdateName(update);
            return Task.CompletedTask;
        }

        public Task Update(UpdatePassword update)
        {
            var userToBeUpdated = _virtualRepository.GetUserById(update.Id).Result;
            userToBeUpdated.UpdatePassword(update);
            return Task.CompletedTask;
        }
    }
}
