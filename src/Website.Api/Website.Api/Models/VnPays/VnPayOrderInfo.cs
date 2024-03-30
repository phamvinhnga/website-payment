namespace Website.Api.Models.VnPays
{
    public class VnPayOrderInfo
    {
        // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
        public long OrderId { get; set; }
        // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
        public long Amount { get; set; }
        public string OrderDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
        public string Status { get; set; }
        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
    }
}
