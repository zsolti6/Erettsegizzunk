## 🛰️ Backend API (ASP.NET Core)

Ez a `backend` branch egy ASP.NET Core Web API projektet tartalmaz, amely a frontend alkalmazás és az adatbázis közötti kapcsolatot biztosítja RESTful API-k segítségével.

## 🔧 Technológiák

- ASP.NET Core
- Entity Framework Core
- C#
- Nuget packagek

## 🌐 Elérhetőség

A bebuldelt backend amit a frontend használ, elérhető ezen az URL-en keresztül: [https://erettsegizzunk.onrender.com](https://erettsegizzunk.onrender.com)

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
   
2. Projekt futtatása

   ```bash
   dotnet run
   ```
Az API elérhető lesz a (https://localhost:7066) vagy (http://localhost:5000) címen.

Kérdésed van? Nyugodtan nyiss egy [Issue-t](https://github.com/zsolti6/Erettsegizzunk/issues)!
