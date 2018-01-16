using System.Data.Entity;

namespace TaskManage.Models
{
    /// <summary>
    /// Контекст для работы с БД
    /// </summary>
    public class TaskManageContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Status> Statuses { get; set; }
    }
}