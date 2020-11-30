using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services.SubscriptionProducts
{
    public class SubscriptionService : BaseService
    {
        public SubscriptionService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<GetSubscriptionModel>> GetList()
        {
            var subscriptions = await Storage.SubscriptionProducts.Select(
                    e =>
                        new GetSubscriptionModel
                        {
                            Id = e.Id,
                            Title = e.Title,
                            Price = e.Price,
                            Currency = e.Currency,
                            DistributorMarkupAsPercent = e.DistributorMarkupAsPercent,
                            ResellerMarkupAsPercent = e.ResellerMarkupAsPercent,
                            TotalPrice = e.Price * (1+e.DistributorMarkupAsPercent/100) * (1+e.ResellerMarkupAsPercent/100)
                        })
                .ToListAsync();

            return subscriptions;
        }

        public async Task<GetSubscriptionModel> GetItem(Guid subscriptionId)
        {
            var list = await GetList();
            return list.FirstOrDefault(item => item.Id == subscriptionId);
        }

        public async Task AddEditSubscription(AddEditSubscriptionModel model)
        {
            if (model.Title.IsNullOrEmpty())
            {
                throw new ValidationException("Subscription Title is required.");
            }
            if (model.Price <= 0)
            {
                throw new ValidationException("Subscription Price should be more then zero.");
            }
            if (model.DistributorMarkupAsPercent < 0)
            {
                throw new ValidationException("Distributor markup should has positive value.");
            }
            if (model.ResellerMarkupAsPercent < 0)
            {
                throw new ValidationException("Reseller markup should has positive value.");
            }

            if (model.Id == Guid.Empty)
            {
                Storage.SubscriptionProducts.Add(new SubscriptionProductEntity
                {
                    Title = model.Title,
                    Price = model.Price,
                    Currency = model.Currency,
                    DistributorMarkupAsPercent = model.DistributorMarkupAsPercent,
                    ResellerMarkupAsPercent = model.ResellerMarkupAsPercent,
                });
            }
            else
            {
                var editingSubscription = await Storage.SubscriptionProducts.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (editingSubscription == null)
                {
                    return;
                }

                editingSubscription.Title = model.Title;
                editingSubscription.Price = model.Price;
                editingSubscription.Currency = model.Currency;
                editingSubscription.DistributorMarkupAsPercent = model.DistributorMarkupAsPercent;
                editingSubscription.ResellerMarkupAsPercent = model.ResellerMarkupAsPercent;
            }

            await Storage.SaveChangesAsync();
        }

        public List<CurrencyType> GetCurrencyTypes()
        {
            return EnumHelper.ToList<CurrencyType>();
        }
    }
}
