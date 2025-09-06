## generacion de proyecto con login

dotnet new mvc --auth Individual

dotnet build

dotnet watch run

## control de versiones

git config user.name "XXXXX"

git config user.email "xxxx@mail.com"

## customizacion del gestor de identidades (usuario y password)

dotnet aspnet-codegenerator identity -dc apptrade.Data.ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout;Account.ForgotPassword;Account.ResetPassword"

OPCIONAL

dotnet tool install --global dotnet-aspnet-codegenerator --version 9.0.0
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 9.0.0

## EF migrations

dotnet ef migrations add MiPrimeraMigra --context apptrade.Data.ApplicationDbContext -o "C:\opt\code\USMP2\apptrade\Data\Migrations"

dotnet ef database update

OPCIONAL solo en caso que no tengas el EF

dotnet tool install --global dotnet-ef

dotnet ef migrations add ContactoMigracion --context apptrade.Data.ApplicationDbContext -o "C:\opt\code\USMP2\apptrade\Data\Migrations"
