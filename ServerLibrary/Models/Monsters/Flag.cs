using System.Drawing;
using Library;
using Server.Envir;

namespace Server.Models.Monsters
{
    public class Flag : MonsterObject
    {
        public override bool Blocking => true;
        public override bool CanMove => false;

        public Flag()
        {
            NameColour = Color.SkyBlue;
        }

        public override void ProcessAction(DelayedAction action)
        {

            base.ProcessAction(action);
        }

        public override void ProcessSearch()
        {
            ProperSearch();
        }

        protected override bool InAttackRange()
        {
            return Target.CurrentMap == CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, ViewRange);
        }
        public override void ProcessNameColour()
        {
            NameColour = Color.SkyBlue;
        }

        public override int Attacked(MapObject ob, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            return 1;
        }

        public override bool ShouldAttackTarget(MapObject ob)
        {
            return CanAttackTarget(ob);
        }
        public override bool CanAttackTarget(MapObject ob)
        {
            return false;
        }

        protected override void Attack()
        {
            if (!CanAttackTarget(Target))
            {
                Target = null;
                return;
            }
        }

        private void Attack(MapObject ob)
        {

        }

        public override void Activate()
        {
            if (Activated)
                return;

            Activated = true;
            SEnvir.AddActiveObject(this);
        }
        public override void DeActivate()
        {
        }

    }
}
