namespace RequestAndSharePosition.Shared
{
    public sealed class RequestPosition
    {
        /// <summary>
        /// User who want to be referred to.
        /// </summary>
        public required string Sender { get; set; }

        /// <summary>
        /// The reference user.
        /// </summary>
        public required string Receiver { get; set; }

        /// <summary>
        /// the topic of the request.
        /// </summary>
        public string? Message { get; set; }
    }
}
