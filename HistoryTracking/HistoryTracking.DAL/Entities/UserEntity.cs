﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public virtual ICollection<SubscriptionProductEntity> SubscriptionProducts { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }

        public virtual ICollection<TrackedEntityChange> TrackEntityChanges { get; set; }

        public virtual ICollection<UserAddressEntity> Addresses { get; set; }

        public virtual ICollection<UserContactEntity> Contacts { get; set; }
    }
}
