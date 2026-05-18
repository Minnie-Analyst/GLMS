using GLMS.Data;
using GLMS.Services;
using Microsoft.EntityFrameworkCore;
using GLMS.Services; // (you’ll add this later for CurrencyService)

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<ContractService>();

// Add services to the container.
builder.Services.AddControllersWithViews();


// ADD THIS — DbContext (LocalDB)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<CurrencyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
