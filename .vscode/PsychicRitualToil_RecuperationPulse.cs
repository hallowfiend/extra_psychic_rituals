using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Gythian_ExtraPsychicRituals
{
    public class PsychicRitualToil_RecuperationPulse : PsychicRitualToil
    {
        private PsychicRitualRoleDef invokerRole;

        protected PsychicRitualToil_RecuperationPulse()
        {
        }

        public PsychicRitualToil_RecuperationPulse(PsychicRitualRoleDef invokerRole)
        {
            this.invokerRole = invokerRole;
        }

        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            base.Start(psychicRitual, parent);
            Pawn invoker = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
            float duration = ((PsychicRitualDef_RecuperationPulse)psychicRitual.def).durationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
            psychicRitual.ReleaseAllPawnsAndBuildings();
            if (invoker == null)
                return;
            this.ApplyOutcome(psychicRitual, invoker, duration);
        }

        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration)
        {
            foreach (Pawn pawn in invoker.Map.mapPawns.AllHumanlikeSpawned)
            {
                if ((double)pawn.GetStatValue(StatDefOf.PsychicSensitivity) > 0.0)
                {
                    List<HediffDef> pulseList = new List<HediffDef>
                        {
                    HediffDefOf.NeurosisPulse,
                    HediffDefOf.PleasurePulse,
                    InternalDefOf.Gythian_ProliferationPulse,
                    InternalDefOf.Gythian_RecuperationPulse
                        };
                    if (HoraxianUtilities.HasAnyPulse(pawn) == true)
                        foreach (Hediff pawnhediff in pawn.health.hediffSet.hediffs)
                            foreach (HediffDef pulse in pulseList)
                                if (pawnhediff.def == pulse)
                                    pawn.health.RemoveHediff(pawnhediff);
                    Hediff hediff = HediffMaker.MakeHediff(InternalDefOf.Gythian_RecuperationPulse, pawn);
                    HediffComp_Disappears comp = hediff.TryGetComp<HediffComp_Disappears>();
                    if (comp != null)
                        comp.ticksToDisappear = Mathf.RoundToInt(duration * 60000f);
                    pawn.health.AddHediff(hediff);
                }
            }
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate((NamedArgument)psychicRitual.def.label), "RecuperationPulseCompleteText".Translate((NamedArgument)(Thing)invoker, (NamedArgument)Mathf.FloorToInt(duration * 60000f).ToStringTicksToDays(), psychicRitual.def.Named("RITUAL")), LetterDefOf.NeutralEvent);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
        }
    }
}

