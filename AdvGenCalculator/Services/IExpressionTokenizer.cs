using AdvGenCalculator.Models;

namespace AdvGenCalculator.Services
{
    public interface IExpressionTokenizer
    {
        IEnumerable<Token> Tokenize(string expression);
    }
}