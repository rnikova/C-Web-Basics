using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.BindingModels.Products
{
    public class ProductsOrderBindingModel
    {
        [RequiredSis]
        public string Product { get; set; }
    }
}
