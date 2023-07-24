using System;
using System.Linq;
using AreaAccountData.Models;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace AreaAccountApi.Sieve.Filters;

public class AreaAccountCustomFilters : ISieveCustomFilterMethods
{
    public IQueryable<AreaAccount>
        HasTenants(IQueryable<AreaAccount> accounts, string op,
            string[] values)
    {
        var result = accounts.Where(p => p.Tenants.Any());
        return result;
    }

    public IQueryable<AreaAccount>
        TenantFullName(IQueryable<AreaAccount> accounts, string op,
            string[] values)
    {
        if (values is not { Length: > 0 })
            throw new ArgumentException("Invalid TenantFullName filter arguments, values couldn't be null");
        return accounts.Where(x => x.Tenants.Any(person => EF.Functions.Like(person.FullName, @$"%{values[0]}%")));
    }
}