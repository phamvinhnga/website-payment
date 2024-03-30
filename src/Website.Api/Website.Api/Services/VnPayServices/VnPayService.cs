using Microsoft.Extensions.Options;
using Mysqlx.Crud;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Serilog;
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
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "::1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _vnpayOptions.VnpReturnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            await Task.CompletedTask;
            //Add Params of 2.1.0 Version
            return vnpay.CreateRequestUrl(_vnpayOptions.VnpUrl, _vnpayOptions.VnpHashSecret);
        }

        public async Task<bool> PaymentResultAsync(VnPayReturnInput input)
        {
            var vnpay = new VnPayLibrary();
  
            var properties = typeof(VnPayReturnInput).GetProperties();
            foreach (var property in properties.Where(property => property.Name.StartsWith("vnp_")))
            {
                vnpay.AddResponseData(property.Name, property.GetValue(input).ToString());
            }

            bool checkSignature = vnpay.ValidateSignature(_vnpayOptions.VnpHashSecret, _vnpayOptions.VnpHashSecret);
            if (checkSignature)
            {
                if (input.vnp_ResponseCode == "00" && input.vnp_TransactionStatus == "00")
                {
                    //Thanh toan thanh cong
                    _logger.LogInformation("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", input.vnp_TxnRef, input.vnp_TransactionNo);
                }
                else
                {
                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                    _logger.LogInformation("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", input.vnp_TxnRef, input.vnp_TransactionNo, input.vnp_ResponseCode);
                    return false;
                }
            }
            else
            {
                _logger.LogInformation("Invalid signature, InputData={0}", JsonConvert.SerializeObject(input));
                return false;
            }

            await Task.CompletedTask;
            return true;
        }
    }
}
