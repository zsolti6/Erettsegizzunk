# 🛰️ Backend API (ASP.NET Core)

A `Backend` branch egy ASP.NET Core Web API projektet tartalmaz, amely a frontend alkalmazás és az adatbázis közötti kapcsolatot biztosítja RESTful API-k segítségével.

## 🔧 Technológiák

- ASP.NET Core
- Entity Framework Core
- C#
- Nuget packagek

## 🌐 Elérhetőség

A bebuildelt backend amit a frontend használ, elérhető ezen az URL-en keresztül: [https://erettsegizzunk.onrender.com](https://erettsegizzunk.onrender.com)

## ⚙️ Funkciók

- Teljes CRUD műveletek az adatbázison keresztül
- REST API-k a frontend által használva
- Adatmodellek kezelése EF Core-on keresztül

## 🔌 Futtatás

1. **Repo klónozása és branch beállítása**
   
   ```bash
   git clone  https://github.com/zsolti6/Erettsegizzunk.git
   cd Erettsegizzunk
   git checkout Backend
   ```
   
2. Projekt futtatása

   ```bash
   dotnet run
   ```
Az API elérhető lesz a https://localhost:7066 vagy http://localhost:5000 címen.

## 🗃️ Adatbázis

A `backend` branch tartalmazza az adatbázis konfigurációját és sémáját is, amely az Entity Framework Core segítségével van kezelve. Az adatbázis inicializálása és kezelése kódból történik, így az adatbázis migrációk könnyedén verziókövethetők és szinkronban tarthatók a kódbázissal.

Az adatbázis sémát az `DbContext` osztály tartalmazza, amely az adatbázis tábláit és azok kapcsolatait definiálja. Az adatbázis migrációk kezelése a `dotnet ef migrations` parancsokkal történik.

A projekt első indításakor a szükséges adatbázis-migrációk automatikusan alkalmazásra kerülnek, és a táblák létrejönnek. Az adatbázis konfigurációja a `appsettings.json` fájlban található, ahol a kapcsolat string beállítható.

Kérdésed van? Nyugodtan nyiss egy [Issue-t](https://github.com/zsolti6/Erettsegizzunk/issues)!
