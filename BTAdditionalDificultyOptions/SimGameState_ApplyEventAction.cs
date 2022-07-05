using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using BattleTech;

namespace BTAdditionalDificultyOptions
{
    [HarmonyPatch(typeof(SimGameState), "ApplyEventAction")]
    class SimGameState_ApplyEventAction
    {
        public static bool Prefix(SimGameResultAction action, object additionalObject, ref bool __result)
        {
            SimGameState simulation = UnityGameInstance.BattleTechGame.Simulation;
            if (simulation == null)
                return true;
            if (action.Type == SimGameResultAction.ActionType.System_SetDropship)
            {
                if (!action.value.ToLower().Contains("leopard") && !action.value.ToLower().Contains("argo"))
                {
                    DifficultyOptionsMain.ApplyEventResult(simulation, action.valueConstant, action.additionalValues);
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }
}
