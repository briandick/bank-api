using Bank.Domain.Models;
using System.Transactions;

namespace Bank.Domain.Services
{

    public class AccountService
    {
        private readonly IRepository<Models.Account> _accountRepository;
        private readonly IRepository<Models.Customer> _customerRepository;
        private readonly IRepository<Models.Deposit> _depositRepository;
        private readonly IRepository<Models.Withdrawal> _withdrawalRepository;
        private readonly IValidator _validator;

        public AccountService(IRepository<Models.Account> accountRepository,
            IRepository<Customer> customerRepository,
            IRepository<Deposit> depositRepository,
            IRepository<Withdrawal> withdrawalRepository,
            IValidator validator)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _depositRepository = depositRepository;
            _withdrawalRepository = withdrawalRepository;
            _validator = validator;
        }

        public async void Initialize()
        {
            var newCustomer = new Customer()
            {
                Id = 5,
                FirstName = "Test",
                LastName = "User"
            };
            await _customerRepository.Add(newCustomer);

            var newAccount = new Account()
            {
                Id = 17,
                CustomerId = 5,
                AccountType = AccountType.Checking,
                AccountStatus = AccountStatus.Active,
                Balance = (decimal)3289.89
            };
            await _accountRepository.Add(newAccount);
        }

        public async Task<Account?> OpenAccount(Bank.Domain.Models.OpenAccount openAccount)
        {
            var customer = await _customerRepository.GetById(openAccount.CustomerId);
            _validator.CustomerValidation(customer);

            var accounts = _accountRepository.GetAll();
            _validator.OpenAccountValidation(openAccount, accounts);

            var newAccount = new Account()
            {
                CustomerId = openAccount.CustomerId,
                AccountType = openAccount.AccountType,
                Balance = openAccount.InitialDeposit
            };
            await _accountRepository.Update(newAccount);

            return newAccount;
        }

        public async Task<Account?> MakeDeposit(Bank.Domain.Models.Deposit deposit)
        {
            using (TransactionScope transactionScope = new())
            {
                var account = await _accountRepository.GetById(deposit.AccountId);
                _validator.AccountValidation(account);
                _validator.DepositValidation(account!, deposit);

                //Requirement: the balance returned should reflect the current balance after the operation
                account!.Balance += deposit.Amount;

                await _accountRepository.Update(account);
                await _depositRepository.Add(deposit);

                // If everything is successful, complete the transaction
                transactionScope.Complete();

                return account;
            }
        }

        public async Task<Account?> MakeWithdrawal(Bank.Domain.Models.Withdrawal withdrawal)
        {
            using (TransactionScope transactionScope = new())
            {
                var account = await _accountRepository.GetById(withdrawal.AccountId);
                _validator.AccountValidation(account);
                _validator.WithdrawalValidation(account!, withdrawal);

                //Requirement: the balance returned should reflect the current balance after the operation
                account!.Balance -= withdrawal.Amount;

                await _accountRepository.Update(account);
                await _withdrawalRepository.Add(withdrawal);

                // If everything is successful, complete the transaction
                transactionScope.Complete();

                return account;
            }
        }

        public async Task<Account?> CloseAccount(Bank.Domain.Models.CloseAccount closeAccount)
        {
            var account = await _accountRepository.GetById(closeAccount.AccountId);
            _validator.AccountValidation(account);
            _validator.CloseAccountValidation(account!, closeAccount);

            account!.AccountStatus = AccountStatus.Closed;
            await _accountRepository.Update(account);
            return account;
        }

    }
}