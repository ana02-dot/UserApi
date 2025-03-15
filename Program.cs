using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UserProfileAPI.Data;
using UserProfileAPI.Filters;
using UserProfileAPI.Interfaces;
using UserProfileAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Physical User Directory",
        Description = "A rest API for managing physical users and their relationship to others"

    });
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "UserProfileAPI.xml");
    c.IncludeXmlComments(xmlPath);

});


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRelationUserRepository, RelatedUserRepository>();
builder.Services.AddScoped<ValidationFilter>();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Profile API v1");

    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
