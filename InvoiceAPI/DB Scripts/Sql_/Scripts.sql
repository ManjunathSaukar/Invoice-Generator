CREATE TABLE Products
(
    ProductId      INT IDENTITY(1,1) PRIMARY KEY,

    ProductName    NVARCHAR(100) NOT NULL,

    Price          DECIMAL(18,2) NOT NULL,

    IsImported     BIT NOT NULL,

    IsActive       BIT NOT NULL DEFAULT 1,

    CreatedDate    DATETIME NOT NULL DEFAULT GETDATE(),

    ModifiedDate   DATETIME NULL
);


INSERT INTO Products
(
ProductName,
Price,
IsImported
)
VALUES
('Soap',30.50,0),
('Chips',22.50,0),
('Music CD',250.59,0),
('Perfume',2100.99,1),
('Crocin',19.75,0),
('Chocolate',450.25,1),
('Handbag',2200.59,1),
('Sunglasses',1250.00,1),
('Teddy Bear',250.59,0);


CREATE TABLE Invoices
(
    InvoiceId          INT IDENTITY(1,1) PRIMARY KEY,

    InvoiceNumber      NVARCHAR(30) NOT NULL UNIQUE,

    InvoiceDate        DATETIME NOT NULL,

    SubTotal           DECIMAL(18,2) NOT NULL,

    VatAmount          DECIMAL(18,2) NOT NULL,

    AdditionalTax      DECIMAL(18,2) NOT NULL,

    GrandTotal         DECIMAL(18,2) NOT NULL,

    CreatedDate        DATETIME NOT NULL DEFAULT GETDATE()
);



CREATE TABLE InvoiceItems
(
    InvoiceItemId      INT IDENTITY(1,1) PRIMARY KEY,

    InvoiceId          INT NOT NULL,

    ProductId          INT NOT NULL,

    Quantity           INT NOT NULL,

    UnitPrice          DECIMAL(18,2) NOT NULL,

    LineAmount         DECIMAL(18,2) NOT NULL,

    AdditionalTax      DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_InvoiceItems_Invoice
        FOREIGN KEY (InvoiceId)
        REFERENCES Invoices(InvoiceId),

    CONSTRAINT FK_InvoiceItems_Product
        FOREIGN KEY (ProductId)
        REFERENCES Products(ProductId)
);


CREATE INDEX IX_Products_ProductName
ON Products(ProductName);

CREATE INDEX IX_Invoices_InvoiceNumber
ON Invoices(InvoiceNumber);

CREATE INDEX IX_InvoiceItems_InvoiceId
ON InvoiceItems(InvoiceId);


CREATE PROCEDURE sp_GetProducts
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ProductId,
        ProductName,
        Price,
        IsImported
    FROM Products
    WHERE IsActive = 1
    ORDER BY ProductName;
END
GO


CREATE PROCEDURE sp_GetProductById
(
    @ProductId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ProductId,
        ProductName,
        Price,
        IsImported
    FROM Products
    WHERE ProductId = @ProductId
      AND IsActive = 1;
END
GO


CREATE PROCEDURE sp_InsertInvoice
(
    @InvoiceNumber NVARCHAR(30),
    @InvoiceDate DATETIME,
    @SubTotal DECIMAL(18,2),
    @VatAmount DECIMAL(18,2),
    @AdditionalTax DECIMAL(18,2),
    @GrandTotal DECIMAL(18,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Invoices
    (
        InvoiceNumber,
        InvoiceDate,
        SubTotal,
        VatAmount,
        AdditionalTax,
        GrandTotal
    )
    VALUES
    (
        @InvoiceNumber,
        @InvoiceDate,
        @SubTotal,
        @VatAmount,
        @AdditionalTax,
        @GrandTotal
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS InvoiceId;
END
GO

CREATE PROCEDURE sp_InsertInvoiceItem
(
    @InvoiceId INT,
    @ProductId INT,
    @Quantity INT,
    @UnitPrice DECIMAL(18,2),
    @LineAmount DECIMAL(18,2),
    @AdditionalTax DECIMAL(18,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO InvoiceItems
    (
        InvoiceId,
        ProductId,
        Quantity,
        UnitPrice,
        LineAmount,
        AdditionalTax
    )
    VALUES
    (
        @InvoiceId,
        @ProductId,
        @Quantity,
        @UnitPrice,
        @LineAmount,
        @AdditionalTax
    );
END
GO


CREATE PROCEDURE sp_GetInvoiceById
(
    @InvoiceId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        InvoiceId,
        InvoiceNumber,
        InvoiceDate,
        SubTotal,
        VatAmount,
        AdditionalTax,
        GrandTotal,
        CreatedDate
    FROM Invoices
    WHERE InvoiceId = @InvoiceId;
END
GO


CREATE PROCEDURE sp_GetInvoiceItemsByInvoiceId
(
    @InvoiceId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ii.InvoiceItemId,
        ii.InvoiceId,
        ii.ProductId,
        p.ProductName,
        ii.Quantity,
        ii.UnitPrice,
        ii.LineAmount,
        ii.AdditionalTax
    FROM InvoiceItems ii
    INNER JOIN Products p
        ON ii.ProductId = p.ProductId
    WHERE ii.InvoiceId = @InvoiceId
    ORDER BY ii.InvoiceItemId;
END
GO

ALTER TABLE Invoices
ALTER COLUMN InvoiceNumber NVARCHAR(30) NULL;
GO

CREATE PROCEDURE sp_InsertInvoice
(
    @InvoiceDate DATETIME,
    @SubTotal DECIMAL(18,2),
    @VatAmount DECIMAL(18,2),
    @AdditionalTax DECIMAL(18,2),
    @GrandTotal DECIMAL(18,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InvoiceId INT;
    DECLARE @InvoiceNumber NVARCHAR(30);

    -- Insert the invoice
    INSERT INTO Invoices
    (
        InvoiceDate,
        SubTotal,
        VatAmount,
        AdditionalTax,
        GrandTotal
    )
    VALUES
    (
        @InvoiceDate,
        @SubTotal,
        @VatAmount,
        @AdditionalTax,
        @GrandTotal
    );

    -- Get generated identity
    SET @InvoiceId = SCOPE_IDENTITY();

    -- Generate invoice number
    SET @InvoiceNumber = 'INV' + RIGHT('000000' + CAST(@InvoiceId AS VARCHAR(6)), 6);

    -- Update invoice number
    UPDATE Invoices
    SET InvoiceNumber = @InvoiceNumber
    WHERE InvoiceId = @InvoiceId;

    -- Return generated values
    SELECT
        @InvoiceId AS InvoiceId,
        @InvoiceNumber AS InvoiceNumber;
END
GO

ALTER TABLE InvoiceItems
ADD CONSTRAINT CK_InvoiceItems_Quantity
CHECK (Quantity > 0);

ALTER TABLE Products
ADD CONSTRAINT CK_Products_Price
CHECK (Price >= 0);

ALTER TABLE Invoices
ADD CONSTRAINT CK_Invoices_GrandTotal
CHECK (GrandTotal >= 0);


CREATE PROCEDURE sp_GetInvoiceByNumber
(
    @InvoiceNumber NVARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        InvoiceId,
        InvoiceNumber,
        InvoiceDate,
        SubTotal,
        VATAmount,
        AdditionalTax,
        GrandTotal
    FROM Invoices
    WHERE InvoiceNumber = @InvoiceNumber;
END
GO