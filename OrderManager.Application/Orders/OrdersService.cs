using Microsoft.Extensions.Logging;
using OrderManager.Application.Common;
using OrderManager.Application.Extensions;
using OrderManager.Application.Orders.Queries;
using OrderManager.Core.DbContext;
using OrderManager.Core.Models;

namespace OrderManager.Application.Orders;

public class OrdersService
{
    private readonly OrderManagerDbContext _context;
    private readonly ILogger<OrdersService> _logger;
    private readonly IUser _user;

    public OrdersService(OrderManagerDbContext context, IUser user, ILogger<OrdersService> logger)
    {
        _context = context;
        _user = user;
        _logger = logger;
    }

    public async Task<PaginatedList<OrderDto>> GetOrders(OrdersWithPaginationQuery query)
    {
        return await _context.Orders
            // JAG not sure the intention since that method returns hardcoded false
            // needs further investigation -> .Where(order => order.IsSystemGenerated())
            .Select(OrderDto.FromOrder)
            .OrderByDescending(x => x.Id)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize);
    }

    public async Task<int?> CreateOrder(CreateOrderCommand command)
    {
        var newOrder = new Order
        {
            Description = command.Description,
            CreatedAt = command.CreatedAt,
            // JAG
            //TODO:  CreatedBy cannot be null, further investigation is required to implement this logic.
            CreatedBy = "NA" //null 
        };
        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();
        return newOrder.Id;
    }

    public async Task DeleteOrder(int id)
    {
        _logger.LogInformation($"User [{_user.Name}] is deleting an order");
        var order = await _context.Orders.FindAsync(id);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}