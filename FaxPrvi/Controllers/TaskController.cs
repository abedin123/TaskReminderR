using GenerateSuccess.ViewModels;
using GenerateSuccess.AppDBContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using GenerateSuccess.Services;
using System.ComponentModel.DataAnnotations;
using GenerateSuccess.Models;
using Newtonsoft.Json;
using TimeZoneConverter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GenerateSuccess.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class TaskController : Controller
    {
        private List<TaskDB> TaskList=new List<TaskDB>();

        private readonly ITaskService _taskService;
        private string _currentLanguage;
        private IHttpContextAccessor _httpContextAccessor;

        private int numberoftasks = 0;

        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            _taskService = taskService;
            _httpContextAccessor = httpContextAccessor;
        }

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

        private bool IfExistName(List<TaskDB> list, string name)
        {
            foreach (var item in list)
            {
                if (item.TaskName == name)
                {
                    return true;
                }
            }
            return false;
        }

        private string GenerateTaskName()
        {
            _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            numberoftasks = TaskList.Count;
            string name = "Activity";
            if (_currentLanguage == "ja-JP")
            {
                name = "アクティビティ";
            }
            if (_currentLanguage == "th-TH")
            {
                name = "กิจกรรม";
            }
            if (_currentLanguage == "pt-BR")
            {
                name = "Atividade";
            }
            if (_currentLanguage == "vi-VN")
            {
                name = "Hoạt động";
            }
            if (_currentLanguage == "uk-UA")
            {
                name = "Діяльність";
            }
            if (numberoftasks >= 1)
            {
                name += "[" + (numberoftasks + 1).ToString() + "]";
            }
            else
            {
                name += "[1]";
            }
            while (IfExistName(TaskList, name))
            {
                numberoftasks++;
                name += "[" + (numberoftasks + 1).ToString() + "]";
            }
            return name;
        }

        private TempDataObject InitialTaskList()
        {
            string CurrentKey = Request.Cookies["RealKey"];
            TempDataObject tempobj;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CurrentKey)))
            {
                tempobj = JsonConvert.DeserializeObject<TempDataObject>(HttpContext.Session.GetString(CurrentKey));
                if (tempobj.TaskList != null)
                {
                    TaskList = tempobj.TaskList;
                }
                else
                {
                    TaskList = new List<TaskDB>();
                }
            }
            else
            {
                TaskList = new List<TaskDB>();
                tempobj = new TempDataObject();
            }
            
            return tempobj;
        }

        private DateTime InitialCurrentTime()
        {
            string CurrentTime = null;

            CurrentTime = Request.Cookies["TimeZone"];

            int Month = int.Parse((CurrentTime[0].ToString() + CurrentTime[1].ToString()));
            int Day = int.Parse((CurrentTime[3].ToString() + CurrentTime[4].ToString()));
            int Year = int.Parse((CurrentTime[6].ToString() + CurrentTime[7].ToString() + CurrentTime[8].ToString() + CurrentTime[9].ToString()));
            int Hour = int.Parse((CurrentTime[11].ToString() + CurrentTime[12].ToString()));
            int Minute = int.Parse((CurrentTime[14].ToString() + CurrentTime[15].ToString()));
            DateTime CurrentRealTime = new DateTime(Year, Month, Day, Hour, Minute, 0);
            if (CurrentRealTime.ToShortDateString().Contains("2564")|| CurrentRealTime.ToShortDateString().Contains("2565"))
            {
                CurrentRealTime = CurrentRealTime.AddYears(-543);
            }
            _taskService.SetCurrentTime(CurrentRealTime);
            
            return CurrentRealTime;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult DateVal(DateTime StartDate, DateTime EndDate, DateTime StartTimeOnce, DateTime EndTimeOnce, DateTime StartTimeCustom, DateTime EndTimeCustom, string CreatedFor)
        {
            DateTime Curr=InitialCurrentTime();
            _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            if (CreatedFor == "1")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor))
                {
                    if (_currentLanguage == "en-US")
                    {
                        return Json($"Start time can't be same or higher than end time!");
                    }
                    if (_currentLanguage == "ja-JP")
                    {
                        return Json($"開始時間は終了時間と同じまたはそれより長くすることはできません！");
                    }
                    if (_currentLanguage == "th-TH")
                    {
                        return Json($"เวลาเริ่มต้นต้องไม่เท่ากันหรือสูงกว่าเวลาสิ้นสุด!");
                    }
                    if (_currentLanguage == "pt-BR")
                    {
                        return Json($"A hora de início não pode ser igual ou superior à hora de término!");
                    }
                    if (_currentLanguage == "vi-VN")
                    {
                        return Json($"Thời gian bắt đầu không được bằng hoặc cao hơn thời gian kết thúc!");
                    }
                    if (_currentLanguage == "uk-UA")
                    {
                        return Json($"Час початку не може бути таким самим або вищим за час закінчення!");
                    }
                }
            }
            if (CreatedFor == "2")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeCustom, EndTimeCustom, CreatedFor))
                {
                    if (_currentLanguage == "en-US")
                    {
                        return Json($"Start time can't be same or higher than end time!");
                    }
                    if (_currentLanguage == "ja-JP")
                    {
                        return Json($"開始時間は終了時間と同じまたはそれより長くすることはできません！");
                    }
                    if (_currentLanguage == "th-TH")
                    {
                        return Json($"เวลาเริ่มต้นต้องไม่เท่ากันหรือสูงกว่าเวลาสิ้นสุด!");
                    }
                    if (_currentLanguage == "pt-BR")
                    {
                        return Json($"A hora de início não pode ser igual ou superior à hora de término!");
                    }
                    if (_currentLanguage == "vi-VN")
                    {
                        return Json($"Thời gian bắt đầu không được bằng hoặc cao hơn thời gian kết thúc!");
                    }
                    if (_currentLanguage == "uk-UA")
                    {
                        return Json($"Час початку не може бути таким самим або вищим за час закінчення!");
                    }
                }
            }

            if (CreatedFor == "1")
            {
                if (!_taskService.DateInThePast(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor, 0))
                {
                    if (_currentLanguage == "en-US")
                    {
                        return Json($"Date can't be in the past!");
                    }
                    if (_currentLanguage == "ja-JP")
                    {
                        return Json($"日付は過去にすることはできません！");
                    }
                    if (_currentLanguage == "th-TH")
                    {
                        return Json($"วันที่ต้องไม่ผ่าน!");
                    }
                    if (_currentLanguage == "pt-BR")
                    {
                        return Json($"A data não pode estar no passado!");
                    }
                    if (_currentLanguage == "vi-VN")
                    {
                        return Json($"Ngày không thể là quá khứ!");
                    }
                    if (_currentLanguage == "uk-UA")
                    {
                        return Json($"Дата не може бути в минулому!");
                    }
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult EndDateVal(DateTime StartDate, DateTime EndDate, DateTime StartTimeOnce, DateTime EndTimeOnce, DateTime StartTimeCustom, DateTime EndTimeCustom, string CreatedFor)
        {
            DateTime Curr=InitialCurrentTime();
            _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            if (CreatedFor == "1")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor))
                {
                    if (_currentLanguage == "en-US")
                    {
                        return Json($"End time can't be same or lower than start time!");
                    }
                    if (_currentLanguage == "ja-JP")
                    {
                        return Json($"終了時間は開始時間と同じまたはそれより短くすることはできません！");
                    }
                    if (_currentLanguage == "th-TH")
                    {
                        return Json($"เวลาสิ้นสุดต้องไม่เท่ากันหรือต่ำกว่าเวลาเริ่มต้น!");
                    }
                    if (_currentLanguage == "pt-BR")
                    {
                        return Json($"O horário de término não pode ser igual ou inferior ao horário de início!");
                    }
                    if (_currentLanguage == "vi-VN")
                    {
                        return Json($"Thời gian kết thúc không được giống hoặc thấp hơn thời gian bắt đầu!");
                    }
                    if (_currentLanguage == "uk-UA")
                    {
                        return Json($"Час завершення не може бути таким самим або меншим за час початку!");
                    }
                }
            }
            if (CreatedFor == "2")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeCustom, EndTimeCustom, CreatedFor))
                {
                    if (_currentLanguage == "en-US")
                    {
                        return Json($"End time can't be same or lower than start time!");
                    }
                    if (_currentLanguage == "ja-JP")
                    {
                        return Json($"終了時間は開始時間と同じまたはそれより短くすることはできません！");
                    }
                    if (_currentLanguage == "th-TH")
                    {
                        return Json($"เวลาสิ้นสุดต้องไม่เท่ากันหรือต่ำกว่าเวลาเริ่มต้น!");
                    }
                    if (_currentLanguage == "pt-BR")
                    {
                        return Json($"O horário de término não pode ser igual ou inferior ao horário de início!");
                    }
                    if (_currentLanguage == "vi-VN")
                    {
                        return Json($"Thời gian kết thúc không được giống hoặc thấp hơn thời gian bắt đầu!");
                    }
                    if (_currentLanguage == "uk-UA")
                    {
                        return Json($"Час завершення не може бути таким самим або меншим за час початку!");
                    }
                }
            }

            if (CreatedFor == "1")
            {
                if (!_taskService.DateInThePast(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor, 1))
                {
                    if (_currentLanguage == "en-US")
                    {
                        return Json($"Date can't be in the past!");
                    }
                    if (_currentLanguage == "ja-JP")
                    {
                        return Json($"日付は過去にすることはできません！");
                    }
                    if (_currentLanguage == "th-TH")
                    {
                        return Json($"วันที่ต้องไม่ผ่าน!");
                    }
                    if (_currentLanguage == "pt-BR")
                    {
                        return Json($"A data não pode estar no passado!");
                    }
                    if (_currentLanguage == "vi-VN")
                    {
                        return Json($"Ngày không thể là quá khứ!");
                    }
                    if (_currentLanguage == "uk-UA")
                    {
                        return Json($"Дата не може бути в минулому!");
                    }
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult TimeValHours(int Hours,int Minutes)
        {
            _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            if (Hours==0&& Minutes == 0)
            {
                if (_currentLanguage == "en-US")
                {
                    return Json($"Notification can't be every 0 minutes!");
                }
                if (_currentLanguage == "ja-JP")
                {
                    return Json($"通知は0分ごとにすることはできません！");
                }
                if (_currentLanguage == "th-TH")
                {
                    return Json($"การแจ้งเตือนต้องไม่ทุก 0 นาที!");
                }
                if (_currentLanguage == "pt-BR")
                {
                    return Json($"A notificação não pode ser a cada 0 minutos!");
                }
                if (_currentLanguage == "vi-VN")
                {
                    return Json($"Thông báo không thể cứ 0 phút một lần!");
                }
                if (_currentLanguage == "uk-UA")
                {
                    return Json($"Повідомлення не може надходити кожні 0 хвилин!");
                }
            }
            if (Hours < 0)
            {
                if (_currentLanguage == "en-US")
                {
                    return Json($"This field cannot be negative!");
                }
                if (_currentLanguage == "ja-JP")
                {
                    return Json($"このフィールドを負にすることはできません!");
                }
                if (_currentLanguage == "th-TH")
                {
                    return Json($"ฟิลด์นี้ไม่สามารถเป็นค่าลบได้!");
                }
                if (_currentLanguage == "pt-BR")
                {
                    return Json($"Este campo não pode ser negativo!");
                }
                if (_currentLanguage == "vi-VN")
                {
                    return Json($"Trường này không được âm!");
                }
                if (_currentLanguage == "uk-UA")
                {
                    return Json($"Це поле не може бути негативним!");
                }
            }
            if (Hours > 500)
            {
                if (_currentLanguage == "en-US")
                {
                    return Json($"This field cannot have a value greater than 500!");
                }
                if (_currentLanguage == "ja-JP")
                {
                    return Json($"このフィールドの値を500より大きくすることはできません!");
                }
                if (_currentLanguage == "th-TH")
                {
                    return Json($"ฟิลด์นี้ไม่สามารถมีค่ามากกว่า 500!");
                }
                if (_currentLanguage == "pt-BR")
                {
                    return Json($"Este campo não pode ter um valor maior que 500!");
                }
                if (_currentLanguage == "vi-VN")
                {
                    return Json($"Trường này không được có giá trị lớn hơn 500!");
                }
                if (_currentLanguage == "uk-UA")
                {
                    return Json($"Це поле не може мати значення більше 500!");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult TimeValMinutes(int Minutes)
        {
            _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            if (Minutes < 0)
            {
                if (_currentLanguage == "en-US")
                {
                    return Json($"This field cannot be negative!");
                }
                if (_currentLanguage == "ja-JP")
                {
                    return Json($"このフィールドを負にすることはできません!");
                }
                if (_currentLanguage == "th-TH")
                {
                    return Json($"ฟิลด์นี้ไม่สามารถเป็นค่าลบได้!");
                }
                if (_currentLanguage == "pt-BR")
                {
                    return Json($"Este campo não pode ser negativo!");
                }
                if (_currentLanguage == "vi-VN")
                {
                    return Json($"Trường này không được âm!");
                }
                if (_currentLanguage == "uk-UA")
                {
                    return Json($"Це поле не може бути негативним!");
                }
            }
            if (Minutes > 500)
            {
                if (_currentLanguage == "en-US")
                {
                    return Json($"This field cannot have a value greater than 500!");
                }
                if (_currentLanguage == "ja-JP")
                {
                    return Json($"このフィールドの値を500より大きくすることはできません!");
                }
                if (_currentLanguage == "th-TH")
                {
                    return Json($"ฟิลด์นี้ไม่สามารถมีค่ามากกว่า 500!");
                }
                if (_currentLanguage == "pt-BR")
                {
                    return Json($"Este campo não pode ter um valor maior que 500!");
                }
                if (_currentLanguage == "vi-VN")
                {
                    return Json($"Trường này không được có giá trị lớn hơn 500!");
                }
                if (_currentLanguage == "uk-UA")
                {
                    return Json($"Це поле не може мати значення більше 500!");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CreatedForVal(string CreatedFor)
        {
            if (CreatedFor != "1" && CreatedFor != "2")
                return Json($"Please enter valid values ​​in this field!");

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult PriorityVal(string Priority)
        {
            if (Priority != "1" && Priority != "2" && Priority != "3")
                return Json($"Please enter valid values ​​in this field!");

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult SoundVal(string Sound)
        {
            if (Sound != "1" && Sound != "2" && Sound != "3" && Sound != "4" && Sound != "5" && Sound != "6" && Sound != "7")
                return Json($"Please enter valid values ​​in this field!");

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult TaskStatus(string TaskStatus)
        {
            if (TaskStatus != "0" && TaskStatus != "1" && TaskStatus != "2" && TaskStatus != "3" && TaskStatus != "4")
                return Json($"Please enter valid values ​​in this field!");

            return Json(true);
        }
        
        [HttpPost]
        public IActionResult EditTask(string Key)
        {
            try
            {
                NewTaskVM model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _taskService.EditTask(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempDataObject obj = InitialTaskList();
                    model = _taskService.EditTaskUnAuth(Key, TaskList);
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
        public IActionResult SaveTask(NewTaskVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime current = InitialCurrentTime();
                    if (User.Identity.IsAuthenticated)
                    {
                        TaskDB test = null;
                        test=_taskService.AddTask(obj, User.Identity.Name);
                        if (test == null)
                        {
                            return RedirectToAction("NewTask", "Task");
                        }
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject TempObj = InitialTaskList();
                        TaskDB returned = _taskService.AddTaskUnAuth(obj, TaskList);
                        TaskList.Add(returned);
                        TempObj.TaskList = TaskList;
                        HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(TempObj));
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
        public IActionResult SaveTaskMobile(NewTaskVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime current = InitialCurrentTime();
                    if (User.Identity.IsAuthenticated)
                    {
                        TaskDB test = null;
                        test = _taskService.AddTask(obj, User.Identity.Name);
                        if (test == null)
                        {
                            return RedirectToAction("NewTaskMobile", "Task");
                        }
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject TempObj = InitialTaskList();
                        TaskDB returned = _taskService.AddTaskUnAuth(obj, TaskList);
                        TaskList.Add(returned);
                        TempObj.TaskList = TaskList;
                        HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(TempObj));
                    }
                    return RedirectToAction("MobileTask", "Home");
                }
                else
                {
                    return RedirectToAction("MobileTask", "Home");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SaveTaskChanges(NewTaskVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime current = InitialCurrentTime();
                    if (User.Identity.IsAuthenticated)
                    {
                        TaskDB model = null;
                        model = _taskService.SaveTaskChanges(obj, User.Identity.Name);

                        if (model == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject tempobj = InitialTaskList();

                        TaskDB returned = null;
                        returned = _taskService.SaveTaskChangesUnAuth(obj, TaskList);

                        if (returned == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            int index = 0;
                            for (int i = 0; i < TaskList.Count; i++)
                            {
                                if (TaskList[i].Key == returned.Key)
                                {
                                    index = i;
                                }
                            }
                            TaskList[index] = returned;
                            tempobj.TaskList = TaskList;
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
        public IActionResult DeleteTask(string Key)
        {
            try
            {
                TaskDB model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _taskService.DeleteTask(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    string CurrentKey = Request.Cookies["RealKey"];
                    TempDataObject tempobj = InitialTaskList();

                    List<TaskDB> modellist = null;
                    modellist = _taskService.DeleteTaskUnAuth(Key, TaskList);
                    if (modellist == null)
                    {
                        TaskList = null;
                        tempobj.TaskList = TaskList;
                        HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                        return RedirectToAction("Index", "Home");
                    }
                    tempobj.TaskList = modellist;
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
        public IActionResult DeleteTaskMobile(string Key)
        {
            try
            {
                TaskDB model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _taskService.DeleteTask(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("MobileTask", "Home");
                    }
                }
                else
                {
                    string CurrentKey = Request.Cookies["RealKey"];
                    TempDataObject tempobj = InitialTaskList();

                    List<TaskDB> modellist = null;
                    modellist = _taskService.DeleteTaskUnAuth(Key, TaskList);
                    if (modellist == null)
                    {
                        TaskList = null;
                        tempobj.TaskList = TaskList;
                        HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                        return RedirectToAction("MobileTask", "Home");
                    }
                    tempobj.TaskList = modellist;
                    HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                }
                return RedirectToAction("MobileTask", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            
        }

        [HttpGet]
        public IActionResult NewTask()
        {
            try
            {
                DateTime current = InitialCurrentTime();
                
                string TaskDefaultName = "";
                if (User.Identity.IsAuthenticated)
                {
                    _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
                    TaskDefaultName = _taskService.GenerateTaskName(User.Identity.Name, _currentLanguage);
                }
                else
                {
                    TempDataObject obj= InitialTaskList();
                    TaskDefaultName = GenerateTaskName();
                }
                NewTaskVM model = new NewTaskVM
                {
                    TaskName = TaskDefaultName,
                    StartDate = current.AddHours(3),
                    StartTimeOnce = current.AddHours(3),
                    EndDate = current.AddHours(4),
                    EndTimeOnce = current.AddHours(4),
                    StartTimeCustom = current.AddHours(3),
                    EndTimeCustom = current.AddHours(4)
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult NewTaskMobile()
        {
            try
            {
                DateTime current = InitialCurrentTime();

                string TaskDefaultName = "";
                if (User.Identity.IsAuthenticated)
                {
                    _currentLanguage = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
                    TaskDefaultName = _taskService.GenerateTaskName(User.Identity.Name, _currentLanguage);
                }
                else
                {
                    TempDataObject obj = InitialTaskList();
                    TaskDefaultName = GenerateTaskName();
                }
                NewTaskVM model = new NewTaskVM
                {
                    TaskName = TaskDefaultName,
                    StartDate = current.AddHours(3),
                    StartTimeOnce = current.AddHours(3),
                    EndDate = current.AddHours(4),
                    EndTimeOnce = current.AddHours(4),
                    StartTimeCustom = current.AddHours(3),
                    EndTimeCustom = current.AddHours(4)
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult EditTaskMobile(string Key)
        {
            try
            {
                NewTaskVM model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _taskService.EditTask(Key, User.Identity.Name);
                    if (model == null)
                    {
                        return RedirectToAction("MobileTask", "Home");
                    }
                }
                else
                {
                    TempDataObject obj = InitialTaskList();
                    model = _taskService.EditTaskUnAuth(Key, TaskList);
                    if (model == null)
                    {
                        return RedirectToAction("MobileTask", "Home");
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
        public IActionResult SaveTaskChangesMobile(NewTaskVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime current = InitialCurrentTime();
                    if (User.Identity.IsAuthenticated)
                    {
                        TaskDB model = null;
                        model = _taskService.SaveTaskChanges(obj, User.Identity.Name);

                        if (model == null)
                        {
                            return RedirectToAction("MobileTask", "Home");
                        }
                    }
                    else
                    {
                        string CurrentKey = Request.Cookies["RealKey"];
                        TempDataObject tempobj = InitialTaskList();

                        TaskDB returned = null;
                        returned = _taskService.SaveTaskChangesUnAuth(obj, TaskList);

                        if (returned == null)
                        {
                            return RedirectToAction("MobileTask", "Home");
                        }
                        else
                        {
                            int index = 0;
                            for (int i = 0; i < TaskList.Count; i++)
                            {
                                if (TaskList[i].Key == returned.Key)
                                {
                                    index = i;
                                }
                            }
                            TaskList[index] = returned;
                            tempobj.TaskList = TaskList;
                            HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                        }
                    }
                    return RedirectToAction("MobileTask", "Home");
                }
                else
                {
                    return RedirectToAction("MobileTask", "Home");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult SetLastNotification(string Key,string Value,string Success)
        {
            try
            {
                DateTime current = InitialCurrentTime();
                TaskDB model = null;
                if (User.Identity.IsAuthenticated)
                {
                    model = _taskService.SetLastNotification(Key, Value, User.Identity.Name, Success);
                    if (model == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    string CurrentKey = Request.Cookies["RealKey"];
                    TempDataObject tempobj = InitialTaskList();

                    TaskDB returned = null;
                    returned = _taskService.SetLastNotificationUnAuth(Key, Value, TaskList, Success);

                    if (returned == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        int index = 0;
                        for (int i = 0; i < TaskList.Count; i++)
                        {
                            if (TaskList[i].Key == returned.Key)
                            {
                                index = i;
                            }
                        }
                        TaskList[index] = returned;
                        tempobj.TaskList = TaskList;
                        HttpContext.Session.SetString(CurrentKey, JsonConvert.SerializeObject(tempobj));
                    }
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
