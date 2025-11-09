using AlzaTest.Messaging;
using AlzaTest.Seed;
using AlzaTest.Workers;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Abstraction;
using Services.Abstraction;
using Services.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerPerApiVersion>();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("TestDb"));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IStockUpdateQueue, StockUpdateQueue>();
builder.Services.AddHostedService<StockUpdateProcessor>();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();

app.UseSwaggerUI(o =>
{
    o.RoutePrefix = "docs";
    o.DocumentTitle = "AlzaTest – Multi-Version API";
    foreach (var d in provider.ApiVersionDescriptions)
    {
        o.SwaggerEndpoint($"/swagger/{d.GroupName}/swagger.json",
            $"AlzaTest {d.GroupName.ToUpperInvariant()}");
    }
});

//Data seed
await app.Services.EnsureInitialSeed();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();


public sealed class ConfigureSwaggerPerApiVersion
    : Microsoft.Extensions.Options.IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    public ConfigureSwaggerPerApiVersion(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
    {
        var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = "AlzaTest",
                Version = desc.ApiVersion.ToString(),
                Description = desc.IsDeprecated ? "Deprecated version" : "Current version"
            });
        }
    }
}
