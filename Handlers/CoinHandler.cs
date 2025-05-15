using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerEvents = Exiled.Events.Handlers.Player;

namespace ParlamataCoinFlips.Handlers
{
    public class CoinHandler
    {
        public void Enable()
        {
            PlayerEvents.FlippingCoin += OnCoinFlip;
        }

        public void Disable()
        {
            PlayerEvents.FlippingCoin -= OnCoinFlip;
        }

        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            var config = Plugin.Instance.Config;

            if (!ev.Player.IsAlive || !config.IsEnabled)
                return;

            if (config.Debug)
                Log.Debug($"[CoinHandler] {ev.Player.Nickname} flipped a coin!");

            if (config.GlobalSettings.EnableCooldown &&
                CoinUsesHandler.IsInCooldown(ev.Player))
            {
                HintManager.ShowHint(ev.Player, config.GlobalSettings.CooldownHint, 2f);
                return;
            }

            if (config.GlobalSettings.MaxUsesPerRound > 0 &&
                !CoinUsesHandler.TryUse(ev.Player, config.GlobalSettings.MaxUsesPerRound))
            {
                HintManager.ShowHint(ev.Player, config.GlobalSettings.MaxUsesHint, 2f);
                return;
            }

            EffectHandler.ExecuteCoinFlip(ev.Player);

            var coin = ev.Player.Items.FirstOrDefault(i => i.Type == ItemType.Coin);
            if (coin != null)
            {
                int left = CoinUsesHandler.GetUses(coin.Base);
                Log.Debug($"[CoinHandler] Coin {coin.Base.ItemSerial} has {left} uses left after flip.");
                CoinUsesHandler.UseCoin(ev.Player, coin.Base);
            }
            else
            {
                Log.Debug("[CoinHandler] No Coin item found in inventory after flip.");
            }

            if (config.GlobalSettings.EnableCooldown)
                CoinUsesHandler.SetCooldown(ev.Player, config.CoinCooldown);
        }

        // Няма нужда да вадим coin-а по serial, няма достъп до самия item тук
    }
}
