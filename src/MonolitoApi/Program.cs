// using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<MonolitoApi.Settings.FileImageStorageData>(builder.Configuration.GetSection("ImageStoraDatabase"));
builder.Services.Configure<MonolitoApi.Settings.DbContextConnection>(builder.Configuration.GetSection("DbContextConnection"));
builder.Services.AddDbContext<MonolitoApi.MonolitoDbContext>();
builder.Services.AddSingleton<MonolitoApi.Data.ImageData>();
builder.Services.AddScoped<MonolitoApi.Models.FileImage>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
