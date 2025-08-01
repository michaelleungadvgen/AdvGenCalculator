using System.ComponentModel;

namespace AdvGenCalculator.ViewModels
{
    public interface ICalculatorViewModel : INotifyPropertyChanged
    {
        string CurrentValue { get; }
        string Expression { get; }
        bool IsScientificMode { get; set; }
        void ProcessInput(string input);
        void Clear();
        void ClearEntry();
    }
}