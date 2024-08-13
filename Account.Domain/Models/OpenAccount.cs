using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank.Domain.Models
{
    public class OpenAccount
    {
        public int CustomerId { get; set; }

        public decimal InitialDeposit { get; set; }

        public AccountType AccountType { get; set; }
    }

}
