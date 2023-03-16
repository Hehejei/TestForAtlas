using TestForAtlas.Models;

namespace TestForAtlas.Services
{
    public interface ICalculateService
    {
        public double AnnuityPaymentCalculation(InputDataViewModel InputData);

        public IEnumerable<OutputDataViewModel> GetListPayments(InputDataViewModel InputData);

        public double AnnuityPaymentCalculation(FormLoanPerDayViewModel LoanPerDay);

        public IEnumerable<OutputDataViewModel> GetListPaymentsPerDay(FormLoanPerDayViewModel LoanPerDay);

    }

    public class CalculateService: ICalculateService
    {
        protected const int fullPercent = 100;
        protected const int year = 12;

        public double AnnuityPaymentCalculation(InputDataViewModel InputData)
        {
            var loanInterestRate = (InputData.RateInYear / year) / fullPercent;
            var Top = (loanInterestRate * Math.Pow((1 + loanInterestRate), InputData.LoanTerm));
            var Bottom = (Math.Pow((1 + loanInterestRate), InputData.LoanTerm) - 1);
            var AnnuityRatio = Top / Bottom;
            var AnnuityPayment = AnnuityRatio * InputData.LoanAmount;

            return AnnuityPayment;
        }

        public IEnumerable<OutputDataViewModel> GetListPayments(InputDataViewModel InputData)
        {
            var ListPayments = new List<OutputDataViewModel>();

            var loanInterestRate = (InputData.RateInYear / year) / fullPercent;
            var AnnuityPayment = AnnuityPaymentCalculation(InputData);
            var MainDebt = AnnuityPayment;
            var Balance = InputData.LoanAmount;
            var Date = DateTime.UtcNow;

            for (var count = 1; count <= InputData.LoanTerm; count++)
            {
                var AmountForPercent = Balance * loanInterestRate;
                MainDebt = AnnuityPayment - AmountForPercent;
                Date = Date.AddMonths(1);
                ListPayments.Add(
                    new OutputDataViewModel
                    { 
                        PaymentNumber = count,
                        PaymentDate = Date,
                        PaymentAmountForPercent = Math.Round(AmountForPercent, 2),
                        MainDebt = Math.Round(MainDebt, 2),
                        BodyPaymentAmount = Math.Round(AnnuityPayment, 2),
                        RemainingPrincipal = Math.Round(Balance, 2)
                    });
                Balance = Balance - MainDebt;         
            }
            return ListPayments;
        }

        public double AnnuityPaymentCalculation(FormLoanPerDayViewModel LoanPerDay)
        {
            var loanInterestRate = (LoanPerDay.RateInDay / fullPercent);
            var Top = (loanInterestRate * Math.Pow((1 + loanInterestRate), LoanPerDay.LoanTerm / LoanPerDay.StepPayment));
            var Bottom = (Math.Pow((1 + loanInterestRate), LoanPerDay.LoanTerm / LoanPerDay.StepPayment) - 1);
            var AnnuityRatio = Top / Bottom;
            var AnnuityPayment = AnnuityRatio * LoanPerDay.LoanAmount;

            return AnnuityPayment;
        }

        public IEnumerable<OutputDataViewModel> GetListPaymentsPerDay(FormLoanPerDayViewModel LoanPerDay)
        {
            var ListPayments = new List<OutputDataViewModel>();

            var countPayment = LoanPerDay.LoanTerm / LoanPerDay.StepPayment;
            var loanInterestRate = (LoanPerDay.RateInDay / fullPercent);
            var AnnuityPayment = AnnuityPaymentCalculation(LoanPerDay);
            var MainDebt = AnnuityPayment;

            var Balance = LoanPerDay.LoanAmount;
            var Date = DateTime.UtcNow;

            for (var count = 1; count <= countPayment; count++)
            {
                var AmountForPercent = Balance * loanInterestRate;
                MainDebt = AnnuityPayment - AmountForPercent;
                Date = Date.AddDays(LoanPerDay.StepPayment);
                ListPayments.Add(
                    new OutputDataViewModel
                    { 
                        PaymentNumber = count, 
                        PaymentDate = Date,
                        PaymentAmountForPercent = Math.Round(AmountForPercent, 2), 
                        MainDebt = Math.Round(MainDebt, 2), 
                        BodyPaymentAmount = Math.Round(AnnuityPayment, 2),
                        RemainingPrincipal = Math.Round(Balance, 2)
                    });
                Balance = Balance - MainDebt;
            }
            return ListPayments;
        }
    }
}
