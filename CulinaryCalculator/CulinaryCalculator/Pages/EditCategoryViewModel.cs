using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class EditCategoryViewModel : ModalPageBaseViewModel
    {
        public ICommand GetImage { get; }

        public string Title { get; set; }

        private Category Category { get; set; }

        public EditCategoryViewModel(INavigation navigation) : base(navigation)
        {
            GetImage = new PropertyDependentCommand(this, _ => true,
               // platform dependant, https://docs.microsoft.com/ru-ru/xamarin/xamarin-forms/app-fundamentals/dependency-service/photo-picker
               _ => throw new NotImplementedException());
        }

        protected override void DoSave()
        {
            bool isNew = false;
            if (Category == null)
            {
                isNew = true;
                Category = new Category();
            }
            Category.Title = Title;
            if (isNew) RecipesRepository.AddCategory(Category);
            else RecipesRepository.UpdateCategory(Category);
            base.DoSave();
        }
    }
}
