# AI.Facial.Emotion

**AI.Facial.Emotion** is a .NET library for analyzing facial attributes, including emotion, age, and gender. It seamlessly integrates with ASP.NET Web API, providing efficient and secure facial analysis with embedded
AI models. Ideal for chatbots, customer insights, security, and healthcare applications.

## Features

- Emotion Detection – Recognizes emotions such as happiness, sadness, anger, surprise, and more.
- Age Estimation – Predicts an estimated age range based on facial features.
- Gender Classification – Identifies whether the detected face is male or female.
- Optimized for .NET – Fully compatible with .NET 6/7/8/9.
- **Easy Integration** – Works seamlessly with ASP.NET Web APIs, allowing quick integration into existing projects.
- Supports multiple input formats: **URL, Base64, File Stream**

## Installation

You can install this library via NuGet Package Manager:

```bash
dotnet add package AI.Facial.Emotion
```

## Configuration

Ensure your .NET Web API project must be .NET 6 above.

```csharp
using AI.Facial.Emotion;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<OnnxEmotionAnalyzer>();
var app = builder.Build();
app.MapControllers();
app.Run();
```

## Usage

For example

1️⃣ Analyze emotion from an image URL

````csharp
using EmotionAnalyzerLib.Services;

var analyzer = new EmotionAnalyzer();
var result = await analyzer.AnalyzeEmotionFromUrlAsync("https://example.com/image.jpg");

Console.WriteLine($"Emotion: {result.ToString()}");
````

2️⃣ Analyze emotion from a Base64 image string

````csharp
var base64Image = "iVBORw0KGgoAAAANSUhEUgAA...";
var result = await analyzer.AnalyzeEmotionFromBase64Async(base64Image);

Console.WriteLine($"Emotion: {result.ToString()}");
````

3️⃣ Analyze emotion from a file stream

````csharp
using var fileStream = File.OpenRead("image.jpg");
var result = await analyzer.AnalyzeEmotionFromStreamAsync(fileStream);

Console.WriteLine($"Emotion: {result.ToString()}");
````

## Example Response

```json
{
  "emotion": "joy",
  "confidence": 0.98
}
```

## Contact

For any questions, feel free to contact me or create an issue in the repository.
