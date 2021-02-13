using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using Harmony;

namespace BTAdditionalDificultyOptions
{
    [HarmonyPatch(typeof(SimGameDifficulty), "ApplySetting")]
    class SimGameDifficulty_ApplySetting
    {
        public static void Prefix(SimGameDifficulty __instance, SimGameState ___simState, SimGameDifficulty.DifficultySetting setting, int index, ref List<SimGameDifficulty.DifficultyConstantValue> __state)
        {
            __state = null;
            if (setting == null)
			{
				return;
			}
            SimGameDifficulty.DifficultyOption opt = setting.Options[index];
            if (opt.DifficultyConstants != null)
            {
                __state = opt.DifficultyConstants;
                foreach (SimGameDifficulty.DifficultyConstantValue v in opt.DifficultyConstants)
                {
                    if (v.ConstantType.Equals("AdditionalSettings"))
                    {
                        DifficultyOptionsMain.ApplySettings(___simState, setting.ID, v.ConstantName, v.ConstantValue);
                    }
                }
                opt.DifficultyConstants = opt.DifficultyConstants.Where((x) => !x.ConstantType.Equals("AdditionalSettings")).ToList();
            }
        }

        public static void Postfix(SimGameDifficulty.DifficultySetting setting, int index, List<SimGameDifficulty.DifficultyConstantValue> __state)
        {
            if (__state != null)
                setting.Options[index].DifficultyConstants = __state;
        }
    }
}
