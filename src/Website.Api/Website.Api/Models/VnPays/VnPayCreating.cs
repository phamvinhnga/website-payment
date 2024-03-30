using System.ComponentModel.DataAnnotations;
using Website.Api.Enums;

namespace Website.Api.Models.VnPays
{
    public class VnPayCreating
    {
        public string VnpBankCode { get; set; } = nameof(VnPayEnum.VNBANK);

        [Range(5000, 1000000000, ErrorMessage = "The value must be between 5000 and 1 billion.")]
        public long Amount { get; set; } = 0;
    }
}
