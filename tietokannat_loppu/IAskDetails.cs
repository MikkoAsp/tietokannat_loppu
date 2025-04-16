using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConsoleApp
{
    public interface IAskDetails
    {
        public string AskString(string displayMsg);
        public int AskIntNumber(string displayMsg);
        public int AskIntNumberWithMaxMinRange(string displayMsg, int max, int min);

        public Enum? SelectEnumOption<TEnum>(int min, int? max = null) where TEnum : Enum;
        public void InfoUser(Localrecipe recipe);
        public string FormatEnumDisplayName(Enum value);
        public List<string> AskRecipeIngredients();
        public List<string> AskRecipeInstructions();
    }
}
