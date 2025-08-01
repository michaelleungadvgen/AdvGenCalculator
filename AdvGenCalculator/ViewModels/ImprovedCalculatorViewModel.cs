using AdvGenCalculator.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdvGenCalculator.ViewModels
{
    public class ImprovedCalculatorViewModel : ICalculatorViewModel
    {
        private readonly IExpressionEvaluator _evaluator;
        private string _currentValue = "0";
        private string _expression = "";
        private bool _isScientificMode = false;
        private bool _hasResult = false;
        private bool _lastWasOperator = false;
        private bool _expectingOperand = true;

        public ImprovedCalculatorViewModel(IExpressionEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        public string CurrentValue
        {
            get => _currentValue;
            private set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Expression
        {
            get => _expression;
            private set
            {
                if (_expression != value)
                {
                    _expression = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsScientificMode
        {
            get => _isScientificMode;
            set
            {
                if (_isScientificMode != value)
                {
                    _isScientificMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public void ProcessInput(string input)
        {
            switch (input)
            {
                case "C":
                    Clear();
                    break;
                case "CE":
                    ClearEntry();
                    break;
                case "⌫":
                    Backspace();
                    break;
                case "=":
                    Calculate();
                    break;
                case ".":
                    AddDecimal();
                    break;
                case "+":
                case "-":
                case "×":
                case "÷":
                case "^":
                    AddOperator(input);
                    break;
                case "%":
                    AddPercentage();
                    break;
                case "(":
                    AddLeftParenthesis();
                    break;
                case ")":
                    AddRightParenthesis();
                    break;
                case "sin":
                case "cos":
                case "tan":
                    AddTrigFunction(input);
                    break;
                case "√":
                    AddSquareRoot();
                    break;
                default:
                    if (IsDigit(input))
                        AddDigit(input);
                    break;
            }
        }

        public void Clear()
        {
            CurrentValue = "0";
            Expression = "";
            _hasResult = false;
            _lastWasOperator = false;
            _expectingOperand = true;
        }

        public void ClearEntry()
        {
            CurrentValue = "0";
            _expectingOperand = true;
        }

        private void Backspace()
        {
            if (_hasResult) return;

            if (CurrentValue.Length > 1)
            {
                CurrentValue = CurrentValue.Substring(0, CurrentValue.Length - 1);
            }
            else
            {
                CurrentValue = "0";
                _expectingOperand = true;
            }
        }

        private void AddDigit(string digit)
        {
            if (_hasResult)
            {
                Clear();
            }

            if (CurrentValue == "0" || _expectingOperand)
            {
                CurrentValue = digit;
            }
            else
            {
                CurrentValue += digit;
            }

            _expectingOperand = false;
            _lastWasOperator = false;
        }

        private void AddDecimal()
        {
            if (_hasResult)
            {
                Clear();
            }

            if (_expectingOperand)
            {
                CurrentValue = "0.";
            }
            else if (!CurrentValue.Contains("."))
            {
                CurrentValue += ".";
            }

            _expectingOperand = false;
            _lastWasOperator = false;
        }

        private void AddOperator(string op)
        {
            if (_hasResult)
            {
                // Continue with the result
                Expression = CurrentValue;
                _hasResult = false;
            }
            else if (!_lastWasOperator && !_expectingOperand)
            {
                Expression += CurrentValue;
            }

            // Convert display operators to internal operators
            string internalOp = op switch
            {
                "×" => "*",
                "÷" => "/",
                _ => op
            };

            if (_lastWasOperator)
            {
                // Replace the last operator
                Expression = Expression.TrimEnd().TrimEnd('+', '-', '*', '/', '^');
            }

            Expression += $" {internalOp} ";
            _lastWasOperator = true;
            _expectingOperand = true;
        }

        private void AddPercentage()
        {
            if (_hasResult)
            {
                Expression = CurrentValue;
                _hasResult = false;
            }

            if (!_expectingOperand)
            {
                Expression += CurrentValue + " % ";
                _lastWasOperator = true;
                _expectingOperand = true;
            }
        }

        private void AddLeftParenthesis()
        {
            if (_hasResult)
            {
                Clear();
            }

            if (_expectingOperand || _lastWasOperator)
            {
                Expression += "(";
                _expectingOperand = true;
                _lastWasOperator = false;
            }
        }

        private void AddRightParenthesis()
        {
            if (!_expectingOperand && !_lastWasOperator)
            {
                Expression += CurrentValue + ")";
                CurrentValue = "0";
                _expectingOperand = true;
                _lastWasOperator = false;
            }
        }

        private void AddTrigFunction(string function)
        {
            if (_hasResult)
            {
                Clear();
            }

            if (_expectingOperand || _lastWasOperator)
            {
                Expression += $"{function}(";
                _expectingOperand = true;
                _lastWasOperator = false;
            }
        }

        private void AddSquareRoot()
        {
            if (_hasResult)
            {
                Clear();
            }

            if (_expectingOperand || _lastWasOperator)
            {
                Expression += "sqrt(";
                _expectingOperand = true;
                _lastWasOperator = false;
            }
        }

        private void Calculate()
        {
            try
            {
                string finalExpression = BuildFinalExpression();
                
                if (string.IsNullOrWhiteSpace(finalExpression))
                {
                    return;
                }

                var result = _evaluator.Evaluate(finalExpression);
                
                if (result.IsSuccess)
                {
                    CurrentValue = FormatResult(result.Value);
                    Expression = $"{finalExpression} =";
                    _hasResult = true;
                    _expectingOperand = true;
                    _lastWasOperator = false;
                }
                else
                {
                    CurrentValue = "Error";
                    Expression = result.ErrorMessage;
                    _hasResult = true;
                }
            }
            catch (Exception ex)
            {
                CurrentValue = "Error";
                Expression = ex.Message;
                _hasResult = true;
            }
        }

        private string BuildFinalExpression()
        {
            var expr = Expression.Trim();
            
            if (string.IsNullOrEmpty(expr))
            {
                return CurrentValue;
            }

            // If the expression doesn't end with an operator, append the current value
            if (!_lastWasOperator && !_expectingOperand && CurrentValue != "0")
            {
                expr += CurrentValue;
            }

            // Auto-close any open parentheses
            int openCount = expr.Count(c => c == '(');
            int closeCount = expr.Count(c => c == ')');
            int missingClose = openCount - closeCount;

            if (missingClose > 0)
            {
                expr += new string(')', missingClose);
            }

            return expr;
        }

        private string FormatResult(double value)
        {
            // Handle very large or very small numbers
            if (Math.Abs(value) >= 1e15 || (Math.Abs(value) <= 1e-15 && value != 0))
            {
                return value.ToString("E2");
            }

            // Remove unnecessary decimal places
            return value % 1 == 0 ? value.ToString("F0") : value.ToString("G15");
        }

        private bool IsDigit(string input) => input.Length == 1 && char.IsDigit(input[0]);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}