using CrypticWizard.RandomWordGenerator;
using Microsoft.EntityFrameworkCore;
using tietokannat_loppu.Entities;
using static CrypticWizard.RandomWordGenerator.WordGenerator;

namespace BaseConsoleApp
{
    //Default login account used:
    //Jarkko.Jarmonen@gmail.com
    //JarkonKokkaukset27
    //user id = 1
    internal class Menu
    {
        IAskDetails helper = new Helper();
        IDatabaseHandler dbHandler;
        RecipeManager recipeHandler;
        TietokannatLoppuContext dbContext = new();
        bool running = true;
        LocalUser user;

        string[] possibleMenuOptions = { "1. Add a new recipe", "2. Show all recipes", "3. Update a recipe", "4. Delete A Recipe With Id", "5. Search For Recipes By Ingredients", "6. Search For Recipes By Dish", "7. Search For Recipes Based On Diet", "8. End Program", "9. Debug: SaveToDb" };
        public Menu()
        {
            dbHandler = new DatabaseManager(dbContext);
            recipeHandler = new RecipeHandlingManager(dbHandler, helper);
        }

        public async Task StartMenu()
        {
            user = AskLoginOptions().Result;
            string loginDetails = $"Login as:{user.UserName}\nEmail:{user.Email}\nId:{user.Id}\n";
            while (running)
            {
                MenuOption menuOption = (MenuOption)PrintMenuOptions(possibleMenuOptions, loginDetails);
                switch (menuOption)
                {
                    case MenuOption.AddRecipe:
                        await recipeHandler.AddNewRecipe(user);
                        break;
                    case MenuOption.ShowAllRecipes:
                        recipeHandler.ShowAllRecipes(user);
                        break;
                    case MenuOption.UpdateRecipe:
                        recipeHandler.UpdateRecipe();
                        break;
                    case MenuOption.DeleteRecipe:
                        recipeHandler.DeleteRecipeWithId();
                        break;
                    case MenuOption.SearchWithIngredients:
                        recipeHandler.PrintRecipe(recipeHandler.SearchRecipesByIngredients(helper.AskRecipeIngredients()));
                        break;
                    case MenuOption.SearchWithDish:
                        recipeHandler.SearchRecipesByDishes();
                        break;
                    case MenuOption.SearchWithDiet:
                        recipeHandler.SearchRecipesByDiets();
                        break;
                    case MenuOption.EndProgram:
                        Console.WriteLine("Program ended");
                        running = false;
                        break;
                    case MenuOption.DebugSaveToDb:
                        await dbHandler.SaveRecipesToDatabaseAsync(new Localrecipe("DebugTest", Dish.Main, new List<string> {"DebugIngredient 1", "DebugIngredient 2"}, new List<string> {"Step 1", "Step 2"}, Diet.Meat), user);
                        Console.WriteLine("DONE");
                        break;

                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
            }

        }
        private async Task<LocalUser> AskLoginOptions()
        {
            bool loggingIn = true;
            LocalUser? user;
            while (loggingIn)
            {
                Console.WriteLine("Create a new user or login to existing one with email and password");
                string[] loginOptions = { "Create new user", "Login to Existing user","Debug: login to default account" };
                LoginOption option = (LoginOption)PrintMenuOptions(loginOptions, "Login to existing user with email and password or create a new user.\n");

                if (option == LoginOption.CreateUser)
                {
                    return user = CreateRandomUser().Result;
                }
                else if(option == LoginOption.Login)
                {
                    string email = helper.AskString("Enter email: ");
                    string password = helper.AskString("Enter password: ");
                    user = LoginUser(email, password);

                    if(user != null)
                    {
                        return user;
                    }
                }
                else if(option == LoginOption.DebugLogin)
                {
                    return LoginUser("Jarkko.Jarmonen@gmail.com", "JarkonKokkaukset27");
                }
            }
            throw new Exception("Unexpected error during login.");
        }
        private int PrintMenuOptions(string[] options, string? msg = null)
        {
            int currentIndex = 0;
            int optionsLength = options.Length;
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                Console.WriteLine(msg);
                Console.WriteLine($"Select your option with Arrow Keys, Numbers 1-{optionsLength}, and Enter");

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

                if (keyPressed >= ConsoleKey.D1 && keyPressed <= ConsoleKey.D1 + optionsLength - 1)
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
        private LocalUser? LoginUser(string email, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if(user != null)
            {
                Console.WriteLine("Login success");
                Console.ReadLine();
                return new LocalUser(user.UserId,user.Username, user.Email, user.Password);
            }
            else
            {
                Console.WriteLine("Incorrect login credentials");
                Console.ReadLine();
                return null;
            }
        }
        private async Task<LocalUser> CreateRandomUser()
        {
            WordGenerator generator = new WordGenerator();
            while (true)
            {
                string start = generator.GetWord(PartOfSpeech.adj);
                string middle = generator.GetWord(PartOfSpeech.adj);
                string end = generator.GetWord(PartOfSpeech.noun);

                string userName = $"{start} {middle} {end}";

                string[] options = { "@gmail.com", "@outlook.com", "@yahoo.com", "@hotmail.com", "@icloud.com", "@aol.com", "@zoho.com", "@protonmail.com" };

                Random rand = new Random();

                int value = rand.Next(0, options.Length);

                string emailOption = options[value];

                string email = start + middle + end + emailOption;

                string password = GeneratePassword(rand);

                //Make sure there isn't already a user with this email
                var userExistsWithEmail = dbContext.Users.FirstOrDefault(u => u.Email == email);
                if (userExistsWithEmail == null)
                {
                    var user = await dbHandler.AddUserToDb(email, password, userName);
                    return new LocalUser(user.UserId, userName, email, password);
                }
                else
                {
                    Console.WriteLine("User already exists in db");
                    Console.ReadLine();
                }
            }
        }
        private string GeneratePassword(Random rand)
        {
            string result = "";
            // Generate 20 characters (letters or numbers)
            for (int i = 0; i < 20; i++)
            {
                // Decide randomly whether to add a letter or a number
                if (rand.Next(0, 2) == 0) // 50% chance to add a letter
                {
                    // Generate a random letter from 'a' to 'z'
                    char letter = (char)('a' + rand.Next(0, 26));
                    result += letter;
                }
                else
                {
                    // Generate a random number between 0 and 9
                    char number = (char)('0' + rand.Next(0, 10));
                    result += number;
                }
            }

            return result;
        }

        private enum LoginOption
        {
            CreateUser = 0,
            Login = 1,
            DebugLogin = 2
        }
        private enum MenuOption
        {
            AddRecipe = 0,
            ShowAllRecipes = 1,
            UpdateRecipe = 2,
            DeleteRecipe = 3,
            SearchWithIngredients = 4,
            SearchWithDish = 5,
            SearchWithDiet = 6,
            EndProgram = 7,
            DebugSaveToDb = 8
        }
    }
}
