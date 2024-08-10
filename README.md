# Monolithic App
This will be the code for the first prototype.
Original by Oscar, Nicolas and Me

## Setup backend

Run the Docker compose file to setup the backend.
It's important that you pull and build the docker image from the keycloak repo.

Afterwards you can just `dotnet restore` and `dotnet run` inside the `NXTBackend.App` directory.

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

### FAQ

Q: The frontend. It's pretty ... slow?
A: Give it a bit, let vite optimize the deps and everything. Maybe open in a new tab?

Q: I get 403 when trying to sign in, something with malformed token?
A: [It failed!](#it-failed)

Q: I git pulled and it's all broken >-<
A: `npx prisma db push` usually fixes this.

## It failed!

1. Is your database up?
2. Are your credentials correct?
3. Did you make sure you put the `.env` in the right place?
4. Did you ACTUALLY follow all the steps?
5. ARE THE SECRETS CORRECT???
