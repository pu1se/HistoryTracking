using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.DAL;

namespace HistoryTracking.BL.Services.SubscriptionProducts
{
    public class SubscriptionProductService : BaseService
    {
        public SubscriptionProductService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<GetSubscriptionProductModel>> GetList()
        {
            var users = await Storage.SubscriptionProducts.Select(
                    e =>
                        new GetSubscriptionProductModel
                        {
                            Id = e.Id,
                            Price = e.Price,
                            Currency = e.Currency
                        })
                .ToListAsync();

            return users;
        }
    }
}
