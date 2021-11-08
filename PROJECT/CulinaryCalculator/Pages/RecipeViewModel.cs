using CulinaryCalculator.Infrastructure;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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

        public ICommand SendRecipe { get; }

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

            SendRecipe = new PropertyDependentCommand(this, _ => true, _ => DoSendRecipe());
        }

        private async void DoSendRecipe()
        {
            PdfDocument document = new PdfDocument();
            PdfFont fontForLabel = new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, 20);
            PdfFont font = new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, 14);
            float yPosition = 0;

            // page 1
            PdfPage page = document.Pages.Add();
            page.Graphics.DrawString(Label, fontForLabel, PdfBrushes.Black, new PointF(0, yPosition));
            yPosition += 30;

            if(Image != null)
            {
                using (var ms = new MemoryStream(Image))
                {
                    PdfBitmap bitmap = new PdfBitmap(ms);
                    page.Graphics.DrawImage(bitmap, 0, yPosition);
                    yPosition += bitmap.Height / 1.2f;
                }
            }

            PdfTextElement element = new PdfTextElement(Description);

            element.Brush = new PdfSolidBrush(Color.Black);
            element.Font = font;

            PdfLayoutFormat layoutFormat = new PdfLayoutFormat();
            layoutFormat.Break = PdfLayoutBreakType.FitPage;

            PdfStringFormat format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Left;
            format.LineAlignment = PdfVerticalAlignment.Top;
            element.StringFormat = format;

            RectangleF bounds = new RectangleF(0, yPosition, page.Graphics.ClientSize.Width, page.Graphics.ClientSize.Height - yPosition);
            element.Draw(page, bounds, layoutFormat);


            // table style
            PdfCellStyle cellStyle = new PdfCellStyle();
            cellStyle.BorderPen = new PdfPen(Color.LightGray, 0.1f);
            PdfLightTableStyle pdfLightTableStyle = new PdfLightTableStyle();
            pdfLightTableStyle.DefaultStyle = cellStyle;
            pdfLightTableStyle.DefaultStyle.Font = font;
            pdfLightTableStyle.CellPadding = 4;
            pdfLightTableStyle.BorderPen = new PdfPen(Color.LightGray, 0.1f);

            // page 2
            PdfPage page2 = document.Pages.Add();
            page2.Graphics.DrawString("Шаги", fontForLabel, PdfBrushes.Black, new PointF(0, 0));
            yPosition = 30;

            PdfLightTable pdfLightTable = new PdfLightTable();
            pdfLightTable.Style = pdfLightTableStyle;

            DataTable table = new DataTable();
            table.Columns.Add("№");
            table.Columns.Add("Шаг");
            var orderedSteps = RecipeSteps.OrderBy(s => s.Number).ToList();
            for (int i = 0; i < orderedSteps.Count; i++)
            {
                table.Rows.Add(new string[] { (i + 1).ToString(), orderedSteps[i].Description });
            }
            pdfLightTable.DataSource = table;
            pdfLightTable.Columns[0].Width = 30;
            pdfLightTable.Draw(page2, new PointF(0, yPosition));

            // page 3
            PdfPage page3 = document.Pages.Add();
            page3.Graphics.DrawString("Ингридиенты", fontForLabel, PdfBrushes.Black, new PointF(0, 0));
            yPosition = 30;

            PdfLightTable pdfLightTable2 = new PdfLightTable();
            pdfLightTable2.Style = pdfLightTableStyle;

            DataTable table2 = new DataTable();
            table2.Columns.Add("№");
            table2.Columns.Add("Ингридиент");
            table2.Columns.Add("Количество");
            var converter = new UnitOfMeasureConverter();
            var orderedIngredients = RecipeIngredients.OrderBy(i => i.Title).ToList();
            for (int i = 0; i < orderedIngredients.Count; i++)
            {
                table2.Rows.Add(new string[] {
                    (i+1).ToString(),
                    orderedIngredients[i].Title,
                    $"{orderedIngredients[i].Quantity.ToString()} {converter.Convert(orderedIngredients[i].Unit, typeof(string), null, CultureInfo.CurrentCulture)}",
                    
                });
            }
            pdfLightTable2.DataSource = table2;
            pdfLightTable2.Columns[0].Width = 30;
            pdfLightTable2.Columns[2].Width = 100;
            pdfLightTable2.Draw(page3, new PointF(0, yPosition));

            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);

            await Xamarin.Forms.DependencyService.Get<ISave>().SavePdf("Output.pdf", stream);
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
