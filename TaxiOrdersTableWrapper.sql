
--The goal of this task is to implement a simple ETL project in CLI that inserts data from a CSV into a SINGLE, FLAT TABLE.
--MSSQL

--3. Design a table schema that will hold the processed data; make sure you are using the proper data types.
IF OBJECT_ID('TaxiOrders', 'U') IS NOT NULL
BEGIN
    DROP TABLE TaxiOrders;
END
GO

CREATE TABLE TaxiOrders (
    tpep_pickup_datetime DATETIME NOT NULL,
    tpep_dropoff_datetime DATETIME NOT NULL,
    passenger_count INT NOT NULL,
    trip_distance FLOAT NULL,
    store_and_fwd_flag NVARCHAR(10) NULL,
    PULocationID INT NULL,
    DOLocationID INT NULL,
    fare_amount DECIMAL(10, 2) NOT NULL,
    tip_amount DECIMAL(10, 2) NOT NULL
);
--adde for task "Find the top 100 longest fares in terms of time spent traveling."
ALTER TABLE TaxiOrders
ADD TravelTimeMinutes AS DATEDIFF(MINUTE, tpep_pickup_datetime, tpep_dropoff_datetime);

--4. Users of the table will perform the following queries; ensure your schema is optimized for them:
-- Creating indexes for future queries by: 
-- Find out which `PULocationId` (Pick-up location ID) has the highest tip_amount on average.
CREATE NONCLUSTERED INDEX IDX_TaxiOrder_PULocation_Tip
ON TaxiOrders (PULocationID)
INCLUDE (tip_amount);

-- Find the top 100 longest fares in terms of `trip_distance`.
CREATE NONCLUSTERED INDEX IDX_TaxiOrder_TripDistance
ON TaxiOrders (trip_distance DESC);

-- Find the top 100 longest fares in terms of time spent traveling.
CREATE NONCLUSTERED INDEX IDX_TaxiOrder_TravelTime ON TaxiOrders(TravelTimeMinutes DESC);

-- Search, where part of the conditions is `PULocationId`.
CREATE NONCLUSTERED INDEX IDX_TaxiOrder_PULocation
ON TaxiOrders (PULocationID);