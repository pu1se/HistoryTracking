using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    [Table("OrdersToOffers")]
    public class OrderToOfferEntity
    {
        [Key]
        public Guid Id { get; set; }


        public Guid OrderId { get; set; }
    }
}
