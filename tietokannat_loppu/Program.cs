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
