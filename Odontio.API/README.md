# Getting Started
1. Install .NET 8 SDK
2. Run: `dotnet restore`
3. Run: `dotnet run --project .\Odontio.API\`

## Database Migrations

**Add Migrations**

`dotnet ef migrations add "InitMigration" --project .\Odontio.Infrastructure\ --startup-project .\Odontio.Api\ --output-dir Persistence\Migrations`

**Remove Migrations**

`dotnet ef migrations remove --project .\Odontio.Infrastructure\ --startup-project .\Odontio.Api\`

`dotnet ef database update "UpdateMenuMigration2" --project .\Odontio.Infrastructure\ --startup-project .\Odontio.Api\`

