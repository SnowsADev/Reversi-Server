using Microsoft.AspNetCore.SignalR;
using ReversiMvcApp.Extensions;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.SignalR
{
    public class SpelHub : Hub, ISpelHub
    {
        private readonly ISpelRepository _spelAccessLayer;
        private readonly IUserRepository _userAccessLayer;

        public SpelHub(ISpelRepository spelRepository, IUserRepository userRepository) : base()
        {
            _spelAccessLayer = spelRepository;
            _userAccessLayer = userRepository;
        }

        public async Task SendRefreshGameNotification(Spel spel)
        {
            if (spel == null)
            {
                return;
            }

            foreach (Speler speler in spel.Spelers)
            {
                await sendRefreshNoticiationToUser(speler.Id);
            }
        }

        private Task sendRefreshNoticiationToUser(string userId)
        {
            return Clients.User(userId).SendAsync("ReceiveRefreshGameNotification");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendJoinRequest(string spelID, string spelerId)
        {
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(spelID);
            Speler speler = _userAccessLayer.GetUser(spelerId);

            string message = $"{speler.Naam.FirstCharToUpper()} wil graag deelnemen aan je spel. Ga akkoord om door te gaan.";
            Speler target = spel.Spelers.FirstOrDefault();

            if (spel == null || target == null)
            {
                await SendErrorPopup(spelerId);
                return;
            }

            await Clients.User(target.Id).SendAsync("ReceiveJoinRequest", message, spelID, spelerId);
        }

        public async Task SendJoinRequestResult(string spelId, string targetId, bool isAccepted)
        {
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(spelId);

            // Fail if game is not found
            if (spel == null)
            {
                await SendErrorPopup(targetId);
                return;
            }

            // Fail join request if denied
            if (!isAccepted)
            {
                await SendErrorPopup(targetId, $"U bent geweigerd voor het spel van {spel.Spelers.FirstOrDefault().Naam.FirstCharToUpper()}");
                return;
            }

            _spelAccessLayer.AddSpelerToSpel(spel, _userAccessLayer.GetUser(targetId));

            await Clients.User(targetId).SendAsync("ReceiveJoinRequestResult", $"{{ \"success\": true, \"spelID\": \"{spelId}\"}}");

            //Get player that is already in the game, and have him refresh
            Speler host = spel.Spelers.FirstOrDefault(speler => speler.Id != targetId);
            if (host != null)
            {
                await sendRefreshNoticiationToUser(host.Id);
            }
        }

        public async Task SendErrorPopup(string targetUser, string message = "Er is iets fout gegaan. Probeer het later opnieuw.")
        {
            await Clients.User(targetUser).SendAsync("ReceiveErrorMessage", message);
        }

    }
}
