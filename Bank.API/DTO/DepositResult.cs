namespace Bank.API.DTO
{
    public class DepositResult
    {
        public int Customerid { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public bool Succeeded { get; set; }
    }
}
