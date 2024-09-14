using FoodSalesAPI.Models;
using OfficeOpenXml;
using System.Globalization;

namespace FoodSalesAPI.Services
{
    public class FoodSalesService
    {
        private readonly string _filePath;

        public FoodSalesService(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(environment.ContentRootPath, "Data", "Food sales.xlsx");
        }

        public List<FoodSale> GetAll()
        {
            using var package = new ExcelPackage(new FileInfo(_filePath));
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                throw new InvalidOperationException("No worksheets found in the Excel file.");
            }

            var foodSales = new List<FoodSale>();

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                try
                {
                    foodSales.Add(new FoodSale
                    {
                        Id = row - 1,
                        OrderDate = DateOnly.Parse(worksheet.Cells[row, 1].Text),
                        Region = worksheet.Cells[row, 2].Text,
                        City = worksheet.Cells[row, 3].Text,
                        Category = worksheet.Cells[row, 4].Text,
                        Product = worksheet.Cells[row, 5].Text,
                        Quantity = int.Parse(worksheet.Cells[row, 6].Text),
                        UnitPrice = decimal.Parse(worksheet.Cells[row, 7].Text),
                        TotalPrice = decimal.Parse(worksheet.Cells[row, 8].Text)
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing row {row}: {ex.Message}");
                }
            }

            return foodSales;
        }
        public void Add(FoodSale foodSale)
        {
            using var package = new ExcelPackage(new FileInfo(_filePath));
            var worksheet = package.Workbook.Worksheets[0];
            var row = worksheet.Dimension.Rows + 1;
            worksheet.Cells[row, 1].Value = foodSale.OrderDate;
            worksheet.Cells[row, 2].Value = foodSale.Region;
            worksheet.Cells[row, 3].Value = foodSale.City;
            worksheet.Cells[row, 4].Value = foodSale.Category;
            worksheet.Cells[row, 5].Value = foodSale.Product;
            worksheet.Cells[row, 6].Value = foodSale.Quantity;
            worksheet.Cells[row, 7].Value = foodSale.UnitPrice;
            worksheet.Cells[row, 8].Value = foodSale.TotalPrice;

            package.Save();
        }

        public void Update(int row, FoodSale updatedSale)
        {
            using var package = new ExcelPackage(new FileInfo(_filePath));
            var worksheet = package.Workbook.Worksheets[0];

            worksheet.Cells[row, 1].Value = updatedSale.OrderDate;
            worksheet.Cells[row, 2].Value = updatedSale.Region;
            worksheet.Cells[row, 3].Value = updatedSale.City;
            worksheet.Cells[row, 4].Value = updatedSale.Category;
            worksheet.Cells[row, 5].Value = updatedSale.Product;
            worksheet.Cells[row, 6].Value = updatedSale.Quantity;
            worksheet.Cells[row, 7].Value = updatedSale.UnitPrice;
            worksheet.Cells[row, 8].Value = updatedSale.TotalPrice;

            package.Save();
        }

        public void Delete(int row)
        {
            using var package = new ExcelPackage(new FileInfo(_filePath));
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                throw new InvalidOperationException("No worksheets found in the Excel file.");
            }

            if (row >= 2 && row <= worksheet.Dimension.Rows)
            {
                worksheet.DeleteRow(row);
                package.Save();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row index out of range.");
            }
        }

        public List<FoodSale> GetAll(string sortBy = "OrderDate", bool ascending = true)
        {
            var foodSales = GetAll();
            return ascending ?
                foodSales.OrderBy(f => GetValueByProperty(f, sortBy)).ToList() :
                foodSales.OrderByDescending(f => GetValueByProperty(f, sortBy)).ToList();
        }

        private object GetValueByProperty(FoodSale sale, string propertyName)
        {
            var prop = typeof(FoodSale).GetProperty(propertyName);
            return prop != null ? prop.GetValue(sale) : null;
        }

        public List<FoodSale> Search(string searchTerm)
        {
            return GetAll().Where(f =>
                f.Region.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                f.City.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                f.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                f.Product.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<FoodSale> FilterByDate(DateOnly orderDate)
        {
            return GetAll().Where(f => f.OrderDate == orderDate).ToList();
        }
    }
}

