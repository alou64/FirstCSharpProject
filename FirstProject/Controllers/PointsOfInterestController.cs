using Microsoft.AspNetCore.Mvc;
using FirstProject.Models;

namespace FirstProject.Controllers
{
  [ApiController]
  [Route("api/cities/{cityId}/pointsofinterest")]    // it is a child of cities
  public class PointsOfInterestController : ControllerBase
  {
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
  }
}
