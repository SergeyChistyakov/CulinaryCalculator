using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CulinaryCalculator.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        public override string ToString()
        {
            return Title;
        }
    }

    public class CategoryIdEqualityComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            return x != null && y != null && x.Id == y.Id;
        }

        public int GetHashCode(Category obj)
        {
            return obj?.Id ?? 0;
        }
    }
}
