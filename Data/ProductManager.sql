CREATE DATABASE ProductManager
GO

USE ProductManager
				 
CREATE TABLE Product (
ID INT IDENTITY(1,1),
ProductName NVARCHAR(50) NOT NULL,
ItemNumber NVARCHAR(50) NOT NULL,
ProductDescription NVARCHAR(500),
ProductPrice DECIMAL(10,2)NOT NULL,
UNIQUE (ProductName),
PRIMARY KEY(ItemNumber))

SELECT * FROM Product

DELETE FROM Product

DROP TABLE Product