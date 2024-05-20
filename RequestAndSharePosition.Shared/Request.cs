using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RequestAndSharePosition.Shared
{
    public sealed class Request
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Sender { get; set; }

        public string? SenderName { get; set; }

        [Required]
        public required string Receiver { get; set; }

        public string? Message { get; set; }

        [DefaultValue(false)]
        public bool Accepted { get; set; } = false;

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset AcceptedDate { get; set; }
    }
}
