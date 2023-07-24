using System;
using System.Threading.Tasks;
using AreaAccountApi.DTOs;
using AreaAccountApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace AreaAccountApi.Controllers
{
    [ApiController]
    [Route("area_account")]
    public class AreaAccountController : Controller
    {
        private IAccountService AccountService { get; set; }

        public AreaAccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }


        /// <summary>
        /// Для обработки запросов на получение используется Sieve(https://github.com/Biarity/Sieve).
        /// Пример запроса:
        /// {
        ///     "filters": "tenantFullName==str,activeFrom>=2023-07-23",
        ///     "page":1,
        ///     "pageSize": 10
        ///  }
        /// Добавлены кастомные фильтры: TenantFullName, HasTenants.
        /// TenantFullName - фильтр по полному имени владельцев, вне зависимости от оператора, ищет вхождение подстроки
        /// в строке ФИО.
        /// HasTenants - фильтр наличия "владельцев", используеися как флаг.
        /// </summary>
        [HttpPost("get")]
        public async Task<JsonResult> GetAll(SieveModel sieveModel)
        {
            return Json(await AccountService.GetAccounts(sieveModel));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Create(CreateAreaAccountDto dto)
        {
            var account = await AccountService.CreateAccount(dto);
            return Json(account);
        }

        [HttpPut]
        public async Task<JsonResult> Update(UpdateAreaAccountDTO dto)
        {
            return Json(await AccountService.UpdateAccount(dto));
        }

        [HttpDelete("{id:guid}")]
        public async Task<JsonResult> Delete(Guid id)
        {
            return Json(await AccountService.DeleteAccount(id));
        }
    }
}