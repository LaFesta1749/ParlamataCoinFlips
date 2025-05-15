using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using InventorySystem.Items;
using UnityEngine;

namespace ParlamataCoinFlips.Handlers
{
    public static class CoinUsesHandler
    {
        // 🔄 Пер-играч: брой използвания
        private static readonly Dictionary<string, int> Uses = new();

        // ⏱️ Пер-играч: cooldown timestamp
        private static readonly Dictionary<string, float> Cooldowns = new();

        // 🪙 Пер-стотинка: брой използвания
        private static readonly Dictionary<ushort, int> CoinSerialUses = new();

        /// <summary> Проверява дали играч е в cooldown. </summary>
        public static bool IsInCooldown(Player player)
        {
            if (!Cooldowns.TryGetValue(player.UserId, out float endTime))
                return false;

            return Time.realtimeSinceStartup < endTime;
        }

        /// <summary> Задава нов cooldown за играча. </summary>
        public static void SetCooldown(Player player, float duration)
        {
            Cooldowns[player.UserId] = Time.realtimeSinceStartup + duration;
        }

        /// <summary> Опитва да използва Coin според maxUses за този рунд. </summary>
        public static bool TryUse(Player player, int maxUses)
        {
            string id = player.UserId;

            if (!Uses.ContainsKey(id))
                Uses[id] = 0;

            if (Uses[id] >= maxUses)
                return false;

            Uses[id]++;
            return true;
        }

        /// <summary> Получава оставащите uses за дадена Coin по сериал. </summary>
        public static int GetUses(ItemBase itemBase)
        {
            if (itemBase == null)
                return 0;

            ushort serial = itemBase.ItemSerial;

            if (!CoinSerialUses.TryGetValue(serial, out int uses))
            {
                uses = UnityEngine.Random.Range(
                    Plugin.Instance.Config.MinMaxDefaultCoins[0],
                    Plugin.Instance.Config.MinMaxDefaultCoins[1]
                );

                CoinSerialUses[serial] = uses;

                if (Plugin.Instance.Config.Debug)
                    Log.Debug($"[CoinUsesHandler] Assigned {uses} uses to coin {serial}");
            }

            return uses;
        }

        /// <summary> Намалява uses с 1. Ако са изчерпани, премахва предмета. </summary>
        public static void UseCoin(Player player, ItemBase itemBase)
        {
            if (itemBase == null)
                return;

            ushort serial = itemBase.ItemSerial;
            int usesLeft = GetUses(itemBase) - 1;

            if (usesLeft <= 0)
            {
                // Опитваме директно чрез ItemBase
                var item = player.Items.FirstOrDefault(i => i.Base == itemBase);
                if (item != null)
                {
                    if (Plugin.Instance.Config.Debug)
                        Log.Debug($"[CoinUsesHandler] Removing used-up coin (serial {serial})");

                    player.RemoveItem(item);
                }

                CoinSerialUses.Remove(serial);

                if (Plugin.Instance.Config.Debug)
                    Log.Debug($"[CoinUsesHandler] Coin {serial} broke after last use.");

                HintManager.ShowHint(player, "💥 The coin was used too much and got destroyed!", 3f);
            }
            else
            {
                CoinSerialUses[serial] = usesLeft;

                if (Plugin.Instance.Config.Debug)
                    Log.Debug($"[CoinUsesHandler] Coin {serial} now has {usesLeft} uses left.");
            }
        }

        /// <summary> Изчиства всички runtime данни. </summary>
        public static void Reset()
        {
            Uses.Clear();
            Cooldowns.Clear();
            CoinSerialUses.Clear();
        }
    }
}
