using Panda.Models;
using System.Linq;

namespace Panda.Services
{
    public interface IReceiptsService
    {
        IQueryable<Receipt> GetAll();

        void Create(string packageId, string recipientId, decimal weight);
    }
}
