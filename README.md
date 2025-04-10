## 🛰️ Backend API (ASP.NET Core)

Ez a `backend` branch egy ASP.NET Core Web API projektet tartalmaz, amely a frontend alkalmazás és az adatbázis közötti kapcsolatot biztosítja RESTful API-k segítségével.

## 🔧 Technológiák

- ASP.NET Core
- Entity Framework Core
- C#
- SQL Server / SQLite *(attól függően, mit használtok)*

## ⚙️ Funkciók

- Teljes CRUD műveletek az adatbázison keresztül
- REST API-k a frontend által használva
- Adatmodellek kezelése EF Core-on keresztül

## 🔌 Futtatás

1. **Repo klónozása és branch beállítása**
   
   ```bash
   git clone https://github.com/felhasznalonev/repo-nev.git
   cd repo-nev
   git checkout backend
   ```
2. Konfiguráció beállítása

   Állítsd be a kapcsolatot az adatbázissal a appsettings.json vagy appsettings.Development.json fájlban.

3. Adatbázis migráció futtatása
 
   ```bash
   dotnet ef database update
   ```
5. Projekt futtatása

   ```bash
   dotnet run
   ```
Az API elérhető lesz a https://localhost:7066 vagy http://localhost:5000 címen.
