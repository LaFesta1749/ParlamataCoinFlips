using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;

namespace ParlamataCoinFlips.Utilities
{
    public static class PlayerExtensions
    {
        public static bool IsValidTarget(this Player player)
        {
            return player is { IsAlive: true } &&
                   player.Role.Side != Side.Scp &&
                   player.Role.Type is not RoleTypeId.None and not RoleTypeId.Spectator;
        }
    }
}
