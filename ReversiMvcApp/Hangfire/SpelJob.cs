using Microsoft.AspNetCore.SignalR;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using ReversiMvcApp.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Hangfire
{
    public class SpelJob : ISpelJob
    {
        private readonly ISpelRepository _spelAccessLayer;
        private readonly IHubContext<SpelHub> _hubContext;

        private readonly int maxAFKDuration = 5;

        public SpelJob(ISpelRepository spelAccessLayer, IHubContext<SpelHub> hubContext)
        {
            _spelAccessLayer = spelAccessLayer;
            _hubContext = hubContext;
        }

        public async Task UpdateAFKGames()
        {
            //Every game that has been updated more than 5min ago.
            List<Spel> spellen = _spelAccessLayer.GetLopendeSpellenAsList()
                .Where((x) => x.LastUpdated < DateTime.Now.AddMinutes(-maxAFKDuration))
                .ToList();

            foreach (Spel spel in spellen)
            {
                if (spel.Spelers.Count == 2)
                {
                    foreach (Speler speler in spel.Spelers)
                    {
                        //This tells the client it needs to refresh the game, making an API call so when a player is AFK it'll end the game automatically.
                        await _hubContext.Clients.User(speler.Id).SendAsync("ReceiveRefreshGameNotification");
                    }
                }
            }

        }
    }
}
