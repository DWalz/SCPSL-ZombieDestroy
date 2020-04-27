using EXILED;

namespace SCPSL_ZombieDestroy
{
    public class Configs
    {
        internal static bool IsEnabled = true;
        internal static int TwoZombiesLimit;
        internal static bool BreakUnbreakable;

        public static void ReloadConfigs()
        {
            if (!Plugin.Config.GetBool("zombie_destroy_enabled", true))
            {
                IsEnabled = false;
                return;
            }   
            TwoZombiesLimit = Plugin.Config.GetInt("two_zombies_player_limit", 6);

            BreakUnbreakable = Plugin.Config.GetBool("allow_unbreakable_destroy", true);
        }
        
    }
}