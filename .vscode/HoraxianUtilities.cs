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
                HediffDef.Named("NeurosisPulse"),
                HediffDef.Named("PleasurePulse"),
                HediffDef.Named("Gythian_ProliferationPulse"),
                HediffDef.Named("Gythian_RecuperationPulse")
            };
            var pulses = hediffdefs.Intersect(pulseList);
            return pulses.Any();
        }

    }
}
