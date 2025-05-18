using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyTask.Core.ContractsAndDTO_s
{
    public record LoginPasswordUserRequest(
        string login,
        string password
        );
}
