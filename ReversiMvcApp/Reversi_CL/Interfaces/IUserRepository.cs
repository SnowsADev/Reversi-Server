﻿using Microsoft.AspNetCore.Identity;
using Reversi_CL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Reversi_CL.Interfaces
{
    public interface IUserRepository
    {
        //Create
        public Task CreateUserAsync(Speler user);

        //Read
        public Speler GetUser(string userId);
        public Task<Speler> GetUserAsync(string userId);
        public Task<Speler> GetUserAsync(ClaimsPrincipal user);

        //Update
        public Task<int> UpdateUserAsync(Speler user);
        public void UpdateUser(Speler user);

        //Delete
        public void DeleteUser(Speler user);
        public Task<int> DeleteUserAsync(Speler user);

        //List of users
        public List<Speler> GetUsersAsList();
        public Task<List<Speler>> GetUsersAsListAsync();
    }
}
