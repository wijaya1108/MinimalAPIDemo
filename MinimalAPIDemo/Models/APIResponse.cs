using System.Net;

namespace MinimalAPIDemo.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
            Success = false;
            StatusCode = HttpStatusCode.BadRequest;
        }

        public bool Success { get; set; }
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
