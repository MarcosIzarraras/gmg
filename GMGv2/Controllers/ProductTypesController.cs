using GMG.Application.Common.Pagination;
using GMG.Application.Feactures.Products.Commands.CreateProductType;
using GMG.Application.Feactures.Products.Queries.GetProductTypeById;
using GMG.Application.Feactures.Products.Queries.GetProductTypesPaginated;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Threading.Tasks;

namespace GMGv2.Controllers
{
    public class ProductTypesController(IMediator mediator) : Controller
    {
        public async Task<IActionResult> Index()
            =>  View();

        [HttpPost]
        public async Task<IActionResult> Index(SaveProductTypeCommand command)
        {
            var result = await mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Pagination(Pagination pagination)
            => Json(await mediator.Send(new GetProductTypesPaginatedQuery(pagination)));

        public async Task<IActionResult> PartialTable(Pagination pagination)
            => PartialView("Partials/ProductTypesTable", await mediator.Send(new GetProductTypesPaginatedQuery(pagination)));

        public async Task<IActionResult> PartialForm(Guid? id)
        {
            if (id is null)
                return PartialView("Partials/ProductTypeForm", new SaveProductTypeCommand(null, string.Empty));

            var result = await mediator.Send(new GetProductTypeByIdQuery(id.Value));
            return result is not null ? PartialView("Partials/ProductTypeForm", new SaveProductTypeCommand(result.Id, result.Name)) : NotFound();
        }
    }
}
