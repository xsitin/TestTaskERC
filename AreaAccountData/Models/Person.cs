using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AreaAccountData.Models
{
    [PrimaryKey(nameof(PassportSerial), nameof(PassportNumber))]
    [Index(nameof(FullName))]
    public class Person
    {
        public int PassportSerial { get; set; }
        public int PassportNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public string FullName
        {
            get => $"{Surname} {Name} {Patronymic}";
            set
            {
                var split = value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                Surname = split[0];
                Name = split[1];
                Patronymic = split[2];
            }
        }

        public List<AreaAccount> AreaAccounts { get; set; }
    }
}