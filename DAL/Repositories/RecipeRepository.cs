using DAL.Models;

// Palavras chaves sao armazenadas criptografadas, com md5 (nao eh + recomandavel)
using DAL.Helpers;

// Para uso do Banco de Dadose
using Microsoft.Data.SqlClient;

// Para ler do arquivo settings.json
using Microsoft.Extensions.Configuration;

/*
O que temos?
Pegar todos Users
Adicionar User

O que falta?
Get By Id
Delete (?)
Update
*/

namespace DAL.Repositories
{
    public class RecipeRepository
    {
        private readonly string _connString;

        public RecipeRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        // Pega todas as 
        public List<Recipe> GetAllShort(bool isPending)
        {
            List<Recipe> recipes = new();

            using SqlConnection conn = new SqlConnection(_connString);

            string query = @"
            SELECT 
                r.Id,
                r.Title,
                r.isApproved
            FROM Recipes r
            ";

            if (isPending)
            {
                query += " WHERE r.isApproved = 0";
            }
            else
            {
                query += " WHERE r.isApproved = 1";
            }

            using SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                recipes.Add(new Recipe
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    IsApproved = reader.GetBoolean(2)
                });
            }

            return recipes;
        }

        public void Add(Recipe recipe)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Recipes (creator, title, preparation, categoryID, difficultID)" +
                " VALUES (@Creator, @Title, @Preparation, @CategoryId, @DifficultId); " +
                "SELECT SCOPE_IDENTITY();", // Insert returna nullo quando o ExecutaScalar é chamado, mas precisamos do ID na ultima receita insirada
                conn
            );

            cmd.Parameters.AddWithValue("@Creator", recipe.CreatorId);
            cmd.Parameters.AddWithValue("@Title", recipe.Title);
            cmd.Parameters.AddWithValue("@Preparation", recipe.Preparation);
            cmd.Parameters.AddWithValue("@CategoryId", recipe.CategoryId);
            cmd.Parameters.AddWithValue("@DifficultId", recipe.DifficultId);

            conn.Open();
            // https://learn.microsoft.com/en-us/sql/t-sql/functions/scope-identity-transact-sql?view=sql-server-ver17
            int recipeId = Convert.ToInt32(cmd.ExecuteScalar()); // Returna o ultimo IDENTITY gracas ao SELECT SCOPE_IDENTITY(); na query

            foreach (var ingredient in recipe.Ingredients)
            {
                using SqlCommand ingCmd = new SqlCommand(
                    "INSERT INTO IngredientsRecipes (recipeID, ingredientId, unity, quantity) " +
                    "VALUES (@RecipeId, @IngredientId, @Unity, @Quantity)",
                    conn
                );
                ingCmd.Parameters.AddWithValue("@RecipeId", recipeId);
                ingCmd.Parameters.AddWithValue("@IngredientId", ingredient.IngredientId);
                ingCmd.Parameters.AddWithValue("@Unity", (object)ingredient.Unity ?? DBNull.Value); //  Sintaxe -> Se esquerda == null, use direita.
                ingCmd.Parameters.AddWithValue("@Quantity", ingredient.Quantity);
                ingCmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        public Recipe? GetById(int id, int? userId = null, bool isApproved = true)
        {

            // Busca a recipe, junta om o criador atraves do Id para pegar o nome dele
            // depois us ao where para pegar um receita especifica

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
               @"SELECT 
                r.Id, 
                r.Title, 
                r.Preparation, 
                r.CategoryId,
                c.category AS CategoryName,
                r.DifficultId,
                d.difficult AS DifficultyName,
                r.CreationDate,
                u.FirstName AS CreatorName
            FROM Recipes r 
            JOIN Users u ON r.creator = u.Id
            JOIN Categories c ON r.CategoryId = c.Id
            JOIN Difficulties d ON r.DifficultId = d.Id
            WHERE r.Id = @Id AND r.isApproved = @IsApproved",
               conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@IsApproved", isApproved);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            // Se nao tem receita, retorna nulo
            if (!reader.Read()) return null;

            var recipe = new Recipe
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Preparation = reader.GetString(2),
                CategoryId = reader.GetInt32(3),
                CategoryName = reader.GetString(4),
                DifficultId = reader.GetInt32(5),
                DifficultyName = reader.GetString(6),
                CreationDate = reader.GetDateTime(7),
                CreatorName = reader.GetString(8),
                Ingredients = new List<RecipeIngredient>()
            };
            reader.Close();

            // Pega os ingredientes da receita
            using SqlCommand ingredientCmd = new SqlCommand(
            "SELECT i.ingredient, ir.quantity, ir.unity " +
            "FROM IngredientsRecipes ir " +
            "JOIN Ingredients i ON ir.ingredientId = i.ID " +
            "WHERE ir.recipeID = @Id",
            conn
        );

            ingredientCmd.Parameters.AddWithValue("@Id", id);
            using SqlDataReader ingredientReader = ingredientCmd.ExecuteReader();
            while (ingredientReader.Read())
            {
                recipe.Ingredients.Add(new RecipeIngredient
                {
                    Name = ingredientReader.GetString(0),
                    Quantity = ingredientReader.GetString(1),
                    Unity = ingredientReader.IsDBNull(2) ? null : ingredientReader.GetString(2)
                });
            }
            ingredientReader.Close();

            // Calculate a media das avaliacoes
            using SqlCommand avgCmd = new SqlCommand(
            "SELECT AVG(CAST(rating AS FLOAT)) FROM Ratings WHERE recipeID = @Id", conn);
            avgCmd.Parameters.AddWithValue("@Id", id);
            var avg = avgCmd.ExecuteScalar();
            recipe.CalculatedRatings = avg == DBNull.Value ? 0 : Convert.ToDouble(avg);

            // Pega o numero de avaliacoes
            using SqlCommand countCmd = new SqlCommand(
            "SELECT COUNT(*) FROM Ratings WHERE recipeID = @Id",
            conn
            );
            countCmd.Parameters.AddWithValue("@Id", id);
            recipe.TotalRatings = Convert.ToInt32(countCmd.ExecuteScalar());


            // Se usuario logado,
            // pega se favoritou, e avaliou
            if (userId != null)
            {
                // Favoritou?
                using SqlCommand favCmd = new SqlCommand(
                    "SELECT COUNT(1) FROM Favorites WHERE recipeID = @Id AND usernameID = @UserId", conn);
                favCmd.Parameters.AddWithValue("@Id", id);
                favCmd.Parameters.AddWithValue("@UserId", userId);
                recipe.isFavoritedByUser = Convert.ToInt32(favCmd.ExecuteScalar()) > 0;

                // Avaliou?
                using SqlCommand ratedCmd = new SqlCommand(
                    "SELECT rating FROM Ratings WHERE recipeID = @Id AND usernameID = @UserId", conn);
                ratedCmd.Parameters.AddWithValue("@Id", id);
                ratedCmd.Parameters.AddWithValue("@UserId", userId);
                var result = ratedCmd.ExecuteScalar();
                if (result != null)
                {
                    recipe.isRatedByUser = true;
                    recipe.UserRating = Convert.ToInt32(result);
                }
                else
                {
                    recipe.isRatedByUser = false;
                    recipe.UserRating = null;
                }

            }

            // Pega os comentarios da receita
            using SqlCommand commentCmd = new SqlCommand(
            "SELECT rc.Id, u.FirstName, rc.Comment, rc.creationDate " +
            "FROM Comments rc JOIN Users u ON rc.userId = u.Id " +
            "WHERE rc.recipeID = @Id ORDER BY rc.creationDate DESC",
            conn);
            commentCmd.Parameters.AddWithValue("@Id", id);
            using SqlDataReader commentReader = commentCmd.ExecuteReader();
            while (commentReader.Read())
            {
                recipe.Comments.Add(new RecipeComment
                {
                    Id = commentReader.GetInt32(0),
                    UserName = commentReader.GetString(1),
                    Comment = commentReader.GetString(2),
                    CreatedAt = commentReader.GetDateTime(3)
                });
            }
            commentReader.Close();
            return recipe;
        }

        // Pega as Count receitas, e utiliza o ID para saber as favoritadas
        public List<Recipe> GetTopRecipes(int count, int? userId = null)
        {
            List<Recipe> recipes = new();

            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(@"
            SELECT TOP (@Count)
                r.Id,
                r.Title,
                COUNT(f.usernameID) AS IsFavorited
            FROM Recipes r
            LEFT JOIN Ratings rt ON r.Id = rt.recipeID
            LEFT JOIN Favorites f 
                ON r.Id = f.recipeID AND f.usernameId = @UserId
            WHERE r.isApproved = 1
            GROUP BY r.Id, r.Title
            ORDER BY ISNULL(AVG(CAST(rt.rating AS FLOAT)), 0) DESC;
            ", conn);

            cmd.Parameters.AddWithValue("@Count", count);
            cmd.Parameters.AddWithValue("@UserId", (object?)userId ?? DBNull.Value);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                recipes.Add(new Recipe
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    isFavoritedByUser = reader.GetInt32(2) > 0
                });
            }

            return recipes;
        }


        // Muda o estado do favorito
        public bool ToggleFavorite(int userId, int recipeId)
        {
            using SqlConnection conn = new SqlConnection(_connString);
            conn.Open();

            // 1. Verifica se ja foi favoritada
            using SqlCommand checkCmd = new SqlCommand(
                "SELECT COUNT(1) FROM Favorites WHERE usernameID = @UserId AND recipeID = @RecipeId",
                conn
            );

            checkCmd.Parameters.AddWithValue("@UserId", userId);
            checkCmd.Parameters.AddWithValue("@RecipeId", recipeId);

            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

            // Sim foi!
            if (count > 0)
            {
                // 2. Remove
                using SqlCommand deleteCmd = new SqlCommand(
                    "DELETE FROM Favorites WHERE usernameID = @UserId AND recipeID = @RecipeId",
                    conn
                );

                deleteCmd.Parameters.AddWithValue("@UserId", userId);
                deleteCmd.Parameters.AddWithValue("@RecipeId", recipeId);

                deleteCmd.ExecuteNonQuery();

                return false; // Devolve falso
            }
            else
            {
                // 3. Nao estava favoritada
                using SqlCommand insertCmd = new SqlCommand(
                    "INSERT INTO Favorites (usernameID, recipeID) VALUES (@UserId, @RecipeId)",
                    conn
                );

                insertCmd.Parameters.AddWithValue("@UserId", userId);
                insertCmd.Parameters.AddWithValue("@RecipeId", recipeId);

                insertCmd.ExecuteNonQuery();

                return true; // Favborita, e deovolve true!
            }
        }

        // Consulta as receitas favoritadas pelo usuario
        public List<Recipe> GetFavoritesByUser(int? userId)
        {
            List<Recipe> recipes = new();

            if (userId == null)
                return recipes;

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(@"
            SELECT r.Id, r.Title
            FROM Favorites f
            JOIN Recipes r ON r.Id = f.recipeID
            WHERE f.usernameID = @UserId AND r.isApproved = 1
            ", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                recipes.Add(new Recipe
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    isFavoritedByUser = true
                });
            }

            return recipes;
        }

        // Adiciona comentario ao um receita especifica
        public void AddComment(int userId, int recipeId, string comment)
        {
            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Comments (userId, recipeID, comment, creationDate) VALUES (@UserId, @RecipeId, @Comment, GETDATE())",
                conn
            );

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@RecipeId", recipeId);
            cmd.Parameters.AddWithValue("@Comment", comment);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // Segue a mesma ideia do Toggle Favorito
        public void AddOrUpdateRating(int userId, int recipeId, int rating)
        {
            using SqlConnection conn = new SqlConnection(_connString);
            conn.Open();

            using SqlCommand checkCmd = new SqlCommand(
                "SELECT COUNT(1) FROM Ratings WHERE usernameID = @UserId AND recipeID = @RecipeId",
                conn
            );

            checkCmd.Parameters.AddWithValue("@UserId", userId);
            checkCmd.Parameters.AddWithValue("@RecipeId", recipeId);

            int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (exists > 0)
            {
                using SqlCommand updateCmd = new SqlCommand(
                    "UPDATE Ratings SET rating = @Rating WHERE usernameID = @UserId AND recipeID = @RecipeId",
                    conn
                );

                updateCmd.Parameters.AddWithValue("@UserId", userId);
                updateCmd.Parameters.AddWithValue("@RecipeId", recipeId);
                updateCmd.Parameters.AddWithValue("@Rating", rating);

                updateCmd.ExecuteNonQuery();
            }
            else
            {
                using SqlCommand insertCmd = new SqlCommand(
                    "INSERT INTO Ratings (usernameID, recipeID, rating) VALUES (@UserId, @RecipeId, @Rating)",
                    conn
                );

                insertCmd.Parameters.AddWithValue("@UserId", userId);
                insertCmd.Parameters.AddWithValue("@RecipeId", recipeId);
                insertCmd.Parameters.AddWithValue("@Rating", rating);

                insertCmd.ExecuteNonQuery();
            }
        }

        // Aprove receitas
        public void ApproveRecipe(int recipeId)
        {
            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "UPDATE Recipes SET isApproved = 1 WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", recipeId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        // Busca as receitas cujo titulo contenha a palavra pesquisada
        public List<Recipe> Search(string search, int? userId)
        {
            List<Recipe> recipes = new();

            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(@"
            SELECT TOP 100
                r.Id,
                r.Title,
                COUNT(f.usernameID) AS IsFavorited
            FROM Recipes r
            LEFT JOIN Favorites f 
                ON r.Id = f.recipeID AND f.usernameId = @UserId
            WHERE r.isApproved = 1
            AND r.Title LIKE @Search
            GROUP BY r.Id, r.Title
            ", conn);

            cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
            cmd.Parameters.AddWithValue("@UserId", (object?)userId ?? DBNull.Value);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                recipes.Add(new Recipe
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    isFavoritedByUser = reader.GetInt32(2) > 0
                });
            }

            return recipes;
        }


    }
}
