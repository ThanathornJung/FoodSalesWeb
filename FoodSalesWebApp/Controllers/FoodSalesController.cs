using FoodSalesAPI.Models;
using FoodSalesWebApp.Models;
using FoodSalesWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodSalesWebApp.Controllers
{
    public class FoodSalesController : Controller
    {
        private readonly FoodSalesApiService _service;

        public FoodSalesController(FoodSalesApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string searchTerm, DateOnly? orderDate)
        {
            List<FoodSale> foodSales;

            if (orderDate.HasValue)
            {
                foodSales = await _service.FilterByDateAsync(orderDate.Value);
            }
            else
            {
                foodSales = await _service.GetAllAsync();
            }

            var viewModel = foodSales.Select(fs => new FoodSaleViewModel
            {
                Id = fs.Id,
                OrderDate = fs.OrderDate,
                Region = fs.Region,
                City = fs.City,
                Category = fs.Category,
                Product = fs.Product,
                Quantity = fs.Quantity,
                UnitPrice = fs.UnitPrice,
                TotalPrice = fs.TotalPrice
            }).ToList();

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodSaleViewModel newSale)
        {
            var foodSale = new FoodSale
            {
                Id = newSale.Id,
                OrderDate = newSale.OrderDate,
                Region = newSale.Region,
                City = newSale.City,
                Category = newSale.Category,
                Product = newSale.Product,
                Quantity = newSale.Quantity,
                UnitPrice = newSale.UnitPrice,
                TotalPrice = newSale.TotalPrice
            };
            await _service.CreateAsync(foodSale);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int row)
        {
            var sale = await _service.GetByIdAsync(row);
            var viewModel = new FoodSaleViewModel
            {
                Id = sale.Id,
                OrderDate = sale.OrderDate,
                Region = sale.Region,
                City = sale.City,
                Category = sale.Category,
                Product = sale.Product,
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                TotalPrice = sale.TotalPrice
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int row, FoodSaleViewModel updatedSale)
        {
            var foodSale = new FoodSale
            {
                Id = updatedSale.Id,
                OrderDate = updatedSale.OrderDate,
                Region = updatedSale.Region,
                City = updatedSale.City,
                Category = updatedSale.Category,
                Product = updatedSale.Product,
                Quantity = updatedSale.Quantity,
                UnitPrice = updatedSale.UnitPrice,
                TotalPrice = updatedSale.TotalPrice
            };
            await _service.UpdateAsync(row, foodSale);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int row)
        {
            await _service.DeleteAsync(row);
            return RedirectToAction(nameof(Index));
        }
    }
}