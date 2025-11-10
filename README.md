# CSV to MSSQL Loader

This application loads data from a CSV file into a single MSSQL database table. It was implemented as a test assessment for **DevelopsToday**.

## How to Run

1. Install MSSQL Server.  
2. Create a database and set its connection string in the `App.config` file.  
3. Execute the SQL script located at `DatabaseWrappers/TaxiOrdersTableWrapper.sql` in the database.  
4. Run the `TestAssessmentSavenko` project in .NET.

## Results

- A `duplicates.csv` file will be created in the `bin` directory.  
- Console output will display the number of rows loaded into the database.  
- The MSSQL table will be updated with the new items.

## Answer

9. Assume your program will be used on much larger data files. Describe in a few sentences what you would change if you knew it would be used for a 10GB CSV input file.

I would implement asynchronous parsing and a parallel(it would be better than async in this case) validation process. Additionally, I would include a loading animation to provide feedback to the user.
