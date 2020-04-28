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
            if (!Plugin.Config.GetBool("zd_enabled", true))
            {
                IsEnabled = false;
                return;
            }   
            
            TwoZombiesLimit = Plugin.Config.GetInt("zd_two_zombies_limit", 6);
            BreakUnbreakable = Plugin.Config.GetBool("zd_allow_unbreakable_destroy", true);
        }
        
    }
}