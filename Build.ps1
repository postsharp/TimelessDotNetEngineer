(& dotnet nuget locals http-cache -c) | Out-Null
& dotnet run --project "$PSScriptRoot\eng\src\BuildTimelessDotNetEngineer.csproj" -- $args
exit $LASTEXITCODE

