using Microsoft.EntityFrameworkCore;

namespace CulinaryCalculator.Model
{
    public class RecipesContext : DbContext
    {
        private readonly string m_DatabasePath;
        public DbSet<Category> Categories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<IngredientItem> IngredientItem { get; set; }

        public DbSet<RecipeStep> RecipeStep { get; set; }

        public RecipesContext(string databasePath)
        {
            m_DatabasePath = databasePath;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlite($"Filename={m_DatabasePath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<RecipeStep>()
                .HasOne(rs => rs.Recipe)
                .WithMany(r => r.Steps)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder
                .Entity<IngredientItem>()
                .HasOne(rs => rs.Recipe)
                .WithMany(r => r.IngredientItems)
                .OnDelete(DeleteBehavior.ClientCascade);

            var categoryBuilder = modelBuilder.Entity<Category>();
            categoryBuilder.HasData(new Category() { Id = 1, Title = "Пироги", Image = ReadBinaryFromResource("meatpie.jpg") });
            categoryBuilder.HasData(new Category() { Id = 2, Title = "Пирожные", Image = ReadBinaryFromResource("cookies.jpg") });
            categoryBuilder.HasData(new Category() { Id = 3, Title = "Печенье", Image = ReadBinaryFromResource("сakes.jpg") });

            var recipeBuilder = modelBuilder.Entity<Recipe>();
            recipeBuilder.HasData(new Recipe()
            {
                Id = 1,
                Title = "Мясной пирог на кефире",
                CategoryId = 1,
                Image = ReadBinaryFromResource("Recipe1.jpg"),
                Description = "Очень вкусный пирог с хрустящим тестом и сочной начинкой! По способу формовки напоминает кавказские пироги, но делается без дрожжей."
            });

            var recipeStepBuilder = modelBuilder.Entity<RecipeStep>();
            recipeStepBuilder.HasData(new RecipeStep() { Id = 1, Number = 1, Description = "Приготовим тесто. Яйцо слегка разболтать с солью, добавить кефир, соду, растительное масло и хорошо размешать.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 2, Number = 2, Description = "Затем добавить муку, сколько возьмет тесто. Примерно может понадобиться 2-2,5 ст. Тесто должно быть достаточно мягким, слегка липким, но отставать от рук. Вымесить тесто на столе и оставить под полотенцем на 20 минут \"отдохнуть\".", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 3, Number = 3, Description = "Пока тесто отдыхает, приготовим начинку. Мясо измельчить в процессоре или прокрутить на мясорубке вместе с зубчиком чеснока и половиной луковицы. Свинину лучше брать пожирнее, тогда начинка будет сочнее.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 4, Number = 4, Description = "Остальной лук порезать тонкой соломкой, зелень измельчить и добавить в фарш. Чем больше лука в начинке, тем пирог получится сочнее. Добавить сливки или молоко, посолить, поперчить и перемешать.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 5, Number = 5, Description = "Тесто раскатать на столе одной большой лепешкой, или разделить на две части и раскатать две лепешки поменьше. Толщина теста должна быть не менее 8 мм. Выложить в центр начинку, разровнять.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 6, Number = 6, Description = "Собрать края к середине и защипать, не оставляя зазоров через которые может вытечь сок при выпечке.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 7, Number = 7, Description = "Разровнять, перевернуть и раскатать скалкой до ровного и плоского круга. Пирог перенести на противень, застеленный пекарской бумагой и смазанной маслом. В середине сделать маленькое отверстие для выхода пара. Пирог можно наколоть вилкой, не затрагивая дно.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 8, Number = 8, Description = "Смазать сливками или яйцом.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 9, Number = 9, Description = "Посыпать кунжутом. Поставить выпекаться при 180 гр. на 30 минут.", RecipeId = 1 });
            recipeStepBuilder.HasData(new RecipeStep() { Id = 10, Number = 10, Description = "Готовый мясной пирог сбрызнуть водой и накрыть полотенцем. Оставить на 15 минут.", RecipeId = 1 });

            var ingredientItemBuilder = modelBuilder.Entity<IngredientItem>();
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 1, RecipeId = 1, Title = "мука", Unit = UnitOfMeasure.TableSpoon, Quantity = 2 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 2, RecipeId = 1, Title = "кефир", Unit = UnitOfMeasure.Milliliter, Quantity = 250 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 3, RecipeId = 1, Title = "растительное масло", Unit = UnitOfMeasure.TableSpoon, Quantity = 2 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 4, RecipeId = 1, Title = "соль", Unit = UnitOfMeasure.TeaSpoon, Quantity = 0.5 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 5, RecipeId = 1, Title = "яйцо ", Unit = UnitOfMeasure.Item, Quantity = 1 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 6, RecipeId = 1, Title = "кунжут ", Unit = UnitOfMeasure.TableSpoon, Quantity = 1 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 7, RecipeId = 1, Title = "свинина", Unit = UnitOfMeasure.Gram, Quantity = 400 });           
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 8, RecipeId = 1, Title = "репчатый лук", Unit = UnitOfMeasure.Item, Quantity = 2 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 9, RecipeId = 1, Title = "зелень", Unit = UnitOfMeasure.TableSpoon, Quantity = 3 });
            ingredientItemBuilder.HasData(new IngredientItem() { Id = 10, RecipeId = 1, Title = "сливки", Unit = UnitOfMeasure.TableSpoon, Quantity = 3 });
        }

        private byte[] ReadBinaryFromResource(string resourceName)
        {
            var assembley = typeof(RecipesContext).Assembly;
            var resourcePrefix = "CulinaryCalculator.Model.DataToSeed.";
            var stream = assembley.GetManifestResourceStream($"{resourcePrefix}{resourceName}");
            var binary = new byte[stream.Length];
            stream.Read(binary, 0, (int)stream.Length);
            return binary;
        }
    }
}
