using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using OGCP.Curriculums.DAL.Model;

namespace OGCP.Curriculums.ODATA.Controllers;

public class ProfilesController : ODataController
{
    private readonly ProfilesContext context;

    public ProfilesController(ProfilesContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await context.Profiles.ToListAsync());
    }
}
