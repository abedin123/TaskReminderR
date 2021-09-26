using GenerateSuccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.Services
{
    public interface IUserService
    {
        public bool UserNameCharVal(string username);
        public bool UserNameExistVal(string username);
        public bool PasswordAZ(string Password);
        public bool PasswordNum(string Password);
        public bool PasswordSpec(string Password);
        public Task<bool> Register(RegisterVM model);
        public Task<bool> Login(LoginVM model);
        public void LoginOut();
    }
}
