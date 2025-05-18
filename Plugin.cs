using Exiled.API.Features;
using ParlamataCoinFlips.Handlers;
using Exiled.Events.Handlers;
using Exiled.Events.EventArgs.Server;

namespace ParlamataCoinFlips
{
    public class Plugin : Plugin<Config.Config>
    {
        public override string Author => "LaFesta1749";
        public override string Name => "ParlamataCoinFlips";
        public override string Prefix => "parlamata_coinflips";
        public override Version Version => new(1, 0, 1);
        public override Version RequiredExiledVersion => new(9, 6, 0);

        public static Plugin Instance { get; private set; } = null!;

        private CoinHandler? coinHandler;

        public override void OnEnabled()
        {
            Instance = this;

            coinHandler = new CoinHandler();
            coinHandler.Enable();

            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;

            Log.Info("ParlamataCoinFlips enabled.");
            base.OnEnabled();
        }
        
        public override void OnDisabled()
        {
            coinHandler?.Disable();
            coinHandler = null;
            Instance = null!;

            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;

            Log.Info("ParlamataCoinFlips disabled.");
            base.OnDisabled();
        }

        private void OnRoundEnded(RoundEndedEventArgs _)
        {
            CoinUsesHandler.Reset();
        }
    }
}
