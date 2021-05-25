using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.BL.Services.Changes.Models
{
    public class GetChangesListModel
    {
        public List<string> EntityNames { get; set; } = new List<string>();
        public List<Guid> UserIds { get; set; } = new List<Guid>();
        public int? TakeHistoryForLastNumberOfDays { get; set; }

    }
}
