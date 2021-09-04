using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class EditFilterViewModel : ModalPageBaseViewModel, IConvertible
    {
        private string m_IngredientToAdd;
        private string m_SelectedIngredient;
        public event Action<IEnumerable<Category>, IEnumerable<string>, string> FilterChanged;
        public ICommand AddIngredient { get; }
        public ICommand DeleteIngredient { get; }

        public EditFilterViewModel(INavigation navigation) : base(navigation)
        {
            AddIngredient = new PropertyDependentCommand(this, _ => !string.IsNullOrEmpty(IngredientToAdd), _ => { DoAddIngredient(); });
            DeleteIngredient = new PropertyDependentCommand(this, _ => !string.IsNullOrEmpty(SelectedIngredient), _ => { DoDeleteIngredient(); });
            Categories = new ObservableCollection<object>(RecipesRepository.GetCategories());
        }

        public ObservableCollection<object> Categories { get; }
        public ObservableCollection<object> SelectedCategories { get; set; } = new ObservableCollection<object>();

        public string IngredientToAdd
        {
            get => m_IngredientToAdd;
            set => Set(ref m_IngredientToAdd, value);
        }

        public string SelectedIngredient
        {
            get => m_SelectedIngredient;
            set => Set(ref m_SelectedIngredient, value);
        }
        public ObservableCollection<string> Ingredients { get; set; } = new ObservableCollection<string>();
        public string Substring { get; set; }
        protected override void DoSave()
        {
            FilterChanged?.Invoke(SelectedCategories.Cast<Category>(), Ingredients, Substring);
            base.DoSave();
        }
        private void DoAddIngredient()
        {
            Ingredients.Add(IngredientToAdd);
            IngredientToAdd = string.Empty;
        }

        private void DoDeleteIngredient()
        {
            Ingredients.Remove(SelectedIngredient);
        }

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
