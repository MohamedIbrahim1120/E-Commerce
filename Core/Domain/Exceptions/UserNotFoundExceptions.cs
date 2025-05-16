using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotFoundExceptions(string email):
        NotFoundException($"User With Email ({email}) Not Found")
    {
    }
}
