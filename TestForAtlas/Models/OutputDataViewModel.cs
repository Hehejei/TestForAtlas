namespace TestForAtlas.Models
{
    public class OutputDataViewModel
    {
        public int PaymentNumber { get; set; } // Номер платежа

        public DateTime PaymentDate { get; set; } // Дата платежа

        public double BodyPaymentAmount { get; set; } // Размер платежа по телу

        public double MainDebt { get; set; } // Основной долг

        public double PaymentAmountForPercent { get; set; } // Размер платежа по %

        public double RemainingPrincipal { get; set; } // Остаток основного долга
    }
}
