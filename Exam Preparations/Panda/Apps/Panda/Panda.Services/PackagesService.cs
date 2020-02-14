using Panda.Data;
using Panda.Models;
using System;
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

        public void Create(string description, decimal weight, string shippingAddress, string recipient)
        {
            var userId = dbContext.Users.Where(x => x.Username == recipient).Select(x => x.Id).FirstOrDefault();

            if (userId == null)
            {
                return;
            }

            var package = new Package
            {
                Description = description,
                Weight = weight,
                ShippingAddress = shippingAddress,
                Status = StatusPackage.Pending,
                EstimatedDeliveryDate = DateTime.UtcNow,
                RecipientId = userId
            };

            this.dbContext.Packages.Add(package);
            this.dbContext.SaveChanges();
        }

        public IQueryable<Package> GetAllByStatus(StatusPackage status)
        {
            var packages = this.dbContext.Packages.Where(x => x.Status == status);

            return packages;
        }

        public void Deliver(string id)
        {
            var package = this.dbContext.Packages.FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return;
            }

            package.Status = StatusPackage.Delivered;
            this.dbContext.SaveChanges();

            this.receiptsService.Create(package.Id, package.RecipientId, package.Weight);
        }
    }
}
