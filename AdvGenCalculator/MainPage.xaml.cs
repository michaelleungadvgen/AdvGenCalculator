namespace AdvGenCalculator
{
    public partial class MainPage : ContentPage
    {
        private CalculatorViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new CalculatorViewModel();
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
