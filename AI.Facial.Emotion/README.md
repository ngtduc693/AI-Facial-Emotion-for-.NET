# AI.Facial.Emotion

**AI.Facial.Emotion** is a .NET library for analyzing facial attributes, including emotion. It seamlessly integrates with C#.NET, providing efficient and secure facial analysis with embedded
AI models. Ideal for chatbots, customer insights, security, and healthcare applications.

![Illustration](https://raw.githubusercontent.com/ngtduc693/AI-Facial-Emotion-for-.NET/refs/heads/main/imgs/hapiness.png)

## 🚀 Demo: [My demo website is hosted on Azure (FREE SKU) here](http://ai-facial.azurewebsites.net/Emotion)

![Azure demo](https://raw.githubusercontent.com/ngtduc693/AI-Facial-Emotion-for-.NET/refs/heads/main/imgs/azure-demo.png)

## 🚀 Features

- Emotion Detection – Recognizes emotions such as happiness, sadness, anger, surprise, and more.
- Optimized for .NET – Fully compatible with **.NET 6, .NET 8, .NET 9 and .NET 10**.
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

## 📦 Contact

For any questions, feel free to contact me or create an [issue](https://github.com/ngtduc693/AI-Facial-Emotion-for-.NET) in the repository.

## ⚡ Release note

````bash
25.3.16 Support for the newly released .NET 9 and .NET 10
25.3.11 Update the Readme
25.3.4.2231. Allow user can adjust the Target hardware like CPU, CUDA
25.3.4. Allow user can adjust the threshold, topK and nms
25.3.3. Improve the performance
````