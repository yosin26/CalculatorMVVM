using System;

namespace Calculator.Model
{
    public class CalculatorModel
    {
        private double _a;
        private double _b;
        private string _operation;
        private double _result;
        private double _memory;

        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

        public double B
        {
            get { return _b; }
            set { _b = value; }
        }

        public string Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }

        public double Memory
        {
            get { return _memory; }
            set { _memory = value; }
        }

        public double Result => _result;

        public void Calculate()
        {
            switch (_operation)
            {
                case "+":
                    _result = _a + _b;
                    break;
                case "-":
                    _result = _a - _b;
                    break;
                case "*":
                    _result = _a * _b;
                    break;
                case "/":
                    if (_b != 0)
                        _result = _a / _b;
                    else
                        throw new InvalidOperationException("Division by zero is not allowed.");
                    break;
                case "^":
                    _result = Math.Pow(_a, _b);
                    break;
                case "1/x":
                    CalculateSingleOperand();
                    break;
                case "sqrt":
                    CalculateSingleOperand();
                    break;
                case "%":
                    CalculateSingleOperand();
                    break;
                default:
                    throw new InvalidOperationException("Invalid operation.");
            }
        }

        public void Clear()
        {
            _a = 0;
            _b = 0;
            _operation = string.Empty;
            _result = 0;
        }

        public void CalculateSingleOperand()
        {
            switch (_operation)
            {
                case "1/x":
                    if (_a != 0)
                        _result = 1 / _a;
                    else
                        throw new InvalidOperationException("Cannot divide by zero.");
                    break;
                case "sqrt":
                    if (_a >= 0)
                        _result = Math.Sqrt(_a);
                    else
                        throw new InvalidOperationException("Cannot take the square root of a negative number.");
                    break;
                case "%":
                    _result = _a / 100;
                    break;
                default:
                    throw new InvalidOperationException("Invalid operation for single operand.");
            }
        }

        public void MemoryClear() => _memory = 0;

        public void MemoryAdd(string a)
        {
            if (double.TryParse(a, out double value))
                _memory += value;
        }

        public void MemorySubtract(string a)
        {
            if (double.TryParse(a, out double value))
                _memory -= value;
        }

        public void MemoryStore(string a)
        {
            if (double.TryParse(a, out double value))
                _memory = value;
        }

        public string MemoryRecall() => _memory.ToString();

        public void ToggleSign()
        {
            _a = -_a;
            _result = _a;
        }
    }
}
