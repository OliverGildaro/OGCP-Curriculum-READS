using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using OGCP.Curriculums.DAL.Model;
using OGCP.Curriculums.ODATA.Helpers;
using System;

namespace OGCP.Curriculums.ODATA.Controllers;

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
    //[HttpGet("profiles")]
    //MaxSkip and maxTop is not server paging, this is just configuration
    //If the client send a query withouth paging filters will get all items from db
    //To enable server driven paging we use PageSize
    [EnableQuery(MaxExpansionDepth =3, MaxSkip =10, MaxTop =10, PageSize =6)]
    public IActionResult GetProfiles()
    {
        return Ok(context.Profiles);
    }

    [EnableQuery]
    public ActionResult<Profile> GetProfile([FromRoute] int key)
    {
        var profiles = context.Profiles.Where(d => d.Id.Equals(key));

        if (!profiles.Any())
        {
            return NotFound();
        }

        return Ok(SingleResult.Create(profiles));
    }

    [HttpGet("odata/profiles({id})/Summary")]
    [HttpGet("odata/profiles({id})/FirstName")]
    [HttpGet("odata/profiles({id})/LastName")]
    //we are already able to select properties with [EnableQueyr]
    //I will only use this method if I need to scale with async programming
    public async Task<IActionResult> GetProfileProperty(int id)
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

    [HttpGet("odata/profiles({id})/Summary/$value")]
    [HttpGet("odata/profiles({id})/FirstName/$value")]
    [HttpGet("odata/profiles({id})/LastName/$value")]
    public async Task<IActionResult> GetProfilePropertyRawValue(int id)
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


    [HttpPost("odata/profiles")]
    //TODO: research on how post with odata
    //DO we need two separates data models ??
    public async Task<IActionResult> CreateProfile([FromBody] Profile profile)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        context.Profiles.Add(profile);
        await context.SaveChangesAsync();

        return Created(profile);
    }

    [HttpPut("odata/profiles({id})")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] Profile profile)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentProfile = await context.Profiles
          .FirstOrDefaultAsync(p => p.Id== id);

        if (currentProfile == null)
        {
            return NotFound();
        }

        profile.Id= currentProfile.Id;
        //updating in this way we ensure that both persons are the same
        //Since from body we can receive an id too !!
        //here if both ids are different weill fail the update
        context.Entry(currentProfile).CurrentValues.SetValues(profile);
        await context.SaveChangesAsync();

        return NoContent();
    }


    [HttpPatch("odata/profiles({id})")]
    public async Task<IActionResult> PartiallyUpdateProfile(int id,
        [FromBody] Delta<Profile> patch)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentPerson = await context.Profiles
                       .FirstOrDefaultAsync(p => p.Id == id);

        if (currentPerson == null)
        {
            return NotFound();
        }

        patch.Patch(currentPerson);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("odata/profiles({id})")]
    public async Task<IActionResult> DeleteOneProfile(int id)
    {
        var currentPerson = await context.Profiles
            .FirstOrDefaultAsync(p => p.Id == id);

        if (currentPerson == null)
        {
            return NotFound();
        }

        context.Profiles.Remove(currentPerson);
        await context.SaveChangesAsync();
        return NoContent();
    }


    [HttpGet("odata/profiles({key})/languages")]
    [EnableQuery]
    public IActionResult GetLanguagesForProfile(int key)
    {
        if (!context.Profiles.Any(p => p.Id == key))
        {
            return NotFound();
        }

        return Ok(context.Profiles
            .Where(v => v.Id == key)
            .SelectMany(p => p.Languages));
    }

    [EnableQuery]
    [HttpGet("odata/profiles({key})/educations")]
    public IActionResult GetEducationsForProfile(int key)
    {
        if (!context.Profiles.Any(p => p.Id == key))
        {
            return NotFound();
        }

        return Ok(context.Profiles
            .Where(v => v.Id == key)
            .SelectMany(p => p.Educations));
    }

    //[HttpGet("odata/profiles({id})/Languages")]
    //[HttpGet("odata/profiles({id})/educations")]
    //[HttpGet("odata/profiles({id})/jobExperiences")]
    //public IActionResult GetProfileCollectionProperty(int id)
    //{
    //    var collectionPopertyToGet = new Uri(HttpContext.Request.GetEncodedUrl())
    //        .Segments.Last();

    //    var profile = context.Profiles
    //          .Include(collectionPopertyToGet)
    //          .FirstOrDefault(p => p.Id == id);

    //    if (profile == null)
    //    {
    //        return NotFound();
    //    }

    //    if (!profile.HasProperty(collectionPopertyToGet))
    //    {
    //        return NotFound();
    //    }

    //    return Ok(profile.GetValue(collectionPopertyToGet));
    //}

}
