using Nancy;

namespace Taskmaster.Web
{
    public static class ResponseExtensions
    {
        public static Response WithHeaders(this Response response, params dynamic[] headers)
        {
            foreach(var header in headers)
            {
                response.Headers[header.Header] = header.Value;
            }
            return response;
        }

        public static Response WithNoCache(this Response response)
        {
            return response.WithHeaders(new { Header = "Cache-Control", Value = "no-store" }, new { Header = "Pragma", Value = "no-cache" });
        }
        
    }
}