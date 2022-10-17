Add-Migration -Name [DateTime]::Now.ToString("yyyyMMdd-HHmmss") -StartupProject Api -Project MaidContexts


dotnet ef migrations add ([DateTime]::Now.ToString("yyyyMMdd-HHmmss")) --startup-project .\Api\ --project .\TestDbContext