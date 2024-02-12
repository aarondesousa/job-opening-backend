# job-opening-backend
ASP.NET Core Web API for job opening management.

## Prerequisites

Before running the API, make sure you have the following installed on your system:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [PostgreSQL database](https://www.postgresql.org/download)

## Setup

1. **App Settings**:
- Create an `appsettings.json` file in the root directory of the project.
- Copy and paste the following configuration into the `appsettings.json` file:

    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Host=localhost;Port=5432;Database=job_opening_db;Username=postgres;Password=password;"
        },
        "Jwt": {
            "Key": "Your256BitKeyAbcdefghijklmnopqrs",
            "Issuer": "YourAuthenticationServer",
            "Audience": "YourServiceClient",
            "Subject": "YourServiceAccessToken"
        },
        "AllowedHosts": "*"
    }
    ```
- Replace the `DefaultConnection` string in the `appsettings.json` file with your PostgreSQL database settings. Modify the `Host`, `Port`, `Database`, `Username`, and `Password` values according to your PostgreSQL configuration.
- Replace the values in the `Jwt` section of the `appsettings.json` file with your actual JWT settings. Modify the `Key`, `Issuer`, `Audience`, and `Subject` according to your JWT configuration.

## Run the Application

1. **Run the Application**:
- Open your terminal and navigate to the project directory.
- Execute the following command to run the API:
    ```bash
    dotnet run
    ```

2. **Ensure ID Sequence is in Sync**:
- After running the application for the first time, execute the following SQL command to ensure the Jobs Id sequence is in sync:
    ```sql
    DO $$
    BEGIN
        EXECUTE 'SELECT setval(''"Jobs_Id_seq"'', COALESCE((SELECT MAX("Id") FROM "Jobs"), 0))';
    END $$;
    ```