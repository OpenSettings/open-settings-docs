# Docker Guide  

## 📌 Introduction  
This guide explains how to configure OpenSettings for Docker.  

OpenSettings runs as a provider configuration inside a Docker container. Configuration can be set using **environment variables** or by **mounting an `appsettings.json` file**.  

## 🚀 Quick Start  

Run OpenSettings with:  
```bash
docker run -d -p 5388:8080 --name container-open-settings opensettings/open-settings:latest
```  
Access it at: **`http://localhost:5388/settings`**  

## ⚙️ Configuration  

By default, the OpenSettings Docker setup uses **Sqlite** storage.  

### 📌 Environment Variables  

| Environment Variable                                             | Default Value                            | Description                                                                                |
|------------------------------------------------------------------|------------------------------------------|--------------------------------------------------------------------------------------------|
| `OPENSETTINGS_Configuration__DbProviderName`                     | `Sqlite`                                 | Available values: `MySql`, `Oracle`, `PostgreSql`, `Sqlite`, `SqlServer`, `InMemory`.      |
| `OPENSETTINGS_Configuration__ConnectionString`                   | `""`                                     | Database connection string.                                                                |
| `OPENSETTINGS_Configuration__LicenseKey`                         | `YOUR-LICENSE-KEY`                       | If not specified default license is **Community Edition**.                                 |
| `OPENSETTINGS_Configuration__InstanceName`                       | `provider-1`                             | Used to distinguish between different instances (e.g., `Instance-1`, `Instance-2`).        |
| `OPENSETTINGS_Configuration__IdentifierName`                     | `""`                                     | Used to distinguish between different environments (e.g., `Production`, `Development`).    |
| `OPENSETTINGS_Configuration__Client__Id`                         | `adbdf741-bb4d-4673-b2a8-23e677fcf454`   | Unique ID of the client.                                                                   |
| `OPENSETTINGS_Configuration__Client__Secret`                     | `4294a5e3-0839-4358-a03d-1ac52585ae5f`   | Secret of the client.                                                                      |
| `OPENSETTINGS_Configuration__Provider__Orm__EnablePooling`       | `true`                                   | Whether DbContext pooling is enabled.                                                      |
| `OPENSETTINGS_Configuration__Provider__Orm__PoolSize`            | `null`                                   | Pool size when DbContext pooling is enabled (default is `128`).                            |
| `OPENSETTINGS_Configuration__Provider__Orm__Redis__IsActive`     | `false`                                  | Enable Redis for instant updates.                                                          |
| `OPENSETTINGS_Configuration__Provider__Orm__Redis__Configuration`| `localhost:6379`                         | Redis configuration endpoint.                                                              |
| `OPENSETTINGS_Configuration__Provider__Orm__Redis__Channel`      | `Settings`                               | Redis channel name used for pub/sub.                                                       |
| `OPENSETTINGS_Configuration__Provider__CompressionType`          | `5`                                      | Compression type: 0 (None), 1 (Snappy), 2 (Deflate), 3 (Gzip), 4 (Zstd), 5 (Brotli).       |
| `OPENSETTINGS_Configuration__Provider__CompressionLevel`         | `0`                                      | Compression level: 0 (Optimal), 1 (Fastest), 2 (NoCompression).                            |
| `OPENSETTINGS_Configuration__Controller__Route`                  | `api/settings`                           | The base route for the service controller's endpoints.                            |
| `OPENSETTINGS_Configuration__Controller__AllowFromExploring`     | `false`                                  | Specifies whether the open settings controller's endpoints should be exposed in Api docs (e.g., for Swagger or Other Api Explorers.) |
| `OPENSETTINGS_Configuration__Controller__Authorize`              | `false`                                  | Indicates whether the controller requires authentication for access.                            |
| `OPENSETTINGS_Configuration__Controller__OAuth2__Authority`       | `null`                                   | The authority URL for the OAuth2 provider.                            |
| `OPENSETTINGS_Configuration__Controller__OAuth2__ClientId`        | `null`                                   | The client id used to authenticate with the OAuth2 provider.                            |
| `OPENSETTINGS_Configuration__Controller__OAuth2__ClientSecret`    | `null`                                   | The client secret used to authenticate with the OAuth2 provider.                            |
| `OPENSETTINGS_Configuration__Controller__OAuth2__SignedOutRedirectUri`| `settings`                           | The URI to redirect to after the user is signed out.                            |
| `OPENSETTINGS_Configuration__Controller__OAuth2__AllowOfflineAccess`  | `false`                              | Indicates whether offline access is allowed.                            |
| `OPENSETTINGS_Configuration__Controller__OAuth2__IsActive`        | `false`                                  | Indicates whether the OAuth2 configuration is active.                            |
| `OPENSETTINGS_Configuration__Spa__RoutePrefix`                   | `0`                                      | Specifies the prefix used to access the open settings Spa page.                            |
| `OPENSETTINGS_Configuration__Spa__DocumentTitle`                 | `0`                                      | The title of the document for the open settings page.                            |
| `OPENSETTINGS_Configuration__Spa__IsActive`                      | `true`                                   | Indicates whether the open settings Spa (Single Page Application) is active.                            |
| `OPENSETTINGS_Configuration__SyncAppDataMaxRetryCount`           | `-1`                                     | -1 = Infinite retries, 0 or any negative (other than -1) = No Retries.                     |
| `OPENSETTINGS_Configuration__SyncAppDataRetryDelayMilliseconds`  | `1000`                                   | Delay in milliseconds between retry attempts.                                              |
| `OPENSETTINGS_Configuration__Operation`                          | `1`                                      | Operation mode: 1 (ReadOrInitialize), 2 (Read).                                            |
| `OPENSETTINGS_Configuration__StoreInSeparateFile`                | `false`                                  | Whether to store settings in a separate file.                                              |
| `OPENSETTINGS_Configuration__IgnoreOnFileChange`                 | `false`                                  | Whether to ignore changes on the configuration file.                                       |
| `OPENSETTINGS_Configuration__RegistrationMode`                   | `3`                                      | Registration mode: 1 (Configure), 2 (Singleton), 3 (Both).                                 |

### 📌 YAML Configuration for a Docker setup Example

```yaml
services:
  open-settings:
    image: opensettings/open-settings:latest
    container_name: container-open-settings
    environment:
      - OPENSETTINGS_Configuration__DbProviderName=Sqlite
      - OPENSETTINGS_Configuration__ConnectionString=Data Source=OpenSettings.db
      - OPENSETTINGS_Configuration__LicenseKey=YOUR-LICENSE-KEY
      - OPENSETTINGS_Configuration__InstanceName=provider-1
      - OPENSETTINGS_Configuration__IdentifierName=Production
      - OPENSETTINGS_Configuration__Client__Id=adbdf741-bb4d-4673-b2a8-23e677fcf454
      - OPENSETTINGS_Configuration__Client__Secret=4294a5e3-0839-4358-a03d-1ac52585ae5f
      - OPENSETTINGS_Configuration__Provider__Orm__EnablePooling=true
      - OPENSETTINGS_Configuration__Provider__Orm__PoolSize=null
      - OPENSETTINGS_Configuration__Provider__Orm__Redis__IsActive=false
      - OPENSETTINGS_Configuration__Provider__Orm__Redis__Configuration=localhost:6379
      - OPENSETTINGS_Configuration__Provider__Orm__Redis__Channel=Settings
      - OPENSETTINGS_Configuration__Provider__CompressionType=5
      - OPENSETTINGS_Configuration__Provider__CompressionLevel=0
      - OPENSETTINGS_Configuration__SyncAppDataMaxRetryCount=-1
      - OPENSETTINGS_Configuration__SyncAppDataRetryDelayMilliseconds=1000
      - OPENSETTINGS_Configuration__Operation=1
      - OPENSETTINGS_Configuration__StoreInSeparateFile=false
      - OPENSETTINGS_Configuration__IgnoreOnFileChange=false
      - OPENSETTINGS_Configuration__RegistrationMode=3
    ports:
      - "5388:8080"
```

### 📌 Json Configuration Example  

```json
"Configuration": {
  "ConnectionString": "Data Source=OpenSettings.db",
  "DbProviderName": "Sqlite", // MySql | Oracle | PostgreSql | Sqlite (default) | SqlServer | InMemory 
  "InstanceName": "provider-1", // To distinguish between different instances, such as Instance-1, Instance-2, etc.
  "IdentifierName": "", // To distinguish between different environments, such as Production, Development, etc.
  "Client": {
    "Id": "adbdf741-bb4d-4673-b2a8-23e677fcf454", // Unique id of the client.
    "Secret": "4294a5e3-0839-4358-a03d-1ac52585ae5f" // Secret of the client.
  },
  "Selection": 1, // 1: Provider | 2: Consumer
  "Provider": {
    "Selection": 1, // 1 : Orm
    "Orm": {
      "EnablePooling": true,
      "PoolSize": null // null: default -> ( 128 )
    },
    "Redis": {
      "IsActive": false,
      "Configuration": "localhost:6379",
      "Channel": "Settings"
    },
    "CompressionType": 5, // 0 (None) - 1 (Snappy) - 2 (Deflate) - 3 (Gzip) - 4 (Zstd) - 5 (Brotli)
    "CompressionLevel": 0 // 0 (Optimal) - 1 (Fastest) - 2 (NoCompression)
  },
  "Controller": {
    "Route": "api/settings",
    "AllowFromExploring": false,
    "Authorize": false,
    "OAuth2": {
      "Authority": null,
      "ClientId": null,
      "ClientSecret": null,
      "SignedOutRedirectUri": "settings",
      "AllowOfflineAccess": false,
      "IsActive": false
    }
  },
  "Spa": {
    "RoutePrefix": "settings",
    "DocumentTitle": "OpenSettings Spa",
    "IsActive": true
  },
  "SyncAppDataMaxRetryCount": -1, // -1 = Infinite retries | 0 or any negative values other than -1 = No Retries
  "SyncAppDataRetryDelayMilliseconds": 1000, // Delay in milliseconds between retry attempts
  "Operation": 1, // 1 (ReadOrInitialize) - 2 (Read)
  "StoreInSeparateFile": false,
  "IgnoreOnFileChange": false,
  "RegistrationMode": 3 // 1 (Configure) - 2 (Singleton) - 3 (Both)
}
```

## 🔑 Adding License Key

If you have a license key and want to auto-setup via a file, follow these steps:

1. Create a file named `OpenSettings-License.key`.
2. Paste your license key into the file.
3. Place this file next to the executable DLL.

When running in a container, you can mount this file into the container using a Docker volume. For example, if you're using Docker Compose, add a volume mapping to ensure the license file is available in the container at the appropriate path:

```yaml
services:
  open-settings:
    image: opensettings/open-settings:latest
    container_name: container-open-settings
    volumes:
      - ./OpenSettings-License.key:/app/OpenSettings-License.key
```

If the file is not found or the license is invalid, OpenSettings will attempt to retrieve the license from the database. Please refer to the [License Setup Guide](license-setup-guide.md) for more details.


## 🗜️ Built-in Compression Support  

OpenSettings supports multiple compression algorithms (`Brotli`, `Zstd`, `Gzip`, `Snappy`, `Deflate`). If a client app requests data with a supported encoding, the provider will respond with compressed data accordingly. For configuring request encodings, please refer to the [Configuration Guide For Consumer](configuration-guide.md#-consumer-) for more details.

## 📥 Integrating OpenSettings With Your Apps

To integrate OpenSettings into your .NET app, check out the [Quick Start Consumer Guide](quick-start-consumer.md).  

---

## ✅ What's Next?

---

✨ *OpenSettings makes settings management simple and efficient!*