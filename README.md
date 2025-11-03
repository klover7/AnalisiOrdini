# Order Analyzer
Uno strumento da riga di comando per analizzare i dati degli ordini da file CSV. Lo strumento elabora i record degli ordini e identifica le statistiche chiave, tra cui i totali più elevati, le quantità e gli importi degli sconti. 
## Caratteristiche - Identifica l'ordine con l'importo totale più elevato (dopo lo sconto) - Trova l'ordine con la quantità più elevata - Individua l'ordine con il maggiore risparmio grazie allo sconto - Convalida la coerenza dei dati inseriti - Gestione degli errori  con codici di uscita descrittivi 
## Requisiti - [.NET 6.0 SDK]

Tradotto con DeepL.com (versione gratuita)(https://dotnet.microsoft.com/download/dotnet/6.0) or higher - CsvHelper NuGet package (automatically restored) ## Installation 1. Clone this repository:
bash
git clone https://github.com/yourusername/order-analyzer.git
cd order-analyzer
2. Restore dependencies:
bash
dotnet restore
3. Build the project:
bash
dotnet build
## Usage Run the application from the command line, providing the path to your CSV file as an argument:
bash
dotnet run <path-to-csv-file>
### Examples
bash
# Using a file in the current directory
dotnet run orders.csv

# Using a file with a relative path
dotnet run data/orders.csv

# Using an absolute path (Windows)
dotnet run "C:\Users\username\documents\orders.csv"

# Using an absolute path (Linux/Mac)
dotnet run "/home/username/documents/orders.csv"
## Input Format The CSV file must contain the following columns (header row required): | Column | Type | Description | |--------|------|-------------| | Id | Integer | Unique order identifier | | Article Name | String | Product name | | Quantity | Integer | Number of items ordered | | Unit price | Decimal | Price per unit | | Percentage discount | Number | Discount percentage (0-100) | | Buyer | String | Customer name | ### Sample CSV
csv
Id,Article Name,Quantity,Unit price,Percentage discount,Buyer
1,Coke,10,1,0,Mario Rossi
2,Coke,15,2,0,Luca Neri
3,Fanta,5,3,2,Luca Neri
4,Water,20,1,10,Mario Rossi
5,Fanta,1,4,15,Andrea Bianchi
## Output The tool displays three key statistics as specified in the requirements:
=== ORDER ANALYSIS ===

Record with highest total amount:
  ID: 2
  Article: Coke
  Buyer: Luca Neri
  Total (with discount): $30.00

Record with highest quantity:
  ID: 4
  Article: Water
  Buyer: Mario Rossi
  Quantity: 20

Record with highest discount amount:
  ID: 4
  Article: Water
  Buyer: Mario Rossi
  Discount: $2.00 (10%)
  Total without discount: $20.00
  Total with discount: $18.00
### Output Explanation 1. **Highest Total Amount**: The order with the greatest final amount after applying discounts 2. **Highest Quantity**: The order with the most units purchased 3. **Highest Discount Amount**: The order with the largest absolute discount in currency (difference between total without discount and total with discount) ## Exit Codes The application uses the following exit codes for proper error handling: | Code | Meaning | |------|---------| | 0 | Success - analysis completed | | 1 | File not found or no CSV path provided | | 2 | CSV parsing error (invalid format) | | 3 | Unexpected error | ## Data Validation The tool automatically filters out invalid records. An order is considered valid if: - Article name is not empty - Buyer name is not empty - Quantity is greater than 0 - Unit price is non-negative - Percentage discount is between 0 and 100 Invalid records are silently skipped. If all records are invalid, a warning is displayed. ## Technical Details ### Calculations The tool performs the following calculations for each order:
Total Without Discount = Quantity × Unit Price
Total With Discount = Total Without Discount × (1 - Percentage Discount / 100)
Discount Amount = Total Without Discount - Total With Discount
### CSV Parsing Configuration - Uses CultureInfo.InvariantCulture for consistent number parsing - Trims whitespace from all fields - Case-insensitive header matching - Skips rows with missing required fields ## Project Structure
order-analyzer/
├── Program.cs              # Main application logic
├── OrderAnalyzer.csproj    # Project configuration
├── README.md               # This file
└── dati.csv               # Sample data file (example)
## Technologies Used - **Language**: C# 10 - **Framework**: .NET 6.0+ - **Libraries**: - [CsvHelper](https://joshclose.github.io/CsvHelper/) 30.0.1 - CSV parsing and mapping - System.Linq - Data analysis and queries ## Error Handling The application handles several error scenarios: - **Missing CSV path**: Displays usage information - **File not found**: Clear error message with file path - **Invalid CSV format**: Catches parsing errors with descriptive messages - **Empty file**: Warns when no valid records are found - **Invalid data**: Skips records that don't meet validation criteria ## Contributing Contributions are welcome! Please feel free to submit a Pull Request. ### Development Guidelines - Follow C# naming conventions - Add XML documentation for public methods - Maintain consistent code formatting - Test with various CSV formats and edge cases ## License This project is licensed under the MIT License - see the LICENSE file for details. ## Author [Your Name] [Your Email or GitHub Profile] ## Acknowledgments - Developed as a technical assessment for e-commerce data analysis - Uses the excellent [CsvHelper](https://joshclose.github.io/CsvHelper/) library by Josh Close - Follows .NET best practices for command-line applications
