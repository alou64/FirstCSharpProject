using System.ComponentModel.DataAnnotations;


namespace FirstProject.Models
{
  public class PointOfInterestForUpdateDto
  {
    [Required(ErrorMessage = "Must provide name value")]    // data annotation
    [MaxLength(60)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }
  }
}
