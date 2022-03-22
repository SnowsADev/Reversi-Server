using Microsoft.AspNetCore.Identity;
using Reversi_CL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reversi_CL.Interfaces
{
    public interface IUserRepository<T> where T : IdentityUser
    {
        //Create
        public Task CreateUserAsync(T user);

        //Read
        public Speler GetUser(string userId);
        public Task<Speler> GetUserAsync(string userId);
        
        //Update
        public Task<int> UpdateUserAsync(T user);
        public void UpdateUser(T user);

        //Delete
        public void DeleteUser(T user);
        public Task<int> DeleteUserAsync(T user);

        //List of users
        public List<Speler> GetUsersAsList();
        public Task<List<Speler>> GetUsersAsListAsync();
    }
}
