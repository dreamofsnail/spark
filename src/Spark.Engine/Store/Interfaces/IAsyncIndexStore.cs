
using Spark.Engine.Core;
using Spark.Engine.Model;
using System.Threading.Tasks;

namespace Spark.Engine.Store.Interfaces
{
    public interface IAsyncIndexStore
    {
        Task SaveAsync(IndexValue indexValue);
        Task DeleteAsync(Entry entry);
        Task CleanAsync();
    }
}