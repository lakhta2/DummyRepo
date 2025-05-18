using DummyTask.Core.ContractsAndDTO_s;
using DummyTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyTask.Core.Abstractions
{
    public interface IUserService
    {
        Task Create(CreateUser user);
        Task Update(UpdateBirthday update);
        Task Update(UpdateGender update);
        Task Update(UpdateLogin update);
        Task Update(UpdateName update);
        Task Update(UpdatePassword update);
        Task<IEnumerable<User>> GetAllActiveUsersAsync();
        Task<IEnumerable<User>> GetUserByLoginAsync(string login);
        Task<IEnumerable<User>> GetUserByLoginAndPasswordAsync(string login, string password);
        Task<IEnumerable<User>> GetAllUsersOlderThanAsync(int age);
        Task SoftDelete(DeleteSoft user);
        Task<User> Restore(Restore restore);
    }
}
