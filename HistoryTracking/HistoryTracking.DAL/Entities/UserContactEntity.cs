using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.Entities
{
    [Table("UserContacts")]
    public class UserContactEntity : BaseEntity
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}
