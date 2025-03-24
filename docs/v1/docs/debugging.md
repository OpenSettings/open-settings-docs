---
uid: debugging
title: Debugging
---

# Debugging Guide for OpenSettings Developers

## Getting Started

Folder structure:

```
├── OpenSettings/
│   └── open-settings
│   └── open-settings-spa
│   └── OpenSettings.Api
```

### 1. Create a Folder for OpenSettings

Start by creating the `OpenSettings` folder and navigating to it:

```bash
mkdir OpenSettings
cd OpenSettings
```

### 2. Clone the OpenSettings Repositories
Next, clone the necessary OpenSettings repositories:

```bash
git clone git@github.com:OpenSettings/open-settings.git
git clone git@github.com:OpenSettings/open-settings-spa.git
```

### 3. Install Required NPM Packages for OpenSettings SPA
Before proceeding, ensure that you have Node.js installed. The recommended approach is to use **NVM (Node Version Manager)**, which helps manage different Node.js versions. Follow the instructions at [NVM for Windows](https://github.com/coreybutler/nvm-windows/releases) to install it.

Once installed, check if NVM is running correctly by using the following command:

```bash
nvm --version
```

If NVM is running, it will show the current version. OpenSettings SPA uses **Node.js version 20.11.0**, so let's install and use this version:

```bash
nvm install 20.11.0
nvm use 20.11.0
```

### 4. Install Angular CLI
The SPA project uses **Angular CLI version 17.1.3**. If you don't have Angular CLI installed, use the following command to install the same version:

```bash
npm install -g @angular/cli@17.1.3
```

### 5. Install NPM Packages for OpenSettings Spa
Now, navigate to the `open-settings-spa` directory and install the required NPM packages:

```bash
cd open-settings-spa
npm install
```

### 6. Run the Development Server
Once the packages are installed, run the Angular development server with the following command:

```bash
ng serve --host 0.0.0.0 --port 4200
```

Navigate to `http://localhost:4200`. The application will automatically reload if you change any of the source files.

---

## Running OpenSettings

### 7. Install Required SDKs and IDE
To run OpenSettings, you need the appropriate **SDKs** and an **IDE** (such as Visual Studio, Visual Studio Code, or Rider). Install your desired IDE and the required SDKs by visiting [the .NET SDK download page](https://dotnet.microsoft.com/en-us/download).

OpenSettings supports the following target frameworks:

- netstandard2.0
- netstandard2.1
- netcoreapp3.1
- net5.0
- net6.0
- net7.0
- net8.0

If you need older versions, you can install them by expanding the out-of-support versions on the [SDK download page](https://dotnet.microsoft.com/en-us/download/dotnet).

### 8. Create a Web Application to Run OpenSettings
Since OpenSettings libraries are not executable files, you need to create a web application to use them. Run the following commands in the **OpenSettings** folder:

```bash
dotnet new web -o OpenSettings.Api
dotnet add OpenSettings.Api reference open-settings/src/OpenSettings open-settings/src/OpenSettings.AspNetCore open-settings/src/OpenSettings.AspNetCore.Spa
```

It's recommended to add all libraries as references to ensure that any changes are reflected in the app after building.

---

## Setting Up OpenSettings

### 9. Quick Start Guide
Check the [Setting Up The Provider](quick-start-provider.md) quick start to set up OpenSettings. Skip the part where you add the `OpenSettings.AspNetCore` package, as we'll use it as a reference.

### 10. Viewing the Settings Page
When you run **OpenSettings.Api** and navigate to your URL (`.../settings`), you will see the settings page. This page is served from the `OpenSettings.AspNetCore.Spa/open-settings-spa-dist` folder.

---

## Handling Spa Builds and Running Separately

### 11. Enabling BuildSpa Target
To reflect changes in the **open-settings-spa** (Angular) folder, you need to enable the **BuildSpa** target in the **OpenSettings.AspNetCore.Spa** project.

Right-click the **OpenSettings.AspNetCore.Spa** project and edit the `.csproj` file. Un-comment the following line to enable the build process:

```xml
<!--<Exec Command="build-spa-$(Configuration).sh" />-->
```

```xml
<Exec Command="build-spa-$(Configuration).sh" />
```

This will ensure that each build copies the updated SPA files to the `open-settings-spa-dist` folder.

### 12. Running Angular Dev Server Separately
If you're not making changes to the embedded components, you can run the Angular development server separately. Start the server with:

```bash
ng serve --host 0.0.0.0 --port 4200
```

When testing the development server, you may need to update the `index.html` file. In the `<script>` section, there is a try-catch loop that tries to parse variables into appropriate types. Since it is running on a dev server, it will throw an exception. The **SettingsSpaMiddleware** usually replaces the content based on the information it receives, but since this isn't possible in the dev server, you will need to manually set the values in the catch scope.

Make sure to update the route to your dev server's listening port (OpenSettings.Api's port), which you can find in `launchSettings.json`.

In the file `open-settings-spa/src/index.html`, update the controllerOptions & oAuth2 as shown below:

```javascript
controllerOptions = {
  route: 'http://localhost:5288/api/settings',
  ...
};
...
```

```javascript
providerInfo = {
  ...
  oAuth2: {
    authority: 'http://localhost:5288',
    ...
  }
};
```

---

✨ *OpenSettings makes settings management simple and efficient!* 🚀