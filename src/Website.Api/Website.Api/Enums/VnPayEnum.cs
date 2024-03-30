using System.ComponentModel;

namespace Website.Api.Enums
{
    public enum VnPayEnum
    {
        VNPAYQR,
        VNBANK,
        INTCARD
    }

    public enum VnPayTransactionStatusCodeEnum
    {
        [Description("Giao dịch thành công")]
        Success = 0,

        [Description("Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường).")]
        SuspiciousTransaction = 7,

        [Description("Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng.")]
        CustomerNotRegistered = 9,

        [Description("Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần.")]
        IncorrectAuthentication = 10,

        [Description("Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch.")]
        PaymentExpired = 11,

        [Description("Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa.")]
        CustomerAccountLocked = 12,

        [Description("Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch.")]
        IncorrectOTP = 13,

        [Description("Giao dịch không thành công do: Khách hàng hủy giao dịch")]
        TransactionCancelledByCustomer = 24,

        [Description("Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.")]
        InsufficientBalance = 51,

        [Description("Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày.")]
        ExceededDailyTransactionLimit = 65,

        [Description("Ngân hàng thanh toán đang bảo trì.")]
        PaymentBankUnderMaintenance = 75,

        [Description("Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định. Xin quý khách vui lòng thực hiện lại giao dịch.")]
        IncorrectPaymentPasswordAttempts = 79,

        [Description("Các lỗi khác (lỗi còn lại, không có trong danh sách mã lỗi đã liệt kê).")]
        OtherErrors = 99,

        // Transaction Query Errors
        [Description("Merchant không hợp lệ (kiểm tra lại vnp_TmnCode).")]
        InvalidMerchant = 2,

        [Description("Dữ liệu gửi sang không đúng định dạng.")]
        InvalidDataFormat = 3,

        [Description("Không tìm thấy giao dịch yêu cầu.")]
        TransactionNotFound = 91,

        [Description("Yêu cầu bị trùng lặp trong thời gian giới hạn của API (Giới hạn trong 5 phút).")]
        DuplicateRequest = 94,

        [Description("Chữ ký không hợp lệ.")]
        InvalidSignature = 97,

        // Refund Request Errors
        [Description("Tổng số tiền hoản trả lớn hơn số tiền gốc.")]
        RefundAmountExceedsOriginal = 2,

        [Description("Dữ liệu gửi sang không đúng định dạng.")]
        RefundDataFormatIncorrect = 3,

        [Description("Không cho phép hoàn trả toàn phần sau khi hoàn trả một phần.")]
        NotAllowedFullRefundAfterPartial = 4,

        [Description("Chỉ cho phép hoàn trả một phần.")]
        OnlyPartialRefundAllowed = 13,

        [Description("Không tìm thấy giao dịch yêu cầu hoàn trả.")]
        RefundTransactionNotFound = 91,

        [Description("Số tiền hoàn trả không hợp lệ. Số tiền hoàn trả phải nhỏ hơn hoặc bằng số tiền thanh toán.")]
        InvalidRefundAmount = 93,

        [Description("Yêu cầu bị trùng lặp trong thời gian giới hạn của API (Giới hạn trong 5 phút).")]
        RefundRequestDuplicate = 94,

        [Description("Giao dịch này không thành công bên VNPAY. VNPAY từ chối xử lý yêu cầu.")]
        VNPAYRejectedRequest = 95,

        [Description("Timeout Exception.")]
        TimeoutException = 98,

        [Description("Các lỗi khác (lỗi còn lại, không có trong danh sách mã lỗi đã liệt kê).")]
        RefundOtherErrors = 99
    } 
}
