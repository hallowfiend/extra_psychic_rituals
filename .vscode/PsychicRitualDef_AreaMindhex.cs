using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using RimWorld;

namespace Gythian_ExtraPsychicRituals
{

    public class PsychicRitualDef_AreaMindhex : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve durationHoursFromQualityCurve;
        public int goodwillLoss;

        public override List<PsychicRitualToil> CreateToils(
          PsychicRitual psychicRitual,
          PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> toils = base.CreateToils(psychicRitual, graph);
            toils.Add((PsychicRitualToil)new PsychicRitualToil_AreaMindhex(this.InvokerRole));
            return toils;
        }

        public override TaggedString OutcomeDescription(
          FloatRange qualityRange,
          string qualityNumber,
          PsychicRitualRoleAssignments assignments)
        {
            return this.outcomeDescription.Formatted((NamedArgument)Mathf.FloorToInt(this.durationHoursFromQualityCurve.Evaluate(qualityRange.min) * 2500f).ToStringTicksToPeriod());
        }
    }
}
   