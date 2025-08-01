using AdvGenCalculator.Core.Models;

namespace AdvGenCalculator.Core.Services
{
    public interface IExpressionTokenizer
    {
        IEnumerable<Token> Tokenize(string expression);
    }
}