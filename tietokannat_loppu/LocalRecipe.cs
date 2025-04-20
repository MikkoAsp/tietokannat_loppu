using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using tietokannat_loppu.Entities;
namespace BaseConsoleApp
{
    public class Localrecipe
    {
        private string name;
        public string Name => name;

        private Dish dish;
        public Dish Dish => dish;
        private Diet diet;
        public Diet Diet => diet;
        private List<string> ingredients = new();
        public List<string> Ingredients => ingredients;
        private List<string> instructions = new();
        public List<string> Instructions => instructions;

        public Localrecipe(string name, Dish dish, List<string> ingredients,List<string>instructions , Diet diet)
        {
            this.name = name;
            this.dish = dish;
            this.ingredients = ingredients;
            this.instructions = instructions;
            this.diet = diet;
        }
    }
}
