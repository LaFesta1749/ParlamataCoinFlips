using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerEvents = Exiled.Events.Handlers.Player;
using MEC;

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

            PlayCoinFlipSound(ev.Player, ev.IsTails == false);
        }

        private void PlayCoinFlipSound(Player player, bool isHeads)
        {
            var settings = Plugin.Instance.Config.CoinFlipSounds;

            if (!settings.Enabled)
                return;

            string fileName = isHeads ? settings.HeadSoundFile : settings.TailSoundFile;
            float delay = isHeads ? settings.HeadAutoDestroyDelay : settings.TailAutoDestroyDelay;

            string fullPath = Path.Combine(Paths.Configs, "ParlamataCoinFlips", "audio", fileName);

            if (!File.Exists(fullPath))
            {
                Log.Warn($"[CoinFlipSound] Missing sound file: {fullPath}");
                return;
            }

            // Презареждаме клипа (ако не е зареден)
            try
            {
                AudioClipStorage.LoadClip(fullPath, fileName);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("is already loaded"))
                    Log.Warn($"[CoinFlipSound] Failed to load audio clip: {ex.Message}");
            }

            // Създаваме или взимаме AudioPlayer
            var audioPlayer = AudioPlayer.CreateOrGet($"coinflip_{player.UserId}", onIntialCreation: (p) =>
            {
                // Закачаме към играча
                p.transform.parent = player.GameObject.transform;

                // Създаваме Speaker и го закачаме към играча
                var speaker = p.AddSpeaker("Main", isSpatial: true, minDistance: 2f, maxDistance: 15f);
                speaker.transform.parent = player.GameObject.transform;
                speaker.transform.localPosition = UnityEngine.Vector3.zero;
            });

            // Добавяме клипа (и той се пуска автоматично)
            audioPlayer.AddClip(fileName);

            // След зададеното време го унищожаваме
            Timing.CallDelayed(delay, () =>
            {
                AudioPlayer.Destroy(audioPlayer);
            });
        }
    }
}
