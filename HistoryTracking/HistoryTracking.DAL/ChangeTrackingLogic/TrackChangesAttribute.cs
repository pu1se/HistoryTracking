using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TrackChangesAttribute : Attribute
    {
    }
}
