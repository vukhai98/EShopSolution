using eShopSolution.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Text;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            // Convert 1 object sang string
            var json = JsonConvert.SerializeObject(request);

            // Convert sang dạng file json để truyền cho Back-end
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Tạo client để kết nối tới back-end
            var client = _httpClientFactory.CreateClient();

            // Tạo ra đường dẫn để ko cần tạo lại
            client.BaseAddress = new Uri("https://localhost:5001");

            // Từ client gọi đến api authenicate của webAPI
            var response = await client.PostAsync("/api/user/authenticate", httpContent);

            // Đọc token trả ra từ respone sang rạng string
            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
    }
}
