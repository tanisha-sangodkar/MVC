-- AdminDetails Tbl
CREATE TABLE AdminDetails (
    AdminId INT PRIMARY KEY AUTO_INCREMENT, -- Adjust data type and auto-increment as per your DBMS
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DateOfBirth DATE,
    Age INT,
    EmailId VARCHAR(100),
    Password VARCHAR(100), -- Assuming encrypted password storage
    PhoneNumber VARCHAR(20),
    Gender VARCHAR(10),
    State VARCHAR(50),
    City VARCHAR(50),
    UserName VARCHAR(50),
    ImageData VARBINARY(MAX) -- Adjust size as per your DBMS
);

-- BookDetails
CREATE TABLE BookDetails (
    Id INT PRIMARY KEY AUTO_INCREMENT, -- Adjust data type and auto-increment as per your DBMS
    BookName VARCHAR(100) NOT NULL,
    Author VARCHAR(20) NOT NULL,
    Genre VARCHAR(20) NOT NULL,
    Description VARCHAR(200) NOT NULL,
    Price INT NOT NULL,
    Quantity INT NOT NULL,
    Pages INT NOT NULL,
    Language VARCHAR(200) NOT NULL,
    PublicationYear INT NOT NULL,
    BookImage VARCHAR(MAX), -- Assuming storing file path or reference to image
    TotalAmount INT NOT NULL
);

-- CartDetails
CREATE TABLE CartDetails (
    OrderId INT PRIMARY KEY AUTO_INCREMENT,
    CustomerId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    FOREIGN KEY (BookId) REFERENCES BookDetails(Id)
);

-- ContactUserDetails
CREATE TABLE ContactUserDetails (
    Id INT PRIMARY KEY AUTO_INCREMENT, -- Adjust data type and auto-increment as per your DBMS
    FirstName VARCHAR(20) NOT NULL,
    EmailId VARCHAR(100) NOT NULL,
    Suggestion TEXT NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL
);

-- CustomerDetails
CREATE TABLE CustomerDetails (
    Id INT PRIMARY KEY AUTO_INCREMENT, -- Adjust data type and auto-increment as per your DBMS
    FirstName VARCHAR(20) NOT NULL,
    LastName VARCHAR(20) NOT NULL,
    DateOfBirth DATE NOT NULL,
    EmailId VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    State VARCHAR(50) NOT NULL,
    City VARCHAR(50) NOT NULL,
    UserName VARCHAR(20) NOT NULL,
    ImageData VARCHAR(MAX) -- Assuming storing image data as file path or reference
);

-- 
