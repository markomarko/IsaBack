# IsaBack
Projekat je kreiran iz dva dela, prvi se odnosi na backend aplikaciju koja je razvijana u .net coru 2.2. Drugi deo je angular frontend aplikacija. 
Pri prvom startovanju backenda
u koliko migracije nisu povucene sa githuba potrebno je u package manager konzoli inicijalizovati migraciju i update-ovati bazu (komande: add-migration NAZIV, update-database).
Svi ostali NuGet package ce biti skinuti i importovani pri prvom pokretanju (automapper, automapper di, sql server..)
Ako je baza prazna pokrece se seeder koji postavlja inicijalnog administratora klinickog centra ( email markogajicgaja@gmail.com pass Gaja1234!) koji posle moze dodati se ostale entitete koji su potrebni za funcionisanje sistema. Takodje u .rar fajlu data-sql postoje inicijalni podaci za bazu koji se mogu koristit samo ih je prethodno potrebno importovati.
Sto se tice frontend dela aplikacije, potrebno je skinuti node-package-module (komanda npm install). Njegovo raspakivanje pokrice sve importe koji su korisceni za nas projekat. 
Komandom ng serve pokrece se aplikacija po defaultu na portu 4200. (Prilikom pokretanja mogu se javiti dve greske u AddClinic.ts fileu. Potrebno je otvoriti taj file i pritisnuti ctrl+c, greske ce nestati i projekat ce moci da se pokrene)
