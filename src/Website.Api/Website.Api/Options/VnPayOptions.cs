using System.ComponentModel.DataAnnotations;

namespace Website.Api.Options
{
    public class VnPayOptions
    {
        public static string Position = "VnPaySetting";

        public string VnpUrl { get; set; }
        public string VnpApi { get; set; }
        [Required]
        public string VnpTmnCode { get; set; }
        [Required]
        public string VnpHashSecret { get; set; }
        public string VnpReturnurl { get; set; }
    }
}
