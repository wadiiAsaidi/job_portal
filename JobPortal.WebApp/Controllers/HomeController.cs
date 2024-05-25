using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data.DataContext;
using JobPortal.WebApp.Models;
using System.Diagnostics;
using System.Management;
using System.Net.Sockets;

namespace JobPortal.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataDbContext _context;

        public HomeController(DataDbContext dataDbContext)
        {
            this._context = dataDbContext;
        }

        public IActionResult Index()
        {


            string targetIP = "192.168.52.128"; // Replace with the IP address of your VM

            // Scan ports 1 through 1024
            var list = new List<int>();
            for (int port = 1; port <= 1024; port++)
            {
                // Create a TCP client
                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        // Attempt to connect to the target IP and port
                        tcpClient.Connect(targetIP, port);
                        Console.WriteLine($"Port {port} is open");
                        list.Add(port);
                    }
                    catch (SocketException ex)
                    {
                        // Port is closed
                        // Uncomment the next line if you want to see closed ports as well
                        // Console.WriteLine($"Port {port} is closed");
                    }
                }
            }



            var random = new Random();

            //for model
            var jobs = _context.Jobs.ToList();

            //For search filter area
            ViewBag.FilterProvinces = _context.Provinces.OrderBy(p => p.Id).ToList();
            ViewBag.FilterSkills = _context.Skills.OrderBy(s => s.Name).ToList();

            //random employers - 4
            var employerList = _context.Users.Where(e => e.Status == 2).Include(e => e.Province).Include(e => e.Jobs).ToList();
            ViewBag.RandomEmployers = employerList.OrderBy(e => Guid.NewGuid()).Where(e => e.Jobs.Count > 0).Take(4).ToList();

            //random skills - 6
            var skillList = _context.Skills.ToList();
            ViewBag.RandomSkills = skillList.OrderBy(s => random.Next()).Take(6).ToList();

            //random jobs - 6
            var jobList = _context.Jobs
                .Include(j => j.Province)
                .Include(j => j.AppUser)
                .Include(j => j.Title)
                .Include(j => j.Time)
                .Include(j => j.Skills)
                .ToList();
            var randomJobs = jobList.OrderBy(j => random.Next()).Take(6).ToList();
            ViewBag.RandomJobs = randomJobs;

            return View(jobs);
        }

        [Route("about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("price")]
        public IActionResult Price()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("elements")]
        public IActionResult Elements()
        {
            return View();
        }

        [Route("privacy")]
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