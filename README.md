# ReseptenAPI
gemaakt door Anton Meun s1191515

## User manual/ReadMe
Gebruikers handleiding voor ReseptenAPI

Beste gebruikers, Hierbij een handleiding hoe je de ReseptenAPI applicatie kan gebruiken.

Allereerst ga je naar Github, selecteer het project en klik op de groene knop "Code".
Daaruit haal je de "HTTPS" link uit. Kopieer de link. Groene knop

Open Visual Studio 2022, of een andere IDE waarbij je C# projecten kan openen.
Klik op "Clone a repository" en vul de gekopieerde link in het vakje onder "Repository Location".
Daarna kies je waar je de repository in op wilt slaan en klik je op "Clone".

Klik daarna op "Tools" en zoek de Nuget Package Manager Console.
Open de console en update de database. Hiervoor typ je "update-database" in de package manager console. Druk op enter op de database te laten draaien. de database is een localdb met de naam "ReceptenS1191515". Er is ceeding data maar mocht dit fout gaan is het mogelijk om weer de Migrations map weg te gooien en het opniew te laten genareren met het comando "add-migration [name]"

Als dat is gelukt, klik je op "Build" of "Run". Hierdoor wordt de applicatie geopend en kun je deze gebruiken.
Als de applicatie wordt gestart in ontwikkelmodus, wordt Swagger automatisch geopend.
In Swagger zijn de te gebruiken endpoints terug te vinden.
