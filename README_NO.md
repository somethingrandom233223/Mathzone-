# MathZone – Windows spill-launcher

MathZone er en moderne, mørk Windows-app-prototype inspirert av spill-launchere som Epic Games Launcher. Prosjektet er skrevet i C# og WPF – du trenger **ikke Node.js** på PC-en.

## Dette følger med
- Moderne mørkt grensesnitt med lilla/blå aksent
- Hjem, butikkvisning, spillbibliotek, søk og nedlastingsside
- Eksempelkort for spill
- «Eget spill» lar deg velge en `.exe`-fil og starte den
- Inno Setup-script for å lage en vanlig Windows-installer med snarvei

## Viktig å vite
Dette er en fungerende startpakke/prototype, ikke en full spillbutikk ennå. Spillkort i butikkvisningen er demoer; MathZone laster ikke ned spill, omgår DRM eller erstatter kontoene til Steam/Epic. For å få en ekte butikk trengs lovlige nedlastingskilder, spillfiler, backend, sikkerhetskontroller og eventuelt konto-/betalingssystem.

## Bygg på Windows
Du trenger Windows og .NET 8 SDK på maskinen som bygger prosjektet. Dette er kun for å bygge appen; sluttbrukeren trenger ikke Node.js.

1. Installer **.NET 8 SDK** fra Microsoft.
2. Åpne PowerShell i prosjektmappen.
3. Kjør:

```powershell
dotnet publish .\MathZone.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o .\publish
```

4. Installer **Inno Setup** fra den offisielle nettsiden.
5. Åpne `installer\MathZone.iss` i Inno Setup og trykk **Compile**.
6. Installasjonsprogrammet blir laget i `installer\output\MathZone-Setup.exe`.

## Starte uten installer
Etter `dotnet publish` kan du starte `publish\MathZone.exe`.

## Neste steg
- Legge til faktiske installasjonsstier for flere spill
- Lagre biblioteket mellom omstarter
- Koble til ekte butikk-/nedlastingsserver
- Lage egen logo og appikon
- Signere installasjonsprogrammet før offentlig distribusjon
