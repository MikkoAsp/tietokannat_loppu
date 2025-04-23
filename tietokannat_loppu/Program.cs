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
            await menu.StartMenu();
        }
    }
}
