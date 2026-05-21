using Employee.Web.Models;
using EmployeePairs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Employee.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a CSV file.");

                return View();
            }

            var csvFilePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(csvFilePath))
            {
                await file.CopyToAsync(stream);
            }

            try
            {
                var pairsService = new EmployeePairsService();
                var result = pairsService.GroupEmployees(csvFilePath);

                var viewModel = MapToViewModel(result);

                return View(viewModel);
            }
            finally 
            {
                System.IO.File.Delete(csvFilePath);
            }

        }

        private List<EmployeePairViewModel> MapToViewModel(Dictionary<Tuple<int,int>, ProjectCollaborationData> pairs)
        {
           return pairs.Select(p => new EmployeePairViewModel
            {
                EmployeeId1 = p.Key.Item1,
                EmployeeId2 = p.Key.Item2,
                TotalDaysWorkedTogether = p.Value.TotalDaysTogether,
                Projects = p.Value.ProjectsWorkedTogether.Select(pp => new ProjectCollaborationViewModel
                {
                    ProjectId = pp.Key,
                    DaysWorked = pp.Value
                }).ToList()
            }).ToList();
        }
    }
}
