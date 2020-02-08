using Panda.Data;
using Panda.Data.Models;
using System.Linq;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly PandaDbContext dbContext;
        private readonly IReceiptsService receiptsService;

        public PackagesService(PandaDbContext dbContext, IReceiptsService receiptsService)
        {
            this.dbContext = dbContext;
            this.receiptsService = receiptsService;
        }

        public void Create(string description, decimal weight, string shippingAddress, string recipientName)
        {
            var userId = dbContext.Users.Where(x => x.Username == recipientName).Select(x => x.Id).FirstOrDefault();

            if (userId == null)
            {
                return;
            }

            var package = new Package()
            {
                Description = description,
                Weight = weight,
                Status = PackageStatus.Pending,
                ShippingAddress = shippingAddress,
                RecipientId = userId
            };

            this.dbContext.Packages.Add(package);
            this.dbContext.SaveChanges();
        }

        public void Deliver(string id)
        {
            var package = this.dbContext.Packages.FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return;
            }

            package.Status = PackageStatus.Delivered;
            this.dbContext.SaveChanges();

            this.receiptsService.CreateFromPackage(package.Weight, package.Id, package.RecipientId);
        }

        public IQueryable<Package> GetAllByStatus(PackageStatus status)
        {
            var packages = this.dbContext.Packages.Where(x => x.Status == status); 

            return packages;
        }
    }
}
