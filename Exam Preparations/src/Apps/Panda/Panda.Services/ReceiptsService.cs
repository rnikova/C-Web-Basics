using Panda.Data;
using Panda.Data.Models;
using System;
using System.Linq;

namespace Panda.Services
{
    public class ReceiptsService : IReceiptsService
    {
        private readonly PandaDbContext dbContext;

        public ReceiptsService(PandaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateFromPackage(decimal weight, string packageId, string recipientId)
        {
            var receipt = new Receipt
            {
                PackageId = packageId,
                RecipientId = recipientId,
                Fee = weight * 2.67M,
                IssuedOn = DateTime.UtcNow,
            };

            this.dbContext.Receipts.Add(receipt);
            this.dbContext.SaveChanges();
        }

        public IQueryable<Receipt> GetAll()
        {
            return this.dbContext.Receipts;
        }
    }
}
