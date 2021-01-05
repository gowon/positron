using MediatR;

namespace WpfHybridApp.Requests
{
    public class Ping : IRequest<string>
    {
        public string Message { get; set; }
    }
}