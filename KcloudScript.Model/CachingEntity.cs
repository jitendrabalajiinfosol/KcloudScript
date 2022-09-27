using System.ComponentModel.DataAnnotations;

namespace KcloudScript.Model
{
    public class CachingEntity
    {
        [Required]
        public string CachingKey { get; set; } = "";
        [Required]
        public int ExpirationTime { get; set; }
        [Required]
        public string CachingValue { get; set; } = "";
    }
}
