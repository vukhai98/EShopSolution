using eShopSolution.Application.System.Languages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;


        public LanguageController(ILanguageService languageService)
        {
           _languageService = languageService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetLanguageAlls()
        {
            var languages = await _languageService.GetLanguageAlls();
            if (languages == null)
            {
                return BadRequest();
            }
            return Ok(languages);
        }
    }
}
