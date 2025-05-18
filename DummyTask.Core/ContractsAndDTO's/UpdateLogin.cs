using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyTask.Core.ContractsAndDTO_s
{
    public record UpdateLogin(
        Guid Id,
        string NewLogin,
        string ModifiedBy
        );
}
