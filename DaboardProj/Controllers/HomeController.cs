using DaboardProj.Data;
using DaboardProj.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;

namespace DaboardProj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            //Random random = new Random();
            //string[] platforms = { "Phone", "Desktop", "Tablet" };
            //for (int i = 0; i < 10; i++)
            //{
            //    Visiter visiter = new Visiter
            //    {
            //        Platform = platforms[random.Next(3)],
            //        DateOfEntryOnSite = DateTime.Now,
            //        DateOfExitFromSite = DateTime.Now.AddMinutes(10),
            //        IsNewVisiter = true
            //    };
            //    _context.Visiters.Add(visiter);
            //    _context.SaveChanges();
            //    Thread.Sleep(5000);
            //}
            if (ViewData["Period"] == null)
            {
                ViewData["Period"] = "year";
            }


            return View();
        }

        [HttpGet]
        public List<object> GetDoughnutData(string period) {
            List<object> data = new List<object>();
            List<string> labels = new List<string> { "Phone", "Desktop", "Tablet" };
            List<int> numberOfVisiters = new List<int>();
            double timePeriodInDays = 0;
            switch (period)
            {
                case "year":
                    timePeriodInDays = 365;
                    break;
                case "month":
                    timePeriodInDays = 30;
                    break;
                case "week":
                    timePeriodInDays = 7;
                    break;
                case "day":
                    timePeriodInDays = 1;
                    break;
                default:
                    break;
            }
            
            DateTime referenceDate = DateTime.Now.AddDays(-timePeriodInDays);

            for (int i = 0; i < 3; i++)
            {
                numberOfVisiters.Add(_context.Visiters
                .Where(v => v.Platform == labels[i] && v.DateOfExitFromSite >= referenceDate && v.DateOfExitFromSite <= DateTime.Now)
                .Count());
            }
            data.Add(labels);
            data.Add(numberOfVisiters);
            return data;
        }


        public List<object> TotalUsers(string period)
        {
            List<object> data = new List<object>();
            List<int> numberOfVisiters = new List<int>();
            List<string> labels = new List<string>();
            CultureInfo russianCulture = new CultureInfo("ru-RU");
            DateTime currentDate = DateTime.Today;
            switch (period)
            {
                case "year":
                    string[] months = new string[]{ "Январь","Февраль","Март", "Апрель","Май","Июнь","Июль",
                                                "Август","Сентябрь","Октябрь","Ноябрь","Декабрь"};
                    for (int i = 0; i < months.Length; i++)
                    {
                        labels.Add(months[i]);
                    }
                    for (int i = 1; i < 12; i++)
                    {
                        DateTime monthStart = new DateTime(DateTime.Today.Year, i, 1); 
                        DateTime monthEnd = new DateTime(DateTime.Today.Year, i+1, 1);
                        numberOfVisiters.Add(_context.Visiters
                         .Where(v => v.DateOfExitFromSite >= monthStart && v.DateOfExitFromSite < monthEnd).Count());
                    }

                    break;
                case "month":
                    
                    DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    int numberOfDaysInMonth = (lastDayOfMonth - firstDayOfMonth).Days + 1;
                    for (int i = 0; i < numberOfDaysInMonth; i++)
                    {
                        labels.Add(i.ToString());
                    }
                    for (int i = 1; i <= numberOfDaysInMonth; i++)
                    {
                        numberOfVisiters.Add(_context.Visiters.Where(v => v.DateOfExitFromSite.Day == i 
                                            && v.DateOfExitFromSite.Month == DateTime.Today.Month).Count());
                    }
                    break;
                case "week":
                    DateTime endOfLastWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek); 
                    DateTime startOfLastWeek = endOfLastWeek.AddDays(-6); 

                    for (int i = 0; i < 7; i++)
                    {
                        DateTime dayInLastWeek = startOfLastWeek.AddDays(i);

                        labels.Add(dayInLastWeek.ToString("dddd", russianCulture));
                        int visitorsForDay = _context.Visiters
                            .Where(v => v.DateOfExitFromSite.Date == dayInLastWeek.Date)
                            .Count();
                        numberOfVisiters.Add(visitorsForDay); 
                    }
                    break;
                case "day":
                    DateTime endOfLast24Hours = currentDate;
                    DateTime startOfLast24Hours = currentDate.AddHours(-24);

                    for (int i = 0; i < 24; i++)
                    {
                        DateTime hourInLast24Hours = startOfLast24Hours.AddHours(i);

                        labels.Add(hourInLast24Hours.ToString("HH:mm", russianCulture)); 
                        int visitorsForHour = _context.Visiters
                            .Where(v => v.DateOfExitFromSite >= hourInLast24Hours && v.DateOfExitFromSite < hourInLast24Hours.AddHours(1))
                            .Count();
                        numberOfVisiters.Add(visitorsForHour); 
                    }
                    break;
            }
            data.Add(labels);
            data.Add(numberOfVisiters);
            //добавить суда количеств процентов и вернуть сдатой
            return data;
        }

        public List<object> TotalUsers2(string period)
        {
            List<object> data = new List<object>();
            List<int> numberOfVisiters = new List<int>();
            List<string> labels = new List<string>();
            CultureInfo russianCulture = new CultureInfo("ru-RU");
            DateTime currentDate = DateTime.Today;
            switch (period)
            {
                case "year":
                    string[] months = new string[]{ "Январь","Февраль","Март", "Апрель","Май","Июнь","Июль",
                                                "Август","Сентябрь","Октябрь","Ноябрь","Декабрь"};
                    for (int i = 0; i < months.Length; i++)
                    {
                        labels.Add(months[i]);
                    }
                    for (int i = 1; i < 12; i++)
                    {
                        DateTime monthStart = new DateTime(DateTime.Today.Year, i, 1);
                        DateTime monthEnd = new DateTime(DateTime.Today.Year, i + 1, 1);
                        numberOfVisiters.Add(_context.Visiters
                         .Where(v => v.DateOfExitFromSite >= monthStart && v.DateOfExitFromSite < monthEnd).Count());
                    }

                    break;
                case "month":

                    DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    int numberOfDaysInMonth = (lastDayOfMonth - firstDayOfMonth).Days + 1;
                    for (int i = 0; i < numberOfDaysInMonth; i++)
                    {
                        labels.Add(i.ToString());
                    }
                    for (int i = 1; i <= numberOfDaysInMonth; i++)
                    {
                        numberOfVisiters.Add(_context.Visiters.Where(v => v.DateOfExitFromSite.Day == i
                                            && v.DateOfExitFromSite.Month == DateTime.Today.Month).Count());
                    }
                    break;
                case "week":
                    DateTime endOfLastWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                    DateTime startOfLastWeek = endOfLastWeek.AddDays(-6);

                    for (int i = 0; i < 7; i++)
                    {
                        DateTime dayInLastWeek = startOfLastWeek.AddDays(i);

                        labels.Add(dayInLastWeek.ToString("dddd", russianCulture));
                        int visitorsForDay = _context.Visiters
                            .Where(v => v.DateOfExitFromSite.Date == dayInLastWeek.Date)
                            .Count();
                        numberOfVisiters.Add(visitorsForDay);
                    }
                    break;
                case "day":
                    DateTime endOfLast24Hours = currentDate;
                    DateTime startOfLast24Hours = currentDate.AddHours(-24);

                    for (int i = 0; i < 24; i++)
                    {
                        DateTime hourInLast24Hours = startOfLast24Hours.AddHours(i);

                        labels.Add(hourInLast24Hours.ToString("HH:mm", russianCulture));
                        int visitorsForHour = _context.Visiters
                            .Where(v => v.DateOfExitFromSite >= hourInLast24Hours && v.DateOfExitFromSite < hourInLast24Hours.AddHours(1))
                            .Count();
                        numberOfVisiters.Add(visitorsForHour);
                    }
                    break;
            }
            data.Add(labels);
            data.Add(numberOfVisiters);
            //добавить суда количеств процентов и вернуть сдатой
            return data;
        }

        public List<object> AverageUserTimeOnSite(string period)
        {
            List<object> data = new List<object>();
            List<double> averageTime = new List<double>();
            List<string> labels = new List<string>();
            CultureInfo russianCulture = new CultureInfo("ru-RU");
            DateTime currentDate = DateTime.Today;
            switch (period)
            {
                case "year":
                    int currentYear = DateTime.Now.Year;

                    string[] months = new string[]{ "Январь","Февраль","Март", "Апрель","Май","Июнь","Июль",
                                                "Август","Сентябрь","Октябрь","Ноябрь","Декабрь"};
                    for (int i = 0; i < months.Length; i++)
                    {
                        labels.Add(months[i]);
                    }
                    for (int month = 1; month <= 12; month++)
                    {
                        DateTime startOfMonth = new DateTime(currentYear, month, 1);
                        DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                        var visitorsInMonth = _context.Visiters
                            .Where(v => v.DateOfEntryOnSite >= startOfMonth && v.DateOfEntryOnSite <= endOfMonth)
                            .ToList();

                        double totalMinutes = visitorsInMonth.Sum(v => (v.DateOfExitFromSite - v.DateOfEntryOnSite).TotalMinutes);
                        double averageTimey = visitorsInMonth.Count > 0 ? totalMinutes / visitorsInMonth.Count : 0;

                        averageTime.Add(averageTimey);
                    }

                    break;
                case "month":
                    int currentYearm = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    int numberOfDaysInMonth = (lastDayOfMonth - firstDayOfMonth).Days + 1;
                    for (int i = 0; i < numberOfDaysInMonth; i++)
                    {
                        labels.Add(i.ToString());
                    }
                    for (int day = 1; day <= lastDayOfMonth.Day; day++)
                    {
                        DateTime startOfDay = new DateTime(currentYearm, currentMonth, day, 0, 0, 0);
                        DateTime endOfDay = new DateTime(currentYearm, currentMonth, day, 23, 59, 59);
                        var visitorsInDay = _context.Visiters
                            .Where(v => v.DateOfEntryOnSite >= startOfDay && v.DateOfEntryOnSite <= endOfDay)
                            .ToList();
                        double totalMinutes = visitorsInDay.Sum(v => (v.DateOfExitFromSite - v.DateOfEntryOnSite).TotalMinutes);
                        double averageTimem = visitorsInDay.Count > 0 ? totalMinutes / visitorsInDay.Count : 0;

                        averageTime.Add(averageTimem);
                    }
                    break;
                case "week":
                    DateTime endOfLastWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                    DateTime startOfLastWeek = endOfLastWeek.AddDays(-6);

                    for (int i = 0; i < 7; i++)
                    {
                        DateTime dayInLastWeek = startOfLastWeek.AddDays(i);
                        labels.Add(dayInLastWeek.ToString("dddd", russianCulture));
  
                        var visitorsInDay = _context.Visiters
                            .Where(v => v.DateOfEntryOnSite >= dayInLastWeek && v.DateOfEntryOnSite <= dayInLastWeek.AddDays(1))
                            .ToList();
                        double totalMinutes = visitorsInDay.Sum(v => (v.DateOfExitFromSite - v.DateOfEntryOnSite).TotalMinutes);

                        // Вычисляем среднее время на сайте для текущего дня
                        double averageTimew = visitorsInDay.Count > 0 ? totalMinutes / visitorsInDay.Count : 0;

                        averageTime.Add(averageTimew);
                    }
                    break;
                case "day":
                    DateTime endOfLast24Hours = currentDate;
                    DateTime startOfLast24Hours = currentDate.AddHours(-24);

                    for (int i = 0; i < 24; i++)
                    {
                        DateTime hourInLast24Hours = startOfLast24Hours.AddHours(i);
                        labels.Add(hourInLast24Hours.ToString("HH:mm", russianCulture));
                        var visitorsInHour = _context.Visiters
                            .Where(v => v.DateOfEntryOnSite >= hourInLast24Hours && v.DateOfEntryOnSite < hourInLast24Hours.AddHours(1))
                            .ToList();
                        double totalMinutes = visitorsInHour.Sum(v => (v.DateOfExitFromSite - v.DateOfEntryOnSite).TotalMinutes);
                        double averageTimeh = visitorsInHour.Count > 0 ? totalMinutes / visitorsInHour.Count : 0;

                        averageTime.Add(averageTimeh);
                    }
                    break;
            }
            data.Add(labels);
            data.Add(averageTime);
            return data;
        }

        public List<object> NewVisiters(string period)
        {
            List<object> data = new List<object>();
            List<int> newVisiters = new List<int>();
            List<string> labels = new List<string>();
            DateTime currentDate = DateTime.Now;
            CultureInfo russianCulture = new CultureInfo("ru-RU");
            switch (period) { 
            case "year":
                int currentYear = DateTime.Now.Year;

                string[] months = new string[]{ "Январь","Февраль","Март", "Апрель","Май","Июнь","Июль",
                                                "Август","Сентябрь","Октябрь","Ноябрь","Декабрь"};
                for (int i = 0; i < months.Length; i++)
                {
                    labels.Add(months[i]);
                }
                for (int month = 1; month <= 12; month++)
                {
                    DateTime startOfMonth = new DateTime(currentYear, month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                        int newVisitorsCount = _context.Visiters.Where(v => v.IsNewVisiter && v.
                        DateOfEntryOnSite >= startOfMonth && v.DateOfEntryOnSite <= endOfMonth).Count();
                        newVisiters.Add(newVisitorsCount);
                }

                break;
            case "month":
                    int currentYearm = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    DateTime firstDayOfMonth = new DateTime(currentYearm, currentMonth, 1);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    int numberOfDaysInMonth = (lastDayOfMonth - firstDayOfMonth).Days + 1;
                    for (int i = 0; i < numberOfDaysInMonth; i++)
                    {
                        labels.Add(i.ToString());
                    }
                    for (int day = 1; day <= lastDayOfMonth.Day; day++)
                {
                        DateTime startOfDay = new DateTime(currentYearm, currentMonth, day, 0, 0, 0);
                        DateTime endOfDay = new DateTime(currentYearm, currentMonth, day, 23, 59, 59);

                        int newVisitorsCount = _context.Visiters
                            .Where(v => v.IsNewVisiter && v.DateOfEntryOnSite >= startOfDay && v.DateOfEntryOnSite <= endOfDay)
                            .Count();

                        newVisiters.Add(newVisitorsCount);
                    }
                break;
            case "week":
                DateTime endOfLastWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                DateTime startOfLastWeek = endOfLastWeek.AddDays(-6);

                for (int i = 0; i < 7; i++)
                {
                    DateTime dayInLastWeek = startOfLastWeek.AddDays(i);
                    labels.Add(dayInLastWeek.ToString("dddd", russianCulture));

                        DateTime startOfDay = new DateTime(dayInLastWeek.Year, dayInLastWeek.Month, dayInLastWeek.Day, 0, 0, 0);
                        DateTime endOfDay = new DateTime(dayInLastWeek.Year, dayInLastWeek.Month, dayInLastWeek.Day, 23, 59, 59);

                        int newVisitorsCount = _context.Visiters
                            .Where(v => v.IsNewVisiter && v.DateOfEntryOnSite >= startOfDay && v.DateOfEntryOnSite <= endOfDay)
                            .Count();

                        newVisiters.Add(newVisitorsCount);
                    }
                break;
            case "day":
                DateTime endOfLast24Hours = currentDate;
                DateTime startOfLast24Hours = currentDate.AddHours(-24);

                for (int i = 0; i < 24; i++)
                {
                    DateTime hourInLast24Hours = startOfLast24Hours.AddHours(i);
                        labels.Add(hourInLast24Hours.ToString("HH:mm", russianCulture));

                        DateTime startOfHour = hourInLast24Hours;
                        DateTime endOfHour = hourInLast24Hours.AddHours(1);

                        int newVisitorsCount = _context.Visiters
                            .Where(v => v.IsNewVisiter && v.DateOfEntryOnSite >= startOfHour && v.DateOfEntryOnSite < endOfHour)
                            .Count();

                        newVisiters.Add(newVisitorsCount);
                    }
                break;
            }


            data.Add(labels);
            data.Add(newVisiters);
            return data;
        }
        public IActionResult ChangePeriod(string newPeriod)
        {
            ViewData["Period"] = newPeriod;
            return View("Index");
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}