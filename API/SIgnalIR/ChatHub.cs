
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SIgnalIR
{
    // habilitar o hub para ser usado em 
    // applicationServiceExtension com services.AddSignalR();
    // adiconar as rotas do signalR hub em program:
    // app.MapHub<ChatHub>("/chat");
    // configurar para pegar o token com o signalR em IdentityServiceExtensions
    // 
    public class ChatHub: Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;            
        }

        public async Task SendComment(Create.Command command)
        {
            var comment = await _mediator.Send(command);

            //quando receber comentarios essa funçao e executada
            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);            
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var ActivityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, ActivityId);
            var result = await _mediator.Send(new List.Query{ActivityId = Guid.Parse(ActivityId)});
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}