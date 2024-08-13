using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank.Domain.Models
{
    public class Deposit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }

}
