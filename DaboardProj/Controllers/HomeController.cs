using DaboardProj.Data;
using DaboardProj.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            if(ViewData["Period"] == null)
            {
                ViewData["Period"] = "year";
            }

            //Random random = new Random();
            //string[] platforms = {"Phone", "Desktop", "Tablet" };
            //for (int i = 0; i < 10; i++)
            //{
            //    Visiter visiter = new Visiter
            //    {
            //        Platform = platforms[random.Next(3)],
            //        DateOfEntryOnSite = DateTime.Now,
            //        DateOfExitFromSite = DateTime.Now.AddMinutes(2)
            //    };
            //    _context.Visiters.Add(visiter);
            //    _context.SaveChanges();
            //    Thread.Sleep(5000);
            //}

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
            switch (period)
            {
                case "year":
                    labels.Add("Январь");
                    for (int i = 1; i < 12; i++)
                    {
                        DateTime monthStart = new DateTime(DateTime.Today.Year, i, 1); 
                        DateTime monthEnd = new DateTime(DateTime.Today.Year, i+1, 1);
                        numberOfVisiters.Add(_context.Visiters
                         .Where(v => v.DateOfExitFromSite >= monthEnd && v.DateOfExitFromSite < monthEnd).Count());
                    }

                    break;
                case "month":

                    break;
                case "week":

                    break;
                case "day":

                    break;
                default:
                    break;
            }

            // мне тут нужно посчитать за каждый месяц за каждый день
            _context.Visiters.Count();
            data.Add(labels);
            data.Add(numberOfVisiters);
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