using FoodSalesWebApp.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient
builder.Services.AddHttpClient<FoodSalesApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});

builder.Services.AddScoped<FoodSalesApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FoodSales}/{action=Index}/{id?}");

app.Run();
