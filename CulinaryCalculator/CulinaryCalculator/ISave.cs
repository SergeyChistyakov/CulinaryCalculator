using System.IO;
using System.Threading.Tasks;

namespace CulinaryCalculator
{
    public interface ISave
    {
        Task SavePdf(string fileName, MemoryStream stream);
    }
}
