using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using OGCP.Curriculums.DAL.Model;
using OGCP.Curriculums.ODATA.Helpers;

namespace OGCP.Curriculums.ODATA.Controllers;

[Route("odata")]
public class ProfilesController : ODataController
{
    private readonly ProfilesContext context;

    public ProfilesController(ProfilesContext context)
    {
        this.context = context;
    }

    //Here we are using the odata routing templates "profiles" to follow up the standard
    //This only works for odata
    //The attribute base routing is the best approach for apis
    [HttpGet("profiles")]
    public async Task<IActionResult> Get()
    {
        return Ok(await context.Profiles.ToListAsync());
    }

    [HttpGet("profiles({id})")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await context.Profiles.FirstOrDefaultAsync(p => p.Id == id));
    }

    [HttpGet("profiles({id})/Summary")]
    [HttpGet("profiles({id})/FirstName")]
    [HttpGet("profiles({id})/LastName")]
    public async Task<IActionResult> GetPersonProperty(int id)
    {
        var person = await context.Profiles
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var propertyToGet = new Uri(HttpContext.Request.GetEncodedUrl()).Segments.Last();

        if (!person.HasProperty(propertyToGet))
        {
            return NotFound();
        }

        var propertyValue = person.GetValue(propertyToGet);

        if (propertyValue == null)
        {
            // null = no content
            return NoContent();
        }

        return Ok(propertyValue);
    }

    [HttpGet("profiles({id})/Summary/$value")]
    [HttpGet("profiles({id})/FirstName/$value")]
    [HttpGet("profiles({id})/LastName/$value")]
    public async Task<IActionResult> GetPersonPropertyRawValue(int id)
    {
        var person = await context.Profiles
          .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var url = HttpContext.Request.GetEncodedUrl();
        var propertyToGet = new Uri(url).Segments[^2].TrimEnd('/');

        if (!person.HasProperty(propertyToGet))
        {
            return NotFound();
        }

        var propertyValue = person.GetValue(propertyToGet);

        if (propertyValue == null)
        {
            // null = no content
            return NoContent();
        }

        return Ok(propertyValue.ToString());
    }

    [HttpGet("profiles({id})/Languages")]
    [HttpGet("profiles({id})/educations")]
    [HttpGet("profiles({id})/jobExperiences")]
    public IActionResult GetPersonCollectionProperty(int id)
    {
        var collectionPopertyToGet = new Uri(HttpContext.Request.GetEncodedUrl())
            .Segments.Last();

        var profile = context.Profiles
              .Include(collectionPopertyToGet)
              .FirstOrDefault(p => p.Id == id);

        if (profile == null)
        {
            return NotFound();
        }

        if (!profile.HasProperty(collectionPopertyToGet))
        {
            return NotFound();
        }

        return Ok(profile.GetValue(collectionPopertyToGet));
    }
}
