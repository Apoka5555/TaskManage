using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManage.Models
{
    /// <summary>
    /// Задача
    /// </summary>
    public class Task
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Срок")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Статус")]
        public int StatusId { get; set; }

        [Display(Name = "Статус")]
        public Status Status { get; set; }

        public int UserId { get; set; }
    }
}