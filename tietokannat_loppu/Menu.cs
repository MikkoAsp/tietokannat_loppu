using tietokannat_loppu.Entities;

namespace BaseConsoleApp
{
    internal class Menu
    {
        public async Task StartMenu()
        {
            bool running = true;
            IDatabaseHandler handler = new DatabaseManager();
            IAskDetails helper = new Helper();
            TietokannatLoppuContext context = new();
            RecipeManager recipeHandling = new RecipeHandlingManager(handler, helper, context);

            while (running)
            {
                switch (PrintMenuOptions())
                {
                    case 0:
                        await recipeHandling.AddNewRecipe();
                        break;
                    case 1:
                        recipeHandling.ShowAllRecipes();
                        break;
                    case 2:
                        recipeHandling.UpdateRecipe();
                        break;
                    case 3:
                        recipeHandling.DeleteRecipeWithId();
                        break;
                    case 4:
                        recipeHandling.PrintRecipe(recipeHandling.SearchRecipesByIngredients(helper.AskRecipeIngredients()));
                        break;
                    case 5:
                        recipeHandling.SearchRecipesByDishes();
                        break;
                    case 6:
                        recipeHandling.SearchRecipesByDiets();
                        break;
                    case 7:
                        running = false;
                        break;
                    case 8:
                        await handler.SaveRecipesToDatabaseAsync(new Localrecipe("DebugTest", Dish.Main, new List<string> {"DebugIngredient 1", "DebugIngredient 2"}, new List<string> {"Step 1", "Step 2"}, Diet.Meat),context);
                        Console.WriteLine("DONE");
                        break;
                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
            }

        }
        private int PrintMenuOptions()
        {
            string[] options = { "1. Add a new recipe", "2. Show all recipes", "3. Update a recipe" ,"4. Delete A Recipe With Id", "5. Search For Recipes By Ingredients", "6. Search For Recipes By Dish", "7. Search For Recipes Based On Diet","8. End Program", "9. Debug: SaveToDb" };
            int currentIndex = 0;
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                Console.WriteLine("Select Function You Want To Use (Use Arrow Keys, Numbers 1-8, and Enter):");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == currentIndex)
                    {
                        Console.WriteLine($"> {options[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"  {options[i]}");
                    }
                }

                keyPressed = Console.ReadKey(true).Key;

                if (keyPressed >= ConsoleKey.D1 && keyPressed <= ConsoleKey.D8)
                {
                    currentIndex = keyPressed - ConsoleKey.D1;
                }
                else if (keyPressed >= ConsoleKey.NumPad1 && keyPressed <= ConsoleKey.NumPad7)
                {
                    currentIndex = keyPressed - ConsoleKey.NumPad1;
                }
                else if (keyPressed == ConsoleKey.UpArrow)
                {
                    currentIndex = (currentIndex - 1 + options.Length) % options.Length;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    currentIndex = (currentIndex + 1) % options.Length;
                }
            }
            while (keyPressed != ConsoleKey.Enter);
            return currentIndex;
        }
    }
}
