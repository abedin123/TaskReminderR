using GenerateSuccess.Models;
using GenerateSuccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.Services
{
    public interface ITaskService
    {
        public bool DateValidation(DateTime StartDate,DateTime EndDate,DateTime StartTime,DateTime EndTime,string CreatedFor);
        public bool DateInThePast(DateTime StartDate, DateTime EndDate, DateTime StartTime,DateTime EndTime, string CreatedFor, int WhichDate);
        public bool CorrectType(int Minutes, int Hours);
        public string GenerateTaskName(string UserName);
        public TaskDB AddTask(NewTaskVM model, string User);
        public TaskDB SaveTaskChanges(NewTaskVM model, string User);
        public TaskDB SaveTaskChangesUnAuth(NewTaskVM model, List<TaskDB> list);
        public NewTaskVM EditTask(string Key, string UserName);
        public NewTaskVM EditTaskUnAuth(string Key, List<TaskDB> list);
        public TaskDB AddTaskUnAuth(NewTaskVM model, List<TaskDB> list);
        public List<TaskPreviewVM> GetListTasks(string User, HomeFilterVM filter);
        public List<TaskPreviewVM> GetListTasksUnAuth(List<TaskDB> listoftasks, HomeFilterVM filter);
        public string SuccessNumber(List<TaskPreviewVM> list);
        public TaskDB DeleteTask(string Key,string UserName);
        public List<TaskDB> DeleteTaskUnAuth(string Key, List<TaskDB> lista);
        public List<TaskNotificationVM> ListTaskNotification(string User);
        public List<TaskNotificationVM> ChangeCurrentUnAuthList(List<TaskDB> lista);
        public TaskDB SetLastNotification(string Key,string Value, string UserName,string Success);
        public TaskDB SetLastNotificationUnAuth(string Key, string Value, List<TaskDB> currentlist, string Success);
        public void SetCurrentTime(DateTime CurrentTimeee);
    }
}
