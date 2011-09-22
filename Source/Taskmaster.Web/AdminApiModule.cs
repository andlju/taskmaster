using Nancy;
using Taskmaster.Service;

namespace Taskmaster.Web
{
    public class AdminApiModule : NancyModule
    {
        public AdminApiModule(IAdminService adminService) : base("/api/admin")
        {
            Get["/"] = parameters =>
                           {
                               adminService.ReplayAllEvents();
                               return "OK";
                           };
        }
    }
}