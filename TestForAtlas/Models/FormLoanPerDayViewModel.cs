using System.ComponentModel.DataAnnotations;

namespace TestForAtlas.Models
{
    public class FormLoanPerDayViewModel
    {
        [Required(ErrorMessage = "Неверная сумма займа")]
        public double LoanAmount { get; set; } // Сумма займа

        [Required(ErrorMessage = "Неверный срок займа")]
        public uint LoanTerm { get; set; } // Срок займа (в днях)

        [Required(ErrorMessage = "Неверная ставка в день")]
        public double RateInDay { get; set; } // Ставка (в день)

        [Required(ErrorMessage = "Неверный шаг")]
        public uint StepPayment { get; set; } // Шаг платежа (в днях)
    }
}
