using Nancy;

namespace TaskmasterApp
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
                View["Index"];
        }

    }
}