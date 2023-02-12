using eShopSolution.AdminApp.Services.IServices;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace eShopSolution.AdminApp.Services
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetProductPagings(GetManageProductPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"api/products/paging?pageIndex={request.PageIndex}" +
                $"&languageId ={request.LanguageId}"+
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var listProductViewModel = (PagedResult<ProductViewModel>)JsonConvert.DeserializeObject(body, typeof(PagedResult<ProductViewModel>));
                return new ApiSuccessResult<PagedResult<ProductViewModel>>(listProductViewModel);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<PagedResult<ProductViewModel>>>(body);

        }
    }
}
