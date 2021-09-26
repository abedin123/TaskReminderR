using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenerateSuccess.AppDBContext;
using GenerateSuccess.Models;
using GenerateSuccess.Services;
using GenerateSuccess.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GenerateSuccess.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserService(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public bool UserNameCharVal(string username)
        {
            for (int i = 0; i < username.Length; i++)
            {
                if ((username[i] < 'a' || username[i] > 'z') && (username[i] < 'A' || username[i] > 'Z') && (username[i] < '0' || username[i] > '9') && username[i]!= ' ')
                    return false;
            }
            return true;
        }
        public bool UserNameExistVal(string username)
        {
            User user = null;
            user = _context.User.Where(a => a.UserName.ToLower() == username.ToLower()).FirstOrDefault();

            if (user != null)
                return true;

            return false;
        }
        public bool PasswordAZ(string Password)
        {
            foreach (var item in Password)
            {
                if (item >= 'A' && item <= 'Z')
                    return true;
            }
            return false;
        }
        public bool PasswordNum(string Password)
        {
            foreach (var item in Password)
            {
                if (item >= '0' && item <= '9')
                    return true;
            }
            return false;
        }
        public bool PasswordSpec(string Password)
        {
            foreach (var item in Password)
            {
                if (item == '!' || item == '#' || item == '$' || item == '%' || item == '&' || item == '/' || item == '(' || item == ')' || item == '!' || item == '?' || item == '=')
                    return true;
            }
            return false;
        }

        public async Task<bool> Register(RegisterVM model)
        {
            var user = new User
            {
                UserName = model.UserName
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return true;
            }
            return false;
        }

        public async Task<bool> Login(LoginVM user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, false);

            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async void LoginOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
