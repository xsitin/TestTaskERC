using System;
using System.Collections.Generic;

namespace AreaAccountApi.DTOs;

public class GetAccountDTO
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; }
    public DateTime ActiveFrom { get; set; }
    public DateTime ActiveUntil { get; set; }
    public string Address { get; set; }
    public double Area { get; set; }
    public List<GetPersonDTO> Tenants { get; set; }
}