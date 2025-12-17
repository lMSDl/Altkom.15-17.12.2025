using Grpc.Core;
using GrpcService.Protos.StreamsProto;
using System.Text;

namespace GrpcService.Services
{
    public class StreamService : GrpcService.Protos.StreamsProto.GrpcStream.GrpcStreamBase
    {
        public override async Task FromServer(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < int.MaxValue && !context.CancellationToken.IsCancellationRequested; i++)
            {
                await responseStream.WriteAsync(new Response { Message = $"Message {i} from server: {request.Message}" });
                await Task.Delay(1000, context.CancellationToken);
            }

        }


        public override async Task<Response> FromClient(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {

            var response = new Response();
            StringBuilder stringBuilder = new StringBuilder();

            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                stringBuilder.AppendLine(request.Message);
            }

            response.Message = $"Messages received from client had {stringBuilder.Length} characters in total:\n" + stringBuilder.ToString();
            return response;
        }


        public override async Task Bidirectional(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            int counter = 0;
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                if (counter++ % 3 == 0)
                {
                    var response = new Response { Message = $"Echo from server: {request.Message}" };
                    await responseStream.WriteAsync(response);
                }
            }
        }
    }
}
