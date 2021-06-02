using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;
using Newtonsoft.Json;

namespace HistoryTracking.DAL.Entities
{
    [Table("SubscriptionProducts")]
    public class SubscriptionProductEntity : BaseEntity
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal DistributorMarkupAsPercent { get; set; }

        public decimal ResellerMarkupAsPercent { get; set; }

        public CurrencyType Currency { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

        public virtual ICollection<UserEntity> OwnerUsers { get; set; }

        public string VendorSubscriptionId { get; set; }

        public string EntitlementId { get; set; }

        public string VendorOrderId { get; set; }

        public Guid OriginalOrderItemId { get; set; }

        /// <summary>
        /// Customer Azure Tenant Id.
        /// </summary>
        public Guid AzureTenantId { get; set; }

        public Guid OfferId { get; set; }

        public string Name { get; set; }

        public DateTime? LastUsageMeteringDateUtc { get; set; }

        public SubscriptionState SubscriptionState { get; set; }

        public DateTime StartDateUtc { get; set; }

        public DateTime EndDateUtc { get; set; }

        public decimal Quantity { get; set; }

        /// <summary>
        /// Reseller to Customer Invoice Contract Id.
        /// Contract (e => e.ConsumerId == subscription.CustomerId && e.ProviderId == customer.TenantId && e.ContractPurpose == ContractPurposeType.Invoice.
        /// </summary>
        public Guid InvoiceContractId { get; set; }

        /// <summary>
        /// Seem to be Subscription.AzureTenant.ContractId, but not true in all cases.
        /// </summary>
        public Guid VendorContractId { get; set; }

        public Guid? AccountId { get; set; }

        /// <summary>
        /// An identity of a customer who owns the subscription.
        /// </summary>
        public Guid CustomerId { get; set; }

        /////// <summary>
        /////// An identifier of a billing cycle to use.
        /////// <para>
        /////// Billing Cycle for the Subscription is defined during subscription creation based on the billing cycle from the Contract with Vendor.
        /////// </para>
        /////// <para>Contract Billing Cycle should not be possible to change through the UI.</para>
        /////// </summary>
        ////public Guid BillingCycleId { get; set; }

        public Guid? BundleId { get; set; }

        ////public OfferRuleFrequency BillingFrequency { get; set; }

        ////public Guid? FirstBillingPeriodId { get; set; }

        /////// <summary>
        /////// A billing cycle to use.
        /////// <para>
        /////// Billing Cycle for the Subscription is defined during subscription creation based on the billing cycle from the Contract with Vendor.
        /////// </para>
        /////// <para>Contract Billing Cycle should not be possible to change through the UI.</para>
        /////// </summary>
        ////public BillingCycle BillingCycle { get; set; }

        public string JsonData { get; set; }

        /// <summary>
        /// Minimum number in months Customer is committed to purchase offer for.
        /// It is applied upon reation from Offer and not changes since.
        /// </summary>
        public byte MinimumCommitmentMonths { get; set; }

        /// <summary>
        /// Some indicator for understanding if user can make changes for subscription.
        /// Is applicable if MinimumCommitmentMonths > 0. Otherwise should not be used.
        /// </summary>
        public decimal? InitialPurchaseValue { get; set; }

        public bool AutoRenewEnabled { get; set; }
    }

    public enum SubscriptionState
    {
        Default
    }
}
