using Betsson.OnlineWallets.Models;

namespace Betsson.OnlineWallets.Factories
{
    internal class BalanceFactory
    {
        public virtual Balance GetBalance => new();
    }
}
