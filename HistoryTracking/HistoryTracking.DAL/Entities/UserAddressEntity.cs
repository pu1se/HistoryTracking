using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.Entities
{
    [Table("UserAddresses")]
    public class UserAddressEntity : BaseEntity
    {
        public string City { get; set; }
        public string HouseAddress { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}
