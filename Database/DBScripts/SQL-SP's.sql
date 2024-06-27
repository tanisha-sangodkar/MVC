-- USPRegisterAdminDetails
CREATE PROCEDURE USPRegisterAdminDetails
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATE,
    @Age INT,
    @EmailId NVARCHAR(100),
    @Password NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Gender NVARCHAR(10),
    @State NVARCHAR(50),
    @City NVARCHAR(50),
    @UserName NVARCHAR(50),
    @ImageData VARBINARY(MAX) -- Adjust size/type as per your DBMS
AS
BEGIN
    INSERT INTO AdminDetails
    (
        FirstName, 
        LastName, 
        DateOfBirth, 
        Age, 
        EmailId, 
        Password, 
        PhoneNumber, 
        Gender, 
        State, 
        City, 
        UserName, 
        ImageData
    ) 
    VALUES 
    (
        @FirstName, 
        @LastName, 
        @DateOfBirth, 
        @Age, 
        @EmailId, 
        @Password, 
        @PhoneNumber, 
        @Gender, 
        @State, 
        @City, 
        @UserName, 
        @ImageData
    );
END;


-- USPAdminChangePassword
CREATE PROCEDURE USPAdminChangePassword
    @password NVARCHAR(100),
    @username NVARCHAR(50)
AS
BEGIN
    UPDATE AdminDetails
    SET Password = @password
    WHERE UserName = @username;
END;

--USPAddBookDetails
CREATE PROCEDURE USPAddBookDetails
    @BookName NVARCHAR(100),
    @Author NVARCHAR(20),
    @Genre NVARCHAR(20),
    @Description NVARCHAR(200),
    @Price INT,
    @Quantity INT,
    @Pages INT,
    @Language NVARCHAR(200),
    @PublicationYear INT,
    @BookImage NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO BookDetails
    (
        BookName,
        Author,
        Genre,
        Description,
        Price,
        Quantity,
        Pages,
        Language,
        PublicationYear,
        BookImage
    )
    VALUES
    (
        @BookName,
        @Author,
        @Genre,
        @Description,
        @Price,
        @Quantity,
        @Pages,
        @Language,
        @PublicationYear,
        @BookImage
    );
END;

-- USPDisplayBookDetails
CREATE PROCEDURE USPDisplayBookDetails
AS
BEGIN
    SELECT 
        Id,
        BookName,
        Author,
        Genre,
        Description,
        Price,
        Quantity,
        Pages,
        Language,
        PublicationYear,
        BookImage
    FROM 
        BookDetails;
END;

-- USPSearchBook
CREATE PROCEDURE USPSearchBook
    @Id INT
AS
BEGIN
    SELECT 
        Id,
        BookName,
        Author,
        Genre,
        Description,
        Price,
        Quantity,
        Pages,
        Language,
        PublicationYear,
        BookImage
    FROM 
        BookDetails
    WHERE 
        Id = @Id;
END;


-- USPEditBook
CREATE PROCEDURE USPEditBook
    @Id INT,
    @BookName NVARCHAR(100),
    @Author NVARCHAR(20),
    @Genre NVARCHAR(20),
    @Description NVARCHAR(200),
    @Price INT,
    @Quantity INT,
    @Pages INT,
    @Language NVARCHAR(200),
    @PublicationYear INT,
    @BookImage NVARCHAR(MAX) -- Adjust size/type as per your DBMS
AS
BEGIN
    UPDATE BookDetails
    SET 
        BookName = @BookName,
        Author = @Author,
        Genre = @Genre,
        Description = @Description,
        Price = @Price,
        Quantity = @Quantity,
        Pages = @Pages,
        Language = @Language,
        PublicationYear = @PublicationYear,
        BookImage = @BookImage
    WHERE 
        Id = @Id;
END;


-- DeleteBook
CREATE PROCEDURE DeleteBook
    @Id INT
AS
BEGIN
    DELETE FROM BookDetails
    WHERE Id = @Id;
END;

-- SearchBookName
CREATE PROCEDURE SearchBookName
    @bookname NVARCHAR(100)
AS
BEGIN
    SELECT 
        Id,
        BookName,
        Author,
        Genre,
        Description,
        Price,
        Quantity,
        Pages,
        Language,
        PublicationYear,
        BookImage
    FROM 
        BookDetails
    WHERE 
        BookName LIKE '%' + @bookname + '%';
END;


-- USPUpdateBookQuantity
CREATE PROCEDURE USPUpdateBookQuantity
    @Id INT,
    @Quantity INT
AS
BEGIN
    UPDATE BookDetails
    SET Quantity = @Quantity
    WHERE Id = @Id;
END;

-- USPAddCartDetails
CREATE PROCEDURE USPAddCartDetails
    @CustomerId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    INSERT INTO CartDetails (CustomerId, BookId, Quantity)
    VALUES (@CustomerId, @BookId, @Quantity);
END;


--USPBookExistInCart
CREATE PROCEDURE USPBookExistInCart
    @customer_id INT,
    @book_id INT
AS
BEGIN
    DECLARE @ExistCount INT;

    SELECT @ExistCount = COUNT(*)
    FROM CartDetails
    WHERE CustomerId = @customer_id
    AND BookId = @book_id;

    SELECT @ExistCount;
END;


-- USPUpdateCartBookQuantity
CREATE PROCEDURE USPUpdateCartBookQuantity
    @BookId INT,
    @CustomerId INT,
    @Quantity INT
AS
BEGIN
    UPDATE CartDetails
    SET Quantity = @Quantity
    WHERE CustomerId = @CustomerId
    AND BookId = @BookId;
END;

-- USPGetCustomerId
CREATE PROCEDURE USPGetCustomerId
    @username NVARCHAR(50)
AS
BEGIN
    SELECT Id, UserName
    FROM CustomerDetails
    WHERE UserName = @username;
END;

-- USPSelectCartDetailsOfUser
CREATE PROCEDURE USPSelectCartDetailsOfUser
    @CustomerId INT
AS
BEGIN
    SELECT Id, BookId, Quantity, CustomerId
    FROM CartDetails
    WHERE CustomerId = @CustomerId;
END;

-- USPSelectBookDetailsByBookId
CREATE PROCEDURE USPSelectBookDetailsByBookId
    @BookId INT
AS
BEGIN
    SELECT Id, BookName, Price, BookImage
    FROM BookDetails
    WHERE Id = @BookId;
END;

-- USPDeleteCart
CREATE PROCEDURE USPDeleteCart
    @bookid INT,
    @customerid INT
AS
BEGIN
    DELETE FROM CartDetails
    WHERE BookId = @bookid AND CustomerId = @customerid;
END;

--USPGetCartDetails
CREATE PROCEDURE USPGetCartDetails
    @bookid INT,
    @customerid INT
AS
BEGIN
    SELECT BookId, Quantity
    FROM CartDetails
    WHERE BookId = @bookid AND CustomerId = @customerid;
END;


-- USPSelectBookDetailsByBookId
CREATE PROCEDURE USPSelectBookDetailsByBookId
    @BookId INT
AS
BEGIN
    SELECT Id, BookName, Price, BookImage
    FROM BookDetails
    WHERE Id = @BookId;
END;


-- USPGetCustomerDetails
CREATE PROCEDURE USPGetCustomerDetails
    @id INT
AS
BEGIN
    SELECT 
        FirstName, 
        LastName, 
        DateOfBirth, 
        Age, 
        EmailId, 
        PhoneNumber, 
        Gender, 
        State, 
        City, 
        ImageData
    FROM CustomerDetails
    WHERE Id = @id;
END;


--USPAddOrderDetails
CREATE PROCEDURE USPAddOrderDetails
    @CustomerId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @BookId INT,
    @BookName NVARCHAR(100),
    @PhoneNumber NVARCHAR(15),
    @Quantity INT,
    @Price DECIMAL(18, 2),
    @TotalAmount DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO OrderDetails (CustomerId, FirstName, LastName, BookId, BookName, PhoneNumber, Quantity, Price, TotalAmount)
    VALUES (@CustomerId, @FirstName, @LastName, @BookId, @BookName, @PhoneNumber, @Quantity, @Price, @TotalAmount);
END;


-- USPDeleteCartByCustomerId
CREATE PROCEDURE USPDeleteCartByCustomerId
    @CustomerId INT
AS
BEGIN
    DELETE FROM CartDetails WHERE CustomerId = @CustomerId;
END;


-- SPGetQuantityByCustomerCart
CREATE PROCEDURE SPGetQuantityByCustomerCart
    @CustomerId INT,
    @BookId INT
AS
BEGIN
    SELECT Quantity
    FROM CartDetails
    WHERE CustomerId = @CustomerId AND BookId = @BookId;
END;


-- USPGetCartById
CREATE PROCEDURE USPGetCartById
    @Id INT
AS
BEGIN
    SELECT Id, BookId, Quantity, CustomerId
    FROM CartDetails
    WHERE Id = @Id;
END;


--USPUpdateQuantityByCartId
CREATE PROCEDURE USPUpdateQuantityByCartId
    @id INT,
    @quantity INT
AS
BEGIN
    UPDATE CartDetails
    SET Quantity = @quantity
    WHERE Id = @id;
END;


-- USPContactDetails
CREATE PROCEDURE USPContactDetails
    @FirstName NVARCHAR(20),
    @EmailId NVARCHAR(50),
    @Suggesstion NVARCHAR(MAX),
    @PhoneNumber NVARCHAR(15)
AS
BEGIN
    INSERT INTO ContactDetails (FirstName, EmailId, Suggesstion, PhoneNumber)
    VALUES (@FirstName, @EmailId, @Suggesstion, @PhoneNumber);
END;


-- USPContactusDetails
CREATE PROCEDURE USPContactusDetails
AS
BEGIN
    SELECT Id, FirstName, EmailId, Suggesstion, PhoneNumber
    FROM ContactDetails;
END;


-- USPDisplayCustomerDetails
CREATE PROCEDURE USPDisplayCustomerDetails
AS
BEGIN
    SELECT Id, FirstName, LastName, DateOfBirth, Age, EmailId, PhoneNumber, Gender, State, City, UserName, ImageData
    FROM CustomerDetails;
END;

--USPGetCustomerId
CREATE PROCEDURE USPGetCustomerId
    @username NVARCHAR(100)
AS
BEGIN
    SELECT Id, UserName
    FROM CustomerDetails
    WHERE UserName = @username;
END;


--USPGetEditCustomerDetail
CREATE PROCEDURE USPGetEditCustomerDetail
    @id INT
AS
BEGIN
    SELECT Id, FirstName, LastName, DateOfBirth, Age, EmailId, PhoneNumber, Gender, State, City, UserName, ImageData
    FROM CustomerDetails
    WHERE Id = @id;
END;


--
CREATE PROCEDURE USPGetEditCustomerDetail
    @id INT
AS
BEGIN
    SELECT Id, FirstName, LastName, DateOfBirth, Age, EmailId, PhoneNumber, Gender, State, City, UserName, ImageData
    FROM CustomerDetails
    WHERE Id = @id;
END;


--
CREATE PROCEDURE USPUpdateCustomerDetails
    @Id INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @DateOfBirth DATE,
    @Age INT,
    @EmailId NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Gender NVARCHAR(10),
    @State NVARCHAR(50),
    @City NVARCHAR(50),
    @UserName NVARCHAR(50),
    @ImageData NVARCHAR(MAX) -- Assuming storing image as base64 string
AS
BEGIN
    UPDATE CustomerDetails
    SET FirstName = @FirstName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        Age = @Age,
        EmailId = @EmailId,
        PhoneNumber = @PhoneNumber,
        Gender = @Gender,
        State = @State,
        City = @City,
        UserName = @UserName,
        ImageData = @ImageData
    WHERE Id = @Id;
END;


--
CREATE PROCEDURE USPDeleteCustomer
    @id INT
AS
BEGIN
    DELETE FROM CustomerDetails
    WHERE Id = @id;
END;


--
CREATE PROCEDURE USPUserChangePassword
    @password NVARCHAR(100),
    @username NVARCHAR(50)
AS
BEGIN
    UPDATE Users
    SET Password = @password
    WHERE UserName = @username;
END;


--
CREATE PROCEDURE USPDuplicateEmailUsernamePhoneNumber
    @EmailId NVARCHAR(100),
    @UserName NVARCHAR(50),
    @PhoneNumber NVARCHAR(20)
AS
BEGIN
    DECLARE @count INT;

    SELECT @count = COUNT(*)
    FROM CustomerDetails
    WHERE EmailId = @EmailId OR UserName = @UserName OR PhoneNumber = @PhoneNumber;

    SELECT @count AS DuplicateCount;
END;

--
CREATE PROCEDURE USPAdDuplicateEmailUsernamePhoneNumber
    @EmailId NVARCHAR(100),
    @UserName NVARCHAR(50),
    @PhoneNumber NVARCHAR(20)
AS
BEGIN
    DECLARE @count INT;

    SELECT @count = COUNT(*)
    FROM AdminDetails
    WHERE EmailId = @EmailId OR UserName = @UserName OR PhoneNumber = @PhoneNumber;

    SELECT @count AS DuplicateCount;
END;


--
CREATE PROCEDURE USPSearchLoginUserCount
    @UserName NVARCHAR(50)
AS
BEGIN
    DECLARE @count INT;

    SELECT @count = COUNT(*)
    FROM CustomerDetails
    WHERE UserName = @UserName;

    SELECT @count AS UserCount;
END;


-- 
CREATE PROCEDURE USPSearchLoginUser
    @UserName NVARCHAR(50)
AS
BEGIN
    SELECT UserName, EmailId, Password
    FROM CustomerDetails
    WHERE UserName = @UserName;
END;


-- 
CREATE PROCEDURE USPSearchLoginAdminCount
    @UserName NVARCHAR(50)
AS
BEGIN
    DECLARE @count INT;

    SELECT @count = COUNT(*)
    FROM AdminDetails
    WHERE UserName = @UserName;

    SELECT @count AS AdminCount;
END;


--
CREATE PROCEDURE USPSearchLoginAdmin
    @UserName NVARCHAR(50)
AS
BEGIN
    SELECT UserName, EmailId, Password
    FROM AdminDetails
    WHERE UserName = @UserName;
END;


--
CREATE PROCEDURE USPSearch
    @UserName NVARCHAR(50)
AS
BEGIN
    SELECT Id, FirstName, LastName, PhoneNumber
    FROM CustomerDetails
    WHERE UserName = @UserName;
END;


-- 
CREATE PROCEDURE USPAddOrderDetails
    @CustomerId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @BookId INT,
    @BookName NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Quantity INT,
    @Price INT,
    @TotalAmount INT
AS
BEGIN
    INSERT INTO OrderDetails (CustomerId, FirstName, LastName, BookId, BookName, PhoneNumber, Quantity, Price, TotalAmount)
    VALUES (@CustomerId, @FirstName, @LastName, @BookId, @BookName, @PhoneNumber, @Quantity, @Price, @TotalAmount);
END;


--
CREATE PROCEDURE USPFetchOrderDetails
    @CustomerId INT
AS
BEGIN
    SELECT id, CustomerId, FirstName, BookId, BookName, Quantity, Price, TotalAmount, OrderDate, DeliveryStatus
    FROM OrderDetails
    WHERE CustomerId = @CustomerId;
END;


--
CREATE PROCEDURE USPGetOrderDetails
AS
BEGIN
    SELECT Id, CustomerId, FirstName, BookId, BookName, Quantity, Price, TotalAmount, DeliveryStatus, OrderDate
    FROM OrderDetails;
END;

-- 
CREATE PROCEDURE USPGetUpdateOrderDelStatus
    @id INT,
    @deliverystatus NVARCHAR(50)
AS
BEGIN
    UPDATE OrderDetails
    SET DeliveryStatus = @deliverystatus
    WHERE Id = @id;
END;


--CHECK table NAME for THIS NOT SURE===============================================================
CREATE PROCEDURE USPRegisterUserDetails
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATE,
    @Age INT,
    @EmailId NVARCHAR(100),
    @Password NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Gender NVARCHAR(10),
    @State NVARCHAR(50),
    @City NVARCHAR(50),
    @UserName NVARCHAR(50),
    @ImageData NVARCHAR(MAX) -- Assuming it's a Base64-encoded string for image data
AS
BEGIN
    INSERT INTO Users (FirstName, LastName, DateOfBirth, Age, EmailId, Password, PhoneNumber, Gender, State, City, UserName, ImageData)
    VALUES (@FirstName, @LastName, @DateOfBirth, @Age, @EmailId, @Password, @PhoneNumber, @Gender, @State, @City, @UserName, @ImageData);
END;


-- --CHECK table NAME for THIS NOT SURE===============================================================
CREATE PROCEDURE USPSearchBook
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        BookName,
        Author,
        Genre,
        Description,
        Price,
        Pages,
        Language,
        PublicationYear,
        BookImage
    FROM 
        Books
    WHERE 
        Id = @Id;
END;


----CHECK table NAME for THIS NOT SURE===============================================================
CREATE PROCEDURE USPSelectBookQuantity
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Quantity
    FROM 
        Books
    WHERE 
        Id = @Id;
END;
