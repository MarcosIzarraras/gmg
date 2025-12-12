using GMG.Application.Common.Pagination;
using GMG.Application.Feactures.Purchases.Commands.CreatePurchase;
using GMG.Application.Feactures.Purchases.Queries.GetPurchasesPaginated;
using GMGv2.Common;
using GMGv2.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GMGv2.Controllers
{
    public class PurchasesController(IMediator mediator) : Controller
    {
        public IActionResult Index()
            => View();

        public IActionResult Create()
        {
            var viewModel = new PurchaseCreateViewModel();
            return View(viewModel);
        }

        public async Task<IActionResult> Pagination(Pagination pagination)
            => Ok(await mediator.Send(new GetPurchasesPaginatedQuery(pagination)));

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseCreateViewModel viewModel)
        {
            var result = await mediator.Send(new CreatePurchaseCommand(viewModel.Details));

            if (result.IsSuccess)
            {
                TempData[Notification.TypeKey] = Notification.Success;
                TempData[Notification.MessageKey] = "Purchase created successfully.";
                return RedirectToAction("Index");
            }

            TempData[Notification.TypeKey] = Notification.Error;
            TempData[Notification.MessageKey] = result.Error;
            return RedirectToAction("Create");
        }
    }
}
