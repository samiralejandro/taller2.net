CREATE TABLE Products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Description TEXT,
    Stock INT NOT NULL DEFAULT 0
);



INSERT INTO Products (Name, price, Description, stock) VALUES ('Camiseta', 20.99, 'Camiseta de algodón', 100);
INSERT INTO Products (Name, price, stock) VALUES ('Pantalón', 39.99, 50);
INSERT INTO Products (Name, price, Description, stock) VALUES ('Zapatillas', 59.99, 'Zapatillas deportivas', 75);
