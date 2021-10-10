using GenerateSuccess.Models;
using GenerateSuccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.Services
{
    public interface IAlarmService
    {
        public string GenerateAlarmName(string UserName,string Language);
        public string AddAlarm(NewAlarmVM model, string User);
        public Alarms AddAlarmUnAuth(NewAlarmVM model, List<Alarms> list);
        public List<AlarmPreviewVM> GetListAlarms(string UserName, HomeFilterVM filter);
        public List<AlarmPreviewVM> GetListAlarmsUnAuth(List<Alarms> listofalarms, HomeFilterVM filter);
        public Alarms DeleteAlarm(string Key, string UserName);
        public List<Alarms> DeleteAlarmUnAuth(string Key, List<Alarms> lista);
        public NewAlarmVM EditAlarm(string Key, string UserName);
        public NewAlarmVM EditAlarmUnAuth(string Key, List<Alarms> list);
        public Alarms SaveAlarmChanges(NewAlarmVM model, string User);
        public Alarms SaveAlarmChangesUnAuth(NewAlarmVM model, List<Alarms> list);
        public List<AlarmNotificationVM> ListAlarmNotification(string User);
        public List<AlarmNotificationVM> ListAlarmNotificationUnAuth(List<Alarms> list);
        public Alarms SetLastAlarmNotification(string Key, string Value,string User);
        public Alarms SetLastAlarmNotificationUnAuth(string Key, string Value, List<Alarms> list);
        public void SetCurrentTime(DateTime CurrentTime);
    }
}
