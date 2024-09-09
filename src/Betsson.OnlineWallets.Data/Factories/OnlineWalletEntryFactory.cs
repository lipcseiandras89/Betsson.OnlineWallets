using Betsson.OnlineWallets.Data.Models;

namespace Betsson.OnlineWallets.Data.Factories
{
    public class OnlineWalletEntryFactory
    {
        public virtual OnlineWalletEntry GetOnlineWalletEntry => new();
    }
}
