using System;
using System.Windows.Input;
using Calculator.Model;

namespace Calculator.ViewModel
{
    public class CalculatorViewModel : BaseViewModel
    {
        private readonly CalculatorModel _calculatorModel;
        private string _displayText;
        private string _currentOperation;
        private double _firstOperand;
        private double _secondOperand;
        private bool _isSecondOperandEntered;
        private bool _isDivideByZeroError;

        public CalculatorViewModel(CalculatorModel calculatorModel)
        {
            _calculatorModel = calculatorModel ?? throw new ArgumentNullException(nameof(calculatorModel));
            _displayText = "0";
            _currentOperation = string.Empty;
            _firstOperand = 0;
            _secondOperand = 0;
            _isSecondOperandEntered = false;
            _isDivideByZeroError = false;

            NumberCommand = new RelayCommand<string>(OnNumberClicked);
            OperationCommand = new RelayCommand<string>(OnOperationClicked);
            EqualsCommand = new RelayCommand<string>(_ => OnEqualsClicked());
            ClearCommand = new RelayCommand<string>(_ => OnClearClicked());
            AddCommand = new RelayCommand<string>(_ => MemoryAdd());
            RecallCommand = new RelayCommand<string>(_ => MemoryRecall());
            MemoryClearCommand = new RelayCommand<string>(_ => MemoryClear());
            MemoryStoreCommand = new RelayCommand<string>(_ => MemoryStore());
            SquareRootCommand = new RelayCommand<string>(_ => CalculateSquareRoot());
            ReciprocalCommand = new RelayCommand<string>(_ => CalculateReciprocal());
            PlusMinusCommand = new RelayCommand<string>(_ => ToggleSign());
            SquareCommand = new RelayCommand<string>(_ => CalculateSquare());
            DecimalCommand = new RelayCommand<string>(_ => AddDecimalPoint());
            PercentageCommand = new RelayCommand<string>(_ => CalculatePercentage());
            BackspaceCommand = new RelayCommand<string>(_ => Backspace());
        }

        public string DisplayText
        {
            get => _displayText;
            set
            {
                _displayText = value;
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        public ICommand BackspaceCommand { get; }
        public ICommand NumberCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RecallCommand { get; }
        public ICommand MemoryClearCommand { get; }
        public ICommand MemoryStoreCommand { get; }
        public ICommand SquareRootCommand { get; }
        public ICommand ReciprocalCommand { get; }
        public ICommand PlusMinusCommand { get; }
        public ICommand SquareCommand { get; }
        public ICommand DecimalCommand { get; }
        public ICommand PercentageCommand { get; }

        private void OnNumberClicked(string number)
        {
            if (_isDivideByZeroError)
            {
                _isDivideByZeroError = false;
                DisplayText = "0";
            }

            if (_isSecondOperandEntered)
            {
                DisplayText = number;
                _isSecondOperandEntered = false;
            }
            else
            {
                DisplayText = DisplayText == "0" ? number : DisplayText + number;
            }
        }


        public void Backspace()
        {
            if (_isDivideByZeroError)
            {
                DisplayText = "0";
                _isDivideByZeroError = false;
                return;
            }

            if (DisplayText.Length > 1)
            {
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
            }
            else
            {
                DisplayText = "0";
            }
        }


        private void OnOperationClicked(string operation)
        {
            if (_isDivideByZeroError) return;

            if (double.TryParse(DisplayText, out double currentOperand))
            {
                if (string.IsNullOrEmpty(_currentOperation))
                {
                    _firstOperand = currentOperand;
                }
                else
                {
                    _secondOperand = currentOperand;
                    CalculateIntermediateResult();
                }

                _currentOperation = operation;
                _isSecondOperandEntered = true;
            }
            else
            {
                DisplayText = "Error";
            }
        }

        private void OnEqualsClicked()
        {
            if (double.TryParse(DisplayText, out _secondOperand))
            {
                CalculateIntermediateResult();
                _currentOperation = string.Empty; // Clear the operation after final result
            }
            else
            {
                DisplayText = "Error";
            }
        }

        private void CalculateIntermediateResult()
        {
            try
            {
                _calculatorModel.A = _firstOperand;
                _calculatorModel.B = _secondOperand;
                _calculatorModel.Operation = _currentOperation;
                _calculatorModel.Calculate();

                _firstOperand = _calculatorModel.Result;
                DisplayText = _firstOperand.ToString();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Division by zero"))
            {
                DisplayText = "Cannot divide by zero";
                _isDivideByZeroError = true;
            }
            catch (Exception)
            {
                DisplayText = "Error";
            }
        }

        private void OnClearClicked()
        {
            _firstOperand = 0;
            _secondOperand = 0;
            _currentOperation = string.Empty;
            DisplayText = "0";
            _isSecondOperandEntered = false;
            _isDivideByZeroError = false;
        }

        private void MemoryAdd()
        {
            _calculatorModel.MemoryAdd(DisplayText);
        }

        private void MemoryRecall()
        {
            DisplayText = _calculatorModel.MemoryRecall();
        }

        private void MemoryClear()
        {
            _calculatorModel.MemoryClear();
        }

        private void MemoryStore()
        {
            _calculatorModel.MemoryStore(DisplayText);
        }

        private void CalculateSquareRoot()
        {
            if (double.TryParse(DisplayText, out double operand) && operand >= 0)
            {
                _calculatorModel.A = operand;
                _calculatorModel.Operation = "sqrt";
                _calculatorModel.Calculate();

                DisplayText = _calculatorModel.Result.ToString();
                _firstOperand = _calculatorModel.Result;
                _isSecondOperandEntered = true;
            }
            else
            {
                DisplayText = "Error";
            }
        }

        private void CalculateReciprocal()
        {
            if (double.TryParse(DisplayText, out double operand) && operand != 0)
            {
                _calculatorModel.A = operand;
                _calculatorModel.Operation = "1/x";
                _calculatorModel.CalculateSingleOperand();

                DisplayText = _calculatorModel.Result.ToString();
                _firstOperand = _calculatorModel.Result;
                _isSecondOperandEntered = true;
            }
            else
            {
                DisplayText = "Error";
            }
        }

        private void ToggleSign()
        {
            if (double.TryParse(DisplayText, out double currentValue))
            {
                DisplayText = (-currentValue).ToString();
            }
        }

        private void CalculateSquare()
        {
            if (double.TryParse(DisplayText, out double operand))
            {
                _calculatorModel.A = operand;
                _calculatorModel.B = 2;
                _calculatorModel.Operation = "^";
                _calculatorModel.Calculate();

                DisplayText = _calculatorModel.Result.ToString();
                _firstOperand = _calculatorModel.Result;
                _isSecondOperandEntered = true;
            }
        }

        private void AddDecimalPoint()
        {
            if (_isDivideByZeroError)
            {
                _isDivideByZeroError = false;
                DisplayText = "0";
            }

            if (!DisplayText.Contains(","))
            {
                DisplayText += ",";
            }
        }

        private void CalculatePercentage()
        {
            if (double.TryParse(DisplayText, out double value))
            {
                DisplayText = (value / 100).ToString();
            }
        }
    }
}
