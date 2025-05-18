// MathExpressionEvaluator.cs
using AdvGenCalculator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace AdvGenCalculator
{
    public class MathExpressionEvaluator
    {
        // Add this to the CalculatorViewModel class to replace the placeholder calculation code
        public static double Evaluate(string expression)
        {
            // Handle scientific functions
            expression = HandleScientificFunctions(expression);

            // Use DataTable's Compute method for basic arithmetic
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);

            return double.Parse((string)row["expression"]);
        }

        private static string HandleScientificFunctions(string expression)
        {
            // Regular expressions to find scientific function calls
            var sinRegex = new Regex(@"sin\s*\(([^)]+)\)");
            var cosRegex = new Regex(@"cos\s*\(([^)]+)\)");
            var tanRegex = new Regex(@"tan\s*\(([^)]+)\)");
            var sqrtRegex = new Regex(@"sqrt\s*\(([^)]+)\)");

            // Fixing the missing ")"
            expression = AutoCloseParentheses(expression);
            // Process each function
            expression = ProcessFunction(expression, sinRegex, arg => Math.Sin(double.Parse(arg) * Math.PI / 180));
            expression = ProcessFunction(expression, cosRegex, arg => Math.Cos(double.Parse(arg) * Math.PI / 180));
            expression = ProcessFunction(expression, tanRegex, arg => Math.Tan(double.Parse(arg) * Math.PI / 180));
            expression = ProcessFunction(expression, sqrtRegex, arg => Math.Sqrt(double.Parse(arg)));

            // Handle exponentiation (^)
            expression = HandleExponentiation(expression);

            return expression;
        }

        private static string AutoCloseParentheses(string expression)
        {
            int open = 0, close = 0;
            foreach (char c in expression)
            {
                if (c == '(') open++;
                else if (c == ')') close++;
            }
            int missing = open - close;
            if (missing > 0)
                expression += new string(')', missing);
            return expression;
        }

        private static string ProcessFunction(string expression, Regex regex, Func<string, double> function)
        {
            var result = expression;
            var match = regex.Match(result);

            while (match.Success)
            {
                var arg = match.Groups[1].Value;
                // Recursively evaluate the argument if it contains nested expressions
                if (regex.IsMatch(arg) || arg.Contains("("))
                {
                    arg = HandleScientificFunctions(arg);
                }

                // Evaluate the function
                var evaluated = function(arg).ToString(CultureInfo.InvariantCulture);

                // Replace the function call with its result
                result = result.Substring(0, match.Index) + evaluated + result.Substring(match.Index + match.Length);

                // Find the next match
                match = regex.Match(result);
            }

            return result;
        }

        private static string HandleExponentiation(string expression)
        {
            // Regular expression to find exponentiation operations
            var powRegex = new Regex(@"([\d.]+)\s*\^\s*([\d.]+)");
            var match = powRegex.Match(expression);

            while (match.Success)
            {
                var baseValue = double.Parse(match.Groups[1].Value);
                var exponent = double.Parse(match.Groups[2].Value);

                // Calculate the power
                var result = Math.Pow(baseValue, exponent).ToString(CultureInfo.InvariantCulture);

                // Replace the power expression with its result
                expression = expression.Substring(0, match.Index) + result + expression.Substring(match.Index + match.Length);

                // Find the next match
                match = powRegex.Match(expression);
            }

            return expression;
        }
    }
}

// Now update the CalculateResult method in CalculatorViewModel.cs:
