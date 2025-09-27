# 🛠 Troubleshooting

## 📌 Introduction  

Welcome to the **OpenSettings Troubleshooting Page**!  
This page is meant to help you diagnose and resolve common issues that may occur while setting up or upgrading OpenSettings.

---

## 🚨 Errors

### Error: Pending Model Changes Warning

> `An error occurred while accessing the Microsoft.Extensions.Hosting services. Continuing without the application service provider. Error: An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': The model for context 'OpenSettingsDbContext' has pending changes. Add a new migration before updating the database. See https://aka.ms/efcore-docs-pending-changes. This exception can be suppressed or logged by passing event ID 'RelationalEventId.PendingModelChangesWarning' to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.` 

#### 💡 Explanation

This error means you are upgrading versions sequentially, and some versions may not have been updated properly. When upgrading, OpenSettings will automatically attempt to update the database when it runs. If you encounter this error, it’s typically because some migrations haven't been applied yet.

#### ✅ Resolution

You can bypass this error by adding the following configuration to your OpenSettings setup to suppress the warning:

```csharp
openSettingsConfiguration.Provider.Orm.ConfigureDbContext = optsBuilder =>
{
    optsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
};
```

This configuration will ignore the `PendingModelChangesWarning` and allow the application to run without halting on this warning.

---

### Error: To change the IDENTITY property of a column, the column needs to be dropped and recreated

> `To change the IDENTITY property of a column, the column needs to be dropped and recreated.`

#### 💡 Explanation

During certain version upgrades we may introduce **breaking schema changes** (for example, switching primary keys from `INT IDENTITY` to `GUID`). SQL Server treats the `IDENTITY` attribute as a **creation-time property**, so it **cannot be altered in place**. When this happens, a new database (or at least a drop-and-recreate of the affected tables) is required.  
We call this out in our **upgrade guides**; for example, the **v1.2 → v2.0** upgrade required a fresh database because the schema changes could not be migrated safely with a simple EF Core migration.

#### ✅ Resolution

- **Recommended (clean recreate):**
  1. Remove `Data` folder in your project. Create a **new migration** for the new version, change your db name in the connection string.
  2. Run the v2 **initial migration** to create the schema:
```sh
dotnet ef migrations add OpenSettingsDbMigration_v2_0 -c OpenSettingsDbContext -o Data/Migrations/OpenSettings/OpenSettingsDb
 ```
  3. (Optional) **Seed** or **re-import** your data.

---

## ❓ Can't Find Your Error?

If the error you're encountering isn't listed here, don’t worry — we’ve got your back.

try running the EF command with the `-v` prefix for verbose logs:

```bash
dotnet ef migrations add OpenSettingsDbMigration_Troubleshooting -c OpenSettingsDbContext -o Data/Migrations/OpenSettings/OpenSettingsDb -v
```

Search on the web if there is already a solution out there. If not, please [open an issue on GitHub](https://github.com/OpenSettings/open-settings/issues/new/choose) with the following details:

- Exact error message and stack trace (if possible)
- Version of OpenSettings you're using
- Steps to reproduce
- Any relevant configuration code

This helps us troubleshoot quickly and potentially add the solution to this guide for others. 🙌

---

✨ *OpenSettings makes settings management simple and efficient!* 🚀