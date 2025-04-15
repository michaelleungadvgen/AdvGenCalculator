# AdvGen Calculator

A modern, dual-mode calculator built with .NET 9 and MAUI for Windows UI.

![AdvGen Calculator Screenshot][(https://raw.githubusercontent.com/username/AdvGenCalculator/main/screenshot.png](https://github.com/user-attachments/assets/3108db7c-78d9-48a7-be70-8273fc7cf22b))


## Overview

AdvGen Calculator is a demonstration project showcasing the capabilities of .NET 9 and MAUI (Multi-platform App UI) for building modern Windows applications. This calculator application features both basic and scientific calculation modes, providing a practical example of how to create versatile UI components with MAUI.

## Features

- **Dual Mode Operation**
  - Basic mode for standard arithmetic operations
  - Scientific mode with advanced mathematical functions

- **Basic Mode Features**
  - Addition, subtraction, multiplication, division
  - Percentage calculations
  - Clear and backspace functions
  - Decimal point support

- **Scientific Mode Features**
  - Trigonometric functions (sin, cos, tan)
  - Square root calculations
  - Exponentiation support
  - Parentheses for complex expressions

- **Modern UI Design**
  - Clean, responsive interface
  - Visual mode toggle
  - Expression history display
  - Intuitive button layout

## Technical Implementation

AdvGen Calculator demonstrates several key MAUI features:

- **XAML-based UI design**
- **MVVM architecture pattern**
- **Data binding**
- **Value converters**
- **Responsive layout using Grids**
- **Custom styling and visual effects**
- **Mathematical expression parsing and evaluation**

## Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio 2025 or newer (with MAUI workload installed)

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/username/AdvGenCalculator.git
   ```

2. Open the solution in Visual Studio:
   ```
   cd AdvGenCalculator
   start AdvGenCalculator.sln
   ```

3. Build and run the application:
   - Select your target device (Windows)
   - Press F5 or click the Run button

## Project Structure

```
AdvGenCalculator/
├── App.xaml                  # Application resources and styles
├── AppShell.xaml             # Application shell configuration
├── MainPage.xaml             # Main calculator UI
├── ViewModel/
│   └── CalculatorViewModel.cs # Calculator logic and state management
├── Services/
│   └── MathExpressionEvaluator.cs # Mathematical expression processor
├── Resources/
│   ├── Styles/
│   │   └── Colors.xaml       # Color definitions
│   │   └── Styles.xaml       # Control styles
│   ├── Fonts/                # Application fonts
│   └── Images/               # Application icons and images
└── MauiProgram.cs            # MAUI application entry point
```

## Learning Points

This project demonstrates several key concepts that are valuable for learning .NET 9 and MAUI development:

1. **XAML-based UI Design**
   - Declarative UI definition
   - Resource dictionaries and styles
   - Layout management

2. **MVVM Pattern**
   - Clean separation of UI and logic
   - Property binding
   - Command binding

3. **MAUI Capabilities**
   - Cross-platform UI components
   - Native control rendering
   - Responsive layout design

4. **C# Concepts**
   - Event handling
   - Property change notification
   - Mathematical expression parsing

## Customization

The calculator can be easily extended or customized:

- Add new scientific functions
- Implement different visual themes
- Add history tracking
- Implement unit conversion features
- Add graphing capabilities

## Contributing

Contributions are welcome! If you'd like to contribute to this project:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- This project was created as a learning example for .NET 9 and MAUI
- Inspired by modern calculator apps with a focus on clean UI design
- Special thanks to the .NET MAUI team for making cross-platform development easier

---

*This project is for educational purposes to demonstrate .NET 9 and MAUI capabilities.*
