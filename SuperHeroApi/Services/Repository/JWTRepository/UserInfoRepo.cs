using Microsoft.AspNetCore.Mvc;
using SuperHeroApi.ViewModels;

namespace SuperHeroApi.Services.Repository.JWTRepository
{
    public class UserInfoRepo : IUserInfoRepo
    {
        private readonly SuperHeroDbContext _context;
        public UserInfoRepo(SuperHeroDbContext context)
        {
            _context = context;
        }

        public UserInfo GetUserInfo(string email)
        {
            var user =  _context.UserInfos.Where(x => x.UserName == email || x.Email == email).FirstOrDefault();

            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        public bool isExistingUser(string email, string userName)
        {
            var isValid = _context.UserInfos.Any(a => a.Email == email || a.UserName == userName);

            return isValid;
        }

        public  bool isUserValid(string email, string password)
        {
            var userDetials = _context.UserInfos.Where(x => x.UserName == email || x.Email == email).FirstOrDefault();

            bool isValid = false;

            if (userDetials != null && !BCrypt.Net.BCrypt.Verify(password, userDetials.Password))
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return  isValid;
        }

        public async Task<bool> UserRegistration(UserInfo info)
        {
            _context.UserInfos.Add(info);

            await _context.SaveChangesAsync();

            if (_context.UserInfos.Any(x => x.Email == info.Email))
            {
               return true;
            }
            else
            {
                return false;
            }
        }
    }
}
