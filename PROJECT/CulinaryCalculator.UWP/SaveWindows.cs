using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Windows.Storage;
using Windows.Storage.Pickers;
using CulinaryCalculator.UWP;
[assembly: Dependency(typeof(SaveWindows))]

namespace CulinaryCalculator.UWP
{
    public class SaveWindows : ISave
    {

        public async Task SavePdf(string fileName, MemoryStream stream)
        {
            StorageFile storageFile = null;
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            savePicker.SuggestedFileName = fileName;
            savePicker.FileTypeChoices.Add("Adobe PDF Document", new List<string>() { ".pdf" });

            storageFile = await savePicker.PickSaveFileAsync();
            if (storageFile == null) return;
            using (Stream outStream = await storageFile.OpenStreamForWriteAsync())
            {
                outStream.Write(stream.ToArray(), 0, (int)stream.Length);
            }

            await Windows.System.Launcher.LaunchFileAsync(storageFile);
        }
    }
}
