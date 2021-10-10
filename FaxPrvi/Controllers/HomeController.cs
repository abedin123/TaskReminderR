using FaxPrvi.Models;
using GenerateSuccess.Models;
using GenerateSuccess.Services;
using GenerateSuccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Http.Features;
using TimeZoneConverter;
using AspNetCore.SEOHelper.Sitemap;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Serialization;
using System.IO;
using Microsoft.AspNetCore.Localization;

namespace GenerateSuccess.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class HomeController : Controller
    {
        private List<TaskDB> TaskList=new List<TaskDB>();
        private List<Alarms> AlarmList= new List<Alarms>();
        private List<TaskPreviewVM> currenttasklist;
        private MainHomePreviewVM currentmainhome;
        private readonly ITaskService _taskservice;
        private readonly IAlarmService _alarmservice;
        private string _currentLanguage;
        
        private DateTime GetDateTimeObject(string CurrentTime)
        {
            int Month = int.Parse((CurrentTime[0].ToString() + CurrentTime[1].ToString()));
            int Day = int.Parse((CurrentTime[3].ToString() + CurrentTime[4].ToString()));
            int Year = int.Parse((CurrentTime[6].ToString() + CurrentTime[7].ToString() + CurrentTime[8].ToString() + CurrentTime[9].ToString()));
            int Hour = int.Parse((CurrentTime[11].ToString() + CurrentTime[12].ToString()));
            int Minute = int.Parse((CurrentTime[14].ToString() + CurrentTime[15].ToString()));
            
            DateTime CurrentRealTime = new DateTime(Year, Month, Day, Hour, Minute, 0);
            if (CurrentRealTime.ToShortDateString().Contains("2564") || CurrentRealTime.ToShortDateString().Contains("2565"))
            {
                CurrentRealTime = CurrentRealTime.AddYears(-543);
            }
            return CurrentRealTime;
        }

        public HomeController(ITaskService taskservice, IAlarmService alarmservice)
        {
            if (currentmainhome == null)
            {
                DateTime current = DateTime.Now.AddHours(2).AddMonths(-1);
                DateTime currentto = DateTime.Now.AddHours(2).AddMonths(1);
                currentmainhome = new MainHomePreviewVM
                {
                    TaskName = null,
                    AlarmStatus = "0",
                    F = "false",
                    From = current,
                    M = "false",
                    S = "false",
                    Sa = "false",
                    T = "false",
                    TaskStatus = null,
                    Th = "false",
                    To = currentto,
                    W = "false"
                };
            }
            _alarmservice = alarmservice;
            _taskservice = taskservice;
        }

        public IActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            
            if (lang != "en-US" && lang !="ja-JP"&&lang!= "th-TH" && lang != "pt-BR" && lang != "vi-VN" && lang != "uk-UA")
            {
                lang = "en-US";
            }


            return RedirectToAction("Index", new {lang=lang});
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

        [HttpGet]
        public IActionResult Index(MainHomePreviewVM ForSend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentCopyKey = null;

                    CurrentCopyKey = Request.Cookies["CopyRealKey"];

                    string CurrentKey = null;

                    CurrentKey = Request.Cookies["RealKey"];

                    if (CurrentCopyKey == null && CurrentKey!=null)
                    {
                        HttpContext.Session.Remove(CurrentKey);
                        HttpContext.Session.Remove(CurrentKey + "Object");
                    }

                    string CurrentZone = null;
                    CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime= DateTime.Now;
                    if (CurrentZone != null)
                    {
                        CurrentRealTime = GetDateTimeObject(CurrentZone);
                        _alarmservice.SetCurrentTime(CurrentRealTime);
                        _taskservice.SetCurrentTime(CurrentRealTime);
                    }

                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey+"Object")))
                    {
                        MainHomePreviewVM tempobject = JsonConvert.DeserializeObject<MainHomePreviewVM>(HttpContext.Session.GetString(CurrentKey + "Object"));
                        currentmainhome = tempobject;
                    }
                    else
                    {
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }

                    ForSend = currentmainhome;
                    HomeFilterVM filter = new HomeFilterVM
                    {
                        TaskName = currentmainhome.TaskName,
                        AlarmStatus = currentmainhome.AlarmStatus,
                        F = currentmainhome.F,
                        From = currentmainhome.From,
                        M = currentmainhome.M,
                        S = currentmainhome.S,
                        Sa = currentmainhome.Sa,
                        T = currentmainhome.T,
                        TaskStatus = currentmainhome.TaskStatus,
                        Th = currentmainhome.Th,
                        To = currentmainhome.To,
                        W = currentmainhome.W
                    };
                    if (filter.From.ToShortDateString().Contains("2564") || filter.From.ToShortDateString().Contains("2565"))
                    {
                        filter.From = filter.From.AddYears(-543);
                        filter.To = filter.To.AddYears(-543);
                    }
                    List<TaskPreviewVM> tasklist;
                    List<AlarmPreviewVM> alarmlist;
                    if (User.Identity.IsAuthenticated)
                    {
                        tasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        currenttasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        alarmlist = _alarmservice.GetListAlarms(User.Identity.Name, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    else
                    {
                        ForSend.TaskList = new List<TaskPreviewVM>();
                        ForSend.AlarmList = new List<AlarmPreviewVM>();
                        if (CurrentKey != null)
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
                            {
                                TempDataObject tempobj= JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                                if (tempobj.AlarmList != null)
                                {
                                    if (tempobj.AlarmList.Count > 0)
                                    {
                                        AlarmList = tempobj.AlarmList;
                                    }
                                }
                                if (tempobj.TaskList != null)
                                {
                                    if (tempobj.TaskList.Count > 0)
                                    {
                                        TaskList = tempobj.TaskList;
                                    }
                                }
                            }
                        }
                        tasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        currenttasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        alarmlist = _alarmservice.GetListAlarmsUnAuth(AlarmList, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    
                    ForSend.Success = _taskservice.SuccessNumber(currenttasklist);
                    MainHomePreviewVM forreturn = new MainHomePreviewVM();
                    forreturn = ForSend;
                    if(forreturn.From.ToShortDateString().Contains("2564")|| forreturn.From.ToShortDateString().Contains("2565"))
                    {
                        forreturn.From = forreturn.From.AddYears(-543);
                        forreturn.To = forreturn.To.AddYears(-543);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotification(User.Identity.Name);
                        notdiv.TaskList = _taskservice.ListTaskNotification(User.Identity.Name);

                        forreturn.MainNotification = notdiv;
                    }
                    else
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotificationUnAuth(AlarmList);
                        notdiv.TaskList = _taskservice.ChangeCurrentUnAuthList(TaskList);

                        forreturn.MainNotification = notdiv;
                    }
                    return View(forreturn);
                }
                else
                {
                    return View(currentmainhome);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Search(MainHomePreviewVM ForSend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentCopyKey = null;

                    CurrentCopyKey = Request.Cookies["CopyRealKey"];

                    string CurrentKey = null;

                    CurrentKey = Request.Cookies["RealKey"];

                    if (CurrentCopyKey == null && CurrentKey != null)
                    {
                        HttpContext.Session.Remove(CurrentKey);
                        HttpContext.Session.Remove(CurrentKey + "Object");
                    }

                    string CurrentZone = null;
                    CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = DateTime.Now;
                    if (CurrentZone != null)
                    {
                        CurrentRealTime = GetDateTimeObject(CurrentZone);
                        _alarmservice.SetCurrentTime(CurrentRealTime);
                        _taskservice.SetCurrentTime(CurrentRealTime);
                    }
                    currentmainhome.TaskName = ForSend.TaskName;
                    currentmainhome.TaskStatus = ForSend.TaskStatus;
                    currentmainhome.From = ForSend.From;
                    currentmainhome.To = ForSend.To;
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey + "Object")))
                    {
                        MainHomePreviewVM tempobject = JsonConvert.DeserializeObject<MainHomePreviewVM>(HttpContext.Session.GetString(CurrentKey + "Object"));
                        currentmainhome = tempobject;
                        currentmainhome.TaskName = ForSend.TaskName;
                        currentmainhome.TaskStatus = ForSend.TaskStatus;
                        currentmainhome.From = ForSend.From;
                        currentmainhome.To = ForSend.To;
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }
                    else
                    {
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }
                    
                    ForSend = currentmainhome;
                    HomeFilterVM filter = new HomeFilterVM
                    {
                        TaskName = currentmainhome.TaskName,
                        AlarmStatus = currentmainhome.AlarmStatus,
                        F = currentmainhome.F,
                        From = currentmainhome.From,
                        M = currentmainhome.M,
                        S = currentmainhome.S,
                        Sa = currentmainhome.Sa,
                        T = currentmainhome.T,
                        TaskStatus = currentmainhome.TaskStatus,
                        Th = currentmainhome.Th,
                        To = currentmainhome.To,
                        W = currentmainhome.W
                    };
                    if (filter.From.ToShortDateString().Contains("2564") || filter.From.ToShortDateString().Contains("2565"))
                    {
                        filter.From = filter.From.AddYears(-543);
                        filter.To = filter.To.AddYears(-543);
                    }
                    List<TaskPreviewVM> tasklist;
                    List<AlarmPreviewVM> alarmlist;
                    if (User.Identity.IsAuthenticated)
                    {
                        tasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        currenttasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        alarmlist = _alarmservice.GetListAlarms(User.Identity.Name, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    else
                    {
                        ForSend.TaskList = new List<TaskPreviewVM>();
                        ForSend.AlarmList = new List<AlarmPreviewVM>();
                        if (CurrentKey != null)
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
                            {
                                TempDataObject tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                                if (tempobj.AlarmList != null)
                                {
                                    if (tempobj.AlarmList.Count > 0)
                                    {
                                        AlarmList = tempobj.AlarmList;
                                    }
                                }
                                if (tempobj.TaskList != null)
                                {
                                    if (tempobj.TaskList.Count > 0)
                                    {
                                        TaskList = tempobj.TaskList;
                                    }
                                }
                            }
                        }
                        tasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        currenttasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        alarmlist = _alarmservice.GetListAlarmsUnAuth(AlarmList, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }

                    ForSend.Success = _taskservice.SuccessNumber(currenttasklist);
                    MainHomePreviewVM forreturn = new MainHomePreviewVM();
                    forreturn = ForSend;
                    if (forreturn.From.ToShortDateString().Contains("2564") || forreturn.From.ToShortDateString().Contains("2565"))
                    {
                        forreturn.From = forreturn.From.AddYears(-543);
                        forreturn.To = forreturn.To.AddYears(-543);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotification(User.Identity.Name);
                        notdiv.TaskList = _taskservice.ListTaskNotification(User.Identity.Name);

                        forreturn.MainNotification = notdiv;
                    }
                    else
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotificationUnAuth(AlarmList);
                        notdiv.TaskList = _taskservice.ChangeCurrentUnAuthList(TaskList);

                        forreturn.MainNotification = notdiv;
                    }

                    return View("Index", currentmainhome);
                }
                else
                {
                    return View("Index", currentmainhome);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult AlarmSearch(MainHomePreviewVM ForSend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentCopyKey = null;

                    CurrentCopyKey = Request.Cookies["CopyRealKey"];

                    string CurrentKey = null;

                    CurrentKey = Request.Cookies["RealKey"];

                    if (CurrentCopyKey == null && CurrentKey != null)
                    {
                        HttpContext.Session.Remove(CurrentKey);
                        HttpContext.Session.Remove(CurrentKey + "Object");
                    }

                    string CurrentZone = null;
                    CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = DateTime.Now;
                    if (CurrentZone != null)
                    {
                        CurrentRealTime = GetDateTimeObject(CurrentZone);
                        _alarmservice.SetCurrentTime(CurrentRealTime);
                        _taskservice.SetCurrentTime(CurrentRealTime);
                    }
                    currentmainhome.AlarmStatus = ForSend.AlarmStatus;
                    currentmainhome.F = ForSend.F;
                    currentmainhome.M = ForSend.M;
                    currentmainhome.S = ForSend.S;
                    currentmainhome.Sa = ForSend.Sa;
                    currentmainhome.T = ForSend.T;
                    currentmainhome.Th = ForSend.Th;
                    currentmainhome.W = ForSend.W;
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey + "Object")))
                    {
                        MainHomePreviewVM tempobject = JsonConvert.DeserializeObject<MainHomePreviewVM>(HttpContext.Session.GetString(CurrentKey + "Object"));
                        currentmainhome = tempobject;
                        currentmainhome.AlarmStatus = ForSend.AlarmStatus;
                        currentmainhome.F = ForSend.F;
                        currentmainhome.M = ForSend.M;
                        currentmainhome.S = ForSend.S;
                        currentmainhome.Sa = ForSend.Sa;
                        currentmainhome.T = ForSend.T;
                        currentmainhome.Th = ForSend.Th;
                        currentmainhome.W = ForSend.W;
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }
                    else
                    {
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }

                    ForSend = currentmainhome;
                    HomeFilterVM filter = new HomeFilterVM
                    {
                        TaskName = currentmainhome.TaskName,
                        AlarmStatus = currentmainhome.AlarmStatus,
                        F = currentmainhome.F,
                        From = currentmainhome.From,
                        M = currentmainhome.M,
                        S = currentmainhome.S,
                        Sa = currentmainhome.Sa,
                        T = currentmainhome.T,
                        TaskStatus = currentmainhome.TaskStatus,
                        Th = currentmainhome.Th,
                        To = currentmainhome.To,
                        W = currentmainhome.W
                    };
                    if (filter.From.ToShortDateString().Contains("2564") || filter.From.ToShortDateString().Contains("2565"))
                    {
                        filter.From = filter.From.AddYears(-543);
                        filter.To = filter.To.AddYears(-543);
                    }
                    List<TaskPreviewVM> tasklist;
                    List<AlarmPreviewVM> alarmlist;
                    if (User.Identity.IsAuthenticated)
                    {
                        tasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        currenttasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        alarmlist = _alarmservice.GetListAlarms(User.Identity.Name, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    else
                    {
                        ForSend.TaskList = new List<TaskPreviewVM>();
                        ForSend.AlarmList = new List<AlarmPreviewVM>();
                        if (CurrentKey != null)
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
                            {
                                TempDataObject tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                                if (tempobj.AlarmList != null)
                                {
                                    if (tempobj.AlarmList.Count > 0)
                                    {
                                        AlarmList = tempobj.AlarmList;
                                    }
                                }
                                if (tempobj.TaskList != null)
                                {
                                    if (tempobj.TaskList.Count > 0)
                                    {
                                        TaskList = tempobj.TaskList;
                                    }
                                }
                            }
                        }
                        tasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        currenttasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        alarmlist = _alarmservice.GetListAlarmsUnAuth(AlarmList, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }

                    ForSend.Success = _taskservice.SuccessNumber(currenttasklist);
                    MainHomePreviewVM forreturn = new MainHomePreviewVM();
                    forreturn = ForSend;
                    if (forreturn.From.ToShortDateString().Contains("2564") || forreturn.From.ToShortDateString().Contains("2565"))
                    {
                        forreturn.From = forreturn.From.AddYears(-543);
                        forreturn.To = forreturn.To.AddYears(-543);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotification(User.Identity.Name);
                        notdiv.TaskList = _taskservice.ListTaskNotification(User.Identity.Name);

                        forreturn.MainNotification = notdiv;
                    }
                    else
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotificationUnAuth(AlarmList);
                        notdiv.TaskList = _taskservice.ChangeCurrentUnAuthList(TaskList);

                        forreturn.MainNotification = notdiv;
                    }

                    return View("Index", currentmainhome);
                }
                else
                {
                    return View("Index", currentmainhome);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult AlarmSearchMobile(MainHomePreviewVM ForSend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentCopyKey = null;

                    CurrentCopyKey = Request.Cookies["CopyRealKey"];

                    string CurrentKey = null;

                    CurrentKey = Request.Cookies["RealKey"];

                    if (CurrentCopyKey == null && CurrentKey != null)
                    {
                        HttpContext.Session.Remove(CurrentKey);
                        HttpContext.Session.Remove(CurrentKey + "Object");
                    }

                    string CurrentZone = null;
                    CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = DateTime.Now;
                    if (CurrentZone != null)
                    {
                        CurrentRealTime = GetDateTimeObject(CurrentZone);
                        _alarmservice.SetCurrentTime(CurrentRealTime);
                        _taskservice.SetCurrentTime(CurrentRealTime);
                    }
                    currentmainhome.AlarmStatus = ForSend.AlarmStatus;
                    currentmainhome.F = ForSend.F;
                    currentmainhome.M = ForSend.M;
                    currentmainhome.S = ForSend.S;
                    currentmainhome.Sa = ForSend.Sa;
                    currentmainhome.T = ForSend.T;
                    currentmainhome.Th = ForSend.Th;
                    currentmainhome.W = ForSend.W;
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey + "Object")))
                    {
                        MainHomePreviewVM tempobject = JsonConvert.DeserializeObject<MainHomePreviewVM>(HttpContext.Session.GetString(CurrentKey + "Object"));
                        currentmainhome = tempobject;
                        currentmainhome.AlarmStatus = ForSend.AlarmStatus;
                        currentmainhome.F = ForSend.F;
                        currentmainhome.M = ForSend.M;
                        currentmainhome.S = ForSend.S;
                        currentmainhome.Sa = ForSend.Sa;
                        currentmainhome.T = ForSend.T;
                        currentmainhome.Th = ForSend.Th;
                        currentmainhome.W = ForSend.W;
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }
                    else
                    {
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }

                    ForSend = currentmainhome;
                    HomeFilterVM filter = new HomeFilterVM
                    {
                        TaskName = currentmainhome.TaskName,
                        AlarmStatus = currentmainhome.AlarmStatus,
                        F = currentmainhome.F,
                        From = currentmainhome.From,
                        M = currentmainhome.M,
                        S = currentmainhome.S,
                        Sa = currentmainhome.Sa,
                        T = currentmainhome.T,
                        TaskStatus = currentmainhome.TaskStatus,
                        Th = currentmainhome.Th,
                        To = currentmainhome.To,
                        W = currentmainhome.W
                    };
                    if (filter.From.ToShortDateString().Contains("2564") || filter.From.ToShortDateString().Contains("2565"))
                    {
                        filter.From = filter.From.AddYears(-543);
                        filter.To = filter.To.AddYears(-543);
                    }
                    List<TaskPreviewVM> tasklist;
                    List<AlarmPreviewVM> alarmlist;
                    if (User.Identity.IsAuthenticated)
                    {
                        tasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        currenttasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        alarmlist = _alarmservice.GetListAlarms(User.Identity.Name, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    else
                    {
                        ForSend.TaskList = new List<TaskPreviewVM>();
                        ForSend.AlarmList = new List<AlarmPreviewVM>();
                        if (CurrentKey != null)
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
                            {
                                TempDataObject tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                                if (tempobj.AlarmList != null)
                                {
                                    if (tempobj.AlarmList.Count > 0)
                                    {
                                        AlarmList = tempobj.AlarmList;
                                    }
                                }
                                if (tempobj.TaskList != null)
                                {
                                    if (tempobj.TaskList.Count > 0)
                                    {
                                        TaskList = tempobj.TaskList;
                                    }
                                }
                            }
                        }
                        tasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        currenttasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        alarmlist = _alarmservice.GetListAlarmsUnAuth(AlarmList, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }

                    ForSend.Success = _taskservice.SuccessNumber(currenttasklist);
                    MainHomePreviewVM forreturn = new MainHomePreviewVM();
                    forreturn = ForSend;
                    if (forreturn.From.ToShortDateString().Contains("2564") || forreturn.From.ToShortDateString().Contains("2565"))
                    {
                        forreturn.From = forreturn.From.AddYears(-543);
                        forreturn.To = forreturn.To.AddYears(-543);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotification(User.Identity.Name);
                        notdiv.TaskList = _taskservice.ListTaskNotification(User.Identity.Name);

                        forreturn.MainNotification = notdiv;
                    }
                    else
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotificationUnAuth(AlarmList);
                        notdiv.TaskList = _taskservice.ChangeCurrentUnAuthList(TaskList);

                        forreturn.MainNotification = notdiv;
                    }

                    return View("MobileAlarm", currentmainhome);
                }
                else
                {
                    return View("MobileAlarm", currentmainhome);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cookies()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MobileTask(MainHomePreviewVM ForSend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentCopyKey = null;

                    CurrentCopyKey = Request.Cookies["CopyRealKey"];

                    string CurrentKey = null;

                    CurrentKey = Request.Cookies["RealKey"];

                    if (CurrentCopyKey == null && CurrentKey != null)
                    {
                        HttpContext.Session.Remove(CurrentKey);
                        HttpContext.Session.Remove(CurrentKey + "Object");
                    }

                    string CurrentZone = null;
                    CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = DateTime.Now;
                    if (CurrentZone != null)
                    {
                        CurrentRealTime = GetDateTimeObject(CurrentZone);
                        _alarmservice.SetCurrentTime(CurrentRealTime);
                        _taskservice.SetCurrentTime(CurrentRealTime);
                    }

                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey + "Object")))
                    {
                        MainHomePreviewVM tempobject = JsonConvert.DeserializeObject<MainHomePreviewVM>(HttpContext.Session.GetString(CurrentKey + "Object"));
                        currentmainhome = tempobject;
                    }
                    else
                    {
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }

                    ForSend = currentmainhome;
                    HomeFilterVM filter = new HomeFilterVM
                    {
                        TaskName = currentmainhome.TaskName,
                        AlarmStatus = currentmainhome.AlarmStatus,
                        F = currentmainhome.F,
                        From = currentmainhome.From,
                        M = currentmainhome.M,
                        S = currentmainhome.S,
                        Sa = currentmainhome.Sa,
                        T = currentmainhome.T,
                        TaskStatus = currentmainhome.TaskStatus,
                        Th = currentmainhome.Th,
                        To = currentmainhome.To,
                        W = currentmainhome.W
                    };
                    if (filter.From.ToShortDateString().Contains("2564") || filter.From.ToShortDateString().Contains("2565"))
                    {
                        filter.From = filter.From.AddYears(-543);
                        filter.To = filter.To.AddYears(-543);
                    }
                    List<TaskPreviewVM> tasklist;
                    List<AlarmPreviewVM> alarmlist;
                    if (User.Identity.IsAuthenticated)
                    {
                        tasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        currenttasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        alarmlist = _alarmservice.GetListAlarms(User.Identity.Name, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    else
                    {
                        ForSend.TaskList = new List<TaskPreviewVM>();
                        ForSend.AlarmList = new List<AlarmPreviewVM>();
                        if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
                        {
                            TempDataObject tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                            if (tempobj.AlarmList != null)
                            {
                                if (tempobj.AlarmList.Count > 0)
                                {
                                    AlarmList = tempobj.AlarmList;
                                }
                            }
                            if (tempobj.TaskList != null)
                            {
                                if (tempobj.TaskList.Count > 0)
                                {
                                    TaskList = tempobj.TaskList;
                                }
                            }
                        }
                        tasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        currenttasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        alarmlist = _alarmservice.GetListAlarmsUnAuth(AlarmList, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }

                    ForSend.Success = _taskservice.SuccessNumber(currenttasklist);
                    MainHomePreviewVM forreturn = new MainHomePreviewVM();
                    forreturn = ForSend;
                    if (forreturn.From.ToShortDateString().Contains("2564") || forreturn.From.ToShortDateString().Contains("2565"))
                    {
                        forreturn.From = forreturn.From.AddYears(-543);
                        forreturn.To = forreturn.To.AddYears(-543);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotification(User.Identity.Name);
                        notdiv.TaskList = _taskservice.ListTaskNotification(User.Identity.Name);

                        forreturn.MainNotification = notdiv;
                    }
                    else
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotificationUnAuth(AlarmList);
                        notdiv.TaskList = _taskservice.ChangeCurrentUnAuthList(TaskList);

                        forreturn.MainNotification = notdiv;
                    }

                    return View(forreturn);
                }
                else
                {
                    return View(currentmainhome);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult MobileTaskSearch(MainHomePreviewVM ForSend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentCopyKey = null;

                    CurrentCopyKey = Request.Cookies["CopyRealKey"];

                    string CurrentKey = null;

                    CurrentKey = Request.Cookies["RealKey"];

                    if (CurrentCopyKey == null && CurrentKey != null)
                    {
                        HttpContext.Session.Remove(CurrentKey);
                        HttpContext.Session.Remove(CurrentKey + "Object");
                    }

                    string CurrentZone = null;
                    CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = DateTime.Now;
                    if (CurrentZone != null)
                    {
                        CurrentRealTime = GetDateTimeObject(CurrentZone);
                        _alarmservice.SetCurrentTime(CurrentRealTime);
                        _taskservice.SetCurrentTime(CurrentRealTime);
                    }
                    currentmainhome.TaskName = ForSend.TaskName;
                    currentmainhome.TaskStatus = ForSend.TaskStatus;
                    currentmainhome.From = ForSend.From;
                    currentmainhome.To = ForSend.To;
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey + "Object")))
                    {
                        MainHomePreviewVM tempobject = JsonConvert.DeserializeObject<MainHomePreviewVM>(HttpContext.Session.GetString(CurrentKey + "Object"));
                        currentmainhome = tempobject;
                        currentmainhome.TaskName = ForSend.TaskName;
                        currentmainhome.TaskStatus = ForSend.TaskStatus;
                        currentmainhome.From = ForSend.From;
                        currentmainhome.To = ForSend.To;
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }
                    else
                    {
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }
                    
                    ForSend = currentmainhome;
                    HomeFilterVM filter = new HomeFilterVM
                    {
                        TaskName = currentmainhome.TaskName,
                        AlarmStatus = currentmainhome.AlarmStatus,
                        F = currentmainhome.F,
                        From = currentmainhome.From,
                        M = currentmainhome.M,
                        S = currentmainhome.S,
                        Sa = currentmainhome.Sa,
                        T = currentmainhome.T,
                        TaskStatus = currentmainhome.TaskStatus,
                        Th = currentmainhome.Th,
                        To = currentmainhome.To,
                        W = currentmainhome.W
                    }; 
                    if (filter.From.ToShortDateString().Contains("2564") || filter.From.ToShortDateString().Contains("2565"))
                    {
                        filter.From = filter.From.AddYears(-543);
                        filter.To = filter.To.AddYears(-543);
                    }
                    List<TaskPreviewVM> tasklist;
                    List<AlarmPreviewVM> alarmlist;
                    if (User.Identity.IsAuthenticated)
                    {
                        tasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        currenttasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        alarmlist = _alarmservice.GetListAlarms(User.Identity.Name, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    else
                    {
                        ForSend.TaskList = new List<TaskPreviewVM>();
                        ForSend.AlarmList = new List<AlarmPreviewVM>();
                        if (CurrentKey != null)
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
                            {
                                TempDataObject tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                                if (tempobj.AlarmList != null)
                                {
                                    if (tempobj.AlarmList.Count > 0)
                                    {
                                        AlarmList = tempobj.AlarmList;
                                    }
                                }
                                if (tempobj.TaskList != null)
                                {
                                    if (tempobj.TaskList.Count > 0)
                                    {
                                        TaskList = tempobj.TaskList;
                                    }
                                }
                            }
                        }
                        tasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        currenttasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        alarmlist = _alarmservice.GetListAlarmsUnAuth(AlarmList, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }

                    ForSend.Success = _taskservice.SuccessNumber(currenttasklist);
                    MainHomePreviewVM forreturn = new MainHomePreviewVM();
                    forreturn = ForSend;
                    if (forreturn.From.ToShortDateString().Contains("2564") || forreturn.From.ToShortDateString().Contains("2565"))
                    {
                        forreturn.From = forreturn.From.AddYears(-543);
                        forreturn.To = forreturn.To.AddYears(-543);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotification(User.Identity.Name);
                        notdiv.TaskList = _taskservice.ListTaskNotification(User.Identity.Name);

                        forreturn.MainNotification = notdiv;
                    }
                    else
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotificationUnAuth(AlarmList);
                        notdiv.TaskList = _taskservice.ChangeCurrentUnAuthList(TaskList);

                        forreturn.MainNotification = notdiv;
                    }

                    return View("MobileTask", currentmainhome);
                }
                else
                {
                    return View("MobileTask", currentmainhome);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult MobileAlarm(MainHomePreviewVM ForSend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentCopyKey = null;

                    CurrentCopyKey = Request.Cookies["CopyRealKey"];

                    string CurrentKey = null;

                    CurrentKey = Request.Cookies["RealKey"];

                    if (CurrentCopyKey == null && CurrentKey != null)
                    {
                        HttpContext.Session.Remove(CurrentKey);
                        HttpContext.Session.Remove(CurrentKey + "Object");
                    }

                    string CurrentZone = null;
                    CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = DateTime.Now;
                    if (CurrentZone != null)
                    {
                        CurrentRealTime = GetDateTimeObject(CurrentZone);
                        _alarmservice.SetCurrentTime(CurrentRealTime);
                        _taskservice.SetCurrentTime(CurrentRealTime);
                    }

                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey + "Object")))
                    {
                        MainHomePreviewVM tempobject = JsonConvert.DeserializeObject<MainHomePreviewVM>(HttpContext.Session.GetString(CurrentKey + "Object"));
                        currentmainhome = tempobject;
                    }
                    else
                    {
                        HttpContext.Session.SetString(CurrentKey + "Object", JsonConvert.SerializeObject(currentmainhome));
                    }

                    ForSend = currentmainhome;
                    HomeFilterVM filter = new HomeFilterVM
                    {
                        TaskName = currentmainhome.TaskName,
                        AlarmStatus = currentmainhome.AlarmStatus,
                        F = currentmainhome.F,
                        From = currentmainhome.From,
                        M = currentmainhome.M,
                        S = currentmainhome.S,
                        Sa = currentmainhome.Sa,
                        T = currentmainhome.T,
                        TaskStatus = currentmainhome.TaskStatus,
                        Th = currentmainhome.Th,
                        To = currentmainhome.To,
                        W = currentmainhome.W
                    };
                    if (filter.From.ToShortDateString().Contains("2564") || filter.From.ToShortDateString().Contains("2565"))
                    {
                        filter.From = filter.From.AddYears(-543);
                        filter.To = filter.To.AddYears(-543);
                    }
                    List<TaskPreviewVM> tasklist;
                    List<AlarmPreviewVM> alarmlist;
                    if (User.Identity.IsAuthenticated)
                    {
                        tasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        currenttasklist = _taskservice.GetListTasks(User.Identity.Name, filter);
                        alarmlist = _alarmservice.GetListAlarms(User.Identity.Name, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }
                    else
                    {
                        ForSend.TaskList = new List<TaskPreviewVM>();
                        ForSend.AlarmList = new List<AlarmPreviewVM>();
                        if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
                        {
                            TempDataObject tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                            if (tempobj.AlarmList != null)
                            {
                                if (tempobj.AlarmList.Count > 0)
                                {
                                    AlarmList = tempobj.AlarmList;
                                }
                            }
                            if (tempobj.TaskList != null)
                            {
                                if (tempobj.TaskList.Count > 0)
                                {
                                    TaskList = tempobj.TaskList;
                                }
                            }
                        }
                        tasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        currenttasklist = _taskservice.GetListTasksUnAuth(TaskList, filter);
                        alarmlist = _alarmservice.GetListAlarmsUnAuth(AlarmList, filter);
                        ForSend.TaskList = tasklist;
                        ForSend.AlarmList = alarmlist;
                    }

                    ForSend.Success = _taskservice.SuccessNumber(currenttasklist);
                    MainHomePreviewVM forreturn = new MainHomePreviewVM();
                    forreturn = ForSend;
                    if (forreturn.From.ToShortDateString().Contains("2564") || forreturn.From.ToShortDateString().Contains("2565"))
                    {
                        forreturn.From = forreturn.From.AddYears(-543);
                        forreturn.To = forreturn.To.AddYears(-543);
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotification(User.Identity.Name);
                        notdiv.TaskList = _taskservice.ListTaskNotification(User.Identity.Name);

                        forreturn.MainNotification = notdiv;
                    }
                    else
                    {
                        MainNotificationVM notdiv = new MainNotificationVM();
                        notdiv.AlarmList = _alarmservice.ListAlarmNotificationUnAuth(AlarmList);
                        notdiv.TaskList = _taskservice.ChangeCurrentUnAuthList(TaskList);

                        forreturn.MainNotification = notdiv;
                    }
                    return View(forreturn);
                }
                else
                {
                    return View(currentmainhome);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
