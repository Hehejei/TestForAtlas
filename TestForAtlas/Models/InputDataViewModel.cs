using System.ComponentModel.DataAnnotations;

namespace TestForAtlas.Models
{
    public class InputDataViewModel
    {
        [Required(ErrorMessage = "Неверная сумма займа")]
        public double LoanAmount { get; set; } // Сумма займа

        [Required(ErrorMessage = "Неверный срок займа")]
        public uint LoanTerm { get; set; } // Срок займа (в месяцах)

        [Required(ErrorMessage = "Неверная ставка в год")]
        public double RateInYear { get; set; } // Ставка (в год)
    }
}
