using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamilBucks.HamilServer.Models
{
    public class Account
    {
        public decimal Balance { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
    }
}
