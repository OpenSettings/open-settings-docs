# Configuration Guide

## 📌 Introduction  
This guide explains how to configure OpenSettings.

---

## Looking Up OpenSettingsConfiguration

### Configuring the `Host.UseOpenSettingsAsync`

### InstanceDynamicId
- **Definition:** Assigns a random GUID at runtime unless a value is manually specified.
- **Use Case:** Used for monitoring the lifespan of the app instance. Can be useful for tracking app lifetime.

### InstanceName
- **Definition:** Case-insensitive name of the instance.
- **Use Case:** Helps in tracking instances by name, e.g., `instance-1`, `instance-2`. Default is `Default`.

### IdentifierName
- **Definition:** Case-insensitive name of the identifier used to distinguish environments (e.g., Production, Development).
- **Use Case:** Differentiate settings between environments.

---

### Client

| **Property**        | **Definition**                                                          | **Use Case**                                            |
|---------------------|-------------------------------------------------------------------------|---------------------------------------------------------|
| **Name**            | Name of the client. Must be unique. Default is entry assembly's name.   | Identifies the client, e.g., `OpenSettings.Api`.        |
| **Id**              | Unique GUID of the client. Each app must have its own ID.               | Unique identification for the app.                     |
| **Secret**          | Unique GUID secret for the client.                                      | Unique secret key for authentication.                  |
| **Version**         | Client version (numerical values only, no `v` prefix). Default is entry assembly's version. | Example: `1.0.0`.                                       |

---

### Selection (ServiceType)
- **Definition:** Specifies whether the service acts as a Provider or Consumer. Available values: `Provider`, `Consumer`.

#### Consumer Configuration

| **Property**              | **Definition**                                                     | **Use Case**                                              |
|---------------------------|--------------------------------------------------------------------|-----------------------------------------------------------|
| **ProviderUrl**           | The URL of the provider for fetching and syncing data.            | Used to connect to the provider API (e.g., `https://.../api/settings`). |
| **RequestEncodings**      | Desired encodings for the data. Provider decides whether to send them. | Controls the data encoding preferences.                   |
| **IsRedisActive**         | Flag to indicate whether Redis is active for pub/sub updates.     |                                                                                                                                                  |
| **SkipInitialSyncAppData**| Whether the initial sync of app data should be skipped.           | For more details, see the [Skipping Initial Sync Guide](skipping-initial-sync-consumer-guide.md). |
| **PollingSettingsWorker** | Configuration for polling settings worker.                        |                                                                                                                                                  |
| **- IsActive**            | Flag to indicate whether polling is active.                       |                                                                                                                                                  |
| **- StartsIn**            | Time span to wait before starting the first polling. Default is `TimeSpan.FromMinutes(5)`. |                                                                                                                                                  |
| **- Period**              | Time span between each polling interval. Default is `TimeSpan.FromMinutes(5)`. |                                                                                                                                                  |

#### Provider Configuration

| **Property**              | **Definition**                                                     | **Use Case**                                              |
|---------------------------|--------------------------------------------------------------------|-----------------------------------------------------------|
| **Selection (DataAccessType)** | Data access strategy. Currently, it can only be `Orm`.         | Defines the data access type.                            |
| **Orm**                    | ORM configuration.                                                 |                                                                                                                                                  |
| **- ConfigureDbContext**   | Callback to configure the DB context. Each provider must adjust the DB and settings used. |                                                                                                                                                  |
| **- EnablePooling**        | Whether DBContext pooling is enabled.                             |                                                                                                                                                  |
| **- PoolSize**             | Pool size when DBContext pooling is enabled.                       | Default pool size is used.                               |
| **- DbProviderName**       | Configured DBContext provider name (internally set, not meant to change). |                                                                                                                                                  |
| **Redis**                  | Redis configuration.                                               |                                                                                                                                                  |
| **- IsActive**             | Flag to indicate if Redis is activated.                            |                                                                                                                                                  |
| **- Configuration**        | Redis configuration string for connections.                       | See Redis [configuration options](https://stackexchange.github.io/StackExchange.Redis/Configuration.html#configuration-options). |
| **- Channel**              | The Redis channel used for communication. Default is `Settings`.   |                                                                                                                                                  |

---

### SyncAppDataMaxRetryCount
- **Definition:** Maximum number of retries for initial data sync. Default is `-1` for infinite retries.
- **Retry Behavior:**
  - `0` or any negative value other than `-1`: No retries (operation will fail immediately on failure).
  - `-1`: Infinite retries (operation will continue retrying until success).
  - Any positive integer: Retry up to the specified number of attempts.

---

### SyncAppDataRetryDelayMilliseconds
- **Definition:** Delay between retry attempts when sync fails. Default is `1000ms`.

---

### Operation
- **Definition:** The operation during setup. Default is `ReadOrInitialize`.
- **Available Values:**
  - `ReadOrInitialize`: Reads from the settings file if it exists, or initializes a new instance if it does not.
  - `Read`: Reads from the settings file, maps the data to the appropriate settings.

---

### StoreInSeparateFile
- **Definition:** Whether settings should be stored in a separate file (e.g., `settings-generated.*.json`). Default is `false`.

---

### IgnoreOnFileChange
- **Definition:** Whether changes to `settings-generated.json` should be ignored.
- **Use Case:** Controls the reloading behavior for the main settings file. If set to `false`, changes to the file will trigger a reload.

---

### RegistrationMode
- **Definition:** The registration mode during setup. Default is `Both`. Available values are `Singleton`, `Configure`, and `Both`.
  - `Singleton`: Settings derived from `ISettings` can be resolved as a singleton instance.
  - `Configure`: Settings can be resolved through configuration options interfaces like `IOptions<T>`, `IOptionsSnapshot<T>`, or `IOptionsMonitor<T>`.

---

### IsConsumerSelected
- **Definition:** Indicates whether the consumer service is selected (internally set).

---

### IsProviderSelected
- **Definition:** Indicates whether the provider service is selected (internally set).

---

### LoggerFactory
- **Definition:** An optional logger factory used for logging information during the settings building process.
- **Use Case:** Assigned later by resolving D&I `LoggerFactory`.

---

### LicenseKey
- **Definition:** The OpenSettings license key. For the community edition, it is not required. See the [License Setup Guide](license-setup-guide.md) for more information.

---

### CompressionType
- **Definition:** The compression type used when storing setting data. Default is `CompressionType.None`.
- **Available Values:**
  - `CompressionType.None`
  - `CompressionType.Gzip`
  - `CompressionType.Deflate`,
  - `CompressionType.Brotli`,
  - `CompressionType.Zstd`,
  - `CompressionType.Snappy`,

---

### CompressionLevel
- **Definition:** The compression level used when storing setting data. Default is `CompressionLevel.Fastest`.
- **Available Values:**
  - `CompressionLevel.Fastest`
  - `CompressionLevel.Optimal`
  - `CompressionLevel.NoCompression`

---

<p>🔹 <strong>Provider</strong></p>

```csharp
var openSettingsConfiguration = new OpenSettingsConfiguration(ServiceType.Provider)
{
    InstanceName = "provider-1",
    IdentifierName = "Production",
    Client = new ClientInfo(
        id: new Guid("adbdf741-bb4d-4673-b2a8-23e677fcf454"), 
        secret: new Guid("4294a5e3-0839-4358-a03d-1ac52585ae5f")),
    Provider = new ProviderConfiguration(DataAccessType.Orm)
    {
        Orm = new OrmConfiguration
        {
            EnablePooling = true,
            PoolSize = null,
            ConfigureDbContext = optsBuilder => optsBuilder.UseInMemoryDatabase("OpenSettings")
        },
        Redis = new RedisConfiguration
        {
            IsActive = false,
            Configuration = "localhost:6379",
            Channel = "Settings"
        },
        CompressionType = CompressionType.Brotli,
        CompressionLevel = CompressionLevel.Optimal,
    },
    SyncAppDataMaxRetryCount = -1,
    SyncAppDataRetryDelayMilliseconds = 1000,
    Operation = Operation.ReadOrInitialize,
    StoreInSeparateFile = false,
    IgnoreOnFileChange = false,
    RegistrationMode = RegistrationMode.Both
};

await builder.Host.UseOpenSettingsAsync(openSettingsConfiguration);
```

<p>🔹 <strong>Consumer</strong></p>

```csharp
var openSettingsConfiguration = new OpenSettingsConfiguration(ServiceType.Consumer)
{
    InstanceName = "consumer-1",
    IdentifierName = "Production",
    Client = new ClientInfo(
        id: new Guid("71059bda-bb49-447f-ac83-60cd15c9518d"), 
        secret: new Guid("6c52c9f7-d43c-44c1-8d6c-451bf9029731")),
    Consumer = new ConsumerConfiguration
    {
        ProviderUrl = "http://localhost:5288/api/settings",
        RequestEncodings = { CompressionType.Brotli },
        IsRedisActive = false,
        SkipInitialSyncAppData = false,
        PollingSettingsWorker = new PollingSettingsWorkerConfiguration(
            isActive: true, 
            startsIn: TimeSpan.FromMinutes(5), 
            period: TimeSpan.FromSeconds(5))
    },
    SyncAppDataMaxRetryCount = -1,
    SyncAppDataRetryDelayMilliseconds = 1000,
    Operation = Operation.ReadOrInitialize,
    StoreInSeparateFile = false,
    IgnoreOnFileChange = false,
    RegistrationMode = RegistrationMode.Both
};

await builder.Host.UseOpenSettingsAsync(openSettingsProviderConfiguration);
```

---

## Looking Up ControllerOptions

### Configuring the `AddOpenSettingsControllers`

| **Property**              | **Definition**                                                     | **Default**                                              |
|---------------------------|--------------------------------------------------------------------|----------------------------------------------------------|
| **Route**                 | Base route for the service controller’s endpoints.                | `api/settings`                                           |
| **AllowFromExploring**     | Whether the controller’s endpoints should be exposed in API documentation (e.g., Swagger). | `false`                                                  |
| **Authorize**              | Whether authentication is required. When `true`, authentication is enforced. | `false`                                                  |
| **OAuth2Options**          | OAuth2 configuration for authentication and authorization.         |                                                          |
| **- Authority**            | The OAuth2 provider’s authority URL.                               |                                                          |
| **- ClientId**             | The OAuth2 client ID.                                              |                                                          |
| **- ClientSecret**         | The OAuth2 client secret.                                          |                                                          |
| **- SignedOutRedirectUri** | URI to redirect to after sign-out. Default is `settings`.          | `settings`                                               |
| **- AllowOfflineAccess**   | Whether offline access is allowed.                                 |                                                          |
| **- IsActive**             | Whether the OAuth2 configuration is active.                        |                                                          |

```csharp
builder.Services
    .AddControllers()
    .AddOpenSettingsController(builder.Configuration, opts =>
    {
        opts.Route = "api/settings";
        opts.AllowFromExploring = true;
        opts.Authorize = true;
        opts.OAuth2Options = new OAuth2Options
        {
            Authority = "https://localhost:5001",
            ClientId = "web",
            ClientSecret = "secret",
            AllowOfflineAccess = true,
            IsActive = true
        };
    });
```

---

## Looking Up SettingsSpaOptions

### Configuring `app.UseOpenSettingsSpa()`

| **Property**    | **Definition**                                                                                                           | **Default**                                                  |
|------------------|---------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------|
| **RoutePrefix**  | Prefix used to access the OpenSettings page.                                                                               | `settings`                                                   |
| **IndexStream**  | Function that returns a `Stream` representing the index HTML file for the OpenSettings SPA.                                | Default embedded `index.html`.                              |
| **DocumentTitle**| Title of the settings page, used in the HTML `<title>` element and displayed in the browser's title bar.                   | `OpenSettings Spa`                                           |

```csharp
app.UseOpenSettingsSpa(opts =>
{
    opts.RoutePrefix = "settings";
    opts.IndexStream = () => typeof(SettingsSpaOptions).GetTypeInfo().Assembly
        .GetManifestResourceStream("OpenSettings.AspNetCore.Spa.open_settings_spa_dist.browser.index.html");
    opts.DocumentTitle = "OpenSettings Spa";
});
```

## Built-in Attributes

There are three built-in attributes that can be applied to settings classes:

- **ComputedIdentifierAttribute**
- **RegistrationModeAttribute**
- **StoreInSeparateFileAttribute**

### ComputedIdentifierAttribute

This attribute uniquely identifies a settings class. OpenSettings uses this identifier to determine which settings class it belongs to and how to match it.

```csharp
namespace OpenSettings.Api;

public class MyFirstSettings : ISettings
{
    public string Name { get; set; }
    public string Description { get; set; }
}
```

For the above class, the **computed identifier** follows the default format:

```csharp
{namespace}.{className}
```

So in this case, the computed identifier will be:

```
OpenSettings.Api.MyFirstSettings = new Guid("121eec96-36e6-a787-1d72-905311bd8493")
```

The identifier is computed using the following method:

```csharp
internal static Guid ComputeIdentifier(MD5 md5, string identifier)
{
    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(identifier));

    return new Guid(hash);
}
```

#### Handling Class Name Changes

If you rename the class from `MyFirstSettings` to something else, even if its properties remain unchanged, OpenSettings will treat it as a **new settings file** and will not match it with existing data.

To avoid this, explicitly specify the computed identifier using the `ComputedIdentifierAttribute`:

```csharp
using OpenSettings.Attributes;

namespace OpenSettings.Api;

// Alternatively, use the GUID directly:
// [ComputedIdentifier("121eec96-36e6-a787-1d72-905311bd8493")]
[ComputedIdentifier("OpenSettings.Api.MyFirstSettings")] 
public class SpecialSetting : ISettings
{
    public string Name { get; set; }

    public string Description { get; set; }
}
```

With this approach, settings synchronization will continue smoothly even after renaming the class.

### RegistrationModeAttribute

By default, `OpenSettingsConfiguration.RegistrationMode` determines how settings files are resolved. However, if you want to control the registration mode for a specific settings file, you can use the `RegistrationModeAttribute`.

> [!NOTE]
> This attribute only affects **new** settings files. If a settings file has already been created, applying this attribute will have no effect.  

```csharp
using OpenSettings.Attributes;

namespace OpenSettings.Api;

[RegistrationMode(RegistrationMode.Both)]
public class MyFirstSettings : ISettings
{
    public string Name { get; set; } = "Open";

    public string Description { get; set; }
}
```

#### Registration Mode Precedence

The **final registration mode** is determined in the following order:

1. **Provider Source Value** – If the provider specifies a registration mode, it overrides all other configurations.
2. **Attribute Value** – If no provider value is found, the value from `RegistrationModeAttribute` is used.
3. **Configuration Value** – If neither the provider nor the attribute specifies a mode, the configuration from the provider source is applied.
4. **Default Configuration Value** – If no other values are found, the default configuration is applied.

> [!CAUTION] 
> Be careful when changing the registration mode!  
> * Switching from `Both` to `Configure` can break your system. If your application resolves settings via **Singleton**, this could cause an error: `Unable to resolve service for type while attempting to activate.`  
> * Switching from `Both` to `Singleton` can break your system. If your application resolves settings via **IOptions interfaces**, this could cause an error: `Unable to resolve service for type while attempting to activate.`

---

### StoreInSeparateFileAttribute

This attribute allows settings to be stored in a **separate file**, such as:

```
settings-generated.MyFirstSettings.json
```

instead of being included in:

```
settings-generated.json
```

#### Why Use Separate Files?

- To **isolate** large settings for better management.
- To **avoid conflicts** when modifying settings dynamically.
- To **organize** settings into logical groups.

> [!NOTE]
> This attribute **only applies to new settings**. If the settings file already exists, marking it with `StoreInSeparateFileAttribute` **will not move it to a separate file**.

#### Storage Behavior

1. **If the settings are new and marked with `StoreInSeparateFileAttribute`**, they will be stored in:

   ```
   settings-generated.MyFirstSettings.json
   ```

2. **If the settings are new but not marked with the attribute**, they will be stored in a separate file **only if** `OpenSettingsConfiguration.StoreInSeparateFile` is set to `true`.
3. **If the settings already exist**, their storage behavior is determined via the **settings page** (`.../settings`).  

> [!NOTE]
> The general configuration (OpenSettingsConfiguration.StoreInSeparateFile) and attribute will not affect **existing** settings.

---

## ✅ What's Next?

---

✨ *OpenSettings makes settings management simple and efficient!* 🚀