using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class EditRecipeViewModel : ModalPageBaseViewModel
    {
        private Action m_onSave;
        private readonly Model.Recipe m_Recipe;

        #region Step1
        public ObservableCollection<Category> Categories { get; }

        private Category m_SelectedCategory;
        public Category SelectedCategory
        {
            get => m_SelectedCategory;
            set { Set(ref m_SelectedCategory, value); }
        }

        public ICommand Next { get; set; }

        public ICommand GetImage { get; set; }

        private string m_RecipeTitle;
        public string RecipeTitle
        {
            get { return m_RecipeTitle; }
            set { Set(ref m_RecipeTitle, value); }
        }

        private string m_RecipeDescription;
        public string RecipeDescription
        {
            get { return m_RecipeDescription; }
            set { Set(ref m_RecipeDescription, value); }
        }

        private byte[] m_RecipeImage;
        public byte[] RecipeImage
        {
            get { return m_RecipeImage; }
            set { Set(ref m_RecipeImage, value); }
        }
        #endregion

        #region Step2

        private string m_IngredientToAdd;
        public string IngredientToAdd
        {
            get { return m_IngredientToAdd; }
            set { Set(ref m_IngredientToAdd, value); }
        }

        public ObservableCollection<UnitOfMeasure> Measures { get; set; }

        private UnitOfMeasure m_SelectedMeasure;

        public UnitOfMeasure SelectedMeasure
        {
            get { return m_SelectedMeasure; }
            set { Set(ref m_SelectedMeasure, value); }
        }

        private double m_Quantity;

        public double Quantity
        {
            get { return m_Quantity; }
            set { Set(ref m_Quantity, value); }
        }

        private IngredientItem m_SelectedIngredient;
        public IngredientItem SelectedIngredient
        {
            get { return m_SelectedIngredient; }
            set { Set(ref m_SelectedIngredient, value); }
        }

        public ObservableCollection<IngredientItem> Ingredients { get; } = new ObservableCollection<IngredientItem>();

        public ICommand AddIngredient { get; }

        public ICommand DeleteIngredient { get; }

        #endregion

        #region Step3
        public ObservableCollection<RecipeStep> RecipeSteps { get; } = new ObservableCollection<RecipeStep>();

        private RecipeStep m_SelectedStep;
        public RecipeStep SelectedStep
        {
            get { return m_SelectedStep; }
            set { Set(ref m_SelectedStep, value); }
        }

        private string m_StepToAdd;
        public string StepToAdd
        {
            get { return m_StepToAdd; }
            set { Set(ref m_StepToAdd, value); }
        }

        public ICommand AddStep { get; }
        public ICommand DeleteStep { get; }

        #endregion

        #region Steps
        private bool m_Step1IsVisible = true;
        public bool Step1IsVisible
        {
            get { return m_Step1IsVisible; }
            set { Set(ref m_Step1IsVisible, value); }
        }

        private bool m_Step2IsVisible;
        public bool Step2IsVisible
        {
            get { return m_Step2IsVisible; }
            set { Set(ref m_Step2IsVisible, value); }
        }

        private bool m_Step3IsVisible;
        public bool Step3IsVisible
        {
            get { return m_Step3IsVisible; }
            set { Set(ref m_Step3IsVisible, value); }
        }
        #endregion

        public EditRecipeViewModel(INavigation navigation, Action onSave, int? recipeId = null) : base(navigation)
        {
            m_onSave = onSave;
            Next = new PropertyDependentCommand(this, _ => true, _ => DoNext());
            Categories = new ObservableCollection<Category>(RecipesRepository.GetCategories());
            GetImage = new PropertyDependentCommand(this, _ => true, _ => DoGetImage());
            AddStep = new PropertyDependentCommand(this, _ => !string.IsNullOrWhiteSpace(StepToAdd), DoAddStep);
            DeleteStep = new PropertyDependentCommand(this, _ => SelectedStep != null, DoDeleteStep);
            AddIngredient = new PropertyDependentCommand(this, _ => !string.IsNullOrWhiteSpace(IngredientToAdd), DoAddIngredient);
            DeleteIngredient = new PropertyDependentCommand(this, _ => SelectedIngredient != null, DoDeleteIngredient);
            Measures = new ObservableCollection<UnitOfMeasure>(Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().ToList());

            if (recipeId == null)
            {
                m_Recipe = new Model.Recipe();
            }

            else
            {
                m_Recipe = RecipesRepository.GetRecipe(recipeId.Value);
                SelectedCategory = Categories.Where(c => c.Id == m_Recipe.Category.Id).First();
                RecipeDescription = m_Recipe.Description;
                RecipeImage = m_Recipe.Image;
                RecipeTitle = m_Recipe.Title;
                m_Recipe.Steps.ForEach(item => RecipeSteps.Add(item));
                m_Recipe.IngredientItems.ForEach(item => Ingredients.Add(item));
            }
        }

        protected override void DoSave()
        {
            m_Recipe.CategoryId = SelectedCategory.Id;
            m_Recipe.Description = RecipeDescription;
            m_Recipe.Image = RecipeImage;
            m_Recipe.Title = RecipeTitle;

            m_Recipe.Steps = new List<RecipeStep>();
            foreach (var step in RecipeSteps)
            {
                m_Recipe.Steps.Add(step);
            }

            m_Recipe.IngredientItems = new List<IngredientItem>();
            foreach (var item in Ingredients)
            {
                m_Recipe.IngredientItems.Add(item);
            }

            if (m_Recipe.Id > 0)
            {
                m_Recipe.Category = SelectedCategory;
                RecipesRepository.UpdateRecipe(m_Recipe);
            }
            else
            {
                RecipesRepository.AddRecipe(m_Recipe);
            }

            base.DoSave();
            m_onSave();
        }

        private async void DoGetImage()
        {
            Stream stream = await DependencyService.Get<IPhotoPicker>().GetImageStreamAsync();
            if (stream != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    RecipeImage = ms.ToArray();
                }
            }
        }

        private void DoNext()
        {
            if (Step1IsVisible)
            {
                Step1IsVisible = Step3IsVisible = false;
                Step2IsVisible = true;
            }
            else if (Step2IsVisible)
            {
                Step1IsVisible = Step2IsVisible = false;
                Step3IsVisible = true;
            }
        }

        private void DoAddStep(object _)
        {
            RecipeStep lastStep = RecipeSteps.LastOrDefault();
            var stepToAdd = new RecipeStep() { Description = StepToAdd, Number = (lastStep?.Number ?? 0) + 1, Recipe = m_Recipe };
            RecipeSteps.Add(stepToAdd);
            StepToAdd = string.Empty;          
        }
        private void DoDeleteStep(object _)
        {
            RecipeSteps.Remove(SelectedStep);
            CleanStepNumbers();
        }

        private void CleanStepNumbers()
        {
            var orderedSteps = RecipeSteps.OrderBy(s => s.Number).ToList();
            for (int i=0; i< orderedSteps.Count; i++)
            {
                orderedSteps[i].Number = i + 1;
            }
        }


        private void DoDeleteIngredient(object _)
        {
            Ingredients.Remove(SelectedIngredient);
        }

        private void DoAddIngredient(object _)
        {
            var ingredientToAdd = new IngredientItem() { Quantity = Quantity, Recipe = m_Recipe, Title = IngredientToAdd, Unit = SelectedMeasure };
            Ingredients.Add(ingredientToAdd);
            Quantity = 0;
            IngredientToAdd = string.Empty;
            SelectedMeasure = UnitOfMeasure.Gram;
        }
    }
}
