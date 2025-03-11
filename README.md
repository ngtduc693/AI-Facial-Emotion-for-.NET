# AI.Facial.Emotion

[![NuGet](https://img.shields.io/nuget/v/AI.Facial.Emotion.svg)](https://www.nuget.org/packages/AI.Facial.Emotion/)

**AI.Facial.Emotion** is a .NET library for analyzing facial attributes, including emotion. It seamlessly integrates with C#.NET, providing efficient and secure facial analysis with embedded
AI models. Ideal for chatbots, customer insights, security, and healthcare applications.

![Illustration](https://raw.githubusercontent.com/ngtduc693/AI-Facial-Emotion-for-.NET/refs/heads/main/imgs/hapiness.png)

## 🚀 Demo [My demo website is hosted on Azure (FREE SKU) here](https://facial-emotion.azurewebsites.net/)

![Azure demo](https://raw.githubusercontent.com/ngtduc693/AI-Facial-Emotion-for-.NET/refs/heads/main/imgs/azure-demo.png)

## 🚀 Features

- Emotion Detection – Recognizes emotions such as happiness, sadness, anger, surprise, and more.
- Optimized for .NET – Fully compatible with .NET 6/7/8/9.
- **Easy Integration** – Works seamlessly with ASP.NET Web APIs, Windows Application, allowing quick integration into existing projects.
- Supports multiple input formats: **URL, Base64, File Stream**

## 📦 Installation

You can install this library via NuGet Package Manager:

```bash
dotnet add package AI.Facial.Emotion
```

## 📦 Mandatory ingredients

- If your server runs windows operating system

```bash
dotnet add package Emgu.CV.runtime.windows
```

- If your server runs ubuntu operating system

```bash
dotnet add package Emgu.CV.runtime.ubuntu-x64
```


## ⚡ Usage

For example

1️⃣ Analyze emotion from an image URL

````csharp
using AI.Facial.Emotion;

var analyzer = new EmotionAnalyzer();
var result = await analyzer.AnalyzeEmotionFromUrlAsync("https://example.com/image.jpg");

Console.WriteLine($"Emotion: {result.Emotion}");
````

2️⃣ Analyze emotion from a Base64 image string

````csharp
var base64Image = "iVBORw0KGgoAAAANSUhEUgAA...";
var result = await analyzer.AnalyzeEmotionFromBase64Async(base64Image);

Console.WriteLine($"Emotion: {result.Emotion}");
````

3️⃣ Analyze emotion from a file stream

````csharp
using var fileStream = File.OpenRead("image.jpg");
var result = await analyzer.AnalyzeEmotionFromStreamAsync(fileStream);

Console.WriteLine($"Emotion: {result.Emotion}");
````

## Example Response

```json
{
  "emotion": "sadness"
}
```

## Contact

For any questions, feel free to contact me or create an issue in the repository.
