using Microsoft.AspNetCore.Mvc.Formatters; // To use IOutputFormatter
using Microsoft.Extensions.Caching.Memory; // to use IMemoryCache and so on
using Northwind.EntityModels; //To use NorthwindContext
using Northwind.WebApi.Repositories;// To use ICustomerRepository and CustomerRespository
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.HttpLogging; // To use HttpLoggingField

const string corsPolicyName = "allowWasmClient";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096; // default is 3k
    options.ResponseBodyLogLimit = 4096; // Default is 3k

});
builder.Services.AddControllers(options =>
{
    WriteLine("Default output formatters");
    foreach(IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;

        if (mediaFormatter is null)
        {
            WriteLine(formatter.GetType().Name);
        }
        else
        {
            WriteLine("{0}, media type: {1}",
                mediaFormatter.GetType().Name,
                string.Join(", ", mediaFormatter.SupportedMediaTypes));
        }
    }
}).AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
    {
        policy.AllowAnyHeader();
        policy.WithOrigins("https://localhost:5161",
        "http://localhost:5160");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNorthwindContext();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind Service API Version 1");
        c.SupportedSubmitMethods([SubmitMethod.Get, SubmitMethod.Put, SubmitMethod.Post, SubmitMethod.Delete]);
    });
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
