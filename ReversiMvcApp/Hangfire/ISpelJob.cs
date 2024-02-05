using System.Threading.Tasks;

namespace ReversiMvcApp.Hangfire
{
    public interface ISpelJob
    {
        public Task UpdateAFKGames();
    }
}
