using Microsoft.AspNetCore.Mvc;
using uCondo.HandsOn.Domain.Dtos;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Domain.Interfaces.Services;
using uCondo.HandsOn.Domain.Validation;

namespace uCondo.HandsOn.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : BaseController
    {
        readonly IAccountsService _service;

        public AccountsController(IAccountsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string search, [FromQuery] AccountType? type, [FromQuery] bool? allowEntries)
        {
            var result = await _service.GetAsync(search, type, allowEntries);

            if (result.Invalid)
                return BadRequest(result);

            return new OkObjectResult(result.Data);
        }

        [HttpGet("{code}/next")]
        public async Task<IActionResult> GetNextCodeAsync(string code)
        {
            var result = await _service.GetNextCodeAsync(code);

            if (result.Invalid)
                return BadRequest(result);

            return new OkObjectResult(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AccountCreateDto dto)
        {
            if (dto is IValidationDto dtoValidation)
            {
                var validationResult = dtoValidation.IsValid();

                if (validationResult.Invalid)
                    return BadRequest(validationResult);
            }

            var result = await _service.CreateAsync(dto);

            if (result.Invalid)
                return BadRequest(result);

            return new CreatedResult("api/accounts", result.Data);
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteAsync(string code)
        {
            var result = await _service.DeleteAsync(code);

            if (result.Invalid)
                return BadRequest(result);

            return new NoContentResult();
        }
    }
}