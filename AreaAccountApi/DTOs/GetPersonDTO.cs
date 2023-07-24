namespace AreaAccountApi.DTOs;

public class GetPersonDTO
{
    public int PassportSerial { get; set; }
    public int PassportNumber { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string FullName { get; set; }
}