using System.Net;

namespace TodoMinimalAPI.Models.Responses
{
    public class APIResponse
    {
        public APIResponse()
        {
            Success = true;
            Errors = new List<string>();
            StatusCode = HttpStatusCode.OK;
        }


        public bool Success { get; set; }

        public object? Result { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public List<string>? Errors { get; set; }
    }
}
