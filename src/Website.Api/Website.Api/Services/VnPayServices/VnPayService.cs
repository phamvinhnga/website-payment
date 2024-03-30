using Microsoft.Extensions.Options;
using Mysqlx.Crud;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Serilog;
using Website.Api.Enums;
using Website.Api.Helpers;
using Website.Api.Models.VnPays;
using Website.Api.Options;

namespace Website.Api.Services
{
    public interface IVnPayService
    {
        Task<string> CreateNewPaymentAsync(VnPayCreating input, string ipAddress);
        Task<bool> PaymentResultAsync(VnPayReturnInput input);
    }

    public class VnPayService : IVnPayService
    {
        private readonly VnPayOptions _vnpayOptions;
        private readonly ILogger<VnPayService> _logger;

        public VnPayService(
            IOptions<VnPayOptions> vnpayOptions, ILogger<VnPayService> logger)
        {
            _vnpayOptions = vnpayOptions.Value;
            _logger = logger;
        }

        public async Task<string> CreateNewPaymentAsync(VnPayCreating input, string ipAddress)
        {

            //Get Config Info
            //Get payment input
            var order = new VnPayOrderInfo();
            order.OrderId = DateTime.Now.Ticks; 
            order.Amount = input.Amount; 
            order.Status = "0"; 
            order.CreatedDate = DateTime.Now;

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _vnpayOptions.VnpTmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); 
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "::1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _vnpayOptions.VnpReturnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());

            await Task.CompletedTask;
            //Add Params of 2.1.0 Version
            return vnpay.CreateRequestUrl(_vnpayOptions.VnpUrl, _vnpayOptions.VnpHashSecret);
        }

        public async Task<bool> PaymentResultAsync(VnPayReturnInput input)
        {
            _logger.LogInformation($"Receive payment information from vnpay: {JsonConvert.SerializeObject(input)}");

            var vnpay = new VnPayLibrary();
            var properties = typeof(VnPayReturnInput).GetProperties();
            foreach (var property in properties.Where(property => property.Name.StartsWith("vnp_")))
            {
                vnpay.AddResponseData(property.Name, property.GetValue(input)?.ToString());
            }

            var checkSignature = vnpay.ValidateSignature(input.vnp_SecureHash, _vnpayOptions.VnpHashSecret);
            if (checkSignature)
            {
                var vnp_ResponseCode =  (VnPayTransactionStatusCodeEnum)Int32.Parse(input.vnp_ResponseCode);
                var vnp_TransactionStatus = (VnPayTransactionStatusCodeEnum) Int32.Parse(input.vnp_TransactionStatus);
                if (vnp_ResponseCode == VnPayTransactionStatusCodeEnum.Success && vnp_TransactionStatus == VnPayTransactionStatusCodeEnum.Success)
                {
                    _logger.LogInformation($"ResponseCode success: {vnp_ResponseCode.GetEnumDescription()}");
                    _logger.LogInformation($"TransactionStatus success: {vnp_TransactionStatus.GetEnumDescription()}");
                }
                else
                {
                    _logger.LogInformation($"ResponseCode failure: {vnp_ResponseCode.GetEnumDescription()}");
                    _logger.LogInformation($"TransactionStatus failure: {vnp_TransactionStatus.GetEnumDescription()}");
                    return false;
                }
            }
            else
            {
                _logger.LogWarning("Invalid signature");
                return false;
            }
            await Task.CompletedTask;
            return true;
        }
    }
}
