namespace WebApplication1.Services.DataFetching
{
    public class LightspeedIntegration : ApiIntegration
    {
        HttpClient httpClient;
        public LightspeedIntegration()
        {
            httpClient = new HttpClient();
        }

        public async void GetItems()
        {
            var response = await this.httpClient.GetAsync("https://api.lightspeedapp.com/API/Account/1/Item.json");
            var responseString = response.Content.ToString;
            Console.WriteLine(responseString);
        }

        public async void GetSales() {
            var response = await this.httpClient.GetAsync("https://api.lightspeedapp.com/API/Account/1/Sale.json");
            var responseString = response.Content.ToString;
            Console.WriteLine(responseString);

        }

    }
}
