using GMG.Application.Common.Paginations;
using GMG.Application.Feactures.Products.Queries.GetProducts;
using GMG.Application.Feactures.Products.Queries.GetProductTypes;
using GMG.Application.Feactures.Sales.Commands.CreateSale;
using GMG.Application.Feactures.Sales.Queries.GetSalesPaginated;
using GMGv2.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GMGv2.Controllers
{
    public class SalesController(IMediator mediator) : Controller
    {
        public IActionResult Index()
            => View();

        public async Task<IActionResult> Create(){
            var productTypes = await mediator.Send(new GetProductTypesQuery());
            var products = await mediator.Send(new GetProductsPosQuery());
            return View(new Models.SaleCreateViewModel {
                ProductTypes = productTypes,
                Products = products
            });
        }

        public async Task<IActionResult> Pagination(Pagination pagination)
            => Json(await mediator.Send(new GetSalesPaginatedQuery(pagination)));

        [HttpPost]
        public async Task<IActionResult> Create(SaleCreateViewModel model)
        {
            if (model == null || model.Details == null || !model.Details.Any())
            {
                return BadRequest(new { message = "No sale details provided" });
            }

            var result = await mediator.Send(new CreateSaleCommand(model.Details, model.CustomerId));
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
