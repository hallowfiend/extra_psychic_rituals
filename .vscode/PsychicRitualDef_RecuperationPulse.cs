using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using RimWorld;

namespace Gythian_ExtraPsychicRituals
{

    public class PsychicRitualDef_RecuperationPulse : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve durationDaysFromQualityCurve;

        public override List<PsychicRitualToil> CreateToils(
          PsychicRitual psychicRitual,
          PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> toils = base.CreateToils(psychicRitual, graph);
            toils.Add((PsychicRitualToil)new PsychicRitualToil_RecuperationPulse(this.InvokerRole));
            return toils;
        }

        public override TaggedString OutcomeDescription(
          FloatRange qualityRange,
          string qualityNumber,
          PsychicRitualRoleAssignments assignments)
        {
            return this.outcomeDescription.Formatted((NamedArgument)Mathf.FloorToInt(this.durationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays());
        }
    }
}

