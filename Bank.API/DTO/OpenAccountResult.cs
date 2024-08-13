using Bank.Domain.Models;

namespace Bank.API.DTO
{
    public class OpenAccountResult
    {
        public int Customerid { get; set; }
        public int AccountId { get; set; }
        public AccountType AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public bool Succeeded { get; set; }
    }
}
