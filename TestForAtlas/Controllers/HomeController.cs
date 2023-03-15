using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestForAtlas.Models;
using TestForAtlas.Services;

namespace TestForAtlas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICalculateService _calculateService;

        public HomeController(ILogger<HomeController> logger, ICalculateService calculateService)
        {
            _calculateService = calculateService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index(InputDataViewModel InputData)
        {
            if (ModelState.IsValid)
            {
                if (InputData.LoanTerm <= 0)
                {
                    ModelState.AddModelError("LoanTerm", "Неверный срок займа");
                    return View(InputData);
                }

                return RedirectToAction("CalculateLoan", "Home", new { InputData.LoanAmount, InputData.LoanTerm, InputData.RateInYear });
            }

            return View(InputData);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CalculateLoan(InputDataViewModel inputData)
        {
            var ListLoan = _calculateService.GetListPayment(inputData);
            ViewBag.SumPerc = Math.Round(ListLoan.Sum(x => x.PaymentAmountForPercent), 2);
            ViewBag.ListLoan = ListLoan;

            return View();
        }

        public IActionResult Dop()
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