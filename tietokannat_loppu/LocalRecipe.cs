using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BaseConsoleApp
{
    public enum Dish
    {
        None = 1,
        MainCourse = 2,
        Side = 3,
        Dessert = 4,
        Drink = 5

    }
    public enum Diet
    {
        None = 1,
        Meat = 2,
        Keto = 3,
        Vegetarian = 4,
        Vegan = 5,
        LactoseFree = 6,
        GlutenFree = 7
    }
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
