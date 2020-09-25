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
        public static readonly Guid DistributorUserId = new Guid("22000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid ResellerUserId = new Guid("33000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid CustomerUserId = new Guid("55000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid AnotherCustomerUserId = new Guid("66000000-0000-0000-0000-AAAAAAAAAAAA");

        public static readonly Guid SubscriptionProductId = new Guid("88000000-0000-0000-0000-AAAAAAAAAAAA");
        public static readonly Guid OrderId = new Guid("99000000-0000-0000-0000-AAAAAAAAAAAA");
    }

    public static class DatabaseInitializer
    {
        public static void SeedWithTestData(DataContext storage)
        {
            if (storage.Users.Any(user => user.Id == TestData.SystemUserId))
            {
                return;
            }

            var now = DateTime.UtcNow;
            storage.Users.Add(new UserEntity
            {
                Id = TestData.SystemUserId,
                Email = "system.user@appxite.com",
                Name = "System User",
                UserType = UserType.SystemUser,
                CreatedDate = now,
                UpdatedDate = now,
                CreatedByUserId = TestData.SystemUserId,
                UpdatedByUserId = TestData.SystemUserId
            });
            var distributor = storage.Users.Add(new UserEntity
            {
                Id = TestData.DistributorUserId,
                Email = "distributor@tmp.com",
                Name = "Some Distributor",
                UserType = UserType.Distributor,
                CreatedDate = now,
                UpdatedDate = now,
                CreatedByUserId = TestData.SystemUserId,
                UpdatedByUserId = TestData.SystemUserId
            });
            var reseller = storage.Users.Add(new UserEntity
            {
                Id = TestData.ResellerUserId,
                Email = "reseller@tmp.com",
                Name = "Some Reseller",
                UserType = UserType.Reseller,
                CreatedDate = now,
                UpdatedDate = now,
                CreatedByUserId = TestData.SystemUserId,
                UpdatedByUserId = TestData.SystemUserId
            });
            var customer = storage.Users.Add(new UserEntity
            {
                Id = TestData.CustomerUserId,
                Email = "customer@tmp.com",
                Name = "Some Customer",
                UserType = UserType.Customer,
                CreatedDate = now,
                UpdatedDate = now,
                CreatedByUserId = TestData.SystemUserId,
                UpdatedByUserId = TestData.SystemUserId
            });
            storage.Users.Add(new UserEntity
            {
                Id = TestData.AnotherCustomerUserId,
                Email = "another.customer@tmp.com",
                Name = "Another Customer",
                UserType = UserType.Customer,
                CreatedDate = now,
                UpdatedDate = now,
                CreatedByUserId = TestData.SystemUserId,
                UpdatedByUserId = TestData.SystemUserId
            });

            var offer = storage.SubscriptionProducts.Add(new SubscriptionProductEntity
            {
                Id = TestData.SubscriptionProductId,
                Currency = CurrencyType.Euro,
                Price = 99,
                DistributorMarkupAsPercent = 5,
                ResellerMarkupAsPercent = 10,
                Title = "Super Subscription Product",
                CreatedDate = now,
                UpdatedDate = now,
                CreatedByUserId = TestData.SystemUserId,
                UpdatedByUserId = TestData.SystemUserId,
                SubscriptionOwnerUsers = new List<UserEntity>(new []{distributor, reseller}),
            });
            storage.Orders.Add(new OrderEntity
            {
                Id = TestData.OrderId,
                Comments = "Call me before delivering",
                OrderStatus = OrderStatusType.New,
                PaymentStatus = PaymentStatusType.NotPaid,
                SubscriptionProducts = new List<SubscriptionProductEntity>(new []{offer}),
                CreatedDate = now,
                UpdatedDate = now,
                CreatedByUserId = TestData.SystemUserId,
                UpdatedByUserId = TestData.SystemUserId,
                CustomerUser = customer
            });

            storage.SaveChanges();
        }
    }
}
