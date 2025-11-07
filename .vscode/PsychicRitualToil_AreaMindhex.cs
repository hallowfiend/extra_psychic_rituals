using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using static UnityEngine.GraphicsBuffer;

namespace Gythian_ExtraPsychicRituals
{
    public class PsychicRitualToil_AreaMindhex : PsychicRitualToil
    {
        private PsychicRitualRoleDef invokerRole;
        protected PsychicRitualToil_AreaMindhex()
        {
        }

        public PsychicRitualToil_AreaMindhex(PsychicRitualRoleDef invokerRole)
        {
            this.invokerRole = invokerRole;
        }

        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            base.Start(psychicRitual, parent);
            Pawn invoker = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
            float duration = ((PsychicRitualDef_AreaMindhex)psychicRitual.def).durationHoursFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
            int goodwill = ((PsychicRitualDef_AreaMindhex)psychicRitual.def).goodwillLoss;
            psychicRitual.ReleaseAllPawnsAndBuildings();
            if (invoker == null)
                return;
            this.ApplyOutcome(psychicRitual, invoker, duration, goodwill);
        }

        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration, int goodwill)
        {
            foreach (Pawn pawn in invoker.Map.mapPawns.AllHumanlikeSpawned)
            {
                if (((double)pawn.GetStatValue(StatDefOf.PsychicSensitivity) > 0.0) && pawn.Faction?.HostileTo(Faction.OfPlayer) == true)
                {
                    Hediff hediff = HediffMaker.MakeHediff(HediffDef.Named("Gythian_MaddeningMindhex"), pawn);
                    HediffComp_Disappears comp = hediff.TryGetComp<HediffComp_Disappears>();
                    if (comp != null)
                        comp.ticksToDisappear = Mathf.RoundToInt(duration * 2500f);
                    pawn.health.AddHediff(hediff);
                }
            }
            foreach (Faction allFaction in Find.FactionManager.AllFactions)
            {
                {
                    if (!allFaction.IsPlayer && !allFaction.defeated && !allFaction.HostileTo(Faction.OfPlayer))
                        Faction.OfPlayer.TryAffectGoodwillWith(allFaction, goodwill, reason: DefDatabase<HistoryEventDef>.GetNamed("Gythian_MindhexEchoes"));
                }
            }
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate((NamedArgument)psychicRitual.def.label), "AreaMindhexCompleteText".Translate((NamedArgument)(Thing)invoker, (NamedArgument)Mathf.FloorToInt(duration * 60000f).ToStringTicksToDays(), psychicRitual.def.Named("RITUAL")), LetterDefOf.NeutralEvent);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
        }
    }
}
