namespace FirstProject.Models
{
  public class CityDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfPointsOfInterest
    {
      get
      {
        return PointsOfInterest.Count;
      }
    }

    public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }    // ICollection is an interface that represnts a collection
      = new List<PointOfInterestDto>();    // initialize to an empty collection to avoid null reference issues
  }
}
