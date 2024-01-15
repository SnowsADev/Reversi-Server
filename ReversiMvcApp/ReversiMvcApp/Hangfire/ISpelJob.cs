using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Hangfire
{
    public interface ISpelJob
    {
        public Task UpdateAFKGames();
    }
}
