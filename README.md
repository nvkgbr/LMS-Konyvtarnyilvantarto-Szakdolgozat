# LMS - Könyvtárnyilvántartó rendszer és záródolgozat
Készítette: Horváth Leticia, Novák Gábor

# Elindítási útmutató 

# Adatbázis:
Importálni kell a Database mappában lévő lms.sql fájlt. 
Ez magától létrehozza az lms nevű adatbázist, az összes szükséges táblával.

# Szerver:
Az SQL kapcsolat az appsettings.json-ban található, alapból localhoston, a root felhasználóval, 
jelszó nélkül próbálja elérni az lms nevű adatbázist.


A szerver buildeléséhez szükség van .NET SDK 6.0-ra. Letöltés: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
A 2019-es Visual Studio talán ezt már nem kezeli, így csak a 2022-vel lehet megnyitni.
Sikeres builedelés után a szervert a ```bin\debug\net6.0\Server.exe``` vel lehet futtatni.
A szerver a ```telepítő és futtatható fájlok``` mappában lévő szerver mappán belül található ```Server.exe``` fájllal szintén indítható.

A Reactban és a WPF  kliensben is az 5001-es port van 'beégetve', így más porton csak a kód módosításával futna.


# WPF:
A WPF kliens futtatható a Visual Studio 2022-ből vagy telepítés után az asztalra kihelyezett parancsikonnal.
Ehhez is szükség van a .NET 6.0-ra.
Példa felhasználó az alkalmazásba való bejelentkezéshez:  ```felhasználónév: a``` , ```jelszó: a```

# React:
('kiegészítő' rész a WPF-hez.)

A weboldal futtatásához ajánlott a Yarn-ra. Ez egyszerűen telepíthető az ```npm install --global yarn``` parancsal.
A repo cloneozása után, a ```Frontend_React``` mappán állva ki kell adni a ```yarn install``` parancsot, 
ez letölti a szükséges packageket. A packagek sikeres telepítése után a ```yarn start``` parancsal indítható a weboldal.

A ```yarn install``` és a ```yarn start``` parancsok kiválthatóak az ```npm install``` és ```napm start``` parancsokkal.

Ahhoz, hogy a weboldal összes funkciója használhatóvá váljon, ajánlott bejelentkezni a következő példa felhasználóval: ```email-cím: horvathl1@kkszki.hu``` , ```olvasójegy száma: 438129-001```
