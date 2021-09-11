namespace CulinaryCalculator.Model
{
    public class RecipeStep
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
