using ReversiMvcApp.Models;
using System.Threading.Tasks;

namespace ReversiMvcApp.SignalR
{
    public interface ISpelHub
    {
        Task SendRefreshGameNotification(Spel spel);
        Task SendMessage(string user, string message);
        Task SendJoinRequest(string spelID, string spelerId);
        Task SendJoinRequestResult(string spelId, string spelerId, bool isAccepted);
        Task SendErrorPopup(string targetUser, string message);
    }
}
