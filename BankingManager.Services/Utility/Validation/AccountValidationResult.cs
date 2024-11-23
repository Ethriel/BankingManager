using BankingManager.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManager.Services.Utility.Validation
{
    public class AccountValidationResult
    {
        public bool IsValid { get; set; }
        public string Error { get; set; }
        public string[] Errors { get; set; }
        public BankAccount BankAccount { get; set; }
    }
}
