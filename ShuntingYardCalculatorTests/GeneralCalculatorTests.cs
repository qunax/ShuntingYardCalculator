using ShuntingYardCalculator;

namespace ShuntingYardCalculatorTests
{
    public class GeneralCalculatorTests
    {
        [Fact]
        public void NumberValidation_True_Test()
        {
            bool result = Calculator.IsValidNumber("1.2");

            Assert.True(result);
        }



        [Fact]
        public void NumberValidation_False_Test()
        {
            bool result = Calculator.IsValidNumber("1,2");

            Assert.False(result);
        }



        [Fact]
        public void OperationValidation_True_Test()
        {
            bool result = Calculator.IsOperator("+");

            Assert.True(result);
        }



        [Fact]
        public void OperationValidation_False_Test()
        {
            bool result = Calculator.IsOperator("?");

            Assert.False(result);
        }



        [Theory]
        [InlineData(new object[] { new string[] { "1", "+", "2", "+", "3", "*", "2", "/", "2", "+", "4" }, 10.0 })]
        [InlineData(new object[] { new string[] { "5", "*", "3", "+", "8", "/", "4", "-", "2" }, 15.0 })]
        [InlineData(new object[] { new string[] { "7", "+", "8", "/", "2", "*", "3", "-", "5" }, 14.0 })]
        [InlineData(new object[] { new string[] { "9", "-", "3", "+", "8", "*", "2", "/", "4" }, 10.0 })]
        public void CalculatorEvaluateExpression_Test(string[] expression, double expected)
        {
            double result = Calculator.Evaluate(new List<string>(expression));
            Assert.Equal(expected, result, 1);
        }
    }
}
