using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskManage.Models;

namespace TaskManage.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Контекст для работы с БД
        /// </summary>
        private readonly TaskManageContext _db = new TaskManageContext();

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }

            HashSet<Task> myTasks = GetMyTasks();

            if (myTasks.Count == 0)
            {
                ViewBag.Message = "У вас пока нет ни одной задачи";
            }
            else
            {
                ViewBag.Message = "Всего задач: " + myTasks.Count;
            }

            return View(myTasks);
        }

        /// <summary>
        /// Получение всех задач текущего пользователя
        /// </summary>
        /// <returns></returns>
        private HashSet<Task> GetMyTasks()
        {
            User user = Session["User"] as User;

            var tasks = _db.Tasks.ToList();

            var myTasks = new HashSet<Task>();

            foreach (var task in tasks)
            {
                if (task.UserId == user.Id)
                {
                    task.Status = _db.Statuses.Find(task.StatusId);
                    myTasks.Add(task);
                }
            }

            return myTasks;
        }
        
        /// <summary>
        /// Редакитрование задачи
        /// </summary>
        /// <param name="id">ИД задачи</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTask(int id)
        {
            Task task = _db.Tasks.Find(id);

            ViewBag.Statuses = new SelectList(_db.Statuses, "Id", "Name");

            return View(task);
        }

        /// <summary>
        /// Редакитрование задачи
        /// </summary>
        /// <param name="task">Задача</param>
        /// <param name="action">Действие</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTask(Task task, string action)
        {
            if (action == "update")
            {
                Task currentTask = _db.Tasks.Find(task.Id);

                currentTask.Name = task.Name;
                currentTask.Description = task.Description;
                currentTask.DueDate = task.DueDate;
                currentTask.StatusId = task.StatusId;

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Добавление новой задачи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateTask()
        {
            ViewBag.Statuses = new SelectList(_db.Statuses, "Id", "Name");

            return View();
        }

        /// <summary>
        /// Добавление новой задачи
        /// </summary>
        /// <param name="task">Задача</param>
        /// <param name="action">Действие</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateTask(Task task, string action)
        {
            if (action == "create")
            {
                if (Session["User"] == null)
                {
                    return RedirectToAction("Login");
                }

                User user = Session["User"] as User;

                task.UserId = user.Id;

                _db.Tasks.Add(task);

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="id">ИД задачи</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteTask(int id)
        {
            Task task = _db.Tasks.Find(id);

            return View(task);
        }

        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="task">Задача</param>
        /// <param name="action">Действие</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTask(Task task, string action)
        {
            if (action == "delete")
            {
                Task currentTask = _db.Tasks.Find(task.Id);
  
                _db.Tasks.Remove(currentTask);

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Поиск задач
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskSearch()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Statuses = new SelectList(_db.Statuses, "Id", "Name");

            return View();
        }

        /// <summary>
        /// Поиск задач
        /// </summary>
        /// <param name="currentTask">Задача</param>
        /// <param name="dueDateFrom">Срок от</param>
        /// <param name="dueDateTo">Срок до</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TaskSearchResult(Task currentTask, DateTime? dueDateFrom, DateTime? dueDateTo)
        {
            HashSet<Task> myTasks = GetMyTasks();

            var searchedTasks = new HashSet<Task>();

            foreach (Task task in myTasks)
            {
                if ((string.IsNullOrEmpty(currentTask.Name) || 
                        task.Name.ToLower().Contains(currentTask.Name.ToLower().Trim())) &&
                    (string.IsNullOrEmpty(currentTask.Description) || 
                        task.Description.ToLower().Contains(currentTask.Description.ToLower().Trim())) &&
                    (dueDateFrom == null || 
                        (task.DueDate != null && task.DueDate >= dueDateFrom)) &&
                    (dueDateTo == null || 
                        (task.DueDate != null && task.DueDate <= dueDateTo)) &&
                    (currentTask.StatusId == task.StatusId))
                {
                    searchedTasks.Add(task);
                }
            }

            if (searchedTasks.Count == 0)
            {
                ViewBag.Message = "Не найдено ни одной задачи";
            }
            else
            {
                ViewBag.Message = "Найдено задач: " + searchedTasks.Count;
            }

            return PartialView("TaskSearchResultPartial", searchedTasks);
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="currentUser">Данные пользователя</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(User currentUser)
        {
            var users = _db.Users.ToList();

            foreach (var user in users)
            {
                if (currentUser.Login == user.Login &&
                    currentUser.Password == user.Password)
                {
                    Session["User"] = user;
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="currentUser">Данные пользователя</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterModel currentUser)
        {
            if (string.IsNullOrEmpty(currentUser.Login.Trim()) ||
                string.IsNullOrEmpty(currentUser.Password))
            {
                ModelState.AddModelError("", "Заполните все поля");
                return View();
            }

            if (currentUser.Password != currentUser.ConfirmPassword)
            {
                ModelState.AddModelError("", "Пароли не совпадают");
                return View();
            }

            var users = _db.Users.ToList();

            foreach (var user in users)
            {
                if (currentUser.Login == user.Login)
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                    return View();
                }
            }

            User newUser = new User
            {
                Login = currentUser.Login,
                Password = currentUser.Password,
            };

            _db.Users.Add(newUser);

            _db.SaveChanges();

            Session["User"] = newUser;

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Выход
        /// </summary>
        /// <returns></returns>
        public ActionResult Exit()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }
    }
}