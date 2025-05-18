using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyTask.Core.ContractsAndDTO_s
{
    public record UpdatePassword(
        Guid Id,
        string NewPassword,
        string ModifiedBy
        );
}
