# AI.Facial.Emotion

[![NuGet](https://img.shields.io/nuget/v/AI.Facial.Emotion.svg)](https://www.nuget.org/packages/AI.Facial.Emotion/)
[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/ngtduc693/AI-Facial-Emotion-for-.NET)

**AI.Facial.Emotion** is a .NET library for analyzing facial attributes, including emotion. It seamlessly integrates with C#.NET, providing efficient and secure facial analysis with embedded
AI models. Ideal for chatbots, customer insights, security, and healthcare applications.

![Illustration](https://raw.githubusercontent.com/ngtduc693/AI-Facial-Emotion-for-.NET/refs/heads/main/imgs/hapiness.png)

## 🚀 Demo: [My demo website is hosted on Azure (FREE SKU) here](http://ai-facial.azurewebsites.net/Emotion)

![Azure demo](https://raw.githubusercontent.com/ngtduc693/AI-Facial-Emotion-for-.NET/refs/heads/main/imgs/azure-demo.png)

## 🚀 Features

- Emotion Detection – Recognizes emotions such as happiness, sadness, anger, surprise, and more.
- Optimized for .NET – Fully compatible with **.NET 7, .NET 8, .NET 9 and .NET 10**.
- **Easy Integration** – Works seamlessly with ASP.NET Web APIs, Windows Application, allowing quick integration into existing projects.
- Supports multiple input formats: **URL, Base64, File Stream**

## ⚡ Release note

````bash
25.5.16: Support from .NET 8 to .NET 10 (.NET 6 and .NET 7 is no longer supported)
25.3.26: Optimized for performance in compliance with the latest .NET 10 and C# 14.
25.3.16 Support for the newly released .NET 9 and .NET 10
25.3.11 Update the Readme
25.3.4.2231. Allow user can adjust the Target hardware like CPU, CUDA
25.3.4. Allow user can adjust the threshold, topK and nms
25.3.3. Improve the performance
````

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

Console.WriteLine($"Emotion: {result}");
````

2️⃣ Analyze emotion from a Base64 image string

````csharp
var base64Image = "iVBORw0KGgoAAAANSUhEUgAA...";
var result = await analyzer.AnalyzeEmotionFromBase64Async(base64Image);

Console.WriteLine($"Emotion: {result}");
````

3️⃣ Analyze emotion from a file stream

````csharp
using var fileStream = File.OpenRead("image.jpg");
var result = await analyzer.AnalyzeEmotionFromStreamAsync(fileStream);

Console.WriteLine($"Emotion: {result}");
````

## Example Response

```json
{
  "emotion": "sadness"
}
```

## 📦 System Architecture (Overall Architecture, Core Components) and Data Flow Documentation

For more information about this library, you can read the System Architecture (Overall Architecture, Core Components) and Data Flow [documentation here](https://deepwiki.com/ngtduc693/AI-Facial-Emotion-for-.NET)
