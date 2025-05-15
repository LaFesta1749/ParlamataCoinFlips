using Exiled.API.Enums;
using Exiled.API.Features;
using ParlamataCoinFlips.Config;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using MapGeneration;
using Exiled.API.Features.Roles;
using Respawning;
using System.Linq;
using PlayerRoles;
using Exiled.API.Features.Items;
using MEC;

namespace ParlamataCoinFlips.Handlers
{
    public static class EffectHandler
    {
        private static ParlamataCoinFlips.Config.Config Config => Plugin.Instance.Config;

        public static void ExecuteCoinFlip(Player player)
        {
            if (!Plugin.Instance.Config.IsEnabled)
                return;

            bool isHeads = Random.Range(0f, 1f) <= 0.5f;

            string hint = isHeads
                ? "<color=lime>🟢 HEADS</color> — You're in luck..."
                : "<color=red>🔴 TAILS</color> — Unfortunate outcome.";

            HintManager.ShowHint(player, hint, 2f);

            if (Plugin.Instance.Config.Debug)
                Log.Debug($"[EffectHandler] {player.Nickname} flipped coin → {(isHeads ? "HEADS" : "TAILS")}");

            if (isHeads)
                ExecuteGoodEffect(player);
            else
                ExecuteBadEffect(player);
        }

        private static void ExecuteGoodEffect(Player player)
        {
            var effects = new Dictionary<string, int>
    {
        { "keycard", Config.GoodEvents.KeycardChance },
        { "medical_kit", Config.GoodEvents.MedicalKitChance },
        { "tp_escape", Config.GoodEvents.TeleportToEscapeChance },
        { "heal", Config.GoodEvents.HealChance },
        { "bonus_hp", Config.GoodEvents.BonusHpChance },
        { "hat", Config.GoodEvents.HatChance },
        { "good_effect", Config.GoodEvents.RandomGoodEffectChance },
        { "logicer", Config.GoodEvents.Logicer1AmmoChance },
        { "lightbulb", Config.GoodEvents.LightbulbChance },
        { "pink_candy", Config.GoodEvents.PinkCandyChance },
        { "bad_revo", Config.GoodEvents.BadRevolverChance },
        { "empty_hid", Config.GoodEvents.EmptyHidChance },
        { "force_respawn", Config.GoodEvents.ForceRespawnChance },
        { "size_change", Config.GoodEvents.SizeChangeChance },
        { "random_item", Config.GoodEvents.RandomItemChance },
        { "speed_boost", Config.GoodEvents.SpeedBoostChance },
        { "anti_death", Config.GoodEvents.AntiDeathChance }
    };

            string selected = GetWeightedRandom(effects);

            if (Config.Debug)
                Log.Debug($"[EffectHandler] GOOD outcome selected: {selected}");

            switch (selected)
            {
                case "heal":
                    player.Heal(25);
                    HintManager.ShowHint(player, "💉 You feel better. +25 HP.", 3f);
                    break;

                case "bonus_hp":
                    float bonus = Mathf.Ceil(player.Health * 0.10f);
                    player.Health += bonus;
                    HintManager.ShowHint(player, $"🧬 Bonus vitality! +{bonus} HP", 3f);
                    break;

                case "keycard":
                    var card = ItemType.KeycardContainmentEngineer;
                    if (UnityEngine.Random.Range(0, 100) < Config.GlobalSettings.RedCardChance)
                        card = ItemType.KeycardFacilityManager;
                    player.AddItem(card);
                    HintManager.ShowHint(player, "🪪 You received a keycard.", 3f);
                    break;

                case "medical_kit":
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.Painkillers);
                    HintManager.ShowHint(player, "🧰 Emergency medical supplies delivered.", 3f);
                    break;

                case "tp_escape":
                    player.Position = Room.Get(RoomType.Surface).Position + Vector3.up;
                    HintManager.ShowHint(player, "🚪 You're near at the exit. Use it wisely.", 3f);
                    break;

                case "hat":
                    player.AddItem(ItemType.SCP268);
                    HintManager.ShowHint(player, "🎩 You received the elusive SCP-268.", 3f);
                    break;

                case "good_effect":
                    ApplyRandomEffect(player, true);
                    break;

                case "logicer":
                    var gun = player.AddItem(ItemType.GunLogicer);
                    if (gun is Exiled.API.Features.Items.Firearm firearm)
                        firearm.MagazineAmmo = 1;
                    HintManager.ShowHint(player, "🔫 Logicer with 1 bullet. Use it well.", 3f);
                    break;

                case "lightbulb":
                    player.AddItem(ItemType.SCP2176);
                    HintManager.ShowHint(player, "💡 SCP-2176 appears in your hands.", 3f);
                    break;

                case "pink_candy":
                    player.AddItem(ItemType.SCP330);
                    HintManager.ShowHint(player, "🍬 You found a suspicious pink candy...", 3f);
                    break;

                case "bad_revo":
                    var revo = player.AddItem(ItemType.GunRevolver);
                    if (revo is Exiled.API.Features.Items.Firearm badGun)
                        badGun.MagazineAmmo = 1; // ако искаш минимален капацитет
                    HintManager.ShowHint(player, "🔫 Crappy revolver... nice.", 3f);
                    break;

                case "empty_hid":
                    var hid = player.AddItem(ItemType.MicroHID);
                    if (hid is Exiled.API.Features.Items.Firearm hidGun)
                        hidGun.MagazineAmmo = 0;
                    HintManager.ShowHint(player, "⚡ You received a MicroHID. No charge.", 3f);
                    break;

                case "force_respawn":
                    Cassie.Message("Nine Tailed Fox Alpha unit has entered the facility.", false, false);
                    HintManager.ShowHint(player, "📢 MTF arrival simulation triggered.", 3f);
                    break;

                case "size_change":
                    player.Scale = new Vector3(1.2f, 0.8f, 1f);
                    HintManager.ShowHint(player, "📏 Your proportions changed mysteriously...", 3f);
                    break;

                case "random_item":
                    string itemRaw = Config.GlobalSettings.ItemsToGive.GetRandom();
                    if (Enum.TryParse(itemRaw, out ItemType itemType))
                        player.AddItem(itemType);
                    HintManager.ShowHint(player, "📦 You received a random item!", 3f);
                    break;

                case "speed_boost":
                    byte intensity = (byte)UnityEngine.Random.Range(30, 81);
                    player.EnableEffect(Exiled.API.Enums.EffectType.MovementBoost, duration: 30, intensity: intensity);
                    HintManager.ShowHint(player, $"💨 Speed Surge! Intensity: {intensity}", 3f);
                    break;

                case "anti_death":
                    // TODO: Implement death protection tracking
                    HintManager.ShowHint(player, "🛡 You feel invincible for 30 seconds...", 3f);
                    break;
            }
        }

        private static void ExecuteBadEffect(Player player)
        {
            var effects = new Dictionary<string, int>
    {
        { "hp_reduction", Config.BadEvents.HpReductionChance },
        { "tp_to_class_d", Config.BadEvents.TpToClassDChance },
        { "bad_effect", Config.BadEvents.BadEffectChance },
        { "warhead_toggle", Config.BadEvents.WarheadToggleChance },
        { "lights_out", Config.BadEvents.LightsOutChance },
        { "live_he", Config.BadEvents.LiveHeChance },
        { "troll_flash", Config.BadEvents.TrollFlashChance },
        { "scp_tp", Config.BadEvents.ScpTpChance },
        { "one_hp", Config.BadEvents.OneHpLeftChance },
        { "primed_vase", Config.BadEvents.PrimedVaseChance },
        { "tantrum", Config.BadEvents.TantrumChance },
        { "fake_cassie", Config.BadEvents.FakeCassieChance },
        { "random_scp", Config.BadEvents.RandomScpTransformChance },
        { "inventory_reset", Config.BadEvents.InventoryResetChance },
        { "class_swap", Config.BadEvents.ClassSwapChance },
        { "instant_explosion", Config.BadEvents.InstantExplosionChance },
        { "player_swap", Config.BadEvents.PlayerSwapChance },
        { "kick", Config.BadEvents.KickChance },
        { "spectator_replace", Config.BadEvents.SpectatorReplaceChance },
        { "tesla_tp", Config.BadEvents.TeslaTpChance },
        { "inventory_swap", Config.BadEvents.InventorySwapChance },
        { "handcuff", Config.BadEvents.HandcuffChance },
        { "random_tp", Config.BadEvents.RandomTeleportChance },
        { "infectious_touch", Config.BadEvents.InfectiousTouchChance }
    };

            string selected = GetWeightedRandom(effects);

            if (Config.Debug)
                Log.Debug($"[EffectHandler] BAD outcome selected: {selected}");

            switch (selected)
            {
                case "hp_reduction":
                    float dmg = player.Health * 0.30f;
                    player.Health -= dmg;
                    HintManager.ShowHint(player, $"💔 You feel weaker... -{Mathf.RoundToInt(dmg)} HP", 3f);
                    break;

                case "tp_to_class_d":
                    player.Position = Room.Get(RoomType.LczClassDSpawn).Position + Vector3.up;
                    HintManager.ShowHint(player, "🚽 Back to your cell, criminal.", 3f);
                    break;

                case "bad_effect":
                    ApplyRandomEffect(player, false);
                    break;

                case "warhead_toggle":
                    bool isDetonating = Warhead.IsDetonated;
                    if (isDetonating)
                        Warhead.Stop();
                    else
                        Warhead.Start();
                    string state = isDetonating ? "canceled" : "started";
                    HintManager.ShowHint(player, $"☢️ Warhead sequence {state}!", 3f);
                    break;

                case "lights_out":
                    Map.TurnOffAllLights(Config.GlobalSettings.BlackoutTime);
                    HintManager.ShowHint(player, "💡 Lights out.", 3f);
                    break;

                case "live_he":
                    var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                    grenade.FuseTime = Config.GlobalSettings.GrenadeFuseTime;
                    grenade.SpawnActive(player.Position + Vector3.up, player);
                    HintManager.ShowHint(player, "💣 Surprise HE grenade!", 3f);
                    break;

                case "troll_flash":
                    var flash = (FlashGrenade)Item.Create(ItemType.GrenadeFlash);
                    flash.FuseTime = Config.GlobalSettings.GrenadeFuseTime;
                    flash.SpawnActive(player.Position + Vector3.up, player);
                    HintManager.ShowHint(player, "⚡ Flash out!", 3f);
                    break;

                case "scp_tp":
                    var scps = Player.Get(Side.Scp).Where(p => p.Role.Type != RoleTypeId.Scp079).ToList();
                    if (scps.Any())
                    {
                        var scpTarget = scps.GetRandom();
                        player.Position = scpTarget.Position + Vector3.up;
                        HintManager.ShowHint(player, "🧟 You’ve been sent to an SCP... run.", 3f);
                    }
                    else
                    {
                        player.Hurt(15f);
                        HintManager.ShowHint(player, "❌ No SCP found. You took 15 damage instead.", 3f);
                    }
                    break;

                case "one_hp":
                    player.Health = 1;
                    HintManager.ShowHint(player, "❤️ You’re barely alive.", 3f);
                    break;

                case "primed_vase":
                    var vase = player.AddItem(ItemType.SCP244a);
                    // no real "primed" logic exists – just message
                    HintManager.ShowHint(player, "🥶 You’re holding a cold, dangerous vase.", 3f);
                    break;

                case "tantrum":
                    player.PlaceTantrum();
                    HintManager.ShowHint(player, "😡 You lost your temper... (Tantrum fallback)", 3f);

                    break;

                case "fake_cassie":
                    Cassie.Message("SCP 1 7 3 successfully terminated by tesla gate.", isNoisy: false, isSubtitles: false);
                    HintManager.ShowHint(player, "🎙️ CASSIE broadcast received.", 3f);
                    break;

                case "random_scp":
                    string scpRole = Config.GlobalSettings.ValidScps.GetRandom();
                    if (Enum.TryParse(scpRole, out RoleTypeId scpId))
                    {
                        player.Role.Set(scpId);
                        HintManager.ShowHint(player, "🧪 You transformed into... something.", 3f);
                    }
                    break;

                case "inventory_reset":
                    player.ClearInventory();
                    HintManager.ShowHint(player, "📦 Your pockets are empty now.", 3f);
                    break;

                case "class_swap":
                    switch (player.Role.Type)
                    {
                        case RoleTypeId.Scientist:
                            player.Role.Set(RoleTypeId.ClassD);
                            break;
                        case RoleTypeId.ClassD:
                            player.Role.Set(RoleTypeId.Scientist);
                            break;
                        case RoleTypeId.ChaosConscript:
                        case RoleTypeId.ChaosRifleman:
                            player.Role.Set(RoleTypeId.NtfSergeant);
                            break;
                        case RoleTypeId.ChaosMarauder:
                        case RoleTypeId.ChaosRepressor:
                            player.Role.Set(RoleTypeId.NtfCaptain);
                            break;
                        case RoleTypeId.NtfPrivate:
                        case RoleTypeId.NtfSergeant:
                        case RoleTypeId.NtfSpecialist:
                            player.Role.Set(RoleTypeId.ChaosRifleman);
                            break;
                        case RoleTypeId.NtfCaptain:
                            var chaosOptions = new List<RoleTypeId>
                            {
                                RoleTypeId.ChaosMarauder,
                                RoleTypeId.ChaosRepressor
                            };
                            player.Role.Set(chaosOptions.RandomItem());
                            break;
                    }

                    HintManager.ShowHint(player, "🔄 Your role has been altered.", 3f);
                    break;

                case "instant_explosion":
                    var boom = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                    boom.FuseTime = 0.1f;
                    boom.SpawnActive(player.Position + Vector3.up, player);
                    HintManager.ShowHint(player, "💥 BOOM.", 3f);
                    break;

                case "player_swap":
                    var candidates = Player.List.Where(p => p != player && p.IsAlive && !Config.GlobalSettings.IgnoredRolesForSwap.Contains(p.Role.Type.ToString())).ToList();
                    if (candidates.Count > 0)
                    {
                        var other = candidates.GetRandom();
                        (player.Position, other.Position) = (other.Position, player.Position);

                        HintManager.ShowHint(player, $"🔄 You swapped positions with {other.Nickname}!", 3f);
                        HintManager.ShowHint(other, $"🔄 You swapped positions with {player.Nickname}!", 3f);
                    }
                    break;

                case "kick":
                    Timing.CallDelayed(1f, () => player.Kick(Config.GlobalSettings.KickReason));
                    break;

                case "spectator_replace":
                    var spec = Player.List.FirstOrDefault(p => p.Role == RoleTypeId.Spectator);
                    if (spec != null)
                    {
                        player.Role.Set(RoleTypeId.Spectator);
                        spec.Role.Set(player.Role.Type);
                        HintManager.ShowHint(spec, $"👻 You replaced {player.Nickname}!", 3f);
                    }
                    break;

                case "tesla_tp":
                    var tesla = Room.List.Where(r => r.Type == RoomType.HczTesla).ToList().GetRandom();
                    if (tesla != null)
                    {
                        player.Position = tesla.Position + Vector3.up;
                        HintManager.ShowHint(player, "⚡ Teleported to Tesla zone...", 3f);
                    }
                    break;

                case "inventory_swap":
                    var target = Player.List.FirstOrDefault(p => p != player && p.IsAlive && !Config.GlobalSettings.IgnoredRolesForSwap.Contains(p.Role.Type.ToString()));
                    if (target != null)
                    {
                        var tempInv = player.Items.ToList();
                        player.ClearInventory();

                        foreach (var item in target.Items)
                            player.AddItem(item.Type);

                        target.ClearInventory();
                        foreach (var item in tempInv)
                            target.AddItem(item.Type);

                        HintManager.ShowHint(player, $"🎒 You swapped inventories with {target.Nickname}", 3f);
                        HintManager.ShowHint(target, $"🎒 You swapped inventories with {player.Nickname}", 3f);
                    }
                    break;

                case "handcuff":
                    player.Handcuff();
                    player.ClearInventory();
                    HintManager.ShowHint(player, "🔗 You’ve been cuffed and disarmed!", 3f);
                    break;

                case "random_tp":
                    var room = Config.GlobalSettings.RoomsToTeleport.GetRandom();
                    if (Enum.TryParse(room, out RoomType roomName))
                    {
                        var targetRoom = Room.Get(roomName);
                        if (targetRoom != null)
                        {
                            player.Position = targetRoom.Position + Vector3.up;
                            HintManager.ShowHint(player, $"📍 You've been relocated to {roomName}.", 3f);
                        }
                    }
                    break;

                case "infectious_touch":
                    // Placeholder: This can be expanded with a custom infection system later
                    HintManager.ShowHint(player, "☣️ Your touch is... contagious?", 3f);
                    break;

                default:
                    HintManager.ShowHint(player, "❌ The coin backfired... but nothing happened.", 3f);
                    break;
            }

            if (Config.Debug)
                Log.Debug($"[EffectHandler] BAD effect applied: {selected}");
        }

        private static void ApplyRandomEffect(Player player, bool isGood)
        {
            var list = isGood ? Config.GoodEffects : Config.BadEffects;
            if (list == null || list.Count == 0)
                return;

            var effectName = list[UnityEngine.Random.Range(0, list.Count)];
            if (Enum.TryParse(effectName, out Exiled.API.Enums.EffectType effect))
            {
                player.EnableEffect(effect, 5); // default 5s
                HintManager.ShowHint(player, $"🧬 You feel different... ({effectName})", 3f);

                if (Config.Debug)
                    Log.Debug($"[EffectHandler] {player.Nickname} received effect: {effectName}");
            }
        }

        private static string GetWeightedRandom(Dictionary<string, int> map)
        {
            int total = 0;
            foreach (var val in map.Values)
                total += val;

            int roll = UnityEngine.Random.Range(0, total);
            foreach (var pair in map)
            {
                if (roll < pair.Value)
                    return pair.Key;
                roll -= pair.Value;
            }

            return map.Keys.First(); // fallback
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
