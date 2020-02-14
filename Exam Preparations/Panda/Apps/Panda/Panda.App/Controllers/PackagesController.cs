using Panda.App.ViewModels.Packages;
using Panda.Models;
using Panda.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;

namespace Panda.App.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackagesService packagesService;
        private readonly IUsersService usersService;

        public PackagesController(IPackagesService packagesService, IUsersService usersService)
        {
            this.packagesService = packagesService;
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var users = this.usersService.GetAllUsernames();

            return this.View(users);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            this.packagesService.Create(inputModel.Description, inputModel.Weight, inputModel.ShippingAddress, inputModel.RecipientName);

            return this.Redirect("/Packages/Pending");
        }

        [Authorize]
        public IActionResult Pending()
        {
            var packages = this.packagesService.GetAllByStatus(StatusPackage.Pending)
                .Select(x => new PackageViewModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username,
                }).ToList();

            return this.View(new PackageListViewModel { Packages = packages });
        }

        [Authorize]
        public IActionResult Delivered()
        {
            var packages = this.packagesService.GetAllByStatus(StatusPackage.Delivered)
                .Select(x => new PackageViewModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username,
                }).ToList();

            return this.View(new PackageListViewModel { Packages = packages });
        }

        [Authorize]
        public IActionResult Deliver(string id)
        {
            this.packagesService.Deliver(id);

            return this.Redirect("/Packages/Delivered");
        }
    }
}
