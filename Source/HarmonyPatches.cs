﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using RimWorld;
using Verse;
using UnityEngine;
using Verse.AI;
using RimWorld.Planet;
using System.Reflection.Emit;

namespace SyrDoorMats
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("Syrchalis.Rimworld.Doormats");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Pawn_PathFollower), "SetupMoveIntoNextCell")]
    public static class SetupMoveIntoNextCellPatch
    {
        [HarmonyPostfix]
        public static void SetupMoveIntoNextCell_Postfix(Pawn_PathFollower __instance, Pawn ___pawn)
        {
            Building_DoorMat building_DoorMat = ___pawn.Map.thingGrid.ThingAt<Building_DoorMat>(__instance.nextCell);
            if (building_DoorMat != null)
            {
                building_DoorMat.Notify_PawnApproaching(___pawn);
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new Type[] { typeof(Pawn), typeof(IntVec3) })]
    public static class CostToMoveIntoCellPatch
    {
        [HarmonyPostfix]
        public static void CostToMoveIntoCell_Postfix(ref int __result, Pawn_PathFollower __instance, Pawn ___pawn, Pawn pawn, IntVec3 c)
        {
            if (DoorMatsSettings.slowdown > 0)
            {
                Building_DoorMat building_DoorMat = ___pawn.Map.thingGrid.ThingAt<Building_DoorMat>(c);
                if (building_DoorMat != null)
                {
                    __result += DoorMatsSettings.slowdown;
                }
            }
        }
    }
}
