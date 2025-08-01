using AdvGenCalculator.Models;

namespace AdvGenCalculator.Services
{
    public interface IExpressionEvaluator
    {
        CalculationResult Evaluate(string expression);
    }
}