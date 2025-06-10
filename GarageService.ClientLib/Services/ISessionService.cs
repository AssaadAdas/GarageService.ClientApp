using GarageService.ClientLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Services
{
    public interface ISessionService
    {
        bool IsLoggedIn { get; }
        int UserId { get; }
        int UserType { get; }
        int ProfileId { get; } // ClientId or GarageId
        string Username { get; }

        void CreateSession(User user, ClientProfile clientProfile = null);
        void ClearSession();
    }
}
