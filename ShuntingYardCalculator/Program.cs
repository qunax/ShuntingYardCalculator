using System.Globalization;

namespace ShuntingYardCalculator
{
    //here are all our implemented operations in strategy pattern
    public interface IOperation
    {
        double Execute(double num1, double num2);
    }


   
    public class OperationAdd : IOperation
    {
        public double Execute(double num1, double num2) => num1 + num2;
    }

    public class OperationSubtract : IOperation
    {
        public double Execute(double num1, double num2) => num1 - num2;
    }

    public class OperationMultiplicate : IOperation
    {
        public double Execute(double num1, double num2) => num1 * num2;
    }

    public class OperationDivide : IOperation
    {
        public double Execute(double num1, double num2)
        {
            if (num2 == 0)
                throw new ArgumentException("Nemůžete dělit nulou");
            return num1 / num2;
        }
    }



    public class OperationContext
    {
        private IOperation _operation;
        public IOperation Operation { set { _operation = value; } }

        public double ExecuteOperation(double num1, double num2)
        {
            if (_operation == null)
                throw new NullReferenceException("Operstion must be assigned before executing.");

            return _operation.Execute(num1, num2);
        }
    }



    public class Operator
    {
        public string Sign { get; set; }
        //here we already have priority for operation so its easier to add new ones
        public int Precedence { get; set; } 
        public IOperation Operation { get; set;}



        public Operator(string sign, int precedence, IOperation operation)
        {
            Sign = sign;
            Precedence = precedence;
            Operation = operation;
        }
    }



    public class Calculator
    {
        private readonly static List<Operator> _operators = new List<Operator>
        {
            new Operator("+", 1, new OperationAdd()),
            new Operator ("-", 1, new OperationSubtract()),
            new Operator ("*", 2, new OperationMultiplicate()),
            new Operator ("/", 2, new OperationDivide())
        };



        public static List<Operator> Operators { get {  return _operators; } }

        
        //here is evaluation part of Shunting Yard Algorithm which reads converted string in postfix notation 
        public static double Evaluate(List<string> expression)
        {
            OperationContext opContext = new OperationContext();
            List<string> rpn = ConvertToRPN(expression);
            Stack<double> stack = new Stack<double>();

            foreach (var token in rpn)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (IsOperator(token))
                {
                    double num2 = stack.Pop();
                    double num1 = stack.Pop();
                    opContext.Operation = _operators.Where(o => o.Sign == token).First().Operation;
                    double result = opContext.ExecuteOperation(num1, num2);
                    stack.Push(result);
                }
            }

            return stack.Pop();
        }


        //here is used shunting yard algorithm to ease future expanding of operators list
        private static List<string> ConvertToRPN(List<string> expression)
        {
            List<string> output = new List<string>();
            Stack<string> operators = new Stack<string>();

            foreach (var token in expression)
            {
                if (double.TryParse(token, out _))
                {
                    output.Add(token);
                }
                else if (IsOperator(token))
                {
                    while (operators.Count > 0 && GetPrecedence(operators.Peek()) >= GetPrecedence(token))
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return output;
        }



        private static int GetPrecedence(string opSign)
        {
            Operator op = _operators.Where(o => o.Sign == opSign).First();
            if (op == null)
                throw new NullReferenceException("No such operator defined: " + opSign);

            return op.Precedence;
        }



        public static bool IsValidNumber(string input)
        {
            return Double.TryParse(input, NumberStyles.Float, new CultureInfo("en-GB"), out _);
        }



        public static bool IsOperator(string input)
        {
            return _operators.Where(o => o.Sign == input).Any() || input == "=";
        }
    }



    class Program
    {
        private static string ReadValidDecimalNumber()
        {
            string number = Console.ReadLine() ?? string.Empty;
            while (!Calculator.IsValidNumber(number))
            {
                Console.WriteLine("To není číslo, zadejte číslo znovu (desetinná čárka se píše 1.1)");
                number = Console.ReadLine() ?? string.Empty;
            }
            return number;
        }



        private static string ReadValidExprOperator()
        {
            string exprOperator = Console.ReadLine() ?? string.Empty;
            while (!Calculator.IsOperator(exprOperator))
            {
                Console.WriteLine("To není znaménko, zadejte znovu znaménko +, -, *, / nebo =");
                exprOperator = Console.ReadLine();
            }
            return exprOperator;
        }



        private static List<string> ReadExpression()
        {
            List<string> expression = new List<string>();
            string currentInput;
            
            while (true)
            {
                Console.WriteLine("Zadejte číslo (desetinná čárka se píše 1.1):");
                currentInput = ReadValidDecimalNumber();
                expression.Add(currentInput);

                Console.WriteLine("Váš příklad: " + string.Join(" ", expression));

                Console.WriteLine("Zadejte znaménko '+', '-', '*', '/' nebo '=':");
                currentInput = ReadValidExprOperator();
                if (currentInput == "=")
                    break;
                expression.Add(currentInput);

                Console.WriteLine("Váš příklad: " + string.Join(" ", expression));
            }

            return expression;
        }



        static void Main(string[] args)
        {
            while (true){
                List<string> expression = ReadExpression();

                try
                {
                    double result = Calculator.Evaluate(expression);
                    Console.WriteLine($"Výsledek příkladu {string.Join(" ", expression)} je {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Chyba: {ex.Message}");
                }

                Console.WriteLine("Pokud chcete spustit novy výpočet napište a:");
                if (Console.ReadLine() != "a")
                    break;
            }
        }
    }
}