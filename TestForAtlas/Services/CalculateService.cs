using TestForAtlas.Models;

namespace TestForAtlas.Services
{
    public interface ICalculateService
    {
        public double AnnuityPaymentCalculation(InputDataViewModel InputData);
        public IEnumerable<OutputDataViewModel> GetListPayment(InputDataViewModel InputData);
    }

    public class CalculateService: ICalculateService
    {
        public double AnnuityPaymentCalculation(InputDataViewModel InputData)
        {
            var i = (InputData.RateInYear / 12) / 100;
            var Up = (i * Math.Pow((1 + i), InputData.LoanTerm));
            var Do = (Math.Pow((1 + i), InputData.LoanTerm) - 1);
            var K = Up / Do;
            var A = K * InputData.LoanAmount;

            return A;
        }

        public IEnumerable<OutputDataViewModel> GetListPayment(InputDataViewModel InputData)
        {
            var ListPayment = new List<OutputDataViewModel>();

            var i = (InputData.RateInYear / 12) / 100;

            var A = AnnuityPaymentCalculation(InputData);
            var MainDebt = A;

            var Balance = InputData.LoanAmount;
            var Date = DateTime.UtcNow;

            for (var count = 1; count <= InputData.LoanTerm; count++)
            {
                var percent = Balance * i;
                MainDebt = A - percent;
                Date = Date.AddMonths(1);
                ListPayment.Add(new OutputDataViewModel { PaymentNumber = count, PaymentDate = Date, PaymentAmountForPercent = Math.Round(percent, 2), MainDebt = Math.Round(MainDebt, 2), BodyPaymentAmount = Math.Round(A, 2), RemainingPrincipal = Math.Round(Balance, 2) });
                Balance = Balance - MainDebt;
                
            }
            return ListPayment;
        }
    }
}
