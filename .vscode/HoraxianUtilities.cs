using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Gythian_ExtraPsychicRituals
{
    public static class HoraxianUtilities
    {
        public static bool HasAnyPulse(Pawn pawn)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            List<HediffDef> hediffdefs = new List<HediffDef> { };
            foreach(Hediff hediff in hediffs)
            {
                hediffdefs.Add(hediff.def);
            }
            List<HediffDef> pulseList = new List<HediffDef>
            {
                HediffDefOf.NeurosisPulse,
                HediffDefOf.PleasurePulse,
                InternalDefOf.Gythian_ProliferationPulse,
                InternalDefOf.Gythian_RecuperationPulse
            };
            var pulses = hediffdefs.Intersect(pulseList);
            return pulses.Any();
        }

    }
}
