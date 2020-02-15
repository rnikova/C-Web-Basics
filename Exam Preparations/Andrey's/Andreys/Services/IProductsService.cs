using Andreys.Models;
using Andreys.ViewModels.Products;
using System.Collections.Generic;

namespace Andreys.Services
{
    public interface IProductsService
    {
        int Add(ProductsAddInputModel productAddInputModel);

        IEnumerable<Product> GetAll();

        Product GetById(int id);

        void DeleteById(int id);
    }
}
