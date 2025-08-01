using AdvGenCalculator.Core.Services;
using Xunit;

namespace AdvGenCalculator.Core.Tests
{
    public class ExpressionEvaluatorTests
    {
        private readonly IExpressionEvaluator _evaluator;

        public ExpressionEvaluatorTests()
        {
            var tokenizer = new ExpressionTokenizer();
            _evaluator = new ShuntingYardExpressionEvaluator(tokenizer);
        }

        [Theory]
        [InlineData("2 + 3", 5)]
        [InlineData("2 - 3", -1)]
        [InlineData("2 * 3", 6)]
        [InlineData("6 / 2", 3)]
        [InlineData("10 % 3", 0.3)] // 10% of 3 = 0.3
        public void BasicOperations_ShouldEvaluateCorrectly(string expression, double expected)
        {
            var result = _evaluator.Evaluate(expression);
            
            Assert.True(result.IsSuccess, $"Expression '{expression}' failed: {result.ErrorMessage}");
            Assert.Equal(expected, result.Value, 10);
        }

        [Theory]
        [InlineData("2 ^ 3", 8)]
        [InlineData("2 ^ 3 ^ 2", 512)] // Right associative: 2^(3^2) = 2^9 = 512
        [InlineData("3 + 2 ^ 2", 7)] // Precedence: 3 + (2^2) = 3 + 4 = 7
        [InlineData("2 * 3 ^ 2", 18)] // Precedence: 2 * (3^2) = 2 * 9 = 18
        public void ExponentOperations_ShouldRespectPrecedenceAndAssociativity(string expression, double expected)
        {
            var result = _evaluator.Evaluate(expression);
            
            Assert.True(result.IsSuccess, $"Expression '{expression}' failed: {result.ErrorMessage}");
            Assert.Equal(expected, result.Value, 10);
        }

        [Theory]
        [InlineData("2 + 3 * 4", 14)] // 2 + (3 * 4) = 14
        [InlineData("(2 + 3) * 4", 20)] // (2 + 3) * 4 = 20
        [InlineData("2 * 3 + 4", 10)] // (2 * 3) + 4 = 10
        [InlineData("2 * (3 + 4)", 14)] // 2 * (3 + 4) = 14
        public void OperatorPrecedence_ShouldEvaluateCorrectly(string expression, double expected)
        {
            var result = _evaluator.Evaluate(expression);
            
            Assert.True(result.IsSuccess, $"Expression '{expression}' failed: {result.ErrorMessage}");
            Assert.Equal(expected, result.Value, 10);
        }

        [Theory]
        [InlineData("sin(0)", 0)]
        [InlineData("sin(30)", 0.5)]
        [InlineData("cos(0)", 1)]
        [InlineData("cos(60)", 0.5)]
        [InlineData("tan(0)", 0)]
        [InlineData("sqrt(4)", 2)]
        [InlineData("sqrt(9)", 3)]
        public void ScientificFunctions_ShouldEvaluateCorrectly(string expression, double expected)
        {
            var result = _evaluator.Evaluate(expression);
            
            Assert.True(result.IsSuccess, $"Expression '{expression}' failed: {result.ErrorMessage}");
            Assert.Equal(expected, result.Value, 2);
        }

        [Theory]
        [InlineData("sin(30) + cos(60)", 1)] // 0.5 + 0.5 = 1
        [InlineData("sqrt(sin(30))", 0.707)] // sqrt(0.5) ≈ 0.707
        [InlineData("2 * sqrt(4)", 4)] // 2 * 2 = 4
        [InlineData("sqrt(2 ^ 2)", 2)] // sqrt(4) = 2
        public void ComplexExpressions_ShouldEvaluateCorrectly(string expression, double expected)
        {
            var result = _evaluator.Evaluate(expression);
            
            Assert.True(result.IsSuccess, $"Expression '{expression}' failed: {result.ErrorMessage}");
            Assert.Equal(expected, result.Value, 2);
        }

        [Theory]
        [InlineData("((2 + 3))", 5)]
        [InlineData("(2 + (3 * 4))", 14)]
        [InlineData("((2 + 3) * (4 + 5))", 45)]
        public void NestedParentheses_ShouldEvaluateCorrectly(string expression, double expected)
        {
            var result = _evaluator.Evaluate(expression);
            
            Assert.True(result.IsSuccess, $"Expression '{expression}' failed: {result.ErrorMessage}");
            Assert.Equal(expected, result.Value, 10);
        }

        [Theory]
        [InlineData("5 / 0")]
        [InlineData("sqrt(-1)")]
        [InlineData("")]
        [InlineData("2 +")]
        [InlineData("(2 + 3")]
        [InlineData("2 + 3)")]
        public void InvalidExpressions_ShouldReturnError(string expression)
        {
            var result = _evaluator.Evaluate(expression);
            
            Assert.False(result.IsSuccess, $"Expression '{expression}' should have failed but returned: {result.Value}");
            Assert.NotEmpty(result.ErrorMessage);
        }
    }
}