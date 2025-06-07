using Exiled.API.Features;
using HsmHint = HintServiceMeow.Core.Models.Hints.Hint;
using HintServiceMeow.Core.Enum;
using MEC;
using PlayerRoles;
using HintServiceMeow.Core.Utilities;
using HintServiceMeow;

namespace ParlamataCoinFlips.Handlers
{
    public static class HintManager
    {
        private static readonly Dictionary<string, HsmHint> ActiveHints = new();
        private static readonly Dictionary<string, CoroutineHandle> HintTimers = new();

        public static void ShowHint(Player player, string message, float duration = 3f)
        {
            if (!player.IsAlive || player.Role == RoleTypeId.Spectator || player.ReferenceHub == null)
                return;

            string userId = player.UserId;
            float y = Plugin.Instance.Config.HintYCoordinate;

            // Stop old timer if it exists
            if (HintTimers.TryGetValue(userId, out var existingHandle))
                Timing.KillCoroutines(existingHandle);

            // Create hint if not already cached
            if (!ActiveHints.TryGetValue(userId, out var hint))
            {
                hint = new HsmHint
                {
                    FontSize = 24,
                    YCoordinate = y,
                    XCoordinate = 0f,
                    Alignment = HintAlignment.Center,
                    Text = message
                };

                PlayerDisplay.Get(player).AddHint(hint);
                ActiveHints[userId] = hint;

                //if (Plugin.Instance.Config.Debug)
                    //Log.Debug($"[HintManager] Created new hint for {player.Nickname}");
            }

            // Update text
            hint.Text = message;

            // Auto-remove after duration
            CoroutineHandle handle = Timing.RunCoroutine(RemoveHintDelayed(player, hint, duration));
            HintTimers[userId] = handle;
        }

        private static IEnumerator<float> RemoveHintDelayed(Player player, HsmHint hint, float delay)
        {
            yield return Timing.WaitForSeconds(delay);

            string userId = player.UserId;

            PlayerDisplay.Get(player).RemoveHint(hint);

            ActiveHints.Remove(userId);
            HintTimers.Remove(userId);

            //if (Plugin.Instance.Config.Debug)
                //Log.Debug($"[HintManager] Removed hint for {player.Nickname}");
        }

        public static void Clear(Player player)
        {
            string userId = player.UserId;

            if (HintTimers.TryGetValue(userId, out var handle))
                Timing.KillCoroutines(handle);

            if (ActiveHints.TryGetValue(userId, out var hint))
                PlayerDisplay.Get(player).RemoveHint(hint);

            ActiveHints.Remove(userId);
            HintTimers.Remove(userId);
        }
    }
}
