using Bank.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        private readonly Bank.Domain.Services.AccountService _accountService;

        public AccountController(ILogger<AccountController> logger, Bank.Domain.Services.AccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost]
        [Route(nameof(Initialize))]
        public void Initialize()
        {
            _accountService.Initialize();
        }

        [HttpPut]
        [Route(nameof(MakeDeposit))]
        public async Task<IActionResult> MakeDeposit(Bank.Domain.Models.Deposit deposit)
        {
            Bank.Domain.Models.Account? updatedAccount;
            try
            {
                updatedAccount = await _accountService.MakeDeposit(deposit);
                if (updatedAccount == null)
                {
                    return NotFound(updatedAccount);
                }
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred: " + ex.Message });
            }

            return Ok(new API.DTO.DepositResult()
            {
                AccountId = deposit.AccountId,
                Customerid = deposit.CustomerId,
                Balance = updatedAccount.Balance,
                Succeeded = true
            });
        }

        [HttpPut]
        [Route(nameof(MakeWithdrawal))]
        public async Task<IActionResult> MakeWithdrawal(Bank.Domain.Models.Withdrawal withdrawal)
        {
            Bank.Domain.Models.Account? updatedAccount;
            try
            {
                updatedAccount = await _accountService.MakeWithdrawal(withdrawal);
                if (updatedAccount == null)
                {
                    return NotFound(updatedAccount);
                }
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred: " + ex.Message + " : " + ex.StackTrace });
            }

            return Ok(new API.DTO.WithdrawalResult()
            {
                AccountId = withdrawal.AccountId,
                Customerid = withdrawal.CustomerId,
                Balance = updatedAccount.Balance,
                Succeeded = true
            });
        }

        [HttpPut]
        [Route(nameof(CloseAccount))]
        public async Task<IActionResult> CloseAccount(Bank.Domain.Models.CloseAccount closeAccount)
        {
            Bank.Domain.Models.Account? closedAccount;
            try
            {
                closedAccount = await _accountService.CloseAccount(closeAccount);
                if (closedAccount == null)
                {
                    return NotFound(closedAccount);
                }
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred: " + ex.Message });
            }

            return Ok(new API.DTO.CloseAccountResult()
            {
                AccountId = closeAccount.AccountId,
                Customerid = closeAccount.CustomerId,
                Succeeded = true
            });
        }

        [HttpPost]
        [Route(nameof(OpenAccount))]
        public async Task<IActionResult> OpenAccount(Bank.Domain.Models.OpenAccount openAccount)
        {
            Bank.Domain.Models.Account? openedAccount;
            try
            {
                openedAccount = await _accountService.OpenAccount(openAccount);
                if (openedAccount == null)
                {
                    return NotFound(openedAccount);
                }
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred: " + ex.Message });
            }

            return Ok(new API.DTO.OpenAccountResult()
            {
                AccountId = openedAccount.Id,
                Customerid = openedAccount.CustomerId,
                AccountTypeId = openedAccount.AccountType,
                Balance = openedAccount.Balance,
                Succeeded = true
            });
        }


    }
}
