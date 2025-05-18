using DummyTask.Core.Models;
using System;
using DummyTask.Core.ContractsAndDTO_s;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyTask.Core.Abstractions
{
    public interface IVirtualRepository
    {
        Task AddToSet(User user); 
        Task<IEnumerable<User>> GetAllActiveUsersAsync();
        Task<IEnumerable<User>> GetUserByLoginAsync(string login);
        Task<IEnumerable<User>> GetUserByLoginAndPasswordAsync(string login, string password);
        Task<IEnumerable<User>> GetAllUsersOlderThanAsync(int age);
        Task Restore(Restore restore);
        Task<User> GetUserById(Guid id);
    }
}
