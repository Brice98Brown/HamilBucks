using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HamilBucks.HamilServer.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HamilBucks.HamilServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        public AccountsController(IAccountDAO account, IUserDAO user)
        {
            this.accountDAO = account;
            this.userDAO = user;
        }
    }
}
