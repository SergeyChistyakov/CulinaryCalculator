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
}
