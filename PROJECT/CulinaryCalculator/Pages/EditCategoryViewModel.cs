using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class EditCategoryViewModel : ModalPageBaseViewModel
    {
        public event EventHandler<Category> CategorySaved;

        public ICommand GetImage { get; }

        public string Title { get; set; }

        private Category Category { get; set; }

        private byte[] m_Image;
        public byte[] Image
        {
            get { return m_Image; }
            set { Set(ref m_Image, value); }
        }

        public EditCategoryViewModel(INavigation navigation, Category category = null) : base(navigation)
        {
            GetImage = new PropertyDependentCommand(this, _ => true, _ => DoGetImage());
            if (category!=null)
            {
                Category = category;
                Title = category.Title;
                Image = category.Image;
            }
        }

        private async void DoGetImage()
        {
            Stream stream = await DependencyService.Get<IPhotoPicker>().GetImageStreamAsync();
            if (stream != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    Image = ms.ToArray();
                }
            }
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
            Category.Image = Image;

            if (isNew) RecipesRepository.AddCategory(Category);
            else RecipesRepository.UpdateCategory(Category);
            base.DoSave();
            CategorySaved?.Invoke(this, Category);
        }
    }
}
