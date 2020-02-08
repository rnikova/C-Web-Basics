using Panda.Data.Models;
using Panda.Services;
using Panda.Web.ViewModels.Packages;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;

namespace Panda.Web.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackagesService packageService;
        private readonly IUsersService usersService;

        public PackagesController(IPackagesService packageService, IUsersService usersService)
        {
            this.packageService = packageService;
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var packages = this.usersService.GetUsernames();

            return this.View(packages);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            this.packageService.Create(model.Description, model.Weight, model.ShippingAddress, model.RecipientName);

            return this.Redirect("/Packages/Pending");
        }

        [Authorize]
        public IActionResult Delivered()
        {
            var packages = this.packageService
                .GetAllByStatus(PackageStatus.Delivered)
                .Select(x => new PackageViewModel
                {
                    Description = x.Description,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username,
                    Id = x.Id
                })
                .ToList();

            return this.View(new PackagesListViewModel { Packages = packages });
        }

        [Authorize]
        public IActionResult Pending()
        {
            var packages = this.packageService
                .GetAllByStatus(PackageStatus.Pending)
                .Select(x => new PackageViewModel
                {
                    Description = x.Description,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username,
                    Id = x.Id
                })
                .ToList();

            return this.View(new PackagesListViewModel { Packages = packages });
        }

        [Authorize]
        public IActionResult Deliver(string id)
        {
            this.packageService.Deliver(id);
            return this.Redirect("/Packages/Delivered");
        }
    }
}
