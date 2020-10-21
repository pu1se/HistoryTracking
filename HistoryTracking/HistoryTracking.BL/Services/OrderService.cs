using HistoryTracking.DAL;

namespace HistoryTracking.BL.Services
{
    public class OrderService : BaseService
    {
        public OrderService(DataContext storage) : base(storage)
        {
        }
    }
}
