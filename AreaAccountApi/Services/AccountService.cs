using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaAccountApi.DTOs;
using AreaAccountData.Contexts;
using AreaAccountData.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;

namespace AreaAccountApi.Services;

public interface IAccountService
{
    Task<List<GetAccountDTO>> GetAccounts(SieveModel sieveModel);
    Task<GetAccountDTO> CreateAccount(CreateAreaAccountDto dto);
    Task<bool> DeleteAccount(Guid id);
    Task<GetAccountDTO> UpdateAccount(UpdateAreaAccountDTO dto);
}

public class AccountService : IAccountService
{
    private readonly AreaAccountContext _context;
    private readonly SieveProcessor _processor;
    private readonly ILogger<AccountService> _logger;

    public AccountService(AreaAccountContext context, SieveProcessor processor, ILogger<AccountService> logger)
    {
        _context = context;
        _processor = processor;
        _logger = logger;
    }

    public async Task<List<GetAccountDTO>> GetAccounts(SieveModel sieveModel)
    {
        var areaAccounts = _context.AreaAccount.AsNoTracking();
        return await _processor.Apply(sieveModel, areaAccounts)
            .Include(x => x.Tenants)
            .Select(x => x.Adapt<GetAccountDTO>())
            .ToListAsync();
    }

    public async Task<GetAccountDTO> CreateAccount(CreateAreaAccountDto dto)
    {
        var model = dto.Adapt<AreaAccount>();
        var result = await _context.AreaAccount.FindAsync(model.Id);
        if (result != null)
            throw new ArgumentException("area account already exists");
        var tenantsKeys = model.Tenants.Select(x => (x.PassportSerial, x.PassportNumber)).ToList();
        var personsRequests = tenantsKeys.Select(key => _context.Person.FindAsync(key.PassportSerial, key.PassportNumber)).ToArray();
        var existsPersons = personsRequests.Select(x => x.Result).Where(x => x != null).ToArray();
        model.Tenants = model.Tenants.Select(x => existsPersons.FirstOrDefault(person =>
            x.PassportNumber == person.PassportNumber && x.PassportSerial == person.PassportSerial) ?? x).ToList();
        try
        {
            _context.AreaAccount.Add(model);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception during save");
            throw;
        }

        result = await _context.AreaAccount.FindAsync(model.Id);
        if (result is null)
            throw new AggregateException("Couldn't save to db");
        return result.Adapt<GetAccountDTO>();
    }

    public async Task<bool> DeleteAccount(Guid id)
    {
        var account = await _context.AreaAccount.FindAsync(id);
        if (account is null)
            return false;
        _context.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<GetAccountDTO> UpdateAccount(UpdateAreaAccountDTO dto)
    {
        var model = dto.Adapt<AreaAccount>();
        _context.AreaAccount.Update(model);
        await _context.SaveChangesAsync();
        return model.Adapt<GetAccountDTO>();
    }
}