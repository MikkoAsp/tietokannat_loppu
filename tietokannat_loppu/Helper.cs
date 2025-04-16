namespace BaseConsoleApp
{
    public class Helper : IAskDetails
    {
        public string AskString(string displayMsg)
        {
            while (true)
            {
                Console.Write(displayMsg);
                string recipeName = Console.ReadLine();

                if (string.IsNullOrEmpty(recipeName))
                {
                    Console.WriteLine("Please do not enter empty values");
                }
                else
                {
                    return recipeName;
                }
            }
        }
        public int AskIntNumber(string displayMsg)
        {
            while (true)
            {
                Console.Write(displayMsg);
                int num;
                bool result = int.TryParse(Console.ReadLine(), out num);

                if (!result)
                {
                    Console.WriteLine("Please enter a numeric value.");
                }
                else
                {
                    return num;
                }
            }
        }
        public int AskIntNumberWithMaxMinRange(string displayMsg, int min, int max)
        {
            while (true)
            {
                int num = AskIntNumber(displayMsg);
                
                if(num < min || num > max)
                {
                    Console.WriteLine("Number was not in the correct range of numbers of min " + min + " and " + max);
                }
                else
                {
                    return num;
                }
            }

        }
        public string FormatEnumDisplayName(Enum value)
        {
            var name = value.ToString();
            return System.Text.RegularExpressions.Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
        }

        private void PrintEnumValues<TEnum>() where TEnum : Enum
        {
            Console.WriteLine($"\n{typeof(TEnum).Name} options");
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                // Cast the object to the specific enum type
                TEnum enumValue = (TEnum)value;

                // Formatting the enum value to have spaces if 2 (or more words)
                string formattedValue = FormatEnumDisplayName(enumValue);

                // Print the enum value as its integer value and formatted name
                Console.WriteLine($"{(int)(object)enumValue}. {formattedValue}");
            }
        }

        public Enum? SelectEnumOption<TEnum>(int min, int? max = null) where TEnum : Enum
        {
            PrintEnumValues<TEnum>();
            Console.WriteLine($"\nEnter the {typeof(TEnum).Name} number to confirm it as your choice or enter 0 to return back to menu");

            int num = AskIntNumberWithMaxMinRange("Your option: ", min, max ?? Enum.GetValues(typeof(TEnum)).Length);

            if (num == 0)
            {
                return null;
            }
            else
            {
                TEnum selectedEnumValue = (TEnum)Enum.ToObject(typeof(TEnum), num);
                return selectedEnumValue;
            }
        }
        public void InfoUser(Recipe recipe)
        {
            string dietString = (recipe.Diet != Diet.None) ? FormatEnumDisplayName(recipe.Diet) : "";
            Console.WriteLine($"\nNew {dietString} {FormatEnumDisplayName(recipe.Dish)} {recipe.Name} recipe added with ingredients: ");
            PrintAllIngredients(recipe.Ingredients);
            Console.WriteLine("\nWith the following instructions: ");
            PrintAllInstructions(recipe.Instructions);
        }
        private void PrintAllIngredients(List<string> ingredients)
        {
            int count = 0;
            foreach (var item in ingredients)
            {
                count++;
                if (count == ingredients.Count)
                {
                    Console.Write(item);
                }
                else if (count == ingredients.Count - 1)
                {
                    Console.Write(item + " and ");
                }
                else
                {
                    Console.Write(item + ", ");
                }
            }
            Console.WriteLine();
        }
        private void PrintAllInstructions(List<string> instructions)
        {
            int count = 0;
            foreach (var item in instructions)
            {
                count++;
                Console.WriteLine($"Step {count}: {item}");
            }
            Console.WriteLine();
        }

        public List<string> AskRecipeIngredients()
        {
            while (true)
            {
                Console.WriteLine("Enter Recipe Ingredients (separate them with commas)");

                List<string> addedIngredients = Console.ReadLine()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(ingredient => ingredient.Trim())
                .Where(ingredient => !string.IsNullOrEmpty(ingredient))
                .ToList();

                if (addedIngredients.Count != 0) return addedIngredients;
            }
        }
        public List<string> AskRecipeInstructions()
        {
            List<string> instructions = new List<string>();

            Console.WriteLine("Enter recipe instructions step by step. Type 'done' when finished.");

            int stepNumber = 1;

            while (true)
            {
                string instruction = AskString(stepNumber + ". step: ");

                if (instruction?.ToLower() == "done") break;
                instructions.Add(instruction);
                stepNumber++;
            }
            return instructions;
        }
    }
}
