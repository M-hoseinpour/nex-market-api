using market.Data.Repository;
using market.Exceptions;
using market.Models.Domain;
using market.Models.DTO.Financial;
using Microsoft.EntityFrameworkCore;

namespace market.Services;
public class FinancialTransactionService
{
    private readonly IRepository<FinancialTransaction> _financialRepository;
    private readonly IRepository<Order> _orderRepository;

    public FinancialTransactionService(
        IRepository<FinancialTransaction> financialRepository,
        IRepository<Order> orderRepository)
    {
        _financialRepository = financialRepository;
        _orderRepository = orderRepository;
    }

    public async Task PayOrder(PayOrderInput input, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.TableNoTracking.Include(x => x.OrderDetails).FirstOrDefaultAsync(o => o.Uuid == input.OrderUuid, cancellationToken: cancellationToken) ?? throw new NotFoundException("order not found");

        decimal payAmount = 0;
        foreach (var orderDetail in order.OrderDetails)
        {
            payAmount += orderDetail.Quantity * orderDetail.Price;
        }

        await _financialRepository.AddAsync(new FinancialTransaction { Amount = payAmount, FinancialTransactionType = Models.Enum.FinancialTransactionType.Order, Order = order, FinancialTransactionStatus = NEF.Models.Enums.FinancialTransactionStatus.Succeed }, cancellationToken: cancellationToken);
    }
}