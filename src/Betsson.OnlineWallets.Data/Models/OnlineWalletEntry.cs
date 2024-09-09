namespace Betsson.OnlineWallets.Data.Models
{
    public class OnlineWalletEntry
    {
        public string Id { get; set; }
        public virtual DateTimeOffset EventTime { get; set; } = DateTimeOffset.Now.UtcDateTime;

        public virtual decimal Amount { get; set; }
        public virtual decimal BalanceBefore { get; set; }

        public OnlineWalletEntry() { Construct(); }

        internal virtual void Construct() { Id = Guid.NewGuid().ToString(); }
    }
}
