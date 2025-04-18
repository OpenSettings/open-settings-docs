# <img src="logo/open-settings-logo.png" alt="Header" width="24"/> OpenSettings Docs

[![Docs](https://img.shields.io/badge/docs-online-blue)](https://docs.opensettings.net)
[![GitHub](https://img.shields.io/badge/•-open--settings-blue?logo=github)](https://github.com/OpenSettings/open-settings)

Welcome to the official documentation repository for **OpenSettings**!

OpenSettings is a **centralized settings management** system designed for **.NET applications**. It simplifies app settings management by providing a structured and scalable approach to handling settings.

![Demo](https://github.com/OpenSettings/open-settings-docs/blob/master/docs/v1/assets/gifs/demo.gif)

## 📖 Documentation

The latest OpenSettings documentation is available at [**docs.opensettings.net**](https://docs.opensettings.net)

## 📂 Repository Structure

This repository contains all documentation files in **Markdown (`.md`) format**, and makes use of [DocFX](https://dotnet.github.io/docfx/) for generating static documentation.

## 🚀 Getting Started

To contribute or run the documentation locally, follow these steps:

### 1️⃣ Create Directory and Clone the Repository
```sh
mkdir OpenSettings
git clone https://github.com/OpenSettings/open-settings-docs.git
cd OpenSettings/open-settings-docs/docs
```

### 2️⃣ Install DocFX  
If you don’t have **DocFX** installed, run:
```sh
dotnet tool install -g docfx
```

> ⚠️ **Warning:** PDF generation may require **Node.js**.  
You can download it from: [https://nodejs.org/en/download](https://nodejs.org/en/download)

If you don’t need PDF support, you can comment out or remove the following line from `./build.ps1`:

```ps1
docfx pdf .\v1\docfx.json
```

### 3️⃣ Build and Serve Documentation Locally
```sh
./build.ps1
```

Open your browser and navigate to **[http://localhost:8080](http://localhost:8080)** to preview the documentation! 🚀

### 4️⃣ (Optional) Generate API Reference Files

To include API documentation:

1. Clone the main OpenSettings repo into the same root folder:

   ```sh
   git clone https://github.com/OpenSettings/open-settings.git
   ```

2. Open `build.ps1` in a text editor and uncomment the following line by removing the `#`:

   ```ps1
   docfx metadata .\v1\docfx.json
   ```

3. Run the build script again:

   ```sh
   ./build.ps1
   ```

### 5️⃣ (Optional) Serve the Docs via .NET App

When you run the `open-settings-docs/docs/build.ps1` script, it automatically generates the documentation and copies the `open-settings-docs/docs/_site` folder into:

```
open-settings-docs/src/OpenSettings.Docs/wwwroot/
```

To host it via a .NET application:

1. Make sure you have the .NET SDK installed.
2. Navigate to the app directory:

   ```sh
   open-settings-docs/src/OpenSettings.Docs
   ```

3. Run the app:

   ```sh
   dotnet run
   ```

This will serve the generated documentation using the built-in .NET Core web server.

```bash
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7186
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5184
```

## 💡 License  

Licensed under the [OpenSettings License](https://opensettings.net/license).

## 🤝 Contributing

By contributing this repository, you agree to the [Contribution Terms](https://opensettings.net/contribution-terms).

## 🐞 Issues & Reports

If you encounter any issues or have suggestions, please report them via our GitHub repository.

### How to Report an Issue:
1. **Search for Existing Issues**: Check if your issue has already been reported in the [Issues section](https://github.com/OpenSettings/open-settings-docs/issues).
2. **Submit a New Issue**: If not, create a new issue by clicking **"New issue"** on the [Issues page](https://github.com/OpenSettings/open-settings-docs/issues), describing the problem, and including relevant details like steps to reproduce, error messages, and logs.

### Reporting Guidelines:
- Be specific about the issue, including environment and configuration details.
- Include relevant error logs or screenshots if available.

### Security Concerns:
For security-related issues, **do not** use GitHub Issues. Contact us directly at [security@opensettings.net](mailto:security@opensettings.net).

We appreciate your feedback and will address your concerns as soon as possible!

## 🔗 Useful Links

- 🌍 **Website:** [opensettings.net](https://opensettings.net)
- ❤️ **Become a Sponsor:** [opensettings.net/become-a-sponsor](https://opensettings.net/become-a-sponsor)
- 📜 **License:** [opensettings.net/license](https://opensettings.net/license)
- ⚖️ **Terms & Conditions:** [opensettings.net/terms-and-conditions](https://opensettings.net/terms-and-conditions)
- 🔒 **Privacy Policy:** [opensettings.net/privacy-policy](https://opensettings.net/privacy-policy)

<br>

✨ *OpenSettings makes settings management simple, powerful, and flexible!* 🚀
