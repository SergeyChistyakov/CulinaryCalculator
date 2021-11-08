using Xamarin.Forms;
using CulinaryCalculator.UWP;
using System.IO;
using CulinaryCalculator.Model;

[assembly: Dependency(typeof(UWPPath))]
namespace CulinaryCalculator.UWP
{
    public class UWPPath : IPath
    {
        public string GetDatabaseFolder()
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        }
    }
}
