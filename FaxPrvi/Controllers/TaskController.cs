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

namespace GenerateSuccess.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class TaskController : Controller
    {
        private List<TaskDB> TaskList=new List<TaskDB>();

        private readonly ITaskService _taskService;

        private int numberoftasks = 0;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
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
            numberoftasks = TaskList.Count;
            string name = "Activity";
            if (numberoftasks >= 1)
            {
                name += "[" + (numberoftasks + 1).ToString() + "]";
            }
            else
            {
                name = "Activity[1]";
            }
            while (IfExistName(TaskList, name))
            {
                numberoftasks++;
                name = "Activity[" + (numberoftasks + 1).ToString() + "]";
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

            _taskService.SetCurrentTime(CurrentRealTime);
            
            return CurrentRealTime;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult DateVal(DateTime StartDate, DateTime EndDate, DateTime StartTimeOnce, DateTime EndTimeOnce, DateTime StartTimeCustom, DateTime EndTimeCustom, string CreatedFor)
        {
            DateTime Curr=InitialCurrentTime();

            if (CreatedFor == "1")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor))
                {
                    return Json($"Start time can't be same or higher than end time!");
                }
            }
            if (CreatedFor == "2")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeCustom, EndTimeCustom, CreatedFor))
                {
                    return Json($"Start time can't be same or higher than end time!");
                }
            }

            if (CreatedFor == "1")
            {
                if (!_taskService.DateInThePast(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor, 0))
                {
                    return Json($"Date can't be in the past!");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult EndDateVal(DateTime StartDate, DateTime EndDate, DateTime StartTimeOnce, DateTime EndTimeOnce, DateTime StartTimeCustom, DateTime EndTimeCustom, string CreatedFor)
        {
            DateTime Curr=InitialCurrentTime();
            if (CreatedFor == "1")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor))
                {
                    return Json($"End time can't be same or lower than start time!");
                }
            }
            if (CreatedFor == "2")
            {
                if (!_taskService.DateValidation(StartDate, EndDate, StartTimeCustom, EndTimeCustom, CreatedFor))
                {
                    return Json($"End time can't be same or lower than start time!");
                }
            }

            if (CreatedFor == "1")
            {
                if (!_taskService.DateInThePast(StartDate, EndDate, StartTimeOnce, EndTimeOnce, CreatedFor, 1))
                {
                    return Json($"Date can't be in the past!");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult TimeValHours(int Hours,int Minutes)
        {
            if(Hours==0&& Minutes == 0)
            {
                return Json($"Notification can't be every 0 minutes!");
            }
            if (Hours < 0)
            {
                return Json($"This field cannot be negative!");
            }
            if (Hours > 500)
            {
                return Json($"This field cannot have a value greater than 500!");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult TimeValMinutes(int Minutes)
        {
            if (Minutes < 0)
            {
                return Json($"This field cannot be negative!");
            }
            if (Minutes > 500)
            {
                return Json($"This field cannot have a value greater than 500!");
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
                    TaskDefaultName = _taskService.GenerateTaskName(User.Identity.Name);
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
                    TaskDefaultName = _taskService.GenerateTaskName(User.Identity.Name);
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
