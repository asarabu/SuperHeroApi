using Microsoft.AspNetCore.Mvc;
using SuperHeroApi.ViewModels;

namespace SuperHeroApi.Services.Repository.JWTRepository
{
    public interface IUserInfoRepo
    {
        bool isUserValid(string email, string password);

        UserInfo GetUserInfo(string email);

        bool isExistingUser(string email, string userName);

        Task<bool> UserRegistration(UserInfo info);

    }
}
