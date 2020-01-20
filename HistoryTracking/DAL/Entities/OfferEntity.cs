using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    public enum CurrencyType
    {
        Usd,
        Euro,
        Nok
    }

    public class OfferEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public CurrencyType Currency { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Guid CreatedByUserId { get; set; }

        public Guid UpdatedByUserId { get; set; }
    }
}
