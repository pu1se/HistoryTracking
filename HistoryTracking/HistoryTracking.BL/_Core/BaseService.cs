using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL;

namespace HistoryTracking.BL
{
    public abstract class BaseService : IDisposable
    {
        protected DataContext Storage { get; }
        protected BaseService(DataContext storage)
        {
            Storage = storage;
        }

        public void Dispose()
        {
            ReleaseResources();
            GC.SuppressFinalize(this);
        }

        ~BaseService()
        {
            ReleaseResources();
        }

        private bool _resourcesWasRelease = false;
        private void ReleaseResources()
        {
            if (_resourcesWasRelease)
                return;

            Storage.Dispose();
            
            _resourcesWasRelease = true;
        }
    }
}
