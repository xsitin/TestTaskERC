using System.ComponentModel.DataAnnotations;

namespace AreaAccountApi.DTOs;

public class CreatePersonDTO
{
    [Required] public int PassportNumber { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Surname { get; set; }
    [Required] public string Patronymic { get; set; }
}