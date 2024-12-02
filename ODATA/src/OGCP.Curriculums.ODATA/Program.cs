using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using OGCP.Curriculums.DAL.Model;
using OGCP.Curriculums.ODATA.EDM;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(
    options => options.AddRouteComponents(
        "odata",
        new ProfileEntityDataModel().GetEntityDataModel()));

builder.Services.AddDbContext<ProfilesContext>(options =>
{
    options.UseSqlServer(
        @"Server=DESKTOP-U2F5GGD\SQLEXPRESS;Database=Profiles;Integrated Security=True;TrustServerCertificate=True;")
    .EnableSensitiveDataLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
