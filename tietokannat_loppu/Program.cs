using tietokannat_loppu;
using CrypticWizard.RandomWordGenerator;
using static CrypticWizard.RandomWordGenerator.WordGenerator;
using System.ComponentModel.DataAnnotations; //for brevity, not required
namespace BaseConsoleApp
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Menu menu = new();
            await menu.StartMenu(CreateRandomUser());

        }

        static LocalUser CreateRandomUser()
        {
            WordGenerator generator = new WordGenerator();
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


            return new LocalUser(userName, email, password);
        }

        static string GeneratePassword(Random rand)
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
    }
}
