# Data Storage & Migration Guide

## 📌 Introduction
This guide explains how to configure persistent data storage for OpenSettings and how to handle database migrations using Entity Framework Core (EF Core).

---

## 🛠 Configuring Database Provider
OpenSettings does not include built-in storage, so you must configure your own database provider.

Modify your `Program.cs` to set up the database provider:

```csharp
var migrationsAssembly = typeof(Program).Assembly.GetName().Name; // Assembly where the migrations are in

openSettingsProviderConfiguration.Provider.Orm.ConfigureDbContext = optsBuilder =>
{
    // Configure your database provider here. (e.g. UseSqlServer, UseSqlite, UseNpgsql, UseMySql, etc.)
    optsBuilder.UseSqlServer("YOUR CONNECTION STRING",
        opts => opts.MigrationsAssembly(migrationsAssembly));
};
```

### 🔹 Example Database Providers
Install the necessary package based on your chosen database:

| Database   | Package |
|------------|------------------------------------------------|
| SQL Server | `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` |
| PostgreSQL | `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL` |
| MySQL      | `dotnet add package Pomelo.EntityFrameworkCore.MySql` |
| SQLite     | `dotnet add package Microsoft.EntityFrameworkCore.Sqlite` |

Modify `optsBuilder.Use***()` accordingly for your database provider.

---

## 🔧 Database Migrations
### 1️⃣ **Install EF Core Tools**
Before creating migrations, install the EF Core CLI tools:

```sh
dotnet tool install --global dotnet-ef
```

Additionally, install the **EntityFrameworkCore.Design** package:

```sh
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### 2️⃣ **Generate Migration**
Run the following command inside your executable folder:

```sh
dotnet ef migrations add InitialOpenSettingsDbMigration -c OpenSettingsDbContext -o Data/Migrations/OpenSettings/OpenSettingsDb
```

> [!TIP]
> Make sure not to forget to pass the **--configuration Migration** parameter when generating migrations.

This command:
- Generates database schema changes based on your entity models.
- Stores migration files in `Data/Migrations/OpenSettings/OpenSettingsDb`.

### 3️⃣ **Apply Migrations to Database**
Once migrations are created, apply them to the database:

```sh
dotnet ef database update -c OpenSettingsDbContext
```

This command applies the migration scripts to create/update the database schema. When you run the application, migrations will be automatically applied. This step is optional but recommended.

---

## ✅ **What's Next?**

- **Explore the provider functionality:** [Configuration Guide](configuration-guide.md#provider-configuration)
- **Securing OpenSettings:** Learn more in the [Security Guide](security-guide.md)

---

✨ *OpenSettings makes settings management simple and efficient!* 🚀