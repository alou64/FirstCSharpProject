using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using FirstProject.Models;

namespace FirstProject.Controllers
{
  [ApiController]
  [Route("api/cities/{cityId}/pointsofinterest")]    // it is a child of cities
  public class PointsOfInterestController : ControllerBase
  {
    // constructer injection
    public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public IActionResult GetPointsOfInterest(int cityId)
    {
      var city = CitiesDataStore.Current.Cities
        .FirstOrDefault(c => c.Id == cityId);

      if (city == null)
      {
        return NotFound();
      }

      return Ok(city.PointsOfInterest);
    }

    // get single point of interest
    [HttpGet("{id}", Name = "GetPointOfInterest")]
    public IActionResult GetPointOfInterest(int cityId, int id)
    {
      // find city
      var city = CitiesDataStore.Current.Cities
        .FirstOrDefault(c => c.Id == cityId);

      if (city == null)
      {
        return NotFound();
      }

      // find point of interest
      var pointOfInterest = city.PointsOfInterest
        .FirstOrDefault(c => c.Id == id);

      if (pointOfInterest == null)
      {
        return NotFound();
      }

      return Ok(pointOfInterest);
    }

    [HttpPost]
    public IActionResult CreatePointOfInterest(int cityId,
      [FromBody] PointOfInterestForCreationDto pointOfInterest)
    {
      if (pointOfInterest.Description == pointOfInterest.Name)
      {
        ModelState.AddModelError(
          "Description",
          "The description must be different from the name");
      }

      // have to check modelstate after the above code runs
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // check if city exists
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city == null)
      {
        return NotFound();
      }

      // get id
      var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
        c => c.PointsOfInterest).Max(p => p.Id);

      var finalPointOfInterest = new PointOfInterestDto()
      {
        Id = ++maxPointOfInterestId,
        Name = pointOfInterest.Name,
        Description = pointOfInterest.Description
      };

      city.PointsOfInterest.Add(finalPointOfInterest);

      return CreatedAtRoute(
        "GetPointOfInterest",
        new { cityId, id = finalPointOfInterest.Id },
        finalPointOfInterest);    // return response with location header -> contains uri for newly created point of interest
    }

    [HttpPut("{id}")]    // api/cities/{cityId}/pointsofinterest/{id}
    public IActionResult UpdatePointOfInterest(int cityId, int id,
      [FromBody] PointOfInterestForUpdateDto pointOfInterest)
    {
      if (pointOfInterest.Description == pointOfInterest.Name)
      {
        ModelState.AddModelError(
          "Description",
          "The description must be different from the name");
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // check if city exists
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city == null)
      {
        return NotFound();
      }

      // check if point of interest exists
      var pointOfInterestFromStore = city.PointsOfInterest
        .FirstOrDefault(p => p.Id == id);
      if (pointOfInterestFromStore == null)
      {
        return NotFound();
      }

      // update the fields
      pointOfInterestFromStore.Name = pointOfInterest.Name;
      pointOfInterestFromStore.Description = pointOfInterest.Description;

      // return 204 NoContent result => can also return 200 with the content, but the consumer already has the content since they sent it
      return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
      [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
    {
      // check if city exists
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city == null)
      {
        return NotFound();
      }

      // check if point of interest exists
      var pointOfInterestFromStore = city.PointsOfInterest
        .FirstOrDefault(p => p.Id == id);
      if (pointOfInterestFromStore == null)
      {
        return NotFound();
      }

      // apply patch document
      var pointOfInterestToPatch =
        new PointOfInterestForUpdateDto()
        {
          Name = pointOfInterestFromStore.Name,
          Description = pointOfInterestFromStore.Description
        };

      patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // check validation of patchdoc
      if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
      {
        ModelState.AddModelError(
          "Description",
          "Description and name must be different");
      }

      if (!TryValidateModel(pointOfInterestToPatch))
      {
        return BadRequest(ModelState);
      }

      // update the fields
      pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
      pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePointOfInterest(int cityId, int id)
    {
      // check if city exists
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city == null)
      {
        return NotFound();
      }

      // check if point of interest exists
      var pointOfInterestFromStore = city.PointsOfInterest
        .FirstOrDefault(p => p.Id == id);
      if (pointOfInterestFromStore == null)
      {
        return NotFound();
      }

      // delete point of interest
      city.PointsOfInterest.Remove(pointOfInterestFromStore);

      return NoContent();
    }
  }
}
