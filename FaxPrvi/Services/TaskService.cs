using GenerateSuccess.AppDBContext;
using GenerateSuccess.Models;
using GenerateSuccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.Services
{
    public class TaskService:ITaskService
    {
        private readonly AppDbContext _context;

        private DateTime CurrentTime;

        private readonly List<string> Priority=new List<string> { 
                "Low",
                "Normal",
                "High"
        };

        private readonly List<string> Sounds = new List<string> {
                "TaskSound1",
                "TaskSound2",
                "TaskSound3",
                "TaskSound4",
                "TaskSound5",
                "TaskSound6",
                "TaskSound7"
        };

        private readonly List<string> Days = new List<string> {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
        };

        private readonly List<string> Statss = new List<string> {
                "In processing!",
                "Upcoming!",
                "Finished!"
        };

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public bool DateValidation(DateTime StartDate,DateTime EndDate,DateTime StartTime, DateTime EndTime,string CreatedFor)
        {
            if(CreatedFor=="1")
            {
                DateTime Start = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                DateTime End = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);

                if (Start >= End)
                    return false;
            }
            if (CreatedFor == "2")
            {
                int HoursStart = StartTime.Hour;
                int MinutesStart = StartTime.Minute;
                int HoursEnd = EndTime.Hour;
                int MinutesEnd = EndTime.Minute;

                string startpm = StartTime.ToString("tt");
                string endpm = EndTime.ToString("tt");
                
                if ((startpm == "PM" || startpm == "pm") && (endpm == "AM" || endpm == "am"))
                    return true;

                if (HoursEnd < HoursStart)
                    return false;
                if(HoursEnd==HoursStart)
                {
                    if (MinutesEnd <= MinutesStart)
                        return false;
                }
            }
            return true;
        }

        public bool DateInThePast(DateTime StartDate, DateTime EndDate, DateTime StartTime, DateTime EndTime, string CreatedFor, int WhichDate)
        {
            DateTime now = CurrentTime;
            StartTime = StartTime.AddMinutes(4);
            if (CreatedFor == "1")
            {
                DateTime Start = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                DateTime End = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
                
                if (Start < now && WhichDate==0)
                    return false;

                if (End < now && WhichDate == 1)
                    return false;
            }

            return true;
        }

        public bool CorrectType(int Minutes, int Hours)
        {
            string min = Minutes.ToString();
            foreach (var item in min)
            {
                if (item < '0' || item > '9')
                    return false;
            }
            return true;
        }

        public string GenerateTaskName(string UserName,string Language)
        {
            string name = "Activity";
            if (Language == "ja-JP")
            {
                name = "アクティビティ";
            }
            if (Language == "th-TH")
            {
                name = "กิจกรรม";
            }
            if (Language == "pt-BR")
            {
                name = "Atividade";
            }
            if (Language == "vi-VN")
            {
                name = "Hoạt động";
            }
            if (Language == "uk-UA")
            {
                name = "Діяльність";
            }
            var listoftasks = _context.UserTask.Include(a => a.User).Include(a=>a.Task).Where(a => a.User.UserName == UserName).ToList();
            int numberoftasks = listoftasks.Count();
            if (numberoftasks>=1)
            {
                name += "[" + (numberoftasks+1).ToString()+"]";
            }
            else
            {
                name += "[1]";
            }

            while (ExistTaskName(listoftasks, name) == true)
            {
                numberoftasks++;
                name = GenerateName(numberoftasks, Language);
            }

            return name;
        }

        private string GenerateName(int numberoftasks,string Lang)
        {
            string name = "Activity";
            if (Lang == "ja-JP")
            {
                name = "アクティビティ";
            }
            if (Lang == "th-TH")
            {
                name = "กิจกรรม";
            }
            if (Lang == "pt-BR")
            {
                name = "Atividade";
            }
            if (Lang == "vi-VN")
            {
                name = "Hoạt động";
            }
            if (Lang == "uk-UA")
            {
                name = "Діяльність";
            }
            name += "[" + (numberoftasks + 1).ToString() + "]";
            return name;
        }

        private bool ExistTaskName(List<UserTask> list,string name)
        {
            foreach (var item in list)
            {
                if (item.Task.TaskName == name)
                    return true;
            }
            return false;
        }

        public TaskDB AddTask(NewTaskVM obj,string User)
        {
            int notevery = 0;
            if (obj.Hours != 0)
            {
                notevery += (obj.Hours * 60);
            }
            notevery += obj.Minutes;
            TaskDB model = new TaskDB
            {
                TaskName = obj.TaskName,
                Description = obj.Description,
                CreateTime = CurrentTime,
                LastNotification = CurrentTime,
                Priority = Priority[int.Parse(obj.Priority) - 1],
                SoundName=Sounds[int.Parse(obj.Sound) - 1],
                NotificationEvery= notevery,
                Key=GenerateKey(),
                CreatedFor=obj.CreatedFor,
                Successufull=-1,
                Days= GenerateFinalDaysString(obj),
                AcceptedNotification=true,
                Rated=false,
                LastTimeFinished=CurrentTime.AddDays(-1),
                NumberOfFinished=0,
                NumberOfNotifications=0
            };
            if(obj.CreatedFor=="2")
            {
                model.StartTime = obj.StartTimeCustom;
                model.EndTime = obj.EndTimeCustom;

                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
            }

            if (obj.CreatedFor == "1")
            {
                DateTime Start = new DateTime(obj.StartDate.Year, obj.StartDate.Month, obj.StartDate.Day, obj.StartTimeOnce.Hour, obj.StartTimeOnce.Minute, obj.StartTimeOnce.Second);
                DateTime End = new DateTime(obj.EndDate.Year, obj.EndDate.Month, obj.EndDate.Day, obj.EndTimeOnce.Hour, obj.EndTimeOnce.Minute, obj.EndTimeOnce.Second);

                model.StartTime = Start;
                model.EndTime = End;

                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
            }

            _context.Add(model);
            _context.SaveChanges();

            int taskid = model.ID;
            string userid = null;
            userid=_context.User.Where(a => a.UserName == User).Select(a => a.Id).FirstOrDefault();
            if(userid!=null)
            {
                UserTask usertaskobj = new UserTask
                {
                    TaskId = taskid,
                    UserId=userid
                };
                _context.Add(usertaskobj);
                _context.SaveChanges();
                return model;
            }
            else
            {
                return null;
            }
        }

        private string GenerateKey()
        {
            string forreturn = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[64];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = "";
            foreach (var item in stringChars)
            {
                finalString += item;
            }

            forreturn = finalString;
            TaskDB same = null;

            same = _context.Tasks.Where(a => a.Key.ToLower() == finalString.ToLower()).FirstOrDefault();

            if (same != null)
                forreturn = GenerateKey();

            return forreturn;
        }

        private string GenerateKeyUnAuth(List<TaskDB> list)
        {
            string forreturn = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[64];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = "";
            foreach (var item in stringChars)
            {
                finalString += item;
            }

            forreturn = finalString;
            TaskDB same = null;

            same = list.Where(a => a.Key.ToLower() == finalString.ToLower()).FirstOrDefault();

            if (same != null)
                forreturn = GenerateKeyUnAuth(list);

            return forreturn;
        }

        private string GenerateFinalDaysString(NewTaskVM obj)
        {
            string final = "";

            if (obj.M == "true")
                final += Days[0];

            if (obj.T == "true")
                final += Days[1];

            if (obj.W == "true")
                final += Days[2];

            if (obj.Th == "true")
                final += Days[3];

            if (obj.F == "true")
                final += Days[4];

            if (obj.Sa == "true")
                final += Days[5];

            if (obj.S == "true")
                final += Days[6];

            return final;
        }

        private string GetNewStats(DateTime StartTime, DateTime EndTime, string Days,string CreatedFor,int NumberOfFinished)
        {
            DateTime current = CurrentTime;

            if (CreatedFor == "1")
            {
                if(((StartTime<current)||(StartTime.Year==current.Year&&StartTime.Month==current.Month&&StartTime.Day==current.Day&&StartTime.Hour==current.Hour&&StartTime.Minute==current.Minute))&&current<EndTime)
                {
                    return "In processing!";
                }
                if (current >= EndTime)
                {
                    return "Finished!";
                }
                else
                {
                    return "Upcoming!";
                }
            }
            else
            {
                var ActiveDay = "";
                var firsttime = 0;
                var numberofdays = 0;
                var IsHigher = false;
                var IsLower = false;
                if (EndTime < StartTime)
                {
                    IsLower = true;
                }

                DateTime current1 = CurrentTime;
                DateTime RealStart = CurrentTime;
                DateTime RealEnd= CurrentTime;

                var Open = false;
                var ActiveBefore = false;
                
                while (ActiveDay == "")
                {
                    if ((Days.Contains(current1.AddDays(-1).DayOfWeek.ToString()) == true && Days.Contains(current1.DayOfWeek.ToString()) == true)||(Days.Contains(current1.AddDays(-1).DayOfWeek.ToString()) == true && Days.Contains(current1.DayOfWeek.ToString()) == false))
                    {
                        ActiveBefore = true;
                    }
                    else
                    {
                        if (Days.Contains(current1.AddDays(-1).DayOfWeek.ToString()) == true && Days.Contains(current1.AddDays(-2).DayOfWeek.ToString()) == true)
                        {
                            ActiveBefore = true;
                        }
                    }

                    if (Days.Contains(current1.DayOfWeek.ToString())||(ActiveBefore == true&&IsLower==true))
                    {
                        if (firsttime == 0)
                        {
                            firsttime++;

                            DateTime current2 = CurrentTime;
                            current2.AddSeconds(-current2.Second);
                            RealEnd= new DateTime(current2.Year, current2.Month, current2.Day, EndTime.Hour, EndTime.Minute, 0);
                            
                            RealStart = new DateTime(current2.Year, current2.Month, current2.Day, StartTime.Hour, StartTime.Minute,0);
                            StartTime = RealStart;
                            EndTime = RealEnd;
                            

                            if (IsLower == true && ActiveBefore == true&&StartTime> current2)
                            {
                                StartTime = StartTime.AddDays(-1);
                                IsHigher = true;
                            }

                            if (IsLower == true && ActiveBefore == true && EndTime < current2)
                            {
                                StartTime = StartTime.AddDays(1);
                                IsHigher = false;
                            }

                            if (IsLower == true && EndTime<current2 && Days.Contains(current1.DayOfWeek.ToString()) == true)
                            {
                                ActiveDay = current1.DayOfWeek.ToString();
                                if (IsHigher == false)
                                {
                                    EndTime = EndTime.AddDays(1);
                                }
                            }

                            if (IsLower == false)
                            {
                                if (StartTime > current2)
                                {
                                    ActiveDay = current1.DayOfWeek.ToString();
                                }
                                if(StartTime<= current2&&EndTime>= current2)
                                {
                                    ActiveDay = current1.DayOfWeek.ToString();
                                }
                                else
                                {
                                    numberofdays++;
                                }
                            }
                            
                        }
                        else
                        {
                            ActiveDay = current1.DayOfWeek.ToString();
                        }
                    }
                    else
                    {
                        current1 = current1.AddDays(1);
                        numberofdays++;
                        firsttime++;
                    }
                }

                DateTime CurrentForCreating = CurrentTime;
                DateTime CurrentTimee = CurrentTime;
                if (IsHigher == false)
                {
                    CurrentForCreating = CurrentForCreating.AddDays(numberofdays);
                }
                else
                {
                    CurrentForCreating = CurrentForCreating.AddDays(numberofdays-1);
                }
                DateTime RealStartTime = new DateTime(CurrentForCreating.Year, CurrentForCreating.Month, CurrentForCreating.Day, StartTime.Hour, StartTime.Minute,0);
                DateTime RealEndTime = new DateTime(CurrentForCreating.Year, CurrentForCreating.Month, CurrentForCreating.Day, EndTime.Hour, EndTime.Minute,0);


                if (IsLower == true)
                {
                    RealEndTime = RealEndTime.AddDays(1);
                }

                TimeSpan diffDate = current.Subtract(RealStartTime);
                var DifferenceInMinutes =(diffDate.Days*24*60)+ (diffDate.Hours*60)+diffDate.Minutes;

                if (RealStartTime<= CurrentTimee&& RealEndTime> CurrentTimee)
                {
                    return "In processing!";
                }

                if (DifferenceInMinutes<-2880&&NumberOfFinished>0)
                {
                    return "Finished!";
                }
                else
                {
                    return "Upcoming!";
                }
            }
        }

        private bool IsAnyFinishedFromTheLastTime(DateTime StartTime,DateTime EndTime,string Days,DateTime LastTimeFinished,DateTime CreatedTime,int numberoffinished)
        {
            string ActiveDaySecond = "";
            int numberofdays = 0;
            LastTimeFinished = LastTimeFinished.AddSeconds(-LastTimeFinished.Second);
            DateTime LastTimeTemp = LastTimeFinished;
            if (LastTimeFinished < CreatedTime)
            {
                LastTimeTemp = LastTimeTemp.AddDays(1);
            }
            DateTime RealEndTime = new DateTime(LastTimeTemp.Year, LastTimeTemp.Month, LastTimeTemp.Day,EndTime.Hour,EndTime.Minute,0);
            DateTime RealEndTimeNext = new DateTime(LastTimeTemp.Year, LastTimeTemp.Month, LastTimeTemp.Day,EndTime.Hour,EndTime.Minute,0);
            
            var firsttime = 0;
            var IsLow = false;
            if (EndTime < StartTime)
            {
                IsLow = true;
            }

            DateTime Curr = CurrentTime;

            var ActiveBefore = false;
            var IsHigher = false;

            while (ActiveDaySecond=="")
            {
                if ((Days.Contains(RealEndTimeNext.AddDays(-1).DayOfWeek.ToString()) == true && Days.Contains(RealEndTimeNext.DayOfWeek.ToString()) == true)||(Days.Contains(RealEndTimeNext.AddDays(-1).DayOfWeek.ToString()) == true && Days.Contains(RealEndTimeNext.DayOfWeek.ToString()) == false))
                {
                    ActiveBefore = true;
                }
                else
                {
                    if (Days.Contains(RealEndTimeNext.AddDays(-1).DayOfWeek.ToString()) == true && Days.Contains(RealEndTimeNext.AddDays(-2).DayOfWeek.ToString()) == true)
                    {
                        ActiveBefore = true;
                    }
                }

                if (Days.Contains(RealEndTimeNext.DayOfWeek.ToString()) == true||(ActiveBefore == true && IsLow==true))
                {
                    if (firsttime == 0)
                    {
                        var IsLower = false;
                        if (EndTime < StartTime)
                        {
                            IsLower = true;
                        }

                        if (IsLower == true && ActiveBefore == true)
                        {
                            StartTime = StartTime.AddDays(-1);
                            IsHigher = true;
                        }

                        if (IsLower == true&&RealEndTimeNext<CurrentTime)
                        {
                            if (IsHigher == false)
                            {
                                RealEndTimeNext = RealEndTimeNext.AddDays(1);
                            }
                        }

                        var LastTimeToday = false;
                        if (LastTimeFinished.Year == CurrentTime.Year && LastTimeFinished.Month == CurrentTime.Month && LastTimeFinished.Day == CurrentTime.Day)
                        {
                            LastTimeToday = true;
                        }

                        var LastTimeEqualsLastTime = false;
                        if (LastTimeFinished.Year == RealEndTimeNext.Year && LastTimeFinished.Month == RealEndTimeNext.Month && LastTimeFinished.Day == RealEndTimeNext.Day)
                        {
                            LastTimeEqualsLastTime = true;
                        }

                        if (CurrentTime>=RealEndTimeNext&&LastTimeToday==false&& LastTimeEqualsLastTime==false)
                        {
                            ActiveDaySecond = "Active";
                        }
                        else
                        {
                            RealEndTimeNext =RealEndTimeNext.AddDays(1);
                            numberofdays++;
                            firsttime++;
                        }
                    }
                    else
                    {
                        numberofdays++;
                        ActiveDaySecond = "Active";
                    }
                }
                else
                {
                    RealEndTimeNext=RealEndTimeNext.AddDays(1);
                    firsttime++;
                    numberofdays++;
                }
            }
            DateTime RealCurrentTime = CurrentTime;
           
            if (RealEndTimeNext <= RealCurrentTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public TaskDB AddTaskUnAuth(NewTaskVM obj, List<TaskDB> list)
        {
            int notevery = 0;
            if (obj.Hours != 0)
            {
                notevery += (obj.Hours * 60);
            }
            notevery += obj.Minutes;
            TaskDB model = new TaskDB
            {
                TaskName = obj.TaskName,
                Description = obj.Description,
                CreateTime = CurrentTime,
                LastNotification = CurrentTime,
                Priority = Priority[int.Parse(obj.Priority) - 1],
                SoundName = Sounds[int.Parse(obj.Sound) - 1],
                NotificationEvery = notevery,
                Key = GenerateKeyUnAuth(list),
                CreatedFor = obj.CreatedFor,
                Successufull=-1,
                Days= GenerateFinalDaysString(obj),
                AcceptedNotification=false,
                Rated=false,
                NumberOfNotifications=0,
                NumberOfFinished=0,
                LastTimeFinished=CurrentTime.AddDays(-1)
            };
            if (obj.CreatedFor == "2")
            {
                model.StartTime = obj.StartTimeCustom;
                model.EndTime = obj.EndTimeCustom;

                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
            }

            if (obj.CreatedFor == "1")
            {
                DateTime Start = new DateTime(obj.StartDate.Year, obj.StartDate.Month, obj.StartDate.Day, obj.StartTimeOnce.Hour, obj.StartTimeOnce.Minute, obj.StartTimeOnce.Second);
                DateTime End = new DateTime(obj.EndDate.Year, obj.EndDate.Month, obj.EndDate.Day, obj.EndTimeOnce.Hour, obj.EndTimeOnce.Minute, obj.EndTimeOnce.Second);

                model.StartTime = Start;
                model.EndTime = End;

                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
            }

            return model;
        }

        public List<TaskPreviewVM> GetListTasks(string User, HomeFilterVM filter)
        {
            List<TaskDB> listoftasks= _context.UserTask.Include(a => a.User).Include(a => a.Task).Where(a => a.User.UserName == User).Select(a=>a.Task).ToList();

            foreach (var item in listoftasks)
            {
                TaskDB forchange = item;
                string currentstats = GetNewStats(item.StartTime, item.EndTime, item.Days, item.CreatedFor,item.NumberOfFinished);
                if (item.Stats != currentstats)
                {
                    forchange.AcceptedNotification = false;
                    forchange.Stats = currentstats;
                }

                if (forchange.CreatedFor == "2")
                {
                    if (IsAnyFinishedFromTheLastTime(forchange.StartTime, forchange.EndTime, forchange.Days, forchange.LastTimeFinished,forchange.CreateTime,forchange.NumberOfFinished)&&item.Stats!="In processing!")
                    {
                        forchange.Rated = false;
                        forchange.AcceptedNotification = false;
                    }
                    else
                    {
                        forchange.Rated = true;
                    }
                }
                _context.SaveChanges();
            }

            var list = _context.UserTask.Include(a => a.User).Include(a => a.Task).Where(a => a.User.UserName == User).ToList();

            if (filter.TaskName != null)
            {
                list = list.Where(a => a.Task.TaskName.ToLower().Contains(filter.TaskName.ToLower())).ToList();
            }
            if (filter.From != null && filter.From.Year>1000)
            {
                list = list.Where(a => IsHigherOrSame(a.Task.StartTime, filter.From) || a.Task.CreatedFor=="2").ToList();
            }
            if (filter.To != null && filter.To.Year > 1000)
            {
                list = list.Where(a => IsLowerOrSame(a.Task.StartTime, filter.To) || a.Task.CreatedFor == "2").ToList();
            }
            if (filter.TaskStatus != null)
            {
                if((filter.TaskStatus!= "All" || (filter.TaskStatus == "0" || filter.TaskStatus == "1")))
                {
                    if(filter.TaskStatus == "2"||filter.TaskStatus == "3"|| filter.TaskStatus == "4")
                    {
                        list = list.Where(a => a.Task.Stats== Statss[int.Parse(filter.TaskStatus)-2]).ToList();
                    }
                    else
                    {
                        list = list.ToList();
                    }
                }
            }

            List<TaskPreviewVM> finalelist = new List<TaskPreviewVM>();

            List<TaskPreviewVM> inprocesstasks= list.Where(a=>a.Task.Stats== "In processing!").Select(a => new TaskPreviewVM
            {
                Status = a.Task.Stats,
                TaskName = a.Task.TaskName,
                Key = a.Task.Key,
                TimeStarting = GetTimeStarting(a.Task.StartTime),
                DateDayStarting = GetFirstActiveDay(a.Task.Days, a.Task.StartTime, a.Task.CreatedFor,a.Task.EndTime,CurrentTime),
                Success = SuccessToDouble(a.Task.Successufull),
                CreatedFor=a.Task.CreatedFor,
                NumberOfFinished=a.Task.NumberOfFinished
            }).ToList();


            List<TaskPreviewVM> upcomingtasks = list.Where(a => a.Task.Stats == "Upcoming!").Select(a => new TaskPreviewVM
            {
                Status = a.Task.Stats,
                TaskName = a.Task.TaskName,
                Key = a.Task.Key,
                TimeStarting = GetTimeStarting(a.Task.StartTime),
                DateDayStarting = GetFirstActiveDay(a.Task.Days, a.Task.StartTime, a.Task.CreatedFor, a.Task.EndTime, CurrentTime),
                Success = SuccessToDouble(a.Task.Successufull),
                CreatedFor = a.Task.CreatedFor,
                NumberOfFinished = a.Task.NumberOfFinished
            }).ToList();


            List<TaskPreviewVM> finishedtasks = list.Where(a => a.Task.Stats == "Finished!").Select(a => new TaskPreviewVM
            {
                Status = a.Task.Stats,
                TaskName = a.Task.TaskName,
                Key = a.Task.Key,
                TimeStarting = GetTimeStarting(a.Task.StartTime),
                DateDayStarting = GetFirstActiveDay(a.Task.Days, a.Task.StartTime, a.Task.CreatedFor, a.Task.EndTime, CurrentTime),
                Success = SuccessToDouble(a.Task.Successufull),
                CreatedFor = a.Task.CreatedFor,
                NumberOfFinished = a.Task.NumberOfFinished
            }).ToList();


            foreach (var item in inprocesstasks)
            {
                finalelist.Add(item);
            }

            foreach (var item in upcomingtasks)
            {
                finalelist.Add(item);
            }

            foreach (var item in finishedtasks)
            {
                finalelist.Add(item);
            }

            return finalelist;
        }

        public List<TaskPreviewVM> GetListTasksUnAuth(List<TaskDB> listoftasks, HomeFilterVM filter)
        {
            var list = listoftasks;

            foreach (var item in list)
            {
                string currentstats = GetNewStats(item.StartTime, item.EndTime, item.Days, item.CreatedFor, item.NumberOfFinished);
                if (item.Stats != currentstats)
                {
                    item.AcceptedNotification = false;
                    item.Stats = currentstats;
                }

                if (item.CreatedFor == "2")
                {
                    if (IsAnyFinishedFromTheLastTime(item.StartTime, item.EndTime, item.Days, item.LastTimeFinished,item.CreateTime, item.NumberOfFinished) && item.Stats != "In processing!")
                    {
                        item.Rated = false;
                        item.AcceptedNotification = false;
                    }
                }
            }

            List<TaskPreviewVM> finalelist=new List<TaskPreviewVM>();
            if (list != null)
            {
                if (list.Any())
                {
                    if (filter.TaskName != null)
                    {
                        list = list.Where(a => a.TaskName.ToLower().Contains(filter.TaskName.ToLower())).ToList();
                    }
                    if (filter.From != null && filter.From.Year>1000)
                    {
                        list = list.Where(a => IsHigherOrSame(a.StartTime, filter.From) || a.CreatedFor=="2").ToList();
                    }
                    if (filter.To != null && filter.To.Year > 1000)
                    {
                        list = list.Where(a => IsLowerOrSame(a.StartTime, filter.To) || a.CreatedFor == "2").ToList();
                    }
                    if(filter.TaskStatus != null)
                    {
                        if ((filter.TaskStatus != "All" || (filter.TaskStatus == "0" || filter.TaskStatus == "1")))
                        {
                            if (filter.TaskStatus == "2" || filter.TaskStatus == "3" || filter.TaskStatus == "4")
                            {
                                list = list.Where(a => a.Stats == Statss[int.Parse(filter.TaskStatus) - 2]).ToList();
                            }
                            else
                            {
                                list = list.ToList();
                            }
                        }
                    }

                    List<TaskPreviewVM> inprocesstasks = list.Where(a => a.Stats == "In processing!").Select(a => new TaskPreviewVM
                    {
                        Status = a.Stats,
                        TaskName = a.TaskName,
                        Key = a.Key,
                        TimeStarting = GetTimeStarting(a.StartTime),
                        DateDayStarting = GetFirstActiveDay(a.Days, a.StartTime, a.CreatedFor,a.EndTime, CurrentTime),
                        Success = SuccessToDouble(a.Successufull),
                        CreatedFor=a.CreatedFor,
                        NumberOfFinished=a.NumberOfFinished
                    }).ToList();


                    List<TaskPreviewVM> upcomingtasks = list.Where(a => a.Stats == "Upcoming!").Select(a => new TaskPreviewVM
                    {
                        Status = a.Stats,
                        TaskName = a.TaskName,
                        Key = a.Key,
                        TimeStarting = GetTimeStarting(a.StartTime),
                        DateDayStarting = GetFirstActiveDay(a.Days, a.StartTime, a.CreatedFor, a.EndTime, CurrentTime),
                        Success = SuccessToDouble(a.Successufull),
                        CreatedFor = a.CreatedFor,
                        NumberOfFinished = a.NumberOfFinished
                    }).ToList();


                    List<TaskPreviewVM> finishedtasks = list.Where(a => a.Stats == "Finished!").Select(a => new TaskPreviewVM
                    {
                        Status = a.Stats,
                        TaskName = a.TaskName,
                        Key = a.Key,
                        TimeStarting = GetTimeStarting(a.StartTime),
                        DateDayStarting = GetFirstActiveDay(a.Days, a.StartTime, a.CreatedFor, a.EndTime, CurrentTime),
                        Success = SuccessToDouble(a.Successufull),
                        CreatedFor = a.CreatedFor,
                        NumberOfFinished = a.NumberOfFinished
                    }).ToList();


                    foreach (var item in inprocesstasks)
                    {
                        finalelist.Add(item);
                    }

                    foreach (var item in upcomingtasks)
                    {
                        finalelist.Add(item);
                    }

                    foreach (var item in finishedtasks)
                    {
                        finalelist.Add(item);
                    }
                }
            }
            return finalelist;
        }

        private static string SuccessToDouble(double number)
        {
            string percentage = (number*100).ToString();

            return percentage;
        }

        private string GetTimeStarting(DateTime a)
        {
            string time = "| ";
            if (a.Hour < 10)
            {
                time += "0" + a.Hour.ToString()+":";
            }
            else
            {
                time += a.Hour.ToString() + ":";
            }
            if (a.Minute < 10)
            {
                time += "0" + a.Minute.ToString();
            }
            else
            {
                time += a.Minute.ToString();
            }
            return time;
        }

        private static string GetFirstActiveDay(string Days, DateTime StartTime,string CreatedFor, DateTime EndTime,DateTime CurrentTimee)
        {
            string first = "";
            DateTime current = CurrentTimee;
            if (CreatedFor == "2")
            {
                var ActiveDay = "";
                var firsttime = 0;
                
                DateTime current1 = CurrentTimee;

                
                while (ActiveDay == "")
                {
                    if (Days.Contains(current1.DayOfWeek.ToString()))
                    {
                        if (firsttime == 0)
                        {
                            firsttime++;

                            DateTime current2 = CurrentTimee;
                            DateTime RealEnd = new DateTime(current2.Year, current2.Month, current2.Day, EndTime.Hour, EndTime.Minute, 0);
                            
                            DateTime RealStart = new DateTime(current2.Year, current2.Month, current2.Day, StartTime.Hour, StartTime.Minute, 0);
                            StartTime = RealStart;

                            if(Days.Contains(current2.AddDays(-1).DayOfWeek.ToString()) && RealEnd > current2  && EndTime < StartTime)
                            {
                                StartTime = StartTime.AddDays(-1);
                            }
                            else
                            {
                                if (EndTime < StartTime)
                                {
                                    RealEnd = RealEnd.AddDays(1);
                                }
                            }

                            EndTime = RealEnd;
                            if(RealStart>current2)
                            {
                                ActiveDay = current1.DayOfWeek.ToString();
                            }
                            else
                            {
                                if (((StartTime < current2) || (StartTime.Year == current2.Year && StartTime.Month == current2.Month && StartTime.Day == current2.Day && StartTime.Hour == current2.Hour && StartTime.Minute == current2.Minute)) && current2 < EndTime)
                                {
                                    ActiveDay = current1.DayOfWeek.ToString();
                                }
                                else
                                {
                                    current1 = current1.AddDays(1);
                                }
                            }
                            
                        }
                        else
                        {
                            ActiveDay = current1.DayOfWeek.ToString();
                        }
                    }
                    else
                    {
                        current1 = current1.AddDays(1);
                        firsttime++;
                    }
                }

                return ActiveDay;
            }
            else
            {
                first = StartTime.Date.ToShortDateString();
            }

            return first;
        }

        private bool IsHigherOrSame(DateTime Start,DateTime From)
        {
            DateTime startt = new DateTime(Start.Year, Start.Month, Start.Day);
            DateTime Fromm = new DateTime(From.Year, From.Month, From.Day);

            if (startt >= Fromm)
                return true;
            return false;
        }

        private bool IsLowerOrSame(DateTime EndDate, DateTime To)
        {
            DateTime End = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day);
            DateTime Too = new DateTime(To.Year, To.Month, To.Day);

            if (End <= Too)
                return true;
            return false;
        }

        public string SuccessNumber(List<TaskPreviewVM> list)
        {
            string number = "N/A";
            List<TaskPreviewVM> finishedtasks=null;
            if (list.Any())
            {
                finishedtasks = list.Where(a => a.Status == "Finished!" || (a.CreatedFor=="2" && a.NumberOfFinished>0)).ToList();
            }
            if (list != null)
            {
                if (finishedtasks!=null)
                {
                    if (finishedtasks.Any())
                    {
                        decimal totalnumber= finishedtasks.Count();
                        decimal numberofsuccesscustom=0;
                        var oncefinishedtasks = finishedtasks.Where(a => a.CreatedFor == "1").ToList();
                        decimal totalnumberonce = oncefinishedtasks.Count();
                        decimal numberofsuccessonce = 0;
                        foreach (var itemonce in oncefinishedtasks)
                        {
                            if (decimal.Round(decimal.Parse(itemonce.Success),1) == 100)
                            {
                                numberofsuccessonce += 100;
                            }
                        }
                        decimal numberofonce = 100;
                        if (totalnumberonce > 0)
                        {
                            numberofonce=Decimal.Round(numberofsuccessonce / totalnumberonce, 1);
                        }
                        var numberofcustom = finishedtasks.Where(a => a.CreatedFor == "2" && a.NumberOfFinished>0).Count();
                        foreach (var item in finishedtasks)
                        {
                            if (item.CreatedFor == "2" && item.NumberOfFinished>0)
                            {
                                numberofsuccesscustom += decimal.Parse(item.Success);
                            }
                        }
                        decimal numberofcustomperc = 100;
                        if (numberofcustom>0)
                        {
                            numberofcustomperc=Decimal.Round(numberofsuccesscustom / numberofcustom, 1);
                        }
                        if (totalnumberonce > 0 && numberofcustom > 0)
                        {
                            number = Decimal.Round((numberofcustomperc + numberofonce) / 2,1).ToString()+" %";
                        }

                        if (totalnumberonce == 0 && numberofcustom > 0)
                        {
                            number = Decimal.Round(numberofcustomperc, 1).ToString() + " %";
                        }

                        if (totalnumberonce==0&&numberofcustom==0)
                        {
                            number = "N/A";
                        }
                        if (totalnumberonce > 0 && numberofcustom == 0)
                        {
                            number = Decimal.Round(numberofonce, 1).ToString() + " %";
                        }
                    }
                }
            }
            return number;
        }

        private static string IsActiveDay(string Day, string ListOfDays)
        {
            if (ListOfDays != null)
            {
                if (ListOfDays.Contains(Day))
                    return "true";
            }
            return "false";
        }

        public NewTaskVM EditTask(string Key,string UserName)
        {
            NewTaskVM model = null;
            model = _context.UserTask.Include(a=>a.Task).Include(a=>a.User).Where(a => a.Task.Key == Key && a.User.UserName==UserName).Select(a => new NewTaskVM
            {
                CreatedFor = a.Task.CreatedFor,
                Description = a.Task.Description,
                EndDate = a.Task.EndTime,
                EndTimeCustom = a.Task.EndTime,
                EndTimeOnce = a.Task.EndTime,
                F = IsActiveDay(Days[4], a.Task.Days),
                Hours = a.Task.NotificationEvery / 60,
                M= IsActiveDay(Days[0], a.Task.Days),
                Minutes=a.Task.NotificationEvery -((a.Task.NotificationEvery / 60)*60),
                Priority=a.Task.Priority,
                S= IsActiveDay(Days[6], a.Task.Days),
                Sa= IsActiveDay(Days[5], a.Task.Days),
                Sound=a.Task.SoundName,
                StartDate=a.Task.StartTime,
                StartTimeCustom=a.Task.StartTime,
                StartTimeOnce=a.Task.StartTime,
                T= IsActiveDay(Days[1], a.Task.Days),
                TaskName=a.Task.TaskName,
                Th= IsActiveDay(Days[3], a.Task.Days),
                User=UserName,
                W= IsActiveDay(Days[2], a.Task.Days),
                Key=a.Task.Key
            }).FirstOrDefault();

            return model;
        }

        public NewTaskVM EditTaskUnAuth(string Key,List<TaskDB>list)
        {
            NewTaskVM model = null;
            model=list.Where(a => a.Key == Key).Select(a => new NewTaskVM
            {
                CreatedFor = a.CreatedFor,
                Description = a.Description,
                EndDate = a.EndTime,
                EndTimeCustom = a.EndTime,
                EndTimeOnce = a.EndTime,
                F = IsActiveDay(Days[4], a.Days),
                Hours = a.NotificationEvery / 60,
                M = IsActiveDay(Days[0], a.Days),
                Minutes = a.NotificationEvery - ((a.NotificationEvery / 60) * 60),
                Priority = a.Priority,
                S = IsActiveDay(Days[6], a.Days),
                Sa = IsActiveDay(Days[5], a.Days),
                Sound = a.SoundName,
                StartDate = a.StartTime,
                StartTimeCustom = a.StartTime,
                StartTimeOnce = a.StartTime,
                T = IsActiveDay(Days[1], a.Days),
                TaskName = a.TaskName,
                Th = IsActiveDay(Days[3], a.Days),
                W = IsActiveDay(Days[2], a.Days),
                Key=a.Key
            }).FirstOrDefault();

            return model;
        }

        public TaskDB SaveTaskChanges(NewTaskVM obj, string User)
        {
            int notevery = 0;
            if (obj.Hours != 0)
            {
                notevery += (obj.Hours * 60);
            }
            notevery += obj.Minutes;
            TaskDB model = null;
            model=_context.Tasks.Where(a => a.Key == obj.Key).FirstOrDefault();
            if (model != null)
            { 
                model.TaskName = obj.TaskName;
                model.Description = obj.Description;
                model.Priority = Priority[int.Parse(obj.Priority) - 1];
                model.SoundName = Sounds[int.Parse(obj.Sound) - 1];
                model.NotificationEvery = notevery;
                model.CreatedFor = obj.CreatedFor;
                model.Days= GenerateFinalDaysString(obj);
                model.LastNotification = CurrentTime;
                model.Successufull = -1;
                model.AcceptedNotification = true;
                model.Rated = false;
                model.LastTimeFinished = CurrentTime.AddDays(-1);
                model.NumberOfFinished = 0;

                if (obj.CreatedFor == "2")
                {
                    model.StartTime = obj.StartTimeCustom;
                    model.EndTime = obj.EndTimeCustom;

                    model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
                }

                if (obj.CreatedFor == "1")
                {
                    DateTime Start = new DateTime(obj.StartDate.Year, obj.StartDate.Month, obj.StartDate.Day, obj.StartTimeOnce.Hour, obj.StartTimeOnce.Minute, obj.StartTimeOnce.Second);
                    DateTime End = new DateTime(obj.EndDate.Year, obj.EndDate.Month, obj.EndDate.Day, obj.EndTimeOnce.Hour, obj.EndTimeOnce.Minute, obj.EndTimeOnce.Second);

                    model.StartTime = Start;
                    model.EndTime = End;

                    model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
                }
            }
            _context.SaveChanges();
            return model;
        }

        public TaskDB SaveTaskChangesUnAuth(NewTaskVM obj, List<TaskDB> list)
        {
            int notevery = 0;
            if (obj.Hours != 0)
            {
                notevery += (obj.Hours * 60);
            }
            notevery += obj.Minutes;
            TaskDB model = list.Where(a => a.Key == obj.Key).FirstOrDefault();

            model.TaskName = obj.TaskName;
            model.Description = obj.Description;
            model.Priority = Priority[int.Parse(obj.Priority) - 1];
            model.SoundName = Sounds[int.Parse(obj.Sound) - 1];
            model.NotificationEvery = notevery;
            model.CreatedFor = obj.CreatedFor;
            model.Days= GenerateFinalDaysString(obj);
            model.LastNotification = CurrentTime;
            model.Successufull = -1;
            model.AcceptedNotification = true;
            model.Rated = false;
            model.LastTimeFinished = CurrentTime.AddDays(-1);
            model.NumberOfFinished = 0;

            if (obj.CreatedFor == "2")
            {
                model.StartTime = obj.StartTimeCustom;
                model.EndTime = obj.EndTimeCustom;

                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
            }

            if (obj.CreatedFor == "1")
            {
                DateTime Start = new DateTime(obj.StartDate.Year, obj.StartDate.Month, obj.StartDate.Day, obj.StartTimeOnce.Hour, obj.StartTimeOnce.Minute, obj.StartTimeOnce.Second);
                DateTime End = new DateTime(obj.EndDate.Year, obj.EndDate.Month, obj.EndDate.Day, obj.EndTimeOnce.Hour, obj.EndTimeOnce.Minute, obj.EndTimeOnce.Second);

                model.StartTime = Start;
                model.EndTime = End;

                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
            }

            return model;
        }

        public TaskDB DeleteTask(string Key,string UserName)
        {
            UserTask model = null;
            TaskDB model2 = null;
            model = _context.UserTask.Include(a=>a.User).Include(a=>a.Task).Where(a => a.Task.Key==Key && a.User.UserName==UserName).FirstOrDefault();
            if (model != null)
            {
                model2 = model.Task;
                _context.Remove(model);
                _context.Remove(model2);
                _context.SaveChanges();
            }
            return model2;
        }

        public List<TaskDB> DeleteTaskUnAuth(string Key, List<TaskDB> lista)
        {
            TaskDB model = null;
            model = lista.Where(a => a.Key == Key).FirstOrDefault();
            if (model != null)
            {
                List<TaskDB> temp = new List<TaskDB>();
                foreach (var item in lista)
                {
                    if (item.Key != model.Key)
                    {
                        temp.Add(item);
                    }
                }
                if (temp.Count > 0)
                {
                    return temp;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<TaskNotificationVM> ListTaskNotification(string User)
        {
            var list = _context.UserTask.Include(a => a.User).Include(a => a.Task).Where(a => a.User.UserName == User && (a.Task.Stats!="Finished!"|| (a.Task.AcceptedNotification == false && a.Task.Stats == "Finished!") || (a.Task.Rated == false && a.Task.Stats == "Finished!") || (a.Task.CreatedFor == "2"))).Select(a=>a.Task).ToList();

            var currentlist = list;

            foreach (var item in currentlist)
            {
                string currentstats = GetNewStats(item.StartTime, item.EndTime, item.Days, item.CreatedFor, item.NumberOfFinished);
                if (item.Stats != currentstats)
                {
                    item.AcceptedNotification = false;
                    item.Stats = currentstats;
                }

                if (item.CreatedFor == "2")
                {
                    if (IsAnyFinishedFromTheLastTime(item.StartTime, item.EndTime, item.Days, item.LastTimeFinished, item.CreateTime, item.NumberOfFinished) && item.Stats != "In processing!")
                    {
                        item.Rated = false;
                        item.AcceptedNotification = false;
                    }
                    else
                    {
                        item.Rated = true;
                    }
                }
            }

            List<TaskNotificationVM> listforreturn = new List<TaskNotificationVM>();

            listforreturn = currentlist.Where(a => (a.Stats != "Finished!" || (a.AcceptedNotification == false && a.Stats == "Finished!") || (a.Rated == false && a.Stats == "Finished!") || (a.CreatedFor == "2"))).Select(a => new TaskNotificationVM
            {
                CreatedFor = a.CreatedFor,
                Days = a.Days,
                EndTime = a.EndTime,
                Key = a.Key,
                LastNotification = a.LastNotification,
                NotificationEvery = a.NotificationEvery,
                Priority = a.Priority,
                SoundName = a.SoundName,
                StartTime = a.StartTime,
                Stats = a.Stats,
                TaskName = a.TaskName,
                AcceptedNotification = a.AcceptedNotification,
                Rated = a.Rated
            }).ToList();

            return listforreturn;
        }

        public TaskDB SetLastNotification(string Key,string Value, string UserName,string Success)
        {
            TaskDB model = null;

            model = _context.UserTask.Include(a => a.User).Include(a => a.Task).Where(a => a.User.UserName == UserName && a.Task.Key == Key).Select(a => a.Task).FirstOrDefault();

            if (model != null)
            {
                model.LastNotification = CurrentTime;
                model.AcceptedNotification = true;
                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
                if (Value == "Finishing" || Value == "Finished")
                {
                    model.Rated = true;
                    if (model.CreatedFor == "1")
                    {
                        model.NumberOfFinished = 1;
                        if (Success == "true")
                        {
                            model.Successufull = 1;
                        }
                        else
                        {
                            model.Successufull =0;
                        }
                    }
                    else
                    {
                        model.LastTimeFinished = CurrentTime;
                        model.NumberOfFinished = model.NumberOfFinished + 1;
                        if (model.Successufull == -1)
                        {
                            if (Success == "true")
                            {
                                model.Successufull = 1;
                            }
                            else
                            {
                                model.Successufull = 0;
                            }
                        }
                        else
                        {
                            if (Success == "true")
                            {
                                model.Successufull = (model.Successufull + 1) / model.NumberOfFinished;
                            }
                            else
                            {
                                model.Successufull = (model.Successufull + 0) / model.NumberOfFinished;
                            }
                            
                        }
                    }
                }
                
                model.NumberOfNotifications = model.NumberOfNotifications + 1;
                _context.SaveChanges();
            }

            return model;
        }

        public TaskDB SetLastNotificationUnAuth(string Key, string Value, List<TaskDB> currentlist, string Success)
        {
            TaskDB model = null;

            model = currentlist.Where(a => a.Key == Key).FirstOrDefault();

            if (model != null)
            {
                model.LastNotification = CurrentTime;
                model.AcceptedNotification = true;
                model.Stats = GetNewStats(model.StartTime, model.EndTime, model.Days, model.CreatedFor, model.NumberOfFinished);
                if (Value == "Finishing" || Value == "Finished")
                {
                    model.Rated = true;
                    if (model.CreatedFor == "1")
                    {
                        model.NumberOfFinished = 1;
                        if (Success == "true")
                        {
                            model.Successufull = 1;
                        }
                        else
                        {
                            model.Successufull = 0;
                        }
                    }
                    else
                    {
                        model.LastTimeFinished = CurrentTime;
                        model.NumberOfFinished = model.NumberOfFinished + 1;
                        if (model.Successufull == -1)
                        {
                            if (Success == "true")
                            {
                                model.Successufull = 1;
                            }
                            else
                            {
                                model.Successufull = 0;
                            }
                        }
                        else
                        {
                            if (Success == "true")
                            {
                                model.Successufull = (model.Successufull + 1) / model.NumberOfFinished;
                            }
                            else
                            {
                                model.Successufull = (model.Successufull + 0) / model.NumberOfFinished;
                            }

                        }
                    }
                }

                model.NumberOfNotifications = model.NumberOfNotifications + 1;
            }

            return model;
        }

        public List<TaskNotificationVM> ChangeCurrentUnAuthList(List<TaskDB> list)
        {
            var currentlist = list;

            foreach (var item in currentlist)
            {
                string currentstats = GetNewStats(item.StartTime, item.EndTime, item.Days, item.CreatedFor, item.NumberOfFinished);
                if (item.Stats != currentstats)
                {
                    item.AcceptedNotification = false;
                    item.Stats = currentstats;
                }

                if (item.CreatedFor == "2")
                {
                    if (IsAnyFinishedFromTheLastTime(item.StartTime, item.EndTime, item.Days, item.LastTimeFinished,item.CreateTime, item.NumberOfFinished) && item.Stats != "In processing!")
                    {
                        item.Rated = false;
                        item.AcceptedNotification = false;
                    }
                    else
                    {
                        item.Rated = true;
                    }
                }
            }

            List<TaskNotificationVM> listforreturn = new List<TaskNotificationVM>();

            listforreturn = currentlist.Where(a => (a.Stats != "Finished!" || (a.AcceptedNotification == false && a.Stats == "Finished!") || (a.Rated == false && a.Stats == "Finished!")||(a.CreatedFor=="2"))).Select(a => new TaskNotificationVM
            {
                CreatedFor = a.CreatedFor,
                Days = a.Days,
                EndTime = a.EndTime,
                Key = a.Key,
                LastNotification = a.LastNotification,
                NotificationEvery = a.NotificationEvery,
                Priority = a.Priority,
                SoundName = a.SoundName,
                StartTime = a.StartTime,
                Stats = a.Stats,
                TaskName = a.TaskName,
                AcceptedNotification = a.AcceptedNotification,
                Rated = a.Rated
            }).ToList();

            return listforreturn;
        }

        public void SetCurrentTime(DateTime CurrentTimeee)
        {
            CurrentTime = CurrentTimeee;
        }
    }
}
