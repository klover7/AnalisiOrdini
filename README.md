# Analizzatore di ordini
Uno strumento che permette di analizzare i dati degli ordini da file CSV da riga di comando. Lo strumento elabora i record degli ordini e identifica le statistiche chiave, tra cui i totali più elevati, le quantità e gli importi degli sconti.

## Caratteristiche 
- Identifica l'ordine con l'importo totale più elevato (dopo lo sconto)
- Trova l'ordine con la quantità più elevata
- Individua l'ordine con il maggiore risparmio grazie allo sconto
- Convalida la coerenza dei dati inseriti
- Gestione degli errori con exit code esaustivi

## Requisiti 
- [.NET 6.0 SDK]
(https://dotnet.microsoft.com/download/dotnet/6.0) o superiore
- CsvHelper NuGet package (automatically restored)

## Installazione
  
1. Clona la repository:

  git clone https://github.com/klover7/AnalisiOrdini.git  
  cd AnalisiOrdini


2. Ripristina le dipendenze:
   
  dotnet restore


3. Compila il progetto:

  dotnet build


## Utilizzo

Esegui l'applicazione dalla riga di comando, fornendo il percorso del tuo file CSV come argomento:


dotnet run "path-to-csv-file"


### Esempi

#### File nella directory corrente
dotnet run orders.csv

#### File con un percorso relativo
dotnet run data/orders.csv

#### Percorso assoluto (Windows)
dotnet run "C:\Users\username\documents\orders.csv"

#### Percorso assoluto (Linux/Mac)
dotnet run "/home/username/documents/orders.csv"

## Formato di input

Il file CSV deve contenere le seguenti colonne (riga di intestazione obbligatoria):

| Column | Type | Description | 
|--------|------|-------------| 
| Id | Integer | Numero identificativo unico |
| Article Name | String | Nome dell'articolo |
| Quantity | Integer | Quantità acquistata |
| Unit price | Decimal | Prezzo per unità | 
| Percentage discount | Number | Percentuale di sconto (0-100) | 
| Buyer | String | Nome del cliente | 

### Esempio CSV

Id,Article Name,Quantity,Unit price,Percentage discount,Buyer  
1,Coke,10,1,0,Mario Rossi  
2,Coke,15,2,0,Luca Neri  
3,Fanta,5,3,2,Luca Neri  
4,Water,20,1,10,Mario Rossi  
5,Fanta,1,4,15,Andrea Bianchi  



## Output 

Lo strumento mostra tre statistiche principali:

--- Analisi Ordini ---  

Record con importo totale più alto:  
  ID: 2  
  Article: Coke  
  Buyer: Luca Neri  
  Total (with discount): $30.00  

Record con quantità più alta:  
  ID: 4  
  Article: Water  
  Buyer: Mario Rossi  
  Quantity: 20  

Record con maggior differenza tra totale senza sconto e totale con sconto:  
  ID: 4  
  Article: Water  
  Buyer: Mario Rossi  
  Discount: $2.00 (10%)  
  Total without discount: $20.00  
  Total with discount: $18.00  
  
### Spiegazione dell'output

1. **Record con importo totale più alto**: L'ordine con l'importo finale più elevato dopo l'applicazione degli sconti.  
2. **Record con quantità più alta**: L'ordine con il maggior numero di unità acquistate.  
3. **Record con maggior differenza tra totale senza sconto e totale con sconto**: L’ordine con la differenza più grande tra totale a prezzo pieno e totale scontato.  


## Exit Code

The application uses the following exit codes for proper error handling: 
| Code | Meaning |  
|------|---------|  
| 0 | Successo - analisi completata | 
| 1 | File non trovato o percorso CSV mancante | 
| 2 | Errore di analisi CSV (formato non valido) | 
| 3 | Errore imprevisto | 

## Validazione dei dati

Un ordine è considerato valido se:

- Article name non è vuoto
- Buyer non è vuoto
- Quantity è maggiore di 0
- Unit price non è negativo
- Percentage discount è compreso fra 0 e 100

I record non validi vengono ignorati in modo silenzioso.
Se tutti i record sono invalidi, viene mostrato un avviso. 

## Dettagli tecnici


### Calcoli

Il programma esegue i seguenti calcoli per ogni ordine:

Total Without Discount = Quantity × Unit Price  
Total With Discount = Total Without Discount × (1 - Percentage Discount / 100)  
Discount Amount = Total Without Discount - Total With Discount  


### Configurazione del parsing CSV

- Usa CultureInfo.InvariantCulture per la formattazione numerica coerente
- Confronta le intestazioni senza distinzione tra maiuscole/minuscole
- Ignora le righe con campi mancanti

## Struttura del progetto

AnalisiOrdini/  
├── Program.cs              # Logica principale dell’applicazione  
├── AnalisiOrdini.csproj    # Configurazione del progetto  
├── README.md               # Documentazione  
└── dati.csv                # File di esempio  


## Tecnologie utilizzate
- **Linguaggio**: C# 10
- **Framework**: .NET 6.0+
- **Librerie**:
  - [CsvHelper](https://joshclose.github.io/CsvHelper/) 30.0.1 - Parsing e mapping CSV
  - System.Linq - Analisi e query sui dati


## Gestione degli errori
Il programma gestisce diversi scenari di errore:
- **Percorso CSV mancante**: Mostra le istruzioni d’uso
- **File non trovato**: Messaggio d’errore con percorso specifico
- **Formato CSV non valido**: Segnalazione chiara con dettagli
- **File vuoto**: Avviso di nessun record valido
- **Dati non validi**: Record ignorati automaticamente
