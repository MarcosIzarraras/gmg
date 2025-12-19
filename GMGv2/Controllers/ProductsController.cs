using GMG.Application.Common.Dtos;
using GMG.Application.Common.Interfaces;
using GMG.Application.Common.Paginations;
using GMG.Application.Feactures.Products.Commands.CreateProduct;
using GMG.Application.Feactures.Products.Commands.UpdateProduct;
using GMG.Application.Feactures.Products.Queries.GetProductById;
using GMG.Application.Feactures.Products.Queries.GetProductsPaginated;
using GMG.Application.Feactures.Products.Queries.GetProductTypeById;
using GMG.Application.Feactures.Products.Queries.GetProductTypes;
using GMG.Domain.Products.Entities;
using GMGv2.Common;
using GMGv2.Extensions;
using GMGv2.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GMGv2.Controllers
{
    [Authorize]
    public class ProductsController(IMediator mediator, IWebHostEnvironment webHostEnvironment, IUserContext userContext) : Controller
    {
        public async Task<ActionResult> Index(Pagination pagination)
            => View(await mediator.Send(new GetProductsPaginatedQuery(pagination)));

        public async Task<IActionResult> Pagination(Pagination pagination)
            => Json(await mediator.Send(new GetProductsPaginatedQuery(pagination)));

        public async Task<IActionResult> PartialTable(Pagination pagination)
            => PartialView("Partials/ProductsTable", await mediator.Send(new GetProductsPaginatedQuery(pagination)));

        public async Task<IActionResult> Create() {
            var productTypes = await mediator.Send(new GetProductTypesQuery());
            var viewModel = new ProductCreateViewModel
            {
                ProductTypes = productTypes.ToSelectListItems(x => x.Id.ToString(), x => x.Name)
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var productTypes = await mediator.Send(new GetProductTypesQuery());
            var product = await mediator.Send(new GetProductByIdQuery(id));
            
            if (product is null)
                return NotFound();

            var viewModel = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ProductTypeId = product.ProductTypeId,
                ProductTypes = productTypes.ToSelectListItems(x => x.Id.ToString(), x => x.Name),
                Images = product.ProductImages.Select(i => new ProductImageViewModel() { Id = i.Id.ToString(), Path = i.Path }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel productCreateViewModel)
        {
            List<FileUploadDto> imagesDto = new();
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "products", userContext.OwnerId.ToString());

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            if (productCreateViewModel.Images != null)
            {
                foreach (var image in productCreateViewModel.Images)
                {
                    imagesDto.Add(new FileUploadDto
                    (
                        image.FileName,
                        image.ContentType,
                        image.Length,
                        image.OpenReadStream()
                    ));
                }
            }


            var result = await mediator.Send(new CreateProductCommand(
                productCreateViewModel.Name,
                productCreateViewModel.Description,
                productCreateViewModel.Price,
                productCreateViewModel.InitialStock,
                productCreateViewModel.ProductTypeId, 
                imagesDto));

            if (result.IsSuccess)
            {
                TempData[Notification.TypeKey] = Notification.Success;
                TempData[Notification.MessageKey] = "Product created successfully.";
                return RedirectToAction("Index");
            }

            TempData[Notification.TypeKey] = Notification.Error;
            TempData[Notification.MessageKey] = result.Error;
            return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDetailViewModel productDetailViewModel)
        {


            var result = await mediator.Send(new UpdateProductCommand(
                productDetailViewModel.Id,
                productDetailViewModel.Name,
                productDetailViewModel.Description,
                productDetailViewModel.Price,
                productDetailViewModel.Stock,
                productDetailViewModel.ProductTypeId));

            if (result.IsSuccess)
            {
                TempData[Notification.TypeKey] = Notification.Success;
                TempData[Notification.MessageKey] = "Product updated successfully.";
                return RedirectToAction("Index");
            }

            TempData[Notification.TypeKey] = Notification.Error;
            TempData[Notification.MessageKey] = result.Error;
            return RedirectToAction("Detail", new { Id = productDetailViewModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            var pagination = new Pagination
            {
                Page = 1,
                PageSize = 10,
                Search = search
            };

            var result = await mediator.Send(new GetProductsPaginatedQuery(pagination));

            var products = result.Items.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price
            });

            return Json(products);
        }
    }
}
