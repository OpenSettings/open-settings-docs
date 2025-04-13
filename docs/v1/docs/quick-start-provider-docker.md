# Setting Up The Provider With Docker 🐳

## 📥 Installation and Setup

Before, starting make sure you've installed docker and running on your machine. 

To verify Docker is installed and running properly open your terminal or command line and run:

```bash
docker info
```

This command should return details about the Docker engine. If Docker isn't running, you may see an error. Make sure the Docker Desktop or Docker service is active on your machine.

### 1️⃣ Cloning The Repo

Clone this repo and spin it up:
```bash
git clone https://github.com/OpenSettings/open-settings-samples.git
cd open-settings-samples/versions/v1/quick-starts/4-quick-start-provider-docker
docker-compose up -d
```

This will automatically run **OpenSettings in Provider Mode**.

🔗 Open the settings page in your browser:
**[http://localhost:5388/settings](http://localhost:5388/settings)**  

### 📂 Folder Structure

```
4-quick-start-provider-docker/
├── opensettings/              # (Optional) Configuration files, license key, etc.
├── .env                       # Secrets and environment variables (DB password, etc.)
└── docker-compose.yml         # Service definitions
```

### 🔐 .env File Details

The `.env` file contains secrets and environment variables:

- `ENVIRONMENT` → This is the ASP.NET Core environment (e.g., Development, Production)  
- `IMG_VERSION` → OpenSettings Docker image version  
  ↳ Other versions available at: https://hub.docker.com/r/opensettings/open-settings/tags  
- `INTERNAL_PORT` → Internal port exposed by the container  
- `EXTERNAL_PORT` → External port that maps to the host machine  
- `DB_PASSWORD` → Your selected database password  

### ⚙️ docker-compose.yml Configuration

The default selected `DbProvider` is **Sqlite**.  
If you want to use another database, comment out the Sqlite configuration and uncomment the desired provider section, then update the corresponding environment variables.

For more configuration options, check the [Docker guide YAML configuration section](docker-guide.md#-configuration).

### 🔑 License

The `opensettings/` folder also includes the `OpenSettings-License.key` file.  
By default, it's empty. If you have a license key, place it here.  
If not found or invalid, OpenSettings will fall back to the **Community** license mode.

---

## ✅ What's Next?

---

✨ *OpenSettings makes settings management simple and efficient!*