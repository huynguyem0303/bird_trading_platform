using BirdTrading.Domain.Contants;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Services;
using Microsoft.Extensions.Configuration;
using PayPal.Api;

#nullable disable warnings
namespace BirdTrading.Services.Paypal
{
    public class PaypalServices : IPaypalServices
    {
        private readonly string CURRENCY = BirdTradingSettings.Settings["APP_CURRENCY"].ToString();
        private readonly string PAYMENT_METHOD = BirdTradingSettings.Settings["PAYMENT_METHOD"].ToString();
        private Payment _payment;
        private readonly IConfiguration _configuration;

        public PaypalServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region PaymentCreate
        public Payment CreatePayment(APIContext context, string redirectUrl, string blogId, List<CartDetail> cartDetails)
        {
            var itemList = GetItemList(cartDetails);
            var payer = GetPayer();
            var redirUrls = GetRedirectUrls(redirectUrl);
            //
            var subTotal = GenerateSubTotal(cartDetails);
            //
            var details = GetDetails(subTotal);
            var amount = GetAmount(details, subTotal);
            var transactionList = GetTransactions(itemList, amount);
            //
            _payment = new Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return _payment.Create(context);
        }

        //Sub-TOTAL
        #region GenerateSubTotal
        private decimal GenerateSubTotal(List<CartDetail> cartDetails)
        {
            decimal subTotal = 0;
            foreach (var item in cartDetails)
            {
                subTotal += item.Quantity * item.Product.OriginalPrice;
            }
            return subTotal;
        }
        #endregion

        //ITEM LIST
        #region ItemList
        private ItemList GetItemList(List<CartDetail> cartDetails)
        {
            var itemList = new ItemList
            {
                items = new List<Item>()
            };
            //
            foreach (var item in cartDetails)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Product.Name,
                    currency = CURRENCY,
                    price = item.Product.OriginalPrice.ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = "SKU"
                });
            }

            return itemList;
        }
        #endregion

        //REDIRECT URL
        #region RedirectUrl
        private RedirectUrls GetRedirectUrls(string redirectUrl)
        {
            return new RedirectUrls
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl,
            };
        }
        #endregion

        //PAYER
        #region Payer
        private Payer GetPayer()
        {
            return new Payer()
            {
                payment_method = PAYMENT_METHOD
            };
        }
        #endregion

        //Details
        #region Details
        private Details GetDetails(decimal subTotal)
        {
            return new Details
            {
                tax = "0",
                shipping = "10",
                subtotal = subTotal.ToString(),
            };
        }
        #endregion

        //Amount
        #region Amount
        private Amount GetAmount(Details details, decimal subTotal)
        {
            return new Amount
            {
                currency = CURRENCY,
                total = (subTotal + 10).ToString(),
                details = details
            };
        }
        #endregion

        //TransactionList
        #region TransactionList
        private List<Transaction> GetTransactions(ItemList itemList, Amount amount)
        {
            return new List<Transaction>
            {
                new Transaction()
                {
                    description = "123123",
                    invoice_number = Guid.NewGuid().ToString(),
                    amount = amount,
                    item_list = itemList
                }
            };
        }
        #endregion
        //
        #endregion

        #region ExecutePayment
        public Payment ExecutePayment(APIContext context, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution
            {
                payer_id = payerId
            };
            //
            _payment = new Payment
            {
                id = paymentId
            };
            //

            return _payment.Execute(context, paymentExecution);
        }
        #endregion

        public string GetClientId()
        {
            return _configuration.GetSection("Paypal:Key").Value;
        }

        public string GetClientSecret()
        {
            return _configuration.GetSection("Paypal:Secret").Value;
        }

        public string GetPaypalMode()
        {
            return _configuration.GetSection("Paypal:mode").Value;
        }
    }
}
