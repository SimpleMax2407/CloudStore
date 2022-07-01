
dotnet ef --project CS_DAL migrations add --context ApplicationDbContext Name2
dotnet ef --project CS_DAL database update --context ApplicationDbContext
dotnet ef --project CS_DAL migrations add --context FileDataDbContext Name2
dotnet ef --project CS_DAL database update --context FileDataDbContext