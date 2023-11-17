# DatabaseApplication

Per lanciare il progetto via API basta fare lo start del progetto e viene lanciato Swagger. 

Il progetto TestApplication racchiude tutti i test in NUnit, possono essere lanciati da Text Explorer di VS. 

Il progetto è un take minimale sulla richiesta, è stata imbastita tutta l'architettura con unit test sulle parti presenti, ovviamente non è completo perchè 
non sarebbe stato possibile nell'ora e mezza richiesta, ma da una visione completa sulla comunicazione al suo interno che vede il layer di API comunicare con i singoli
servizi che poi comunicano tutti con il layer di accesso ai dati. Il database scelto per velocità di sviluppo è Sqlite. E' stata gestita la concorrenza in scrittura/lettura
per quanto possibile, ma non è la scelta più adeguata per un ambiente di produzione per i limiti conosciuti di Sqlite. Per una versione produzione virerei su un DB relazionale
più performante, come MariaDB o SqlServer, sarebbe poi solamente necessario cambiare la stringa di connessione di EF.


