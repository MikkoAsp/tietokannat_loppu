using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;
using tietokannat_loppu.Entities;

namespace BaseConsoleApp
{
    public class DatabaseManager : IDatabaseHandler
    {
        TietokannatLoppuContext dbContext;
        public DatabaseManager(TietokannatLoppuContext contex)
        {
            dbContext = contex;
        }
        public async Task AddUserToDb(LocalUser newUser)
        {
            var userToSave = new User
            {
                Username = newUser.UserName,
                Email = newUser.Email,
                CreatedAt = DateTime.UtcNow,
                Password = newUser.Password
            };

            dbContext.Users.Add(userToSave);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("New user created");
            Console.ReadLine();
        }

        public List<Localrecipe>? LoadFromDatabase()
        {
            return new List<Localrecipe>();
        }
        public async Task SaveRecipesToDatabaseAsync(Localrecipe localrecipe, LocalUser localUser)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == localUser.Email);

            if(user != null)
            {
                var recipeToSave = new Recipe
                {
                    RecipeName = localrecipe.Name,
                    Dish = localrecipe.Dish,
                    Diet = localrecipe.Diet,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.UserId
                };

                // Save recipe first
                dbContext.Recipes.Add(recipeToSave);
                await dbContext.SaveChangesAsync();

                int step = 1;

                foreach (var instructionText in localrecipe.Instructions)
                {
                    var instruction = new Instruction
                    {
                        CookingInstructions = instructionText,
                        Step = step++,
                        RecipeId = recipeToSave.RecipeId
                    };
                    dbContext.Instructions.Add(instruction);
                }

                foreach (var ingredientName in localrecipe.Ingredients)
                {
                    var ingredient = await dbContext.Ingredients
                        .FirstOrDefaultAsync(i => i.IngredientName == ingredientName);
                    if (ingredient == null)
                    {
                        ingredient = new Ingredient { IngredientName = ingredientName };
                        dbContext.Ingredients.Add(ingredient);
                        await dbContext.SaveChangesAsync(); // Save to get ID
                    }

                    var recipeIngredient = new RecipeIngredient
                    {
                        RecipeId = recipeToSave.RecipeId,
                        IngredientId = ingredient.IngredientId,
                        Quantity = 1,
                        UnitType = "pcs"
                    };
                    dbContext.RecipeIngredients.Add(recipeIngredient);
                }

                await dbContext.SaveChangesAsync(); // Save instructions and ingredients
            }
            else
            {
                Console.WriteLine("Could find your user!");
            }

        }


        public void UpdateRecipeInDatabase(List<Localrecipe> newRecipes)
        {
          
        }
    }
}
