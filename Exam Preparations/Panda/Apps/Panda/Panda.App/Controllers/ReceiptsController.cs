using Panda.App.ViewModels.Receipts;
using Panda.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;

namespace Panda.App.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsService receiptService;

        public ReceiptsController(IReceiptsService receiptService)
        {
            this.receiptService = receiptService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var viewModel = this.receiptService.GetAll().Select(
                x => new ReceiptsViewModel
                {
                    Id = x.Id,
                    Fee = x.Fee,
                    IssuedOn = x.IssuedOn,
                    RecipientName = x.Recipient.Username,
                }).ToList();

            return this.View(viewModel);
        }
    }
}
