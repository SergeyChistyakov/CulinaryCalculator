using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CulinaryCalculator.Model
{
    public static class RecipesRepository
    {
        static readonly string RecipesDBPath;

        public static event EventHandler<Category> CategoryAdded;

        static RecipesRepository()
        {
            var fileName = "Recipes.db";
            var folder = DependencyService.Get<IPath>().GetDatabaseFolder();
            RecipesDBPath = Path.Combine(folder, fileName);

            if (!File.Exists(RecipesDBPath))
            {
                File.Create(RecipesDBPath);
            }

            using (var db = new RecipesContext(RecipesDBPath))
            {
                db.Database.EnsureCreated();
                //db.Database.Migrate();
            }
        }

        public static void AddCategory(Category category)
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
            CategoryAdded?.Invoke(null, category);
        }

        public static List<Category> GetCategories()
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                return db.Categories.ToList();
            }
        }

        public static void UpdateCategory(Category category)
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                db.Categories.Update(category);
                db.SaveChanges();
            }
        }

        public static void RemoveCategory(Category category)
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                db.Categories.Remove(category);
                db.SaveChanges();
            }
        }

        public static void AddRecipe(Recipe recipe)
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                db.Recipes.Add(recipe);
                db.SaveChanges();
            }
        }

        public static List<Recipe> GetRecipes()
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                return db.Recipes.Include(r => r.Category).Include(r => r.IngredientItems).ToList();
            }
        }

        public static Recipe GetRecipe(int recipeId)
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                return db.Recipes.Include(r => r.Steps).Include(r => r.IngredientItems).Include(r => r.Category).First(r => r.Id == recipeId);
            }
        }

        public static void UpdateRecipe(Recipe recipe)
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                var iiIds = recipe.IngredientItems.Select(ii => ii.Id);
                var ingredientsToDelete = db.IngredientItem.Where(item => !iiIds.Contains(item.Id));
                db.IngredientItem.RemoveRange(ingredientsToDelete);

                var stepsIds = recipe.Steps.Select(ii => ii.Id);
                var stepsToDelete = db.RecipeStep.Where(item => !stepsIds.Contains(item.Id));
                db.RecipeStep.RemoveRange(stepsToDelete);

                db.Recipes.Update(recipe);

                db.SaveChanges();
            }
        }

        public static void RemoveRecipe(Recipe recipe)
        {
            using (var db = new RecipesContext(RecipesDBPath))
            {
                db.Recipes.Remove(recipe);
                db.SaveChanges();
            }
        }
    }
}
