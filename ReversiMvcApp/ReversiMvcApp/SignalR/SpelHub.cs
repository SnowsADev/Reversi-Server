using Microsoft.AspNetCore.SignalR;
using Reversi_CL.Models;
using ReversiMvcApp.Data.ReversiDbContext;
using ReversiMvcApp.Data.ReversiDbIdentityContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi_CL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ReversiMvcApp.SignalR
{
    public class SpelHub : Hub
    {
        private readonly ReversiDbContext _context;

        public SpelHub(ReversiDbContext context) : base()
        {
            _context = context;
        }


        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendJoinRequest(string spelID, string user)
        {
            Spel spel = _context.Spellen
                .Include(spel => spel.Spelers)
                .FirstOrDefault(x => x.ID == int.Parse(spelID));

            // Fout melden als er geen spel is gevonden.
            if (spel == null)
            {
                await SendErrorPopup(user, "Er is iets fout gegaan. Probeer het later opnieuw.");
                return;
            }

            string message = $"{user.FirstCharToUpper()} wil graag deelnemen aan je spel. Ga akkoord om door te gaan.";
            string TargetID = spel.Spelers.FirstOrDefault().Id;

            if (TargetID == null)
            {
                await SendErrorPopup(user, "Er is iets fout gegaan. Probeer het later opnieuw.");
                return;
            }

            await SendErrorPopup(TargetID, "Er is iets fout gegaan. Probeer het later opnieuw.");
            await Clients.User(TargetID).SendAsync("ReceiveJoinRequest", message);
        }

        public async Task SendErrorPopup(string targetUser, string message)
        {
            await Clients.User(targetUser).SendAsync("ReceiveErrorMessage", message);
        }
    }
}
