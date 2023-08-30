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
    public class TransferController
    {
        private readonly ITransferDAO transferDAO;
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        public TransferController(ITransferDAO transfer, IAccountDAO account, IUserDAO user)
        {
            this.transferDAO = transfer;
            this.accountDAO = account;
            this.userDAO = user;
        }
    }
}
