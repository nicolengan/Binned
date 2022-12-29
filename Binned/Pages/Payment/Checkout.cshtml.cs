using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Payment
{
    public class CheckoutModel : PageModel
    {
        public void OnGet()
        {
            var client = new RestClient("https://api.sandbox.hit-pay.com/v1/payment-requests");
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("X-BUSINESS-API-KEY", "7c900cf9b2d676ce7f307273500d53a383bfcc6fe36be0c32fba828ea6bcb5ea");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"amount\":0,\"payment_methods\":{\"\":\"string\"},\"currency\":\"string\",\"email\":\"string\",\"purpose\":\"string\",\"name\":\"string\",\"phone\":\"string\",\"reference_number\":\"string\",\"redirect_url\":\"string\",\"webhook\":\"string\",\"allow_repeated_payments\":true,\"expiry_date\":\"string\",\"send_email\":true,\"send_sms\":true,\"expires_after\":\"string\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}
