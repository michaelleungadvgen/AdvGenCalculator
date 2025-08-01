using AdvGenCalculator.Models;

namespace AdvGenCalculator.Services
{
    public class ShuntingYardExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExpressionTokenizer _tokenizer;

        public ShuntingYardExpressionEvaluator(IExpressionTokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public CalculationResult Evaluate(string expression)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(expression))
                    return CalculationResult.Error("Empty expression");

                var tokens = _tokenizer.Tokenize(expression).ToList();
                if (!tokens.Any())
                    return CalculationResult.Error("Invalid expression");

                var postfixTokens = ConvertToPostfix(tokens);
                var result = EvaluatePostfix(postfixTokens);

                return CalculationResult.Success(result);
            }
            catch (DivideByZeroException)
            {
                return CalculationResult.Error("Cannot divide by zero");
            }
            catch (ArgumentException ex)
            {
                return CalculationResult.Error(ex.Message);
            }
            catch (Exception)
            {
                return CalculationResult.Error("Invalid expression");
            }
        }

        private List<Token> ConvertToPostfix(List<Token> tokens)
        {
            var output = new List<Token>();
            var operatorStack = new Stack<Token>();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        output.Add(token);
                        break;

                    case TokenType.Function:
                        operatorStack.Push(token);
                        break;

                    case TokenType.LeftParenthesis:
                        operatorStack.Push(token);
                        break;

                    case TokenType.RightParenthesis:
                        while (operatorStack.Count > 0 && operatorStack.Peek().Type != TokenType.LeftParenthesis)
                        {
                            output.Add(operatorStack.Pop());
                        }
                        
                        if (operatorStack.Count == 0)
                            throw new ArgumentException("Mismatched parentheses");
                        
                        operatorStack.Pop(); // Remove left parenthesis
                        
                        // If there's a function on top of the stack, add it to output
                        if (operatorStack.Count > 0 && operatorStack.Peek().Type == TokenType.Function)
                        {
                            output.Add(operatorStack.Pop());
                        }
                        break;

                    case TokenType.Operator:
                        while (operatorStack.Count > 0 &&
                               operatorStack.Peek().Type != TokenType.LeftParenthesis &&
                               (operatorStack.Peek().Precedence > token.Precedence ||
                                (operatorStack.Peek().Precedence == token.Precedence && !token.IsRightAssociative)))
                        {
                            output.Add(operatorStack.Pop());
                        }
                        operatorStack.Push(token);
                        break;
                }
            }

            while (operatorStack.Count > 0)
            {
                var op = operatorStack.Pop();
                if (op.Type == TokenType.LeftParenthesis)
                    throw new ArgumentException("Mismatched parentheses");
                output.Add(op);
            }

            return output;
        }

        private double EvaluatePostfix(List<Token> postfixTokens)
        {
            var stack = new Stack<double>();

            foreach (var token in postfixTokens)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        stack.Push(token.NumericValue!.Value);
                        break;

                    case TokenType.Operator:
                        if (stack.Count < 2)
                            throw new ArgumentException("Invalid expression");

                        var b = stack.Pop();
                        var a = stack.Pop();
                        var result = ApplyOperator(token.Value, a, b);
                        stack.Push(result);
                        break;

                    case TokenType.Function:
                        if (stack.Count < 1)
                            throw new ArgumentException("Invalid expression");

                        var operand = stack.Pop();
                        var funcResult = ApplyFunction(token.Value, operand);
                        stack.Push(funcResult);
                        break;
                }
            }

            if (stack.Count != 1)
                throw new ArgumentException("Invalid expression");

            return stack.Pop();
        }

        private double ApplyOperator(string op, double a, double b)
        {
            return op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0 ? throw new DivideByZeroException() : a / b,
                "%" => a * b / 100,
                "^" => Math.Pow(a, b),
                _ => throw new ArgumentException($"Unknown operator: {op}")
            };
        }

        private double ApplyFunction(string function, double operand)
        {
            return function switch
            {
                "sin" => Math.Sin(DegreesToRadians(operand)),
                "cos" => Math.Cos(DegreesToRadians(operand)),
                "tan" => Math.Tan(DegreesToRadians(operand)),
                "sqrt" => operand < 0 ? throw new ArgumentException("Cannot calculate square root of negative number") : Math.Sqrt(operand),
                _ => throw new ArgumentException($"Unknown function: {function}")
            };
        }

        private double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
    }
}