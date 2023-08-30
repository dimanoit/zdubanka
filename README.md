## Working with migrations
1. Install dotnet entity framework tool
```sh
dotnet tool install --global dotnet-ef
```
2. In case if terminal don't see dotnet ef, run this in `cmd`
```sh
 export PATH="$PATH:$HOME/.dotnet/tools/"
```
2.1 
```sh
 cd Infrastructure
```

2.3 Run command
 ```sh
dotnet ef migrations add YourMigrationName --startup-project ../Api
 ```
2.3 To Update database after a successful migration
```sh
dotnet ef database update --startup-project ../Api
```