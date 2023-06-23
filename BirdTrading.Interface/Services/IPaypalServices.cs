using BirdTrading.Domain.Models;
using PayPal.Api;

namespace BirdTrading.Interface.Services
{
    public interface IPaypalServices
    {
        Payment ExecutePayment(APIContext context, string payerId, string paymentId);
        Payment CreatePayment(APIContext context, string redirectUrl, string blogId, List<CartDetail> cartDetails);
        string GetClientId();
        string GetClientSecret();
        string GetPaypalMode();
    }
}
