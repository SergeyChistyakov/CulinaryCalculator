using Android.Content;
using Xamarin.Forms;
using CulinaryCalculator.Droid;
using CulinaryCalculator.Model;

[assembly: Dependency(typeof(AndroidPath))]
namespace CulinaryCalculator.Droid
{
    public class AndroidPath : IPath
    {
        public string GetDatabaseFolder()
        {
            Context context = Android.App.Application.Context;
            return context.GetExternalFilesDir("").AbsolutePath;
        }
    }
}