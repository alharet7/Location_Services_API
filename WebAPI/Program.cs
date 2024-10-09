using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WebAPI.Configurations.MappingConfugarations;
using WebAPI.Entities;
using WebAPI.Repositories;
using WebAPI.Repositories.FloorRepo;
using WebAPI.Repositories.LineRepo;
using WebAPI.Repositories.NodeRepo;
using WebAPI.Repositories.VenueRepo;
using WebAPI.Services.FloorService;
using WebAPI.Services.LineService;
using WebAPI.Services.NodeService;
using WebAPI.Services.VenueService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

// Mapster Configurations 
FloorMapping.ConfigureMappings();
LineMapping.ConfigureMappings();
NodeMapping.ConfigureMappings();
VenueMapping.ConfigureMappings();

// DI LifeTimes Configurations
builder.Services.AddScoped<IFloorRepository, FloorRepository>();
builder.Services.AddScoped<ILineRepository, LineRepository>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

// DI LifeTimes Services Configurations 
builder.Services.AddScoped<IFloorService, FloorService>();
builder.Services.AddScoped<ILineService, LineService>();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<IVenueService, VenueService>();


builder.Services.AddScoped<IGenericRepository, GenericRepository<ApplicationDbContext>>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
