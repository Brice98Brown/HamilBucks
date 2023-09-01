using HamilBucks.HamilServer.DAO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamilBucks.HamilServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController
    {
        private readonly IUserDAO userDAO;
        public LoginController( IUserDAO userDAO)
        {
            this.userDAO = userDAO;
        }
    }
}
