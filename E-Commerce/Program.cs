using E_Commerce.Data;
using E_Commerce.Repositories;
using E_Commerce.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IContactUsRepo, ContactUsRepo>();
builder.Services.AddScoped<IContactUsService, ContactUsService>();

builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IOrderStatusRepo, OrderStatusRepo>();
builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Enable session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
