namespace AdvGenCalculator.Models
{
    public enum TokenType
    {
        Number,
        Operator,
        Function,
        LeftParenthesis,
        RightParenthesis
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public double? NumericValue { get; }
        public int Precedence { get; }

        public Token(TokenType type, string value, double? numericValue = null)
        {
            Type = type;
            Value = value;
            NumericValue = numericValue;
            Precedence = GetPrecedence(type, value);
        }

        private static int GetPrecedence(TokenType type, string value)
        {
            return type switch
            {
                TokenType.Operator => value switch
                {
                    "+" or "-" => 1,
                    "*" or "/" or "%" => 2,
                    "^" => 3,
                    _ => 0
                },
                TokenType.Function => 4,
                _ => 0
            };
        }

        public bool IsRightAssociative => Type == TokenType.Operator && Value == "^";
    }
}