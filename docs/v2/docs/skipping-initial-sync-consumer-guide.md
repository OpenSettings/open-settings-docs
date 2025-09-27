# Skipping Initial Sync (Consumer)

## 📌 Introduction 

When the app is running as a **consumer**, it can choose to skip the initial sync of app data to speed up the process. This is especially useful to avoid overwhelming the provider with multiple requests during startup. Additionally, you might want to use the same settings throughout the application in case the provider becomes unavailable and cannot process or send settings. This technique ensures that the service starts reliably.

## Enabling Skipping of Initial Sync

To enable this behavior, configure your **Continuous Integration (C&I) and Continuous Deployment (C&D)** process, build your app, and set the configuration as follows:

```csharp
var openSettingsConfiguration = new OpenSettingsConfiguration(ServiceType.Consumer)
{
    ...
    Consumer = new ConsumerConfiguration
    {
        ...
        SkipInitialSyncAppData = true
        ...
    }
    ...
};
```

## Generating Settings Data

Next, run your application with the environment variable:

```
OPENSETTINGS_GENERATOR_MODE=1
```

This will instruct the app to:

1. Fetch data based on the specified configurations.
2. Generate `settings-generated` files that contain your settings.
3. Terminate the application with:
   - Exit code `0` if the file is successfully generated.
   - Exit code `1` if an error occurs.

After generating the settings, **pack and deploy your environment**, then run the application **without** specifying the `OPENSETTINGS_GENERATOR_MODE` environment variable.

## Handling Missing Configuration

While running the application, if it cannot find the `settings-generated.open-settings.json` file containing the properties `ProviderInfo` and `Configuration`, it will throw the following exception:

```
MissingConfigurationWhenSkipInitialSyncAppDataException
```

This ensures that the application does not start with incomplete or missing settings.

---

## ✅ What's Next?

---

✨ *OpenSettings makes settings management simple and efficient!* 🚀