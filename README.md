# 🧪 Tesztelés (Frontend tesztelés Python script segítségével)

A `Testing` branch egy Python szkriptet tartalmaz, amely automatikusan végrehajt 5 funkcionális tesztet a frontend alkalmazáson. A tesztek segítségével ellenőrizhetjük a webalkalmazás alapvető funkcionalitását és hibamentességét.

## 🔧 Használt technológia

- **Python**: A teszteléshez használt nyelv.
- **Selenium**: A frontend alkalmazás automatizált tesztelésére használt könyvtár.
- **ChromeDriver**: A Selenium Chrome böngészőhöz való integrálásához szükséges illesztőprogram.

## ⚙️ Tesztelés előkészítése

1. **Repository klónozása**
   Klónozd a repository-t és válts a `Testing` branch-re:
   ```bash
   git clone https://github.com/zsolti6/Erettsegizzunk.git
   cd Erettsegizzunk
   git checkout Testing
   ```

2. Környezeti beállítások Telepítsd a szükséges Python csomagokat:
   ```bash
    pip install selenium
    pip install pytest
   ```

3. 🧪 Teszt futtatása
   A teszteléshez használd a következő parancsot:
   ```bash
    cd path/to/your/file
    pytest test.py
   ```
A teszt során az alkalmazás a konfigurált böngészőben elindul, és az 5 alapvető funkcionális tesztet lefuttatja.
