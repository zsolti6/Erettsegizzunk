🪄 Backend API (ASP.NET Core)

Ez a backend branch egy ASP.NET Core Web API projektet tartalmaz, amely a frontend alkalmazás és az adatbázis közötti kapcsolatot biztosítja RESTful API-k segítségével.

🔧 Technológiák

ASP.NET Core

Entity Framework Core

C#

SQL Server / SQLite (attól függően, mit használtok)

⚙️ Funkciók

Teljes CRUD műveletek az adatbázison keresztül

REST API-k a frontend által használva

Adatmodellek kezelése EF Core-on keresztül

🚪 Futtatás

Repo klónozása és branch beállítása

git clone https://github.com/felhasznalonev/repo-nev.git
cd repo-nev
git checkout backend

Konfiguráció beállítása

Állítsd be a kapcsolatot az adatbázissal a appsettings.json vagy appsettings.Development.json fájlban.

Adatbázis migráció futtatása

dotnet ef database update

Projekt futtatása

dotnet run

Az API elérhető lesz a https://localhost:5001 vagy http://localhost:5000 címen.

A backend dokumentációjához vagy teszteléshez használhatsz Swagger UI-t, ha be van kapcsolva a projektben.

Ha több információt szeretnél, vagy hibát találsz, nyiss egy Issue-t!
