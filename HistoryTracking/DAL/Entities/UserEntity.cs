using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DAL.Entities;

namespace DAL.Entities
{
    public enum UserType {
        Customer,
        Reseller,
        SystemUser
    }

    [Table("Users")]
    public class UserEntity : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }



        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
