using Bank.Domain.Models;

namespace Bank.Domain.Services
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }

    public interface IValidator
    {
        void AccountValidation(Account? account);
        void CustomerValidation(Customer? customer);
        void AmountValidation(decimal amount);
        void OpenAccountValidation(Bank.Domain.Models.OpenAccount openAccount, IQueryable<Account?> accounts);
        void DepositValidation(Account account, Bank.Domain.Models.Deposit deposit);
        void WithdrawalValidation(Account account, Bank.Domain.Models.Withdrawal withdrawal);
        void CloseAccountValidation(Account account, Bank.Domain.Models.CloseAccount closeAccount);
    }

    public class Validator : IValidator
    {

        private const decimal MIN_INITIAL_DEPOSIT = 100;
        private const decimal MIN_DEPOSIT = 0;
        private const decimal MIN_BALANCE_AFTER_WITHDRAWAL = 0;

        public void AccountValidation(Account? account)
        {
            if (account == null)
            {
                //Requirement: account must exist
                throw new CustomException($"The account does not exist");
            }
        }

        public void CustomerValidation(Customer? customer)
        {
            if (customer == null)
            {
                //Requirement: customer must exist
                throw new CustomException($"The customer does not exist");
            }
        }

        public void AmountValidation(decimal amount)
        {
            if (amount != Math.Round(amount, 2))
            {
                throw new CustomException($"The amount (${FormatNumber(amount)}) cannot have fractions of a cent");
            }
        }

        public void OpenAccountValidation(Bank.Domain.Models.OpenAccount openAccount, IQueryable<Account?> accounts)
        {
            if (openAccount.InitialDeposit < MIN_INITIAL_DEPOSIT)
            {
                //Requirement: initial depost must be at least 100
                throw new CustomException($"The initial deposit must be at least {FormatCurrency(MIN_INITIAL_DEPOSIT)}; initial deposit is {FormatCurrency(openAccount.InitialDeposit)}");
            }

            if (accounts != null && !accounts.ToList().Where(w => w.CustomerId == openAccount.CustomerId).Any() && openAccount.AccountType != AccountType.Savings)
            {
                //Requirement: first customer account must be savings
                throw new CustomException($"The first customer account must be a savings account; account requested is {openAccount.AccountType}");
            }

            AmountValidation(openAccount.InitialDeposit);
        }

        public void DepositValidation(Account account, Bank.Domain.Models.Deposit deposit)
        {
            if (account.CustomerId != deposit.CustomerId)
            {
                //Requirement: the account must belong to the customer
                throw new CustomException($"The account customer id {account.CustomerId} does not match the requested customer id {deposit.CustomerId}");
            }

            if (account.Id != deposit.AccountId)
            {
                //Requirement: the account must belong to the customer
                throw new CustomException($"The account id {account.Id} does not match the requested account id {deposit.AccountId}");
            }

            if (account.AccountStatus != AccountStatus.Active)
            {
                //Not Stated, but account status must be active
                throw new CustomException($"The account {deposit.AccountId} needs to be {AccountStatus.Active} but has status {account.AccountStatus}");
            }

            if (deposit.Amount < (MIN_DEPOSIT + (decimal)0.01))
            {
                //Requirement: the deposit amount must be greater than 0
                throw new CustomException($"The deposit must be greater than {FormatCurrency(MIN_DEPOSIT)}; requested deposit is {FormatCurrency(deposit.Amount)}");
            }

            AmountValidation(deposit.Amount);
        }

        public void WithdrawalValidation(Account account, Bank.Domain.Models.Withdrawal withdrawal)
        {
            if (account.CustomerId != withdrawal.CustomerId)
            {
                //Requirement: the account must belong to the customer
                throw new CustomException($"The account customer id {account.CustomerId} does not match the requested customer id {withdrawal.CustomerId}");
            }

            if (account.Id != withdrawal.AccountId)
            {
                //Requirement: the account must belong to the customer
                throw new CustomException($"The account id {account.Id} does not match the requested account id {withdrawal.AccountId}");
            }

            if (account.AccountStatus != AccountStatus.Active)
            {
                //Not Stated, but account status must be active
                throw new CustomException($"The account {withdrawal.AccountId} needs to be {AccountStatus.Active} but has status {account.AccountStatus}");
            }

            if (withdrawal.Amount <= 0)
            {
                //Requirement: the deposit amount must be greater than 0
                throw new CustomException($"The deposit must be > $0; requested deposit is {FormatCurrency(withdrawal.Amount)}");
            }

            if (account.Balance - withdrawal.Amount < MIN_BALANCE_AFTER_WITHDRAWAL)
            {
                //Requirement: the withdrawl cannot bring the balance below 0
                throw new CustomException($"The withdrawal cannot bring the balance below {FormatCurrency(MIN_BALANCE_AFTER_WITHDRAWAL)}. Available balance is {account.Balance}, amount requested is {FormatCurrency(withdrawal.Amount)}");
            }

            AmountValidation(withdrawal.Amount);
        }

        public void CloseAccountValidation(Account account, Bank.Domain.Models.CloseAccount closeAccount)
        {
            if (account.CustomerId != closeAccount.CustomerId)
            {
                //Requirement: the account must belong to the customer
                throw new CustomException($"The account customer id {account.CustomerId} does not match the requested customer id {closeAccount.CustomerId}");
            }

            if (account.Id != closeAccount.AccountId)
            {
                //Requirement: the account must belong to the customer
                throw new CustomException($"The account id {account.Id} does not match the requested account id {closeAccount.AccountId}");
            }

            if (account.AccountStatus != AccountStatus.Active)
            {
                //Not Stated, but account status must be active
                throw new CustomException($"The account {closeAccount.AccountId} needs to be {AccountStatus.Active} but has status {account.AccountStatus}");
            }

            if (account.Balance != 0)
            {
                //Requirement: the account can only be closed if the balance is exactly 0
                throw new CustomException($"The account balance must be $0 to close; balance is {FormatCurrency(account.Balance)}");
            }
        }

        private string FormatCurrency(decimal amt)
        {
            return amt.ToString("C2");
        }

        private string FormatNumber(decimal amt)
        {
            return amt.ToString("#,##0.############");
        }

    }
}
