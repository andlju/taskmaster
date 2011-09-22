using Nancy;
using Taskmaster.Service;

namespace Taskmaster.Web
{
    public class AdminApiModule : NancyModule
    {
        public AdminApiModule(IAdminService adminService) : base("/api/admin")
        {
            Get["/replay-all"] = parameters =>
                           {
                               adminService.ReplayAllEvents();
                               return "OK";
                           };
        }
    }
}