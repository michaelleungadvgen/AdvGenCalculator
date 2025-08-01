using AdvGenCalculator.Core.Models;
using System.Text;

namespace AdvGenCalculator.Core.Services
{
    public class ExpressionTokenizer : IExpressionTokenizer
    {
        private readonly Dictionary<string, TokenType> _operators = new()
        {
            { "+", TokenType.Operator },
            { "-", TokenType.Operator },
            { "*", TokenType.Operator },
            { "/", TokenType.Operator },
            { "%", TokenType.Operator },
            { "^", TokenType.Operator }
        };

        private readonly Dictionary<string, TokenType> _functions = new()
        {
            { "sin", TokenType.Function },
            { "cos", TokenType.Function },
            { "tan", TokenType.Function },
            { "sqrt", TokenType.Function }
        };

        public IEnumerable<Token> Tokenize(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                yield break;

            expression = expression.Replace(" ", "").Replace("×", "*").Replace("÷", "/");
            
            for (int i = 0; i < expression.Length; i++)
            {
                char current = expression[i];

                if (char.IsDigit(current) || current == '.')
                {
                    var number = ExtractNumber(expression, ref i);
                    if (double.TryParse(number, out double value))
                    {
                        yield return new Token(TokenType.Number, number, value);
                    }
                }
                else if (current == '(')
                {
                    yield return new Token(TokenType.LeftParenthesis, "(");
                }
                else if (current == ')')
                {
                    yield return new Token(TokenType.RightParenthesis, ")");
                }
                else if (char.IsLetter(current))
                {
                    var function = ExtractFunction(expression, ref i);
                    if (_functions.ContainsKey(function))
                    {
                        yield return new Token(TokenType.Function, function);
                    }
                }
                else if (_operators.ContainsKey(current.ToString()))
                {
                    yield return new Token(TokenType.Operator, current.ToString());
                }
            }
        }

        private string ExtractNumber(string expression, ref int index)
        {
            var number = new StringBuilder();
            bool hasDecimal = false;

            while (index < expression.Length)
            {
                char current = expression[index];
                
                if (char.IsDigit(current))
                {
                    number.Append(current);
                }
                else if (current == '.' && !hasDecimal)
                {
                    hasDecimal = true;
                    number.Append(current);
                }
                else
                {
                    index--; // Step back as we've gone too far
                    break;
                }
                
                index++;
            }

            return number.ToString();
        }

        private string ExtractFunction(string expression, ref int index)
        {
            var function = new StringBuilder();

            while (index < expression.Length && char.IsLetter(expression[index]))
            {
                function.Append(expression[index]);
                index++;
            }

            index--; // Step back as we've gone too far
            return function.ToString();
        }
    }
}