# ReseptenAPI
gemaakt door Anton Meun s1191515

## User manual/ReadMe
Gebruikers handleiding voor ReseptenAPI

Beste gebruikers, hierbij een handleiding over hoe u de ReseptenAPI applicatie kan gebruiken.

Allereerst gaaat u naar Github, selecteer het project en klik op de groene knop "Code".
Daaruit haalt u de "HTTPS" URL. Kopieer de URL door de groene knop te gebruiken. 

Open de Visual Studio 2022, of een andere IDE waarbij u C# projecten kan openen.
Klik op "Clone a repository" en vul de gekopieerde URL in, in het vakje onder "Repository Location".
Daarna kiest u waar u de repository in op wilt slaan en klikt u op "Clone".

Klik daarna op "Tools" en zoek de Nuget Package Manager Console.
Open de console en update de database. Hiervoor typed u "update-database" in de package manager console. Druk op enter om de database te laten draaien. De database is een localdb met de naam "ReceptenS1191515". Er is ceeding data, maar mocht dit fout gaan is het mogelijk om weer de migrations map weg te gooien en het opniew te laten genereren met het comando "add-migration [name]"

Als dat is gelukt, klikt u op "Build" of "Run". Hierdoor wordt de applicatie geopend en kunt u deze gebruiken.
Als de applicatie wordt gestart in ontwikkelmodus, wordt Swagger automatisch geopend.
In Swagger zijn de te gebruiken endpoints terug te vinden.
