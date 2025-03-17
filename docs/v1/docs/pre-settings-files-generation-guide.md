# Pre-settings files Generation

## 📌 Introduction 

In some cases, you might want to prepare default settings for the initial setup or for different environments. In this guide, we'll learn how to do that.

## Creating a Settings Class

Let's start by creating settings derived from `ISettings`:

```csharp
namespace OpenSettings.Api;

public class MyFirstSettings : ISettings
{
    public string Name { get; set; }

    public string Description { get; set; }
}
```

## Defining Default Values

To provide default values, you can use a constructor or field initialization. However, this approach might not be suitable when deploying the application for different environments (such as Production (EU) or Production (US)), where the default values may vary. Since the settings are new and not yet created for a new environment, you might need to create them manually via the settings page or generate pre-settings files that automatically supply the appropriate data.

## Creating Pre-settings Files

Pre-settings files allow you to define default values for settings before the application starts. These files should follow one of the following naming formats:

- **General settings file:** `settings.json`
- **Class-specific settings file:** `settings.{ClassName}.json`
- **Environment-specific settings file:** `settings-{EnvironmentName}.json`
- **Environment + class-specific settings file:** `settings-{EnvironmentName}.{ClassName}.json`

### Example: General Pre-settings File

To define default values for `MyFirstSettings`, create a file named `settings.MyFirstSettings.json` and populate it with the following values:

```json
{
    "OpenSettings.Api.MyFirstSettings": {
        "Name": "OpenSettings",
        "Description": "A centralized settings management system for .NET applications."
    }
}
```

### Example: Environment-Specific Pre-settings File

If you need different values for different environments, create a separate file for each environment. For example, to define values for the `prod-eu` environment, create a file named `settings-prod-eu.MyFirstSettings.json`:

```json
{
    "OpenSettings.Api.MyFirstSettings": {
        "Name": "OpenSettings Prod EU"
    }
}
```

This file will override only the specified properties while keeping other values from the general settings file. When the application runs, your final settings will be the following:

```json
{
    "OpenSettings.Api.MyFirstSettings": {
        "Name": "OpenSettings Prod EU",
        "Description": "A centralized settings management system for .NET applications."
    }
}
```

## Managing Multiple Settings in One File

If you prefer, you can store all settings in a single file. For example:

- **General settings file:** `settings.json`
- **Environment-specific settings file:** `settings-prod-eu.json`

This approach allows you to group settings together while still maintaining environment-based overrides.

## How the Application Detects the Environment

When the application runs, it determines the environment name in the following order:

1. It first checks the `ASPNETCORE_ENVIRONMENT` environment variable.
2. If not found, it checks the `DOTNET_ENVIRONMENT` environment variable.
3. If still not found, it looks for the `ENVIRONMENT` environment variable.
4. If none of these variables are set, the environment defaults to **Production**.

---

## ✅ What's Next?

---

✨ *OpenSettings makes settings management simple and efficient!* 🚀