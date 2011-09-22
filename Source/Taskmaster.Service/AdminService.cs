using System;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service
{
    public class AdminService : IAdminService
    {
        private IEventStorage _eventStorage;
        private IEventBus _bus;

        public AdminService(IEventStorage eventStorage, IEventBus bus)
        {
            _eventStorage = eventStorage;
            _bus = bus;
        }

        public void ReplayAllEvents()
        {
            var events = _eventStorage.GetEventsSince(DateTime.MinValue);
            foreach(var evt in events)
            {
                _bus.Publish(evt);
            }
        }
    }
}