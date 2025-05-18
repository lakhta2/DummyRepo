using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyTask.Core.ContractsAndDTO_s
{
    public record CreateUser(
        string Login,
        string Password,
        string Name,
        int Gender,
        DateTime? Birthday,
        bool Admin
        );
}
