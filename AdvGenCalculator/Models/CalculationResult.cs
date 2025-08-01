namespace AdvGenCalculator.Models
{
    public class CalculationResult
    {
        public double Value { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private CalculationResult(double value, bool isSuccess, string errorMessage = "")
        {
            Value = value;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static CalculationResult Success(double value) => new(value, true);
        public static CalculationResult Error(string message) => new(0, false, message);
    }
}