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
                if (InputData.LoanTerm <= 0 || InputData.LoanTerm > 999)
                {
                    ModelState.AddModelError("LoanTerm", "Неверный срок займа");
                    return View(InputData);
                }

                if (InputData.RateInYear <= 0)
                {
                    ModelState.AddModelError("RateInYear", "Неверная ставка займа");
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
            try
            {
                var ListPayments = _calculateService.GetListPayments(inputData);
                ViewBag.SumPercent = Math.Round(ListPayments.Sum(x => x.PaymentAmountForPercent), 2);
                ViewBag.ListPayments = ListPayments;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Dop(FormLoanPerDayViewModel LoanPerDay)
        { 
            if (ModelState.IsValid)
            {
                if (LoanPerDay.LoanTerm <= 0 || LoanPerDay.LoanTerm > 999)
                {
                    ModelState.AddModelError("LoanTerm", "Неверный срок займа");
                    return View(LoanPerDay);
                }

                if (LoanPerDay.RateInDay <= 0)
                {
                    ModelState.AddModelError("RateInDay", "Неверная ставка займа");
                    return View(LoanPerDay);
                }

                if (LoanPerDay.StepPayment <= 0 || LoanPerDay.StepPayment > LoanPerDay.LoanTerm)
                {
                    ModelState.AddModelError("StepPayment", "Неверный шаг займа");
                    return View(LoanPerDay);
                }

                return RedirectToAction("CalculateLoanPerDay", "Home", new { LoanPerDay.LoanAmount, LoanPerDay.LoanTerm, LoanPerDay.RateInDay, LoanPerDay.StepPayment });
            }

            return View(LoanPerDay);
        }

        public IActionResult Dop()
        {
            return View();
        }

        public IActionResult CalculateLoanPerDay(FormLoanPerDayViewModel LoanPerDay)
        {
            try 
            { 
                var ListPayments = _calculateService.GetListPaymentsPerDay(LoanPerDay);
                ViewBag.SumPercent = Math.Round(ListPayments.Sum(x => x.PaymentAmountForPercent), 2);
                ViewBag.ListPayments = ListPayments;
            }
            catch
            {
                return RedirectToAction("Dop", "Home");
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}