using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank.Domain.Models
{
    public class CloseAccount
    {
        public int CustomerId { get; set; }

        public int AccountId { get; set; }
    }

}
