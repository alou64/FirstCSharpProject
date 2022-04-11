using Microsoft.AspNetCore.Mvc;

namespace FirstProject.Controllers
{
  [ApiController]
  [Route("api/cities")]
  public class CitiesController : ControllerBase
  {
    // get all cities
    [HttpGet]
    public IActionResult GetCities()
    {
      return Ok(CitiesDataStore.Current.Cities);
    }

    // get city by id
    [HttpGet("{id}")]    // parameter in curly brackets
    public IActionResult GetCity(int id)
    {
      // find city
      var CityToReturn = CitiesDataStore.Current.Cities
        .FirstOrDefault(c => c.Id == id);

      if (CityToReturn == null)
      {
        return NotFound();
      }

      return Ok(CityToReturn);
    }
  }
}
