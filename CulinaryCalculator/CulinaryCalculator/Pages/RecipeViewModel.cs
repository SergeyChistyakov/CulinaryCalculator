using CulinaryCalculator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Model = CulinaryCalculator.Model;

namespace CulinaryCalculator.Pages
{
    public class RecipeViewModel : BaseViewModel
    {
        private Model.Recipe m_Recipe;
        private readonly Dictionary<int, double> m_ItemValues = new Dictionary<int, double>();

        private string m_Label;
        public string Label
        {
            get { return m_Label; }
            set { Set(ref m_Label, value); }
        }

        public byte[] Image => m_Recipe.Image;

        private string m_Description;
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        public ICommand ShowDescription { get; }

        private bool m_DescriptionIsVisible;
        public bool DescriptionIsVisible
        {
            get { return m_DescriptionIsVisible; }
            set { Set(ref m_DescriptionIsVisible, value); }
        }

        public ICommand ShowSteps { get; }

        public ICommand Recalculate { get; }

        private bool m_StepsAreVisible;
        public bool StepsAreVisible
        {
            get { return m_StepsAreVisible; }
            set { Set(ref m_StepsAreVisible, value); }
        }

        public ICommand ShowIngredients { get; }

        private bool m_IngredientsAreVisible;
        public bool IngredientsAreVisible
        {
            get { return m_IngredientsAreVisible; }
            set { Set(ref m_IngredientsAreVisible, value); }
        }

        public ObservableCollection<Model.RecipeStep> RecipeSteps { get; } = new ObservableCollection<Model.RecipeStep>();

        public ObservableCollection<Model.IngredientItem> RecipeIngredients { get; } = new ObservableCollection<Model.IngredientItem>();

        public RecipeViewModel()
        {
            IngredientsAreVisible = true;
            DescriptionIsVisible = StepsAreVisible = false;

            ShowDescription = new PropertyDependentCommand(this, _ => !DescriptionIsVisible, _ =>
            {
                DescriptionIsVisible = true;
                StepsAreVisible = IngredientsAreVisible = false;
            });

            ShowSteps = new PropertyDependentCommand(this, _ => !StepsAreVisible, _ =>
            {
                StepsAreVisible = true;
                DescriptionIsVisible = IngredientsAreVisible = false;
            });

            ShowIngredients = new PropertyDependentCommand(this, _ => !IngredientsAreVisible, _ =>
            {
                IngredientsAreVisible = true;
                DescriptionIsVisible = StepsAreVisible = false;
            });

            Recalculate = new PropertyDependentCommand(this, _ => true, parameter => DoRecalculate(parameter));
        }

        private void DoRecalculate(object parameter)
        {
            Model.IngredientItem updatedItem = parameter as Model.IngredientItem;
            if (updatedItem == null) return;

            var quantity = m_ItemValues[updatedItem.Id];

            if (quantity == updatedItem.Quantity) return;

            double multiplicator = updatedItem.Quantity / quantity;

            m_ItemValues[updatedItem.Id] = updatedItem.Quantity;
            foreach (var ingrident in RecipeIngredients)
            {
                if (ingrident.Id != updatedItem.Id)
                {
                    var newQuantity = multiplicator * ingrident.Quantity;
                    ingrident.Quantity = newQuantity;
                    m_ItemValues[ingrident.Id] = newQuantity;
                }
            }

            var arr = RecipeIngredients.ToArray();
            RecipeIngredients.Clear();
            foreach (var ingrident in arr)
            {
                RecipeIngredients.Add(ingrident);
            }
        }

        public void Load(int recipeId)
        {
            m_Recipe = Model.RecipesRepository.GetRecipe(recipeId);
            Label = m_Recipe.Title;
            Description = m_Recipe.Description;
            NotifyOfPropertyChange(nameof(Image));

            if (RecipeSteps.Any()) RecipeSteps.Clear();
            m_Recipe.Steps.OrderBy(step => step.Number).ToList().ForEach(step => RecipeSteps.Add(step));

            if (RecipeIngredients.Any()) RecipeIngredients.Clear();
            m_Recipe.IngredientItems.OrderBy(ingredient => ingredient.Title).ToList().ForEach(ingredient =>
            {
                RecipeIngredients.Add(ingredient);
                m_ItemValues[ingredient.Id] = ingredient.Quantity;
            });
        }
    }
}
