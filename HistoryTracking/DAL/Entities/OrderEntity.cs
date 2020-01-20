using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    public enum OrderStatusType
    {
        Created,
        Paid,
        Delivered,
        Closed
    }

    [Table("Orders")]
    public class OrderEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }        

        public string Title { get; set; }

        public string Comments { get; set; }

        public OrderStatusType Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Guid CreatedByUserId { get; set; }

        public Guid UpdatedByUserId { get; set; }


    }
}
