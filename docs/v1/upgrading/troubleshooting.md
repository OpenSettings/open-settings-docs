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

## ❓ Can't Find Your Error?

If the error you're encountering isn't listed here, don’t worry — we’ve got your back.

try running the EF command with the `-v` prefix for verbose logs:

```bash
dotnet ef migrations add OpenSettingsDbMigration_Troubleshooting -c OpenSettingsDbContext -o Data/Migrations/OpenSettings/OpenSettingsDb -v
```

Search on the web if there is already a solution out there. If not, please [open an issue on GitHub](https://github.com/OpenSettings/open-settings/issues/new) with the following details:

- Exact error message and stack trace (if possible)
- Version of OpenSettings you're using
- Steps to reproduce
- Any relevant configuration code

This helps us troubleshoot quickly and potentially add the solution to this guide for others. 🙌

---

✨ *OpenSettings makes settings management simple and efficient!* 🚀