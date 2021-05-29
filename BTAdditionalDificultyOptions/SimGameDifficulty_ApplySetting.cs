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
        public static void Prefix(SimGameDifficulty __instance, bool force, SimGameState ___simState, SimGameDifficulty.DifficultySetting setting, int index,
            Dictionary<string, int> ___difficultyIndices,
            ref List<SimGameDifficulty.DifficultyConstantValue> __state)
        {
            __state = null;
            if (setting == null)
			{
				return;
            }
            if (!force && ___difficultyIndices[setting.ID] == index)
            {
                return;
            }
            if (index >= setting.Options.Count)
            {
                return;
            }
            if (!setting.Enabled)
            {
                return;
            }
            try
            {
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
            catch (Exception e)
            {

                FileLog.Log(e.ToString());
            }
        }

        public static void Postfix(SimGameDifficulty.DifficultySetting setting, int index, List<SimGameDifficulty.DifficultyConstantValue> __state)
        {
            if (__state != null)
                setting.Options[index].DifficultyConstants = __state;
        }
    }
}
