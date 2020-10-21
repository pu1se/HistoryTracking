using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL;

namespace HistoryTracking.BL
{
    public abstract class BaseService
    {
        protected DataContext Storage { get; }
        protected BaseService(DataContext storage)
        {
            Storage = storage;
        }
    }
}
