using FirstProject.Models;

namespace FirstProject
{
  public class CitiesDataStore    // exposes a list of CityDto cities
  {
    public static CitiesDataStore Current { get; } = new CitiesDataStore();    // property that allows for access

    public List<CityDto> Cities { get; set; }

    public CitiesDataStore()
    {
      // init dummy data
      Cities = new List<CityDto>()
      {
        new CityDto()
        {
          Id = 1,
          Name = "New York City",
          Description = "Test1",
          PointsOfInterest = new List<PointOfInterestDto>()
          {
            new PointOfInterestDto() {
              Id = 1,
              Name = "Central Park",
              Description = "Test description"
            }
          }
        },
        new CityDto()
        {
          Id = 2,
          Name = "Antwerp",
          Description = "Test2",
          PointsOfInterest = new List<PointOfInterestDto>()
          {
            new PointOfInterestDto() {
              Id = 2,
              Name = "IDK",
              Description = "Test description"
            }
          }
        }
      };
    }
  }
}
