# Monolithic App
This will be the code for the first prototype.
Original by Oscar, Nicolas and Me

## Setup backend

1. Clone the repository:
    ```bash
    git clone https://github.com/your-repo/NXTBackend.git
    ```

2. Navigate to the project directory:
    ```bash
    cd NXTBackend
    ```

3. Restore the dependencies:
    ```bash
    dotnet restore
    ```

4. Build the project:
    ```bash
    dotnet build
    ```

5. Run the application:
    ```bash
    dotnet run --project "NXTBackend.API"
    ```

6. Open your browser and navigate to `http://localhost:3000` to access the API.

## Setup frontend

> **Note**: PLEASE, make sure that your backend app is running, the frontend will generate the
> necessary type definitons and therefor requires the backend.

```sh
# Yes, you can also use npm if you so desire.
cd frontend
pnpm install
pnpm dev -- --host
```

## Setup BLOB S3 Storage

In order to avoid polluting the git history with binary files, we use a BLOB storage to store the files.
For this we use Cloudflare R2, which is a S3 compatible storage service.

First you wanna download a CLI tool to interact with the storage. We recommend using the AWS CLI.

### Mac/Linux/Windows
```bash
brew install awscli
```

```bash
sudo apt install awscli
```

```bash
choco install awscli
```

- Run `aws configure` and enter the secrets provided to you by someone else.
- Open `.aws/config` in editor and set:
    ```
    endpoint_url = https://3d9912875c41651a332a963ea5e04c9f.eu.r2.cloudflarestorage.com
    ```
- Run `aws s3 ls s3://oolp-dev/` to list all files in the bucket.
- Run `aws s3 cp --recursive static/ s3://oolp-dev/` to upload all files in the `static` folder to the bucket.
