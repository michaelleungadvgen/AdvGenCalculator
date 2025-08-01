using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using AdvGenCalculator.Services;
using AdvGenCalculator.ViewModels;

namespace AdvGenCalculator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<IExpressionTokenizer, ExpressionTokenizer>();
            builder.Services.AddSingleton<IExpressionEvaluator, ShuntingYardExpressionEvaluator>();
            builder.Services.AddSingleton<ICalculatorViewModel, ImprovedCalculatorViewModel>();
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}