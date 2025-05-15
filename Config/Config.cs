using System.ComponentModel;
using Exiled.API.Interfaces;

namespace ParlamataCoinFlips.Config
{
    public class Config : IConfig
    {
        [Description("Enable the plugin.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Enable debug logs.")]
        public bool Debug { get; set; } = false;

        [Description("Cooldown between coin flips in seconds.")]
        public float CoinCooldown { get; set; } = 5f;

        [Description("Broadcast duration (deprecated, hints are used instead).")]
        public float BroadcastDuration { get; set; } = 3f;

        [Description("Default coin uses (min, max).")]
        public int[] MinMaxDefaultCoins { get; set; } = { 1, 4 };

        [Description("Hint duration (if not managed manually).")]
        public float HintDuration { get; set; } = 3f;

        [Description("Y Coordinate for HSM hints.")]
        public float HintYCoordinate { get; set; } = 900f;

        [Description("Effects and items for good outcomes.")]
        public GoodEventsConfig GoodEvents { get; set; } = new();

        [Description("Effects and items for bad outcomes.")]
        public BadEventsConfig BadEvents { get; set; } = new();

        [Description("Other global settings.")]
        public GlobalSettingsConfig GlobalSettings { get; set; } = new();

        [Description("List of possible good effects.")]
        public List<string> GoodEffects { get; set; } = new()
        {
            "MovementBoost", "Vitality", "Invigorated"
        };

        [Description("List of possible bad effects.")]
        public List<string> BadEffects { get; set; } = new()
        {
            "Bleeding", "Concussed", "Blinded"
        };
    }

    public class GoodEventsConfig
    {
        public int KeycardChance { get; set; } = 20;
        public int MedicalKitChance { get; set; } = 35;
        public int TeleportToEscapeChance { get; set; } = 5;
        public int HealChance { get; set; } = 10;
        public int BonusHpChance { get; set; } = 10;
        public int HatChance { get; set; } = 10;
        public int RandomGoodEffectChance { get; set; } = 30;
        public int Logicer1AmmoChance { get; set; } = 1;
        public int LightbulbChance { get; set; } = 15;
        public int PinkCandyChance { get; set; } = 10;
        public int BadRevolverChance { get; set; } = 5;
        public int EmptyHidChance { get; set; } = 5;
        public int ForceRespawnChance { get; set; } = 15;
        public int SizeChangeChance { get; set; } = 20;
        public int RandomItemChance { get; set; } = 35;
        public int SpeedBoostChance { get; set; } = 15;
        public int AntiDeathChance { get; set; } = 5;
    }

    public class BadEventsConfig
    {
        public int HpReductionChance { get; set; } = 20;
        public int TpToClassDChance { get; set; } = 5;
        public int BadEffectChance { get; set; } = 20;
        public int WarheadToggleChance { get; set; } = 10;
        public int LightsOutChance { get; set; } = 20;
        public int LiveHeChance { get; set; } = 30;
        public int TrollFlashChance { get; set; } = 50;
        public int ScpTpChance { get; set; } = 20;
        public int OneHpLeftChance { get; set; } = 15;
        public int PrimedVaseChance { get; set; } = 20;
        public int TantrumChance { get; set; } = 40;
        public int FakeCassieChance { get; set; } = 50;
        public int RandomScpTransformChance { get; set; } = 30;
        public int InventoryResetChance { get; set; } = 20;
        public int ClassSwapChance { get; set; } = 10;
        public int InstantExplosionChance { get; set; } = 10;
        public int PlayerSwapChance { get; set; } = 20;
        public int KickChance { get; set; } = 5;
        public int SpectatorReplaceChance { get; set; } = 10;
        public int TeslaTpChance { get; set; } = 15;
        public int InventorySwapChance { get; set; } = 20;
        public int HandcuffChance { get; set; } = 10;
        public int RandomTeleportChance { get; set; } = 15;
        public int InfectiousTouchChance { get; set; } = 10;
    }

    public class GlobalSettingsConfig
    {
        [Description("Enable cooldown between coin flips.")]
        public bool EnableCooldown { get; set; } = true;

        [Description("Hint to show when player is on cooldown.")]
        public string CooldownHint { get; set; } = "⏳ Please wait before flipping again.";

        [Description("Max number of coin flips allowed per player per round. Set to 0 to disable.")]
        public int MaxUsesPerRound { get; set; } = 3;

        [Description("Hint shown when player reaches max allowed coin uses.")]
        public string MaxUsesHint { get; set; } = "🚫 No more coin flips allowed this round.";

        public List<string> ValidScps { get; set; } = new()
        {
            "Scp049", "Scp173", "Scp096", "Scp106", "Scp939", "Scp0492"
        };

        public List<string> ItemsToGive { get; set; } = new()
        {
            "Adrenaline", "Coin", "Painkillers", "Medkit", "Radio", "ArmorCombat",
            "GunRevolver", "GunE11SR", "GunFSP9"
        };

        public List<string> RoomsToTeleport { get; set; } = new()
        {
            "Lcz914", "Lcz330", "Hcz106", "EzGateA", "Surface", "LczCheckpointA"
        };

        public List<string> IgnoredRolesForSwap { get; set; } = new()
        {
            "Spectator", "Scp079", "Overwatch", "Tutorial"
        };

        public int RedCardChance { get; set; } = 15;
        [Description("The reason shown to players when kicked.")]
        public string KickReason { get; set; } = "💀 The coin decided your fate.";
        public float BlackoutTime { get; set; } = 10f;
        public float GrenadeFuseTime { get; set; } = 3.25f;
    }
}
