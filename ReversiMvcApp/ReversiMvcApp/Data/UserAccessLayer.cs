using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ReversiMvcApp.Data.Context;
using ReversiMvcApp.Models;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using System.Security.Principal;

namespace ReversiMvcApp.Data
{
    public class UserAccessLayer : IUserRepository
    {
        private readonly ReversiDbIdentityContext _identityContext;
        private readonly UserManager<Speler> _userManager;

        public UserAccessLayer(ReversiDbIdentityContext identityContext, UserManager<Speler> userManager)        
        {
            _identityContext = identityContext;
            _userManager = userManager;
        }

        public async Task CreateUserAsync(Speler user)
        {

            _identityContext.Spelers.Add(user);
            await _identityContext.SaveChangesAsync();
            await _userManager.AddToRoleAsync(user, "Speler");
            
        }

        public void DeleteUser(Speler user)
        {
            user.IsEnabled = false;
            _identityContext.Update(user);
            _identityContext.SaveChanges();
        }

        public Task<int> DeleteUserAsync(Speler user)
        {
            user.IsEnabled = false;
            _identityContext.Update(user);
            
            return _identityContext.SaveChangesAsync();
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);
        }

        public Task<Speler> GetUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }


        public Speler GetUser(string userId)
        {
            return _identityContext.Spelers
                .Where(user => user.IsEnabled)
                .FirstOrDefault(user => user.Id == userId);
        }

        public Task<Speler> GetUserAsync(string userId)
        {
            return _identityContext.Spelers
                .Where(user => user.IsEnabled)
                .FirstOrDefaultAsync(user => user.Id == userId);
        }

        public List<Speler> GetUsersAsList()
        {
            return _identityContext.Spelers
                .Where(user => user.IsEnabled)
                .ToList();
        }

        public Task<List<Speler>> GetUsersAsListAsync()
        {
            return _identityContext.Spelers
                .Where(user => user.IsEnabled)
                .ToListAsync();
        }

        public Task<IList<Speler>> GetUsersWithRole(string roleName)
        {
            return _userManager.GetUsersInRoleAsync(roleName);
        }

        public void UpdateUser(Speler user)
        {
            _identityContext.Update(user);
            _identityContext.SaveChanges();
        }

        public Task<int> UpdateUserAsync(Speler user)
        {
            _identityContext.Update(user);
            return _identityContext.SaveChangesAsync();
        }

        public async Task Disable2FactorAuthentication(Speler user)
        {
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
        }
    }
}
