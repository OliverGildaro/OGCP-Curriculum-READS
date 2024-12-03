using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using OGCP.Curriculums.DAL.Model;
using OGCP.Curriculums.ODATA.EDM;
using System.Reflection.Emit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(10).AddRouteComponents(
    "odata", new ProfileEntityDataModel().GetEntityDataModel()));

builder.Services.AddDbContext<ProfilesContext>(options =>
{
    options.UseSqlServer(
        @"Server=DESKTOP-U2F5GGD\SQLEXPRESS;Database=Profiles;Integrated Security=True;TrustServerCertificate=True;")
    .EnableSensitiveDataLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

//UseRouting adds routing matching to the midleware pipeline
//Looks at the set endpoint defined and select the best matches to the request 
app.UseRouting();

//UseEndpoint adds endpoint execution to the midleware pipeline
//We add mappings to our controller actions
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
