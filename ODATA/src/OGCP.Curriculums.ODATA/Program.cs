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

//UseRouting adds routing matching to the midleware pipeline
//Looks at the set endpoint defined and select the best matches to the request 
app.UseHttpsRedirection();

app.UseAuthorization();

//UseEndpoint adds endpoint execution to the midleware pipeline
//We add mappings to our controller actions
app.MapControllers();

app.Run();
