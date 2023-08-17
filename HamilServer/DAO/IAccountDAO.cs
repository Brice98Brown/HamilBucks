using HamilBucks.HamilServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamilBucks.HamilServer.DAO
{
    public interface IAccountDAO
    {
        Account GetAccountByUserId(int userId);
        Account GetAccountByAccountId(int accountId);
    }
}
