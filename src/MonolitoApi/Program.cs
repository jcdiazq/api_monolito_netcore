// using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
const string TITLE = "Monolito Pragma";
const string VERSION = "v1";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<MonolitoApi.Settings.FileImageStorageData>(builder.Configuration.GetSection("ImageStoraDatabase"));
builder.Services.Configure<MonolitoApi.Settings.DbContextConnection>(builder.Configuration.GetSection("DbContextConnection"));
builder.Services.AddDbContext<MonolitoApi.MonolitoDbContext>();
builder.Services.AddScoped<MonolitoApi.Data.ImageData>();
builder.Services.AddScoped<MonolitoApi.Models.FileImage>();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc(VERSION, new OpenApiInfo { Title = TITLE, Version = VERSION });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint($"/swagger/{VERSION}/swagger.json", TITLE);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
