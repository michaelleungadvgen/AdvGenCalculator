using AdvGenCalculator.Core.Services;

var tokenizer = new ExpressionTokenizer();
var evaluator = new ShuntingYardExpressionEvaluator(tokenizer);

Console.WriteLine("AdvGen Calculator - Console Test");
Console.WriteLine("Demonstrating fixed operator precedence and clean architecture");
Console.WriteLine("===============================================================");

// Test cases that were problematic before
var testCases = new[]
{
    "2 + 3 * 4",        // Should be 14 (not 20)
    "2 ^ 3 ^ 2",        // Should be 512 (right associative: 2^(3^2))
    "3 + 2 ^ 2",        // Should be 7 (precedence: 3 + (2^2))
    "2 * 3 ^ 2",        // Should be 18 (precedence: 2 * (3^2))
    "sin(30) + cos(60)", // Should be 1 (0.5 + 0.5)
    "sqrt(2 ^ 2)",      // Should be 2
    "(2 + 3) * 4",      // Should be 20
    "2 + (3 * 4)",      // Should be 14
    "sqrt(sin(30))",    // Should be ~0.707
};

foreach (var expression in testCases)
{
    var result = evaluator.Evaluate(expression);
    if (result.IsSuccess)
    {
        Console.WriteLine($"{expression,-20} = {result.Value:F3}");
    }
    else
    {
        Console.WriteLine($"{expression,-20} = ERROR: {result.ErrorMessage}");
    }
}

Console.WriteLine();
Console.WriteLine("Interactive mode - Enter expressions (or 'quit' to exit):");
Console.WriteLine("Try: 2^3^2, sin(30)+cos(60), sqrt(16), (2+3)*4");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input) || input.ToLower() == "quit")
        break;
        
    var result = evaluator.Evaluate(input);
    if (result.IsSuccess)
    {
        Console.WriteLine($"  = {result.Value}");
    }
    else
    {
        Console.WriteLine($"  ERROR: {result.ErrorMessage}");
    }
}