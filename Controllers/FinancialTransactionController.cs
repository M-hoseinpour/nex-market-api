using market.Models.DTO.Financial;
using market.Services;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers.Client;

[ApiController]
[Route("api/financial-transactions")]
public class FinancialTransactionController : ControllerBase
{
    private readonly FinancialTransactionService _financialTransactionService;

    public FinancialTransactionController(FinancialTransactionService financialTransactionService)
    {
        _financialTransactionService = financialTransactionService;
    }

    [HttpPost]
    public async Task PayOrder(PayOrderInput input, CancellationToken cancellationToken)
    {
        await _financialTransactionService.PayOrder(input, cancellationToken);
    }
}