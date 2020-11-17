using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;

namespace HistoryTracking.BL.Services
{
    public class OrderService : BaseService
    {
        public OrderService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<GetOrderModel>> GetList()
        {
            var users = await Storage.Orders.Select(
                    e =>
                        new GetOrderModel
                        {
                            Id = e.Id,
                            OrderStatus = e.OrderStatus,
                            PaymentStatus = e.PaymentStatus,
                            CustomerUserId = e.CustomerUserId,
                            CustomerUserName = e.CustomerUser.Name,
                        })
                .ToListAsync();

            return users;
        }
    }
}
