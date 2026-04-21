using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Exodust.Web.Hubs;

[Authorize]
public class GameHub : Hub
{
}
