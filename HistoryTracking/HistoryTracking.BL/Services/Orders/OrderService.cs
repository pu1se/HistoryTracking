﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services
{
    public class OrderService : BaseService
    {
        public OrderService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<OrderModel>> GetList()
        {
            var users = await Storage.Orders.Select(
                    e =>
                        new OrderModel
                        {
                            Id = e.Id,
                            OrderStatus = e.OrderStatus,
                            OrderDate = e.CreatedDateUtc,
                            CustomerUser = new UserModel
                            {
                                Name = e.CustomerUser.Name,
                                Email = e.CustomerUser.Email,
                                Id = e.CustomerUserId,
                                UserType = e.CustomerUser.UserType
                            },
                            SubscriptionList = e.SubscriptionProducts.Select(s => new SubscriptionModel
                            {
                                Id = s.Id,
                                DistributorMarkupAsPercent = s.DistributorMarkupAsPercent,
                                ResellerMarkupAsPercent = s.ResellerMarkupAsPercent,
                                Currency = s.Currency,
                                Title = s.Title,
                                Price = s.Price
                            }).ToList()
                        })
                .ToListAsync();

            return users;
        }

        public async Task<OrderModel> GetItem(Guid orderId)
        {
            var list = await GetList();
            return list.FirstOrDefault(item => item.Id == orderId);
        }

        public async Task AddEditItem(AddEditOrderModel model)
        {
            if (!model.SubscriptionIdList.Any())
            {
                throw new ValidationException("At least one subscription should be selected.");
            }

            var subscriptionEntityList = await Storage.SubscriptionProducts
                .Where(e => model.SubscriptionIdList.Contains(e.Id))
                .ToListAsync();

            if (model.Id == Guid.Empty)
            {
                Storage.Orders.Add(new OrderEntity
                {
                    OrderStatus = model.OrderStatus,
                    CustomerUserId = model.CustomerId,
                    SubscriptionProducts = subscriptionEntityList
                });
            }
            else
            {
                var editingOrder = await Storage.Orders.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (editingOrder == null)
                {
                    return;
                }

                editingOrder.OrderStatus = model.OrderStatus;
            }

            await Storage.SaveChangesAsync();
        }

        public List<OrderStatusType> GetOrderStatusTypes()
        {
            return EnumHelper.ToList<OrderStatusType>();
        }
    }
}
