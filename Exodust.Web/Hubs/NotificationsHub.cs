using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Exodust.Web.Hubs;

// Site notifications only (progress, account events).
// Real-time gameplay does NOT go here: it goes to the Go game server over WebSocket.
[Authorize]
public class NotificationsHub : Hub
{
}
