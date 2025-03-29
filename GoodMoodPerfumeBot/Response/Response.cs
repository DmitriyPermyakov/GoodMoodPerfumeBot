using System.Net;

namespace GoodMoodPerfumeBot.ServerResponse
{
    public class Response
    {
        public HttpStatusCode Status { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public List<string> Errors { get; set; } = new List<string>();
        public Object Result { get; set; } = new object();
    }
}
