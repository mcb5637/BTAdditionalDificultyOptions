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
    public class DifficultyOptionsMain
    {
        public static Action<SimGameState, string, string, string> ApplySettings;
        public static Action<SimGameState, string, string[]> ApplyEventResult;

        public static void Init(string directory, string settingsJSON)
        {
            ApplySettings += ApplyTags;
            ApplyEventResult += RemoveMechs;
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

        private static void RemoveMechs(SimGameState s, string constant, string[] additionals)
        {
            if (constant.Equals("RemoveMech"))
            {
                List<int> toremove = new List<int>();
                foreach (string cid in additionals)
                {
                    foreach (KeyValuePair<int, MechDef> kv in s.ActiveMechs)
                    {
                        if (kv.Value.Chassis.Description.Id.Equals(cid))
                        {
                            toremove.Add(kv.Key);
                        }
                    }
                    if (s.GetItemCount(cid, typeof(MechDef), SimGameState.ItemCountType.UNDAMAGED_ONLY) > 0)
                    {
                        s.RemoveItemStat(cid, typeof(MechDef), false);
                    }
                }
                foreach (int k in toremove)
                    s.ActiveMechs.Remove(k);
            }
        }
    }
}
