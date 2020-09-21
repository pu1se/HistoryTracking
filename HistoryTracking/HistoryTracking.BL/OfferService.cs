using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL._Core;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Entities;

namespace HistoryTracking.BL
{
    public class OfferService : BaseService
    {
        public OfferService(DataContext storage) : base(storage)
        {
        }

        public Task<List<OfferEntity>> GetList()
        {
            return Storage.Offers.ToListAsync();
        }
    }
}
