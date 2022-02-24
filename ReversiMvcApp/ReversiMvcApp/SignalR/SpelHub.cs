using Microsoft.AspNetCore.SignalR;
using Reversi_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi_CL.Extensions;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Data.ReversiDbContext;
using Reversi_CL.Data.ReversiDbIdentityContext;

namespace ReversiMvcApp.SignalR
{
    public class SpelHub : Hub
    {
        private readonly ReversiDbContext _reversiDbContext;
        private readonly ReversiDbIdentityContext _reversiDbIdentityContext;

        public SpelHub(ReversiDbContext reversiDbContext, ReversiDbIdentityContext reversiDbIdentityContext) : base()
        {
            _reversiDbContext = reversiDbContext;
            _reversiDbIdentityContext = reversiDbIdentityContext;
        }


        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendJoinRequest(string spelID, string spelerId)
        {
            Spel spel = _reversiDbContext.Spellen
                .Include(spel => spel.Spelers)
                .FirstOrDefault(x => x.ID == spelID);

            Speler speler = await _reversiDbIdentityContext.Spelers.FirstOrDefaultAsync(s => s.Id == spelerId);

            string message = $"{speler.Naam.FirstCharToUpper()} wil graag deelnemen aan je spel. Ga akkoord om door te gaan.";
            Speler target = spel.Spelers.FirstOrDefault();

            if (spel == null || target == null)
            {
                await SendErrorPopup(spelerId);
                return;
            }

            await Clients.User(target.Id).SendAsync("ReceiveJoinRequest", message, spelID, spelerId);
        }

        public async Task SendJoinRequestResult(string spelId, string spelerId, bool isAccepted)
        {
            Spel spel = _reversiDbContext.Spellen
                .Include(spel => spel.Spelers)
                .FirstOrDefault(x => x.ID == spelId);

            // Fail if game is not found
            if (spel == null)
            {
                await SendErrorPopup(spelerId);
                return;
            }

            // Fail join request if denied
            if (!isAccepted)
            {
                await SendErrorPopup(spelerId, $"U bent geweigerd voor het spel van {spel.Spelers.FirstOrDefault().Naam.FirstCharToUpper()}");
                return;
            }

            await Clients.User(spelerId).SendAsync("ReceiveJoinRequestResult", $"{{ \"success\": true, \"spelID\": \"{spelId}\"}}");
        }

        public async Task SendErrorPopup(string targetUser, string message = "Er is iets fout gegaan. Probeer het later opnieuw.")
        {
            await Clients.User(targetUser).SendAsync("ReceiveErrorMessage", message);
        }
    }
}
