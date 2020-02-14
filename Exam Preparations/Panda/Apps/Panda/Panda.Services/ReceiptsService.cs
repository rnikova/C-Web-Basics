using Panda.Data;
using Panda.Models;
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

        public void Create(string packageId, string recipientId, decimal weight)
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
