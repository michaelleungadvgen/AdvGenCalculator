# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

AdvGen Calculator is a .NET 9 MAUI application that demonstrates modern cross-platform development. It's a dual-mode calculator (basic and scientific) built with XAML UI and MVVM architecture pattern.

## Build and Development Commands

### Building the Project
```bash
# Build the solution
dotnet build AdvGenCalculator.sln

# Build for specific platform (Windows)
dotnet build AdvGenCalculator/AdvGenCalculator.csproj -f net9.0-windows10.0.19041.0

# Clean solution
dotnet clean AdvGenCalculator.sln
```

### Running the Application
```bash
# Run from Visual Studio: Press F5 or use the Run button
# Run from command line (Windows):
dotnet run --project AdvGenCalculator/AdvGenCalculator.csproj -f net9.0-windows10.0.19041.0
```

### Package/Publish
```bash
# Publish for Windows
dotnet publish AdvGenCalculator/AdvGenCalculator.csproj -f net9.0-windows10.0.19041.0 -c Release
```

## Architecture

### Clean Architecture Implementation
The calculator follows clean architecture principles with clear separation of concerns:

#### Core Layer (`AdvGenCalculator.Core`)
- **Models**: Data structures and result types
  - `CalculationResult` (`AdvGenCalculator.Core/Models/CalculationResult.cs`): Encapsulates calculation results with success/error states
  - `Token` (`AdvGenCalculator.Core/Models/Token.cs`): Represents mathematical tokens with proper precedence rules
- **Services**: Business logic interfaces and implementations
  - `IExpressionEvaluator` & `ShuntingYardExpressionEvaluator`: Main calculation engine using Shunting Yard algorithm
  - `IExpressionTokenizer` & `ExpressionTokenizer`: Tokenizes mathematical expressions

#### Presentation Layer (`AdvGenCalculator`)
- **ViewModels**: MVVM pattern implementation
  - `ICalculatorViewModel` & `ImprovedCalculatorViewModel` (`AdvGenCalculator/ViewModels/`): Handles UI state and user interactions
- **Views**: XAML-based UI
  - `MainPage.xaml`: Calculator interface with data binding
- **Converters**: UI value converters for display logic

### Key Architecture Improvements

#### 1. Proper Operator Precedence
- **Shunting Yard Algorithm**: Replaces the flawed DataTable.Compute approach
- **Correct Precedence Order**: 
  - Functions (sin, cos, etc.): 4
  - Exponentiation (^): 3 (right-associative)
  - Multiplication/Division/Percentage (*, /, %): 2
  - Addition/Subtraction (+, -): 1
- **Right Associativity**: Exponentiation properly handles `2^3^2 = 2^(3^2) = 512`

#### 2. Enhanced Expression Handling
- **Proper Tokenization**: Mathematical expressions parsed into structured tokens
- **Parentheses Support**: Full nested parentheses support with auto-closing
- **Function Handling**: Scientific functions (sin, cos, tan, sqrt) properly integrated
- **Error Recovery**: Comprehensive error handling with meaningful messages

#### 3. Dependency Injection
- Services registered in `MauiProgram.cs` for testability and maintainability
- Interface-based design allows easy testing and future enhancements

### Expression Evaluation Flow
1. **Input Processing**: `ImprovedCalculatorViewModel.ProcessInput()` handles user input with proper state management
2. **Expression Building**: Incremental expression building with operator validation and conversion
3. **Tokenization**: `ExpressionTokenizer.Tokenize()` converts string to structured tokens
4. **Evaluation**: `ShuntingYardExpressionEvaluator.Evaluate()` processes tokens with correct precedence
5. **Result Display**: Formatted output with proper error handling

### Testing
- **Unit Tests**: Comprehensive test suite in `AdvGenCalculator.Core.Tests`
- **Test Coverage**: 33 tests covering precedence, associativity, functions, and error cases
- **Run Tests**: `dotnet test AdvGenCalculator.Core.Tests`

## Target Frameworks
- Primary: `net9.0-windows10.0.19041.0` (Windows 10/11)
- Cross-platform: Android, iOS, macOS (configured but Windows-focused)

## Dependencies
- Microsoft.Maui.Controls (MAUI framework)
- Microsoft.Extensions.Logging.Debug
- System.Data (for DataTable.Compute in expression evaluation)

## Development Notes
- Uses nullable reference types enabled
- ImplicitUsings enabled for cleaner code
- Scientific functions work in degrees (converted to radians internally)
- Auto-parentheses closing implemented for incomplete expressions
- Error handling displays error messages in the calculator display