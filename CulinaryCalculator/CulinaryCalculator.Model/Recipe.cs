using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CulinaryCalculator.Model
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
        public List<IngredientItem> IngredientItems { get; set; } = new List<IngredientItem>();
    }
}
