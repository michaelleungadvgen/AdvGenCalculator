// CalculatorViewModel.cs
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AdvGenCalculator
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _currentValue = "0";
        private string _expression = "";
        private bool _isScientificMode = false;
        private bool _hasResult = false;

        public string CurrentValue
        {
            get => _currentValue;
            set
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
            set
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
            if (_hasResult)
            {
                if (IsDigit(input) || IsFunctionName(input))
                {
                    Expression = "";
                    CurrentValue = "0";
                }
                _hasResult = false;
            }

            switch (input)
            {
                case "C":
                    Expression = "";
                    CurrentValue = "0";
                    break;
                case "CE":
                    CurrentValue = "0";
                    break;
                case "⌫":
                    if (CurrentValue.Length > 1)
                        CurrentValue = CurrentValue.Substring(0, CurrentValue.Length - 1);
                    else
                        CurrentValue = "0";
                    break;
                case "+":
                case "-":
                case "×":
                case "÷":
                case "^":
                case "%":
                    AppendToExpression(CurrentValue, input);
                    CurrentValue = "0";
                    break;
                case "=":
                    CalculateResult();
                    break;
                case ".":
                    if (!CurrentValue.Contains("."))
                        CurrentValue += ".";
                    break;
                case "(":
                    Expression += input;
                    break;
                case ")":
                    AppendToExpression(CurrentValue, input);
                    CurrentValue = "0";
                    break;
                case "sin":
                case "cos":
                case "tan":
                    Expression += $"{input}(";
                    break;
                case "√":
                    Expression += "sqrt(";
                    break;
                default:
                    // For digits
                    if (IsDigit(input))
                    {
                        if (CurrentValue == "0")
                            CurrentValue = input;
                        else
                            CurrentValue += input;
                    }
                    break;
            }
        }

        private bool IsDigit(string input)
        {
            return int.TryParse(input, out _);
        }

        private bool IsFunctionName(string input)
        {
            return input == "sin" || input == "cos" || input == "tan" || input == "√";
        }

        private void AppendToExpression(string value, string operation)
        {
            string op = operation;
            switch (operation)
            {
                case "×":
                    op = "*";
                    break;
                case "÷":
                    op = "/";
                    break;
            }
            if (operation == ")")
            {
               Expression += value + ")";
            }
            else
            {
                Expression += value + " " + op + " ";
            }

        }
        private void CalculateResult()
        {
            try
            {
                // Handle the missing bracket case
                if (Expression.EndsWith("(") && CurrentValue == "0")
                {
                    Expression = Expression.Substring(0, Expression.Length - 1);
                }
                if (Expression.Contains("()"))
                {
                    Expression = Expression.Replace("()", string.Format("({0})",CurrentValue));
                }
                string finalExpression = Expression + CurrentValue;
                finalExpression = finalExpression.Replace("×", "*").Replace("÷", "/").Replace(") 0",")").Replace(")0", ")");

                // Use the MathExpressionEvaluator to calculate the result
                double result = MathExpressionEvaluator.Evaluate(finalExpression);
                CurrentValue = result.ToString();

                Expression = finalExpression + " =";
                _hasResult = true;
            }
            catch (Exception ex)
            {
                CurrentValue = "Error";
                Expression = ex.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BoolToModeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Scientific" : "Basic";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == "Scientific";
        }
    }
}