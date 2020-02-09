using Musaca.Data;
using Musaca.Models;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.Services
{
    public class ProductsService : IProductsService
    {
        private readonly MusacaDbContext context;

        public ProductsService(MusacaDbContext dbContext)
        {
            this.context = dbContext;
        }

        public Product CreateProduct(Product product)
        {
            product = this.context.Products.Add(product).Entity;
            this.context.SaveChanges();

            return product;
        }

        public List<Product> GetAll()
        {
            return this.context.Products.ToList();
        }

        public Product GetByName(string name)
        {
            return this.context.Products.SingleOrDefault(product => product.Name == name);
        }
    }
}
