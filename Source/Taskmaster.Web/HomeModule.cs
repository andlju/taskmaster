using Nancy;

namespace Taskmaster.Web
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
                View["Index"];

            Get["/Styles/{file}"] = x =>
            {
                return Response.AsCss("Styles/" + (string)x.file).WithNoCache();
            };

            Get["/Scripts/{file}"] = x =>
            {
                return Response.AsJs("Scripts/" + (string)x.file).WithNoCache();
            };

            Get["/Images/{file}"] = x =>
            {
                return Response.AsImage("Images/" + (string)x.file).WithNoCache();
            };
        }

    }
}