USE master;
GO

DROP DATABASE IF EXISTS ApenasNatasDB
CREATE DATABASE ApenasNatasDB;
GO

USE ApenasNatasDB;
GO

CREATE TABLE Users
(
    ID          int IDENTITY(1,1) PRIMARY KEY,
    firstName   nvarchar(255) NOT NULL,
    lastName    nvarchar(255) NOT NULL,
    email       nvarchar(255) NOT NULL,
    password    nvarchar(255) NOT NULL,
    isApproved  bit DEFAULT 0,
    isAdmin     bit DEFAULT 0
)

/* 0, Ivan, Abreu, ivan.rupt@gmail.com, 1, 1 */
/* https://www.md5hashgenerator.com/ 
* admin = 21232f297a57a5a743894a0e4a801fc3
*/
INSERT INTO Users (firstname, lastName, email, password, isApproved, isAdmin)
VALUES ('Ivan', 'Abreu', 'admin@admin.com', '21232f297a57a5a743894a0e4a801fc3', 1, 1);

/* https://www.md5hashgenerator.com/ 
* teste = 698dc19d489c4e4db73e28a713eab07b
*/
INSERT INTO Users (firstname, lastName, email, password, isApproved, isAdmin)
VALUES ('Dino', 'Sauro', 'nao@aprovado.com', '698dc19d489c4e4db73e28a713eab07b', 0, 0);


/* https://www.md5hashgenerator.com/ 
* teste = 698dc19d489c4e4db73e28a713eab07b
*/
INSERT INTO Users (firstname, lastName, email, password, isApproved, isAdmin)
VALUES ('Natalia', 'Leite', 'sim@aprovado.com', '698dc19d489c4e4db73e28a713eab07b', 1, 0);


SELECT * FROM Users;

--- Start of Dificulties 
-- TABLE
CREATE TABLE Difficulties 
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    difficult nvarchar(255)
)
-- DATA
-- Newborn, Easy, Medium, Hard, Chef
INSERT INTO Difficulties (difficult)
VALUES ('easy');

INSERT INTO Difficulties (difficult)
VALUES ('newborn');

INSERT INTO Difficulties (difficult)
VALUES ('medium');

INSERT INTO Difficulties (difficult)
VALUES ('hard');

INSERT INTO Difficulties (difficult)
VALUES ('chef');

SELECT * FROM Difficulties;
--- End of Dificulties

--- Start of Ingredients
CREATE TABLE Ingredients 
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    ingredient nvarchar(255)
)

-- DATA
/* 
ovo, leite, água, canela, açúcar, farinha de trigo, 
massa folhada, amido de milho, limão 
*/
INSERT INTO Ingredients (ingredient)
VALUES ('ovo');

INSERT INTO Ingredients (ingredient)
VALUES ('leite');

INSERT INTO Ingredients (ingredient)
VALUES ('água');

INSERT INTO Ingredients (ingredient)
VALUES ('canela');

INSERT INTO Ingredients (ingredient)
VALUES ('açúcar');

INSERT INTO Ingredients (ingredient)
VALUES ('farinha de trigo');

INSERT INTO Ingredients (ingredient)
VALUES ('massa folhada');

INSERT INTO Ingredients (ingredient)
VALUES ('amido de milho');

INSERT INTO Ingredients (ingredient)
VALUES ('limão');
--- End of Ingredients

--- Start of Categories
-- TABLE
CREATE TABLE Categories 
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    category nvarchar(255)
)
-- DATA
/*  
Lawful Good,    Neutral Good,       Chaotic Good, 
Lawful Netral,  True Neutral,       Chaotic Neutral,  
Lawful Evil,    Neutral Evil,       Chaotic Evil
*/
INSERT INTO Categories (category)
VALUES ('lawful good');

INSERT INTO Categories (category)
VALUES ('neutral good');

INSERT INTO Categories (category)
VALUES ('chaotic good');

INSERT INTO Categories (category)
VALUES ('lawful netral');

INSERT INTO Categories (category)
VALUES ('true neutral');

INSERT INTO Categories (category)
VALUES ('chaotic neutral');

INSERT INTO Categories (category)
VALUES ('lawful evil');

INSERT INTO Categories (category)
VALUES ('neutral evil');

INSERT INTO Categories (category)
VALUES ('chaotic evil');

SELECT * FROM Categories;
--- End of Categories


--- Start of Recipes
CREATE TABLE Recipes
(
    ID              int IDENTITY(1,1) PRIMARY KEY,
    creator         int FOREIGN KEY REFERENCES Users(ID),
    title           nvarchar(255),
    preparation     nvarchar(MAX),
    categoryID      int FOREIGN KEY REFERENCES Categories(ID),
    difficultID     int FOREIGN KEY REFERENCES Difficulties(ID),
    creationDate    datetime DEFAULT GETDATE(),
    isApproved      bit DEFAULT 0
)
-- DATA
INSERT INTO Recipes 
    (
        creator,
        title,
        preparation, 
        categoryID,
        difficultID    
    )
VALUES  
(
        1, 
        'Pastel de Nata (Nao aprovada)', 
        'Pegue todos os ingredientes, meta tudo no microondas num intervalo entre 5 e 60 minutos. Saia de casa, e não volte.',
        1,
        1
);

SELECT * FROM Recipes;


INSERT INTO Recipes 
    (
        creator,
        title,
        preparation, 
        categoryID,
        difficultID,
        isApproved
    )
VALUES  
(
        1, 
        'Pastel de Nata de Bacalhau (Aprovada)', 
        'Pegue todos os ingredientes, meta tudo no microondas num intervalo entre 5 e 60 minutos. Saia de casa, e não volte.',
        1,
        1,
        1
);

SELECT * FROM Recipes;
--- End of Recipes

--- Start of Comments
CREATE TABLE Comments
(
    ID              int IDENTITY(1,1) PRIMARY KEY,
    userID          int FOREIGN KEY REFERENCES Users(ID),
    recipeID        int FOREIGN KEY REFERENCES Recipes(ID),
    comment         nvarchar(MAX) NOT NULL,
    creationDate    datetime DEFAULT GETDATE(),
)
-- DATA
INSERT INTO Comments 
    (
        userID,
        recipeID,
        comment
    )
VALUES  
(
        1, 
        1, 
        'Não gostei, faltou colocar 5x mais canela'
);

SELECT * FROM Comments;

-- End of Comments


--- Start of Rating
CREATE TABLE Ratings 
(
    ID          int IDENTITY(1,1) PRIMARY KEY,
    usernameID  int FOREIGN KEY REFERENCES Users(ID),
    recipeID    int FOREIGN KEY REFERENCES Recipes(ID),
    rating     int NOT NULL DEFAULT 0,
)

INSERT INTO Ratings (usernameID, recipeID, rating)
VALUES (1, 1, 5);

SELECT * FROM Ratings;
--- End of Rating

--- Start of Rating
CREATE TABLE Favorites 
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    usernameID int,
    recipeID int,
)
INSERT INTO Favorites (usernameID, recipeID)
VALUES (1, 1);

SELECT * FROM Favorites;
--- End of Favorites


--- Start of Ingredients Recipes
CREATE TABLE IngredientsRecipes
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    recipeID int,
    ingredientId int,
    unity nvarchar(32),
    quantity nvarchar(255)
)
INSERT INTO IngredientsRecipes (recipeID, ingredientId, unity, quantity)
VALUES (1, 1, '3', 'unidades');

SELECT * FROM IngredientsRecipes;
-- End of IngredientsRecipes