using ShuntingYardCalculator;

namespace ShuntingYardCalculatorTests
{
    public class CalculatorOperatorsTests
    {
        [Fact]
        public void OperationAdd_Test()
        {
            OperationContext opContext = new OperationContext();
            opContext.Operation = Calculator.Operators[0].Operation;

            double result = opContext.ExecuteOperation(5.5, 5);

            Assert.Equal(10.5, result);
        }



        [Fact]
        public void OperationSubstract_Test()
        {
            OperationContext opContext = new OperationContext();
            opContext.Operation = Calculator.Operators[1].Operation;

            double result = opContext.ExecuteOperation(5.5, 5);

            Assert.Equal(0.5, result);
        }



        [Fact]
        public void OperationMultiplicate_Test()
        {
            OperationContext opContext = new OperationContext();
            opContext.Operation = Calculator.Operators[2].Operation;

            double result = opContext.ExecuteOperation(5.5, 5);

            Assert.Equal(27.5, result);
        }



        [Fact]
        public void OperationDivide_ValidDenomenator_Test()
        {
            OperationContext opContext = new OperationContext();
            opContext.Operation = Calculator.Operators[3].Operation;

            double result = opContext.ExecuteOperation(5.5, 5);

            Assert.Equal(1.1, result);
        }



        [Fact]
        public void OperationDivide_InvalidDenomenator_Test()
        {
            OperationContext opContext = new OperationContext();
            opContext.Operation = Calculator.Operators[3].Operation;

            Assert.Throws<ArgumentException>(() => opContext.ExecuteOperation(5.5, 0));
        }
    }
}