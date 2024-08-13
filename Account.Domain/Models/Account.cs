using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank.Domain.Models
{
    [Table("Account")]
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        [Required]
        public decimal Balance { get; set; }

        //Additional Columns as necessary
    }

    public enum AccountType
    {
        Checking = 1,
        Savings = 2
    }

    public enum AccountStatus
    {
        Active,
        Closed
    }
}
