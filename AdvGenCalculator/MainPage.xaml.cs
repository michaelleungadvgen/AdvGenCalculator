using AdvGenCalculator.ViewModels;
using AdvGenCalculator.Services;

namespace AdvGenCalculator
{
    public partial class MainPage : ContentPage
    {
        private readonly ICalculatorViewModel _viewModel;

        // Constructor for DI
        public MainPage(ICalculatorViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // Parameterless constructor for fallback
        public MainPage()
        {
            InitializeComponent();
            // Create dependencies manually as fallback
            var tokenizer = new ExpressionTokenizer();
            var evaluator = new ShuntingYardExpressionEvaluator(tokenizer);
            _viewModel = new ImprovedCalculatorViewModel(evaluator);
            BindingContext = _viewModel;
        }

        private void OnModeSwitchToggled(object sender, ToggledEventArgs e)
        {
            _viewModel.IsScientificMode = e.Value;
        }

        private void OnCalculatorButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _viewModel.ProcessInput(button.Text);
            }
        }
    }
}
