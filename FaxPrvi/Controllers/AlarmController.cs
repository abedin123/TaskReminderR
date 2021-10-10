using GenerateSuccess.AppDBContext;
using GenerateSuccess.Models;
using GenerateSuccess.Services;
using GenerateSuccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace GenerateSuccess.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class AlarmController : Controller
    {
        private readonly IAlarmService _alarmService;
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

        private List<Alarms> AlarmList = new List<Alarms>();

        public AlarmController(IAlarmService alarmService, IHttpContextAccessor httpContextAccessor)
        {
            _alarmService = alarmService;
            _httpContextAccessor = httpContextAccessor;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CreatedForVal(string CreatedFor)
        {
            if (CreatedFor != "1" && CreatedFor != "2")
            {
                return Json($"Please enter valid values ​​in this field!");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult RingingDurVal(string RingingDuration)
        {
            if (RingingDuration != "1" && RingingDuration != "3" && RingingDuration != "5" && RingingDuration != "10" && RingingDuration != "15" && RingingDuration != "20" && RingingDuration != "30")
            {
                return Json($"Please enter valid values ​​in this field!");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult SnoozedurVal(string SnoozeDuration)
        {
            if (SnoozeDuration != "5" && SnoozeDuration != "10" && SnoozeDuration != "15" && SnoozeDuration != "20" && SnoozeDuration != "25" && SnoozeDuration != "30")
            {
                return Json($"Please enter valid values ​​in this field!");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult SoundVal(string Sound)
        {
            if (Sound != "1" && Sound != "2" && Sound != "3" && Sound != "4" && Sound != "5" && Sound != "6" && Sound!="7")
            {
                return Json($"Please enter valid values ​​in this field!");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult ActivityVal(string Activity)
        {
            if (Activity != "1" && Activity != "2")
            {
                return Json($"Please enter valid values ​​in this field!");
            }

            return Json(true);
        }

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

        private bool IfExistName(List<Alarms> list, string name)
        {
            foreach (var item in list)
            {
                if (item.AlarmName == name)
                {
                    return true;
                }
            }
            return false;
        }

        private string GenerateAlarmName()
        {
            _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            string name = "";
            if (_currentLanguage == "en-US")
            {
                name = "Alarm";
            }
            if (_currentLanguage == "ja-JP")
            {
                name = "警報";
            }
            if (_currentLanguage == "th-TH")
            {
                name = "เตือน";
            }
            if (_currentLanguage == "pt-BR")
            {
                name = "Alarme";
            }
            if (_currentLanguage == "vi-VN")
            {
                name = "Báo thức";
            }
            if (_currentLanguage == "uk-UA")
            {
                name = "Сигналізація";
            }
            int numberoftasks = AlarmList.Count;
            if (numberoftasks >= 1)
            {
                name += "[" + (numberoftasks + 1).ToString() + "]";
            }
            else
            {
                name += "[1]";
            }
            while (IfExistName(AlarmList, name))
            {
                numberoftasks++;
                name += "[" + (numberoftasks + 1).ToString() + "]";
            }
            return name;
        }

        private TempDataObject InitialAlarmList()
        {
            string CurrentKey = Request.Cookies["RealKey"];
            TempDataObject tempobj;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
            {
                tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                if (tempobj.AlarmList != null)
                {
                    AlarmList = tempobj.AlarmList;
                }
                else
                {
                    AlarmList = new List<Alarms>();
                }
            }
            else
            {
                AlarmList = new List<Alarms>();
                tempobj = new TempDataObject();
            }
            
            return tempobj;
        }

        [HttpGet]
        public IActionResult NewAlarm()
        {
            try
            {
                string CurrentZone = null;

                CurrentZone = Request.Cookies["TimeZone"];

                DateTime CurrentRealTime = GetDateTimeObject(CurrentZone);
                
                string AlarmDefaultName;
                if (User.Identity.IsAuthenticated)
                {
                    _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
                    AlarmDefaultName = _alarmService.GenerateAlarmName(User.Identity.Name, _currentLanguage);
                }
                else
                {
                    TempDataObject tempobj = InitialAlarmList();

                    AlarmDefaultName = GenerateAlarmName();
                }
                NewAlarmVM model = new NewAlarmVM
                {
                    AlarmName = AlarmDefaultName,
                    RingingTime = CurrentRealTime.AddHours(3)
                };

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult NewAlarmMobile()
        {
            try
            {
                string CurrentZone = null;

                CurrentZone = Request.Cookies["TimeZone"];

                DateTime CurrentRealTime = GetDateTimeObject(CurrentZone);

                string AlarmDefaultName;
                if (User.Identity.IsAuthenticated)
                {
                    _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
                    AlarmDefaultName = _alarmService.GenerateAlarmName(User.Identity.Name, _currentLanguage);
                }
                else
                {
                    TempDataObject tempobj = InitialAlarmList();

                    AlarmDefaultName = GenerateAlarmName();
                }
                NewAlarmVM model = new NewAlarmVM
                {
                    AlarmName = AlarmDefaultName,
                    RingingTime = CurrentRealTime.AddHours(3)
                };

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SaveAlarm(NewAlarmVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = GetDateTimeObject(CurrentZone);
                    _alarmService.SetCurrentTime(CurrentRealTime);

                    if (User.Identity.IsAuthenticated)
                    {
                        string test = null;
                        test=_alarmService.AddAlarm(obj, User.Identity.Name);

                        if (test == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject tempobj = InitialAlarmList();
                        Alarms returned;
                        returned = _alarmService.AddAlarmUnAuth(obj, AlarmList);
                        
                        AlarmList.Add(returned);
                        tempobj.AlarmList = AlarmList;

                        HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("NewAlarm", obj);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SaveAlarmMobile(NewAlarmVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = GetDateTimeObject(CurrentZone);
                    _alarmService.SetCurrentTime(CurrentRealTime);

                    if (User.Identity.IsAuthenticated)
                    {
                        string test = null;
                        test = _alarmService.AddAlarm(obj, User.Identity.Name);

                        if (test == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject tempobj = InitialAlarmList();

                        Alarms returned;
                        returned = _alarmService.AddAlarmUnAuth(obj, AlarmList);

                        AlarmList.Add(returned);
                        tempobj.AlarmList = AlarmList;

                        HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                    }
                    return RedirectToAction("MobileAlarm", "Home");
                }
                else
                {
                    return View("NewAlarmMobile", obj);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult DeleteAlarm(string Key)
        {
            try
            {
                Alarms model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _alarmService.DeleteAlarm(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    string CurrentKey = Request.Cookies["RealKey"];
                    TempDataObject tempobj = InitialAlarmList();

                    List<Alarms> modellist = null;
                    modellist=_alarmService.DeleteAlarmUnAuth(Key,AlarmList);

                    tempobj.AlarmList = modellist;

                    HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult DeleteAlarmMobile(string Key)
        {
            try
            {
                Alarms model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _alarmService.DeleteAlarm(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("MobileAlarm", "Home");
                    }
                }
                else
                {
                    string CurrentKey = Request.Cookies["RealKey"];
                    TempDataObject tempobj = InitialAlarmList();

                    List<Alarms> modellist = null;
                    modellist = _alarmService.DeleteAlarmUnAuth(Key, AlarmList);

                    tempobj.AlarmList = modellist;

                    HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                }
                return RedirectToAction("MobileAlarm", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult EditAlarm(string Key)
        {
            try
            {
                NewAlarmVM model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _alarmService.EditAlarm(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempDataObject tempobj = InitialAlarmList();
                    model = _alarmService.EditAlarmUnAuth(Key, AlarmList);
                    if (model == null)
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

        [HttpPost]
        public IActionResult EditAlarmMobile(string Key)
        {
            try
            {
                NewAlarmVM model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _alarmService.EditAlarm(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("MobileAlarm", "Home");
                    }
                }
                else
                {
                    TempDataObject tempobj = InitialAlarmList();
                    model = _alarmService.EditAlarmUnAuth(Key, AlarmList);
                    if (model == null)
                    {
                        return RedirectToAction("MobileAlarm", "Home");
                    }
                }
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SaveAlarmChanges(NewAlarmVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = GetDateTimeObject(CurrentZone);
                    _alarmService.SetCurrentTime(CurrentRealTime);

                    if (User.Identity.IsAuthenticated)
                    {
                        Alarms model = null;
                        model = _alarmService.SaveAlarmChanges(obj, User.Identity.Name);

                        if (model == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject tempobj = InitialAlarmList();

                        Alarms returned = null;
                        returned = _alarmService.SaveAlarmChangesUnAuth(obj, AlarmList);

                        if (returned == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            int index = 0;
                            for (int i = 0; i < AlarmList.Count; i++)
                            {
                                if (AlarmList[i].Key == returned.Key)
                                {
                                    index = i;
                                }
                            }

                            AlarmList[index] = returned;

                            tempobj.AlarmList = AlarmList;

                            HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SaveAlarmChangesMobile(NewAlarmVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentZone = Request.Cookies["TimeZone"];
                    DateTime CurrentRealTime = GetDateTimeObject(CurrentZone);
                    _alarmService.SetCurrentTime(CurrentRealTime);

                    if (User.Identity.IsAuthenticated)
                    {
                        Alarms model = null;
                        model = _alarmService.SaveAlarmChanges(obj, User.Identity.Name);

                        if (model == null)
                        {
                            return RedirectToAction("MobileAlarm", "Home");
                        }
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject tempobj = InitialAlarmList();

                        Alarms returned = null;
                        returned = _alarmService.SaveAlarmChangesUnAuth(obj, AlarmList);

                        if (returned == null)
                        {
                            return RedirectToAction("MobileAlarm", "Home");
                        }
                        else
                        {
                            int index = 0;
                            for (int i = 0; i < AlarmList.Count; i++)
                            {
                                if (AlarmList[i].Key == returned.Key)
                                {
                                    index = i;
                                }
                            }

                            AlarmList[index] = returned;

                            tempobj.AlarmList = AlarmList;

                            HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                        }
                    }
                    return RedirectToAction("MobileAlarm", "Home");
                }
                else
                {
                    return RedirectToAction("MobileAlarm", "Home");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SetLastAlarm(string Key,string Value)
        {
            try
            {
                string CurrentZone = Request.Cookies["TimeZone"];
                DateTime CurrentRealTime = GetDateTimeObject(CurrentZone);
                _alarmService.SetCurrentTime(CurrentRealTime);

                Alarms model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _alarmService.SetLastAlarmNotification(Key,Value,User.Identity.Name);
                }
                else
                {
                    string CurrentKey = Request.Cookies["RealKey"];
                    TempDataObject tempobj = InitialAlarmList();

                    Alarms returned = null;
                    returned = _alarmService.SetLastAlarmNotificationUnAuth(Key,Value,AlarmList);

                    int index = 0;
                    for (int i = 0; i < AlarmList.Count; i++)
                    {
                        if (AlarmList[i].Key == returned.Key)
                        {
                            index = i;
                        }
                    }

                    AlarmList[index] = returned;

                    tempobj.AlarmList = AlarmList;

                    HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
