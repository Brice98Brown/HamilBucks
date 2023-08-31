using HamilBucks.HamilServer.DAO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamilBucks.HamilServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
    {
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        public UserController(IAccountDAO account, IUserDAO user)
        {
            this.accountDAO = account;
            this.userDAO = user;
        }
    }
}
