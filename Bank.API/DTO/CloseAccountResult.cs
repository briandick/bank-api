namespace Bank.API.DTO
{
    public class CloseAccountResult
    {
        public int Customerid { get; set; }
        public int AccountId { get; set; }
        public bool Succeeded { get; set; }
    }
}
