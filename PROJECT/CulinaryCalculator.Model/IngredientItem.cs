using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CulinaryCalculator.Model
{
    public class IngredientItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public double Quantity { get; set; }
        public UnitOfMeasure Unit { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }

    public class IngredientItemIdEqualityComarer : IEqualityComparer<IngredientItem>
    {
        public bool Equals(IngredientItem x, IngredientItem y)
        {
            return x != null && y != null && x.Id == y.Id;
        }

        public int GetHashCode(IngredientItem obj)
        {
            return obj?.Id ?? 0;
        }
    }
}
