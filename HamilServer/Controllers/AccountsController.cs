using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HamilBucks.HamilServer.DAO;
using HamilBucks.HamilServer.Models;
using Microsoft.AspNetCore.Authorization;
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
        private int LoggedInUserId
        {
            get
            {
                Claim idClaim = User.FindFirst("sub");
                if (idClaim == null)
                {
                    // User is not logged in
                    return -1;
                }
                else
                {
                    
                    return int.Parse(idClaim.Value);
                }
            }
        }
        [HttpGet()]
        [Authorize]
        public ActionResult GetAccount()
        {
            int userId = LoggedInUserId;
            if (userId <= 0)
            {
                return Unauthorized();
            }
            Account account = accountDAO.GetAccountByUserId(userId);
            if (account == null)
            {
                return NotFound();
            }

            if (userId != account.UserId)
            {
                return Forbid();
            }
            return Ok(account);
        }
    }
}
