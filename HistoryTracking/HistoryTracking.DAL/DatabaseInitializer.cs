using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL
{
    public static class TestData
    {
        public static readonly Guid SystemUserId = new Guid("11000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid ResellerUserId = new Guid("22000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid CustomerUserId = new Guid("33000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid AnotherCustomerUserId = new Guid("44000000-0000-0000-0000-AAAAAAAAAAAA");

        public static readonly Guid OfferId = new Guid("66000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid OrderId = new Guid("77000000-0000-0000-0000-AAAAAAAAAAAA");
    }

    public static class DatabaseInitializer
    {
        public static void SeedWithTestData(DataContext storage)
        {
            if (storage.Users.Any(user => user.Id == TestData.SystemUserId))
            {
                return;
            }

            storage.Users.Add(new UserEntity
            {
                Id = TestData.SystemUserId,
                Email = "system.user@appxite.com",
                Name = "System User",
                UserType = UserType.SystemUser
            });
            storage.Users.Add(new UserEntity
            {
                Id = TestData.ResellerUserId,
                Email = "reseller@tmp.com",
                Name = "Some Reseller",
                UserType = UserType.Reseller
            });
            storage.Users.Add(new UserEntity
            {
                Id = TestData.CustomerUserId,
                Email = "customer@tmp.com",
                Name = "Some Customer",
                UserType = UserType.Customer
            });
            storage.Users.Add(new UserEntity
            {
                Id = TestData.AnotherCustomerUserId,
                Email = "another.customer@tmp.com",
                Name = "Another Customer",
                UserType = UserType.Customer
            });

            var offer = storage.Offers.Add(new OfferEntity
            {
                Id = TestData.OfferId,
                Currency = CurrencyType.Euro,
                Price = 99,
                Title = "Super Offer",
            });
            storage.Orders.Add(new OrderEntity
            {
                Id = TestData.OrderId,
                Comments = "Call me before delivering",
                OrderStatus = OrderStatusType.New,
                PaymentStatus = PaymentStatusType.NotPaid,
                Offers = new List<OfferEntity>(new []{offer})
            });

            storage.SaveChanges();
        }
    }
}
