using Panda.Models;
using System.Linq;

namespace Panda.Services
{
    public interface IPackagesService
    {
        void Create(string description, decimal weight, string shippingAddress, string recipient);

        IQueryable<Package> GetAllByStatus(StatusPackage status);

        void Deliver(string id);
    }
}
