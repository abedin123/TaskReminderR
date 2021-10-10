using GenerateSuccess.Models;
using GenerateSuccess.Services;
using GenerateSuccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        private string _currentLanguage;
        private IHttpContextAccessor _httpContextAccessor;

        public IActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;

            if (lang != "en-US" && lang != "ja-JP" && lang != "th-TH" && lang != "pt-BR" && lang != "vi-VN" && lang != "uk-UA")
            {
                lang = "en-US";
            }


            return RedirectToAction("Index", new { lang = lang });
        }

        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                {
                    return _currentLanguage;
                }

                return _currentLanguage;
            }
        }

        public AccountController(IUserService userservice, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userservice;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult UserNameVal(string UserName)
        {
            _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            if (_userService.UserNameExistVal(UserName))
            {
                if (_currentLanguage == "en-US")
                {
                    return Json($"A username {UserName} already exists.");
                }
                if (_currentLanguage == "ja-JP")
                {
                    return Json($"ユーザー名 {UserName} もう存在している.");
                }
                if (_currentLanguage == "th-TH")
                {
                    return Json($"ชื่อผู้ใช้ {UserName} มีอยู่แล้ว.");
                }
                if (_currentLanguage == "pt-BR")
                {
                    return Json($"Um nome de usuário {UserName} já existe.");
                }
                if (_currentLanguage == "vi-VN")
                {
                    return Json($"Một tên người dùng {UserName} đã tồn tại.");
                }
                if (_currentLanguage == "uk-UA")
                {
                    return Json($"Ім'я користувача {UserName} вже існує.");
                }
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
                _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
                string uncorr = "";
                if (_currentLanguage == "en-US")
                {
                    uncorr = "Login details are not correct!";
                }
                if (_currentLanguage == "ja-JP")
                {
                    uncorr = "ログインの詳細が正しくありません！";
                }
                if (_currentLanguage == "th-TH")
                {
                    return Json($"รายละเอียดการเข้าสู่ระบบไม่ถูกต้อง!");
                }
                if (_currentLanguage == "pt-BR")
                {
                    return Json($"Os detalhes de login não estão corretos!");
                }
                if (_currentLanguage == "vi-VN")
                {
                    return Json($"Chi tiết đăng nhập không chính xác!");
                }
                if (_currentLanguage == "uk-UA")
                {
                    return Json($"Дані для входу неправильні!");
                }
                LoginVM wrong = new LoginVM
                {
                    UserName = user.UserName,
                    Uncorrectdetails = uncorr,
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

            return RedirectToAction("Index", "Home");
        }
    }
}
