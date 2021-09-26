using GenerateSuccess.Models;
using GenerateSuccess.Services;
using GenerateSuccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userservice)
        {
            _userService = userservice;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult UserNameVal(string UserName)
        {
            if (_userService.UserNameExistVal(UserName))
            {
                return Json($"A user named {UserName} already exists.");
            }

            if (!_userService.UserNameCharVal(UserName))
            {
                return Json($"This field only accepts letters and numbers!");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult PasswordVal(string Password)
        {
            if (!_userService.PasswordAZ(Password))
            {
                return Json($"The password must contain at least one capital letter (A,B,C,D...)!");
            }

            if (!_userService.PasswordNum(Password))
            {
                return Json($"The password must contain at least one number (1,2,3,4...)!");
            }

            if (!_userService.PasswordSpec(Password))
            {
                return Json($"The password must contain at least one special character (!,#,$,%,&,/,(,),?,=)!");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult PasswordConfirmVal(string ConfirmPassword)
        {
            if (!_userService.PasswordAZ(ConfirmPassword))
            {
                return Json($"The password must contain at least one capital letter (A,B,C,D...)!");
            }

            if (!_userService.PasswordNum(ConfirmPassword))
            {
                return Json($"The password must contain at least one number (1,2,3,4...)!");
            }

            if (!_userService.PasswordSpec(ConfirmPassword))
            {
                return Json($"The password must contain at least one special character (!,#,$,%,&,/,(,),?,=)!");
            }

            return Json(true);
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool success = false;
                    success = _userService.Register(model).Result;

                    if (success)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Register");
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginVM details = new LoginVM
            {
                Uncorrectdetails = ""
            };
            return View(details);
        }

        [HttpPost]
        public IActionResult Login(LoginVM user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = _userService.Login(user).Result;

                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                LoginVM wrong = new LoginVM
                {
                    UserName = user.UserName,
                    Uncorrectdetails = "Login details are not correct!",
                    Password = user.Password,
                    RememberMe = user.RememberMe
                };
                return View(wrong);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _userService.LoginOut();

            return Redirect("/Home/Index");
        }
    }
}
