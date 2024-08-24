using Microsoft.EntityFrameworkCore;
using RelevancheSearchAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var constr = builder.Configuration.GetConnectionString("SearchDbContext");
builder.Services.AddDbContext<SearchDbContext>(x => x.UseSqlServer(constr));
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
