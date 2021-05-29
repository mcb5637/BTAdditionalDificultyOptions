using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using Harmony;

[assembly:AssemblyVersion("0.1.0.0")]

namespace BTAdditionalDificultyOptions
{
    class DifficultyOptionsMain
    {
        public static Action<SimGameState, string, string, string> ApplySettings;

        public static void Init(string directory, string settingsJSON)
        {
            ApplySettings += ApplyTags;
            HarmonyInstance harmony = HarmonyInstance.Create("com.github.mcb5637.BTAdditionalDificultyOptions");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static void ApplyTags(SimGameState s, string id, string key, string val)
        {
            if (s == null)
                return;
            if (key.Equals("AddSimGameTag") && !s.CompanyTags.Contains(val))
                s.CompanyTags.Add(val);
            if (key.Equals("RemoveSimGameTag") && s.CompanyTags.Contains(val))
                s.CompanyTags.Remove(val);
        }
    }
}
