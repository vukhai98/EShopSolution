using eShopSolution.AdminApp.Services.IServices;
using eShopSolution.Application.System.Languages;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace eShopSolution.AdminApp.Services
{
    public class LanguageApiClient : BaseApiClient, ILanguageApiClient
    {


        public LanguageApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {

        }

        public async Task<ApiResult<List<LanguageViewModel>>> GetLanguageAlls()
        {
            return await GetAsync<ApiResult<List<LanguageViewModel>>>("api/language");
        }
    }
}
