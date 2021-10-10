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
    public class AlarmService : IAlarmService
    {
        private readonly AppDbContext _context;

        private DateTime CurrentTime;

        private readonly List<string> Sounds = new List<string> {
                "AlarmSound1",
                "AlarmSound2",
                "AlarmSound3",
                "AlarmSound4",
                "AlarmSound5",
                "AlarmSound6",
                "AlarmSound7"
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

        private readonly List<string> RingDur = new List<string> {
                "1",
                "3",
                "5",
                "10",
                "15",
                "20",
                "30"
        };

        private readonly List<string> SnoozeDur = new List<string> {
                "5",
                "10",
                "15",
                "20",
                "25",
                "30"
        };

        public AlarmService(AppDbContext context)
        {
            _context = context;
        }

        public string AddAlarm(NewAlarmVM obj, string User)
        {
            Alarms model = new Alarms
            {
                AlarmName = obj.AlarmName,
                CreateTime = CurrentTime,
                RingingTime = obj.RingingTime,
                SoundName=Sounds[int.Parse(obj.Sound) - 1],
                Key=GenerateKey(),
                CreatedFor=obj.CreatedFor,
                Active=true,
                RingDuration= int.Parse(obj.RingingDuration),
                SnoozeDuration= int.Parse(obj.SnoozeDuration),
                Days= GenerateFinalDaysString(obj),
                LastSnooze= CurrentTime,
                LastRinging= CurrentTime.AddDays(-1)
            };
            _context.Add(model);
            _context.SaveChanges();

            int alarmid = model.ID;
            string userid = null;
            userid=_context.User.Where(a => a.UserName == User).Select(a => a.Id).FirstOrDefault();

            if (userid != null)
            {
                UserAlarm useralarmobj = new UserAlarm
                {
                    AlarmId = alarmid,
                    UserId = userid
                };
                _context.Add(useralarmobj);
                _context.SaveChanges();
            }

            return userid;
        }

        public Alarms AddAlarmUnAuth(NewAlarmVM obj, List<Alarms> list)
        {
            Alarms model = new Alarms
            {
                AlarmName = obj.AlarmName,
                CreateTime = CurrentTime,
                RingingTime = obj.RingingTime,
                SoundName = Sounds[int.Parse(obj.Sound) - 1],
                Key = GenerateKeyUnAuth(list),
                CreatedFor = obj.CreatedFor,
                Active = true,
                RingDuration = int.Parse(obj.RingingDuration),
                SnoozeDuration = int.Parse(obj.SnoozeDuration),
                Days= GenerateFinalDaysString(obj),
                LastSnooze= CurrentTime,
                LastRinging= CurrentTime.AddDays(-1)
            };

            return model;
        }

        private string GenerateName(int numberoftasks,string Lang)
        {
            string name = "Alarm";
            if (Lang == "ja-JP")
            {
                name = "警報";
            }
            if (Lang == "th-TH")
            {
                name = "เตือน";
            }
            if (Lang == "pt-BR")
            {
                name = "Alarme";
            }
            if (Lang == "vi-VN")
            {
                name = "Báo thức";
            }
            if (Lang == "uk-UA")
            {
                name = "Сигналізація";
            }
            name += "[" + (numberoftasks + 1).ToString() + "]";
            return name;
        }

        private bool IfExist(List<UserAlarm> list,string AlarmName)
        {
            foreach (var item in list)
            {
                if (item.Alarms.AlarmName == AlarmName)
                {
                    return true;
                }
            }
            return false;
        }

        public string GenerateAlarmName(string UserName,string Language)
        {
            var listofalarms= _context.UserAlarm.Include(a => a.User).Include(a=>a.Alarms).Where(a => a.User.UserName == UserName).ToList();
            string name = "Alarm";
            if (Language == "ja-JP")
            {
                name = "警報";
            }
            if (Language == "th-TH")
            {
                name = "เตือน";
            }
            if (Language == "pt-BR")
            {
                name = "Alarme";
            }
            if (Language == "vi-VN")
            {
                name = "Báo thức";
            }
            if (Language == "uk-UA")
            {
                name = "Сигналізація";
            }
            int numberoftasks = listofalarms.Count();
            if (numberoftasks >= 1)
            {
                name += "[" + (numberoftasks + 1).ToString() + "]";
            }
            else
            {
                name+= "[1]";
            }
            while (IfExist(listofalarms,name)==true)
            {
                numberoftasks++;
                name = GenerateName(numberoftasks,Language);
            }


            return name;
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

        private string GenerateKeyUnAuth(List<Alarms> list)
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
            Alarms same = null;

            if (list!=null)
            {
                same = list.Where(a => a.Key.ToLower() == finalString.ToLower()).FirstOrDefault();
            }

            if (same != null)
                forreturn = GenerateKeyUnAuth(list);

            return forreturn;
        }

        private string GenerateFinalDaysString(NewAlarmVM obj)
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

        public List<AlarmPreviewVM> GetListAlarms(string UserName, HomeFilterVM filter)
        {
            var list = _context.UserAlarm.Include(a => a.User).Include(a => a.Alarms).Where(a => a.User.UserName == UserName).ToList();

            foreach (var item in list)
            {
                DateTime Current = CurrentTime;

                Alarms forchange = item.Alarms;
                forchange.RingingTime = new DateTime(Current.Year, Current.Month, Current.Day, forchange.RingingTime.Hour, forchange.RingingTime.Minute,0);

                TimeSpan diffDate = Current.Subtract(forchange.LastRinging);

                var DifferenceInMinutes = (diffDate.Days * 24 * 60) + (diffDate.Hours * 60) + diffDate.Minutes;
                if (DifferenceInMinutes > forchange.SnoozeDuration)
                {
                    forchange.LastSnooze = forchange.RingingTime;
                }

                _context.SaveChanges();
            }

            if (filter.AlarmStatus != "All" && filter.AlarmStatus != null && filter.AlarmStatus!="0" && filter.AlarmStatus != "1")
            {
                if (filter.AlarmStatus == "2")
                {
                    list = list.Where(a => a.Alarms.Active == true).ToList();
                }
                else
                {
                    list = list.Where(a => a.Alarms.Active == false).ToList();
                }
            }
            List<AlarmPreviewVM> finalelist = new List<AlarmPreviewVM>();
            finalelist = list.Select(a => new AlarmPreviewVM
            {
                AlarmName=a.Alarms.AlarmName,
                Key=a.Alarms.Key,
                Status=GenerateAlarmStatus(a.Alarms.Active),
                TimeStarting = GetTimeStarting(a.Alarms.RingingTime),
                DateDayStarting = GetFirstActiveDay(a.Alarms.Days, a.Alarms.RingingTime,a.Alarms.CreatedFor,CurrentTime),
                Days=a.Alarms.Days,
                CreatedFor=a.Alarms.CreatedFor
            }).ToList();

            if (finalelist.Count > 0)
            {
                if (AnyActiveFilterDay(filter))
                {
                    finalelist = finalelist.Where(a => ContainsFilterDays(a.Days, filter, a.CreatedFor, Days,a.DateDayStarting) == true).ToList();
                }
            }

            return finalelist;
        }

        public List<AlarmPreviewVM> GetListAlarmsUnAuth(List<Alarms> listofalarms, HomeFilterVM filter)
        {
            var list = listofalarms;

            foreach (var item in list)
            {
                DateTime Current = CurrentTime;

                Alarms forchange = item;
                forchange.RingingTime = new DateTime(Current.Year, Current.Month, Current.Day, forchange.RingingTime.Hour, forchange.RingingTime.Minute, 0);

                TimeSpan diffDate = Current.Subtract(forchange.LastRinging);

                var DifferenceInMinutes = (diffDate.Days * 24 * 60) + (diffDate.Hours * 60) + diffDate.Minutes;
                if (DifferenceInMinutes > forchange.SnoozeDuration)
                {
                    forchange.LastSnooze = forchange.RingingTime;
                }
            }

            List<AlarmPreviewVM> finalelist = new List<AlarmPreviewVM>();
            if (listofalarms != null)
            {
                if (listofalarms.Any())
                {
                    if (filter.AlarmStatus != "All" && filter.AlarmStatus != null && filter.AlarmStatus != "0" && filter.AlarmStatus != "1")
                    {
                        if (filter.AlarmStatus == "2")
                        {
                            list = list.Where(a => a.Active == true).ToList();
                        }
                        else
                        {
                            list = list.Where(a => a.Active == false).ToList();
                        }
                    }
                    finalelist = list.Select(a => new AlarmPreviewVM
                    {
                        AlarmName = a.AlarmName,
                        Key = a.Key,
                        Status = GenerateAlarmStatus(a.Active),
                        TimeStarting = GetTimeStarting(a.RingingTime),
                        DateDayStarting = GetFirstActiveDay(a.Days, a.RingingTime,a.CreatedFor,CurrentTime)
                    }).ToList();

                    if (finalelist.Count > 0)
                    {
                        if (AnyActiveFilterDay(filter))
                        {
                            finalelist = finalelist.Where(a => ContainsFilterDays(a.Days, filter, a.CreatedFor, Days, a.DateDayStarting) == true).ToList();
                        }
                    }
                }
            }
            return finalelist;
        }

        private static string GetTimeStarting(DateTime a)
        {
            string time = "| ";
            if (a.Hour < 10)
            {
                time += "0" + a.Hour.ToString() + ":";
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

        private static string GetFirstActiveDay(string Days, DateTime Start, string CreatedFor,DateTime CurrTime)
        {
            string first = "";
            if (CreatedFor == "2")
            {
                DateTime current = CurrTime;
                while (first == "")
                {
                    if (Days.Contains(current.DayOfWeek.ToString()))
                    {
                        if (current.DayOfWeek != CurrTime.DayOfWeek)
                        {
                            first = current.DayOfWeek.ToString();
                            return first;
                        }
                        if (Start.Hour > current.Hour)
                        {
                            first = current.DayOfWeek.ToString();
                            return first;
                        }
                        if (Start.Hour < current.Hour)
                        {
                            current = current.AddDays(1);
                            first = current.DayOfWeek.ToString();
                            return first;
                        }
                        if (Start.Hour == current.Hour)
                        {
                            if (Start.Minute >= current.Minute)
                            {
                                first = current.DayOfWeek.ToString();
                                return first;
                            }
                        }
                        current = current.AddDays(1);
                        first = current.DayOfWeek.ToString();
                        return first;
                    }
                    else
                    {
                        current = current.AddDays(1);
                    }
                }
            }
            else
            {
                DateTime curr = CurrTime;
                if (Start.Hour > curr.Hour)
                {
                    first = curr.DayOfWeek.ToString();
                    return first;
                }

                if (Start.Hour == curr.Hour)
                {
                    if (Start.Minute >= curr.Minute)
                    {
                        first = curr.DayOfWeek.ToString();
                        return first;
                    }
                }

                curr = curr.AddDays(1);
                first = curr.DayOfWeek.ToString();
            }

            return first;
        }

        private static string GenerateAlarmStatus(bool opt)
        {
            if (opt == true)
                return "Active";

            return "Inactive";
        }

        private bool AnyActiveFilterDay(HomeFilterVM obj)
        {
            if (obj.M == "true")
                return true;

            if (obj.T == "true")
                return true;

            if (obj.W == "true")
                return true;

            if (obj.Th == "true")
                return true;

            if (obj.F == "true")
                return true;

            if (obj.Sa == "true")
                return true;

            if (obj.S == "true")
                return true;

            return false;
        }

        private static bool ContainsFilterDays(string d, HomeFilterVM obj,string CreatedFor,List<string> Days, string ActiveDay)
        {
            if (d != null&&CreatedFor=="2")
            {
                if (obj.M == "true")
                {
                    if (d.ToLower().Contains(Days[0].ToLower()))
                        return true;
                }
                if (obj.T == "true")
                {
                    if (d.ToLower().Contains(Days[1].ToLower()))
                        return true;
                }
                if (obj.W == "true")
                {
                    if (d.ToLower().Contains(Days[2].ToLower()))
                        return true;
                }
                if (obj.Th == "true")
                {
                    if (d.ToLower().Contains(Days[3].ToLower()))
                        return true;
                }
                if (obj.F == "true")
                {
                    if (d.ToLower().Contains(Days[4].ToLower()))
                        return true;
                }
                if (obj.Sa == "true")
                {
                    if (d.ToLower().Contains(Days[5].ToLower()))
                        return true;
                }
                if (obj.S == "true")
                {
                    if (d.ToLower().Contains(Days[6].ToLower()))
                        return true;
                }
            }
            else
            {
                if (ActiveDay != null)
                {
                    if (obj.M == "true")
                    {
                        if (ActiveDay==Days[0])
                            return true;
                    }
                    if (obj.T == "true")
                    {
                        if (ActiveDay == Days[1])
                            return true;
                    }
                    if (obj.W == "true")
                    {
                        if (ActiveDay == Days[2])
                            return true;
                    }
                    if (obj.Th == "true")
                    {
                        if (ActiveDay == Days[3])
                            return true;
                    }
                    if (obj.F == "true")
                    {
                        if (ActiveDay == Days[4])
                            return true;
                    }
                    if (obj.Sa == "true")
                    {
                        if (ActiveDay == Days[5])
                            return true;
                    }
                    if (obj.S == "true")
                    {
                        if (ActiveDay == Days[6])
                            return true;
                    }
                }
            }
            return false;
        }

        public Alarms DeleteAlarm(string Key, string UserName)
        {
            UserAlarm model = null;
            Alarms model2 = null;
            model = _context.UserAlarm.Include(a => a.User).Include(a => a.Alarms).Where(a => a.Alarms.Key == Key && a.User.UserName == UserName).FirstOrDefault();
            if (model != null)
            {
                model2 = model.Alarms;
                _context.Remove(model);
                _context.Remove(model2);
                _context.SaveChanges();
            }
            return model2;
        }

        public List<Alarms> DeleteAlarmUnAuth(string Key, List<Alarms> lista)
        {
            Alarms model = null;
            model = lista.Where(a => a.Key == Key).FirstOrDefault();
            if (model != null)
            {
                List<Alarms> temp = new List<Alarms>();
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

        private static string IsActiveDay(string Day, string ListOfDays)
        {
            if (ListOfDays != null)
            {
                if (ListOfDays.Contains(Day))
                    return "true";
            }
            return "false";
        }

        private static string GetRingingDuration(int number, List<string> SnoozeDur)
        {
            foreach (var item in SnoozeDur)
            {
                if (int.Parse(item) == number)
                    return item;
            }
            return null;
        }

        private static string GetSnoozeDuration(int number, List<string> RingDur)
        {
            foreach (var item in RingDur)
            {
                if (int.Parse(item) == number)
                    return item;
            }
            return null;
        }

        private static string GetActivity(bool active)
        {
            if (active)
                return "true";
            return "false";
        }

        private static bool GetActivityBool(string active)
        {
            if (active=="1")
                return true;
            return false;
        }

        public NewAlarmVM EditAlarm(string Key, string UserName)
        {
            NewAlarmVM model = null;
            model = _context.UserAlarm.Include(a => a.Alarms).Include(a => a.User).Where(a => a.Alarms.Key == Key && a.User.UserName == UserName).Select(a => new NewAlarmVM
            {
                RingingDuration = GetRingingDuration(a.Alarms.RingDuration, RingDur),
                SnoozeDuration = GetSnoozeDuration(a.Alarms.SnoozeDuration, SnoozeDur),
                RingingTime = a.Alarms.RingingTime,
                CreatedFor = a.Alarms.CreatedFor,
                F = IsActiveDay(Days[4], a.Alarms.Days),
                M = IsActiveDay(Days[0], a.Alarms.Days),
                S = IsActiveDay(Days[6], a.Alarms.Days),
                Sa = IsActiveDay(Days[5], a.Alarms.Days),
                Sound = a.Alarms.SoundName,
                T = IsActiveDay(Days[1], a.Alarms.Days),
                AlarmName = a.Alarms.AlarmName,
                Th = IsActiveDay(Days[3], a.Alarms.Days),
                User = UserName,
                W = IsActiveDay(Days[2], a.Alarms.Days),
                Activity = GetActivity(a.Alarms.Active),
                Key = a.Alarms.Key
            }).FirstOrDefault();

            return model;
        }

        public NewAlarmVM EditAlarmUnAuth(string Key, List<Alarms> list)
        {
            NewAlarmVM model = null;
            model = list.Where(a => a.Key == Key).Select(a => new NewAlarmVM
            {
                RingingDuration = GetRingingDuration(a.RingDuration, RingDur),
                SnoozeDuration = GetSnoozeDuration(a.SnoozeDuration, SnoozeDur),
                RingingTime = a.RingingTime,
                CreatedFor = a.CreatedFor,
                F = IsActiveDay(Days[4], a.Days),
                M = IsActiveDay(Days[0], a.Days),
                S = IsActiveDay(Days[6], a.Days),
                Sa = IsActiveDay(Days[5], a.Days),
                Sound = a.SoundName,
                T = IsActiveDay(Days[1], a.Days),
                AlarmName = a.AlarmName,
                Th = IsActiveDay(Days[3], a.Days),
                W = IsActiveDay(Days[2], a.Days),
                Activity = GetActivity(a.Active),
                Key = a.Key
            }).FirstOrDefault();

            return model;
        }

        public Alarms SaveAlarmChanges(NewAlarmVM obj, string User)
        {
            Alarms model = null;
            model = _context.UserAlarm.Include(a => a.User).Include(a => a.Alarms).Where(a => a.Alarms.Key == obj.Key && a.User.UserName == User).Select(a=>a.Alarms).FirstOrDefault();

            if (model != null)
            {
                model.AlarmName = obj.AlarmName;
                model.RingingTime = obj.RingingTime;
                model.SoundName = Sounds[int.Parse(obj.Sound) - 1];
                model.CreatedFor = obj.CreatedFor;
                model.Active = GetActivityBool(obj.Activity);
                model.RingDuration = int.Parse(obj.RingingDuration);
                model.SnoozeDuration = int.Parse(obj.SnoozeDuration);
                model.Days = GenerateFinalDaysString(obj);
                model.LastSnooze = CurrentTime;
                model.LastRinging = CurrentTime.AddDays(-1);
            }
            
            _context.SaveChanges();
            return model;
        }

        public Alarms SaveAlarmChangesUnAuth(NewAlarmVM obj, List<Alarms> list)
        {
            Alarms model = null;
            model = list.Where(a => a.Key == obj.Key).FirstOrDefault();

            if (model != null)
            {
                model.AlarmName = obj.AlarmName;
                model.RingingTime = obj.RingingTime;
                model.SoundName = Sounds[int.Parse(obj.Sound) - 1];
                model.CreatedFor = obj.CreatedFor;
                model.Active = GetActivityBool(obj.Activity);
                model.RingDuration = int.Parse(obj.RingingDuration);
                model.SnoozeDuration = int.Parse(obj.SnoozeDuration);
                model.Days = GenerateFinalDaysString(obj);
                model.LastSnooze = CurrentTime;
                model.LastRinging = CurrentTime.AddDays(-1);
            }

            return model;
        }

        public List<AlarmNotificationVM> ListAlarmNotification(string User)
        {
            List<AlarmNotificationVM> list = new List<AlarmNotificationVM>();
            list = _context.UserAlarm.Include(a => a.Alarms).Include(a => a.User).Where(a => a.User.UserName == User && a.Alarms.Active==true).Select(a => new AlarmNotificationVM
            {
                AlarmName=a.Alarms.AlarmName,
                CreatedFor=a.Alarms.CreatedFor,
                Days=a.Alarms.Days,
                Key=a.Alarms.Key,
                LastSnooze=a.Alarms.LastSnooze,
                RingDuration=a.Alarms.RingDuration,
                RingingTime=a.Alarms.RingingTime,
                SnoozeDuration=a.Alarms.SnoozeDuration,
                SoundName=a.Alarms.SoundName,
                LastAlarmRinging=a.Alarms.LastRinging
            }).ToList();

            return list;
        }

        public List<AlarmNotificationVM> ListAlarmNotificationUnAuth(List<Alarms> listt)
        {
            List<AlarmNotificationVM> list = new List<AlarmNotificationVM>();

            List<Alarms> Lista = listt;

            foreach (var item in Lista)
            {
                DateTime Current = CurrentTime;

                Alarms forchange = item;
                forchange.RingingTime = new DateTime(Current.Year, Current.Month, Current.Day, forchange.RingingTime.Hour, forchange.RingingTime.Minute, 0);

                TimeSpan diffDate = Current.Subtract(forchange.LastRinging);

                var DifferenceInMinutes = (diffDate.Days * 24 * 60) + (diffDate.Hours * 60) + diffDate.Minutes;
                if (DifferenceInMinutes > forchange.SnoozeDuration)
                {
                    forchange.LastSnooze = forchange.RingingTime;
                }
            }

            list = Lista.Where(a => a.Active == true).Select(a => new AlarmNotificationVM
            {
                AlarmName = a.AlarmName,
                CreatedFor = a.CreatedFor,
                Days = a.Days,
                Key = a.Key,
                LastSnooze = a.LastSnooze,
                RingDuration = a.RingDuration,
                RingingTime = a.RingingTime,
                SnoozeDuration = a.SnoozeDuration,
                SoundName = a.SoundName,
                LastAlarmRinging = a.LastRinging
            }).ToList();

            return list;
        }

        public Alarms SetLastAlarmNotification(string Key, string Value, string User)
        {
            Alarms obj = null;

            obj = _context.UserAlarm.Include(a => a.Alarms).Include(a => a.User).Where(a => a.Alarms.Key == Key && a.User.UserName == User).Select(a=>a.Alarms).FirstOrDefault();

            if (obj != null)
            {
                DateTime CurrentTimeSecond = CurrentTime;

                DateTime NewRingingTime = new DateTime(CurrentTimeSecond.Year, CurrentTimeSecond.Month, CurrentTimeSecond.Day,obj.RingingTime.Hour,obj.RingingTime.Minute,0);

                obj.RingingTime = NewRingingTime;
                obj.LastSnooze = NewRingingTime;
                obj.LastRinging = CurrentTime;

                if (Value == "Snooze")
                {
                    obj.LastSnooze = CurrentTime.AddMinutes(obj.SnoozeDuration);
                }

                _context.SaveChanges();
            }

            return obj;
        }

        public Alarms SetLastAlarmNotificationUnAuth(string Key, string Value, List<Alarms> List)
        {
            Alarms obj = null;

            obj = List.Where(a => a.Key == Key).FirstOrDefault();

            if (obj != null)
            {
                DateTime CurrentTimeSecond = CurrentTime;

                DateTime NewRingingTime = new DateTime(CurrentTimeSecond.Year, CurrentTimeSecond.Month, CurrentTimeSecond.Day, obj.RingingTime.Hour, obj.RingingTime.Minute, 0);

                obj.RingingTime = NewRingingTime;
                obj.LastSnooze = NewRingingTime;
                obj.LastRinging = CurrentTime;

                if (Value == "Snooze")
                {
                    obj.LastSnooze = CurrentTime.AddMinutes(obj.SnoozeDuration);
                }
            }

            return obj;
        }

        public void SetCurrentTime(DateTime CurrentTimee)
        {
            CurrentTime = CurrentTimee;
        }
    }
}
