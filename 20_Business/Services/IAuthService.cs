using _30_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20_Business.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
    }
}
