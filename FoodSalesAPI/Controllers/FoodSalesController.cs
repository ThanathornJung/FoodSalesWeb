using FoodSalesAPI.Models;
using FoodSalesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FoodSalesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodSalesController : ControllerBase
    {
        private readonly FoodSalesService _service;

        public FoodSalesController(FoodSalesService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var foodSales = _service.GetAll();
            return Ok(foodSales);
        }

        [HttpPost]
        public IActionResult Create([FromBody] FoodSale newSale)
        {
            _service.Add(newSale);
            return CreatedAtAction(nameof(Get), new { id = newSale.Id }, newSale);
        }

        [HttpPut("{row}")]
        public IActionResult Update(int row, [FromBody] FoodSale updatedSale)
        {
            _service.Update(row, updatedSale);
            return NoContent();
        }

        [HttpDelete("{row}")]
        public IActionResult Delete(int row)
        {
            _service.Delete(row);
            return NoContent();
        }

        [HttpGet("sort")]
        public IActionResult Sort(string sortBy = "OrderDate", bool ascending = true)
        {
            var foodSales = _service.GetAll(sortBy, ascending);
            return Ok(foodSales);
        }

        [HttpGet("search")]
        public IActionResult Search(string searchTerm)
        {
            var foodSales = _service.Search(searchTerm);
            return Ok(foodSales);
        }

        [HttpGet("filter")]
        public IActionResult FilterByDate([FromQuery] string orderDate)
        {
            if (DateOnly.TryParse(orderDate, out var dateOnly))
            {
                var foodSales = _service.FilterByDate(dateOnly);
                return Ok(foodSales);
            }
            return BadRequest("Invalid date format.");
        }
    }
}