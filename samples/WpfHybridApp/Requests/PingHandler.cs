using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WpfHybridApp.Requests
{
    public class PingHandler : IRequestHandler<Ping, string>
    {
        public Task<string> Handle(Ping request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Pong {request.Message}");
        }
    }
}