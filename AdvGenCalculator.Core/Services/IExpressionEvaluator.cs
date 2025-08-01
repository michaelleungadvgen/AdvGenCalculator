using AdvGenCalculator.Core.Models;

namespace AdvGenCalculator.Core.Services
{
    public interface IExpressionEvaluator
    {
        CalculationResult Evaluate(string expression);
    }
}