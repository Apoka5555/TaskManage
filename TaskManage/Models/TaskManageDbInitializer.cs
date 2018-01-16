using System;
using System.Data.Entity;

namespace TaskManage.Models
{
    /// <summary>
    /// Инициализация данных
    /// </summary>
    public class TaskManageDbInitializer : DropCreateDatabaseAlways<TaskManageContext>
    {
        protected override void Seed(TaskManageContext db)
        {
            db.Statuses.Add(new Status { Name = "Не начата" });
            db.Statuses.Add(new Status { Name = "Выполняется" });
            db.Statuses.Add(new Status { Name = "Завершена" });

            db.Users.Add(new User { Login = "user1", Password = "5555" });
            db.Users.Add(new User { Login = "user2", Password = "6666" });
            db.Users.Add(new User { Login = "user3", Password = "9999" });

            db.Tasks.Add(new Task { Name = "Спроектировать архитектуру", Description = "Нарисовать схему базы данных", DueDate = new DateTime(2018, 1, 16), UserId = 1, StatusId = 1 });
            db.Tasks.Add(new Task { Name = "Реализовать проект", Description = "Сделать проект в Visual Studio", DueDate = new DateTime(2018, 2, 17), UserId = 1, StatusId = 1 });
            db.Tasks.Add(new Task { Name = "Тестировать проект", Description = "Написать юнит-тесты", DueDate = new DateTime(2018, 2, 25), UserId = 1, StatusId = 1 });

            base.Seed(db);
        }
    }
}