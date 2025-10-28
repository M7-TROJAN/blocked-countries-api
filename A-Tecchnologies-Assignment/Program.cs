using A_Tecchnologies_Assignment.BackgroundJobs;
using A_Tecchnologies_Assignment.Repositories;
using A_Tecchnologies_Assignment.Services;
using A_Tecchnologies_Assignment.Settings;
using FluentValidation;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "A-Technologies Assignment For Dotnet Developer",
        Version = "v1",
        Description = @"This API allows managing blocked countries and validating IP addresses 
                        using external GeoLocation provider (ipapi.com). 
                        It supports permanent and temporary country blocking, IP checking, 
                        and logging of blocked access attempts.",
        Contact = new OpenApiContact
        {
            Name = "Mahmoud Mohamed",
            Email = "mahmoud.abdalaziz@outlook.com",
            Url = new Uri("https://github.com/M7-TROJAN/blocked-countries-Api")
        }
    });

    // organize endpoints by controller name
    options.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    options.DocInclusionPredicate((name, api) => true);

    // Include XML Comments in Swagger UI
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOptions<GeoLocationSettings>()
    .BindConfiguration(GeoLocationSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart(); // متشغلش البرنامج كله اصلا لو فيه حاجة غلط

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IBlockedRepository, InMemoryBlockedRepository>();
builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddHostedService<TemporalBlockCleanupService>();
builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();


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