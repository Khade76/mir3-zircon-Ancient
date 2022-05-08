using Library;
using Server.Envir;

namespace Server.Models.Monsters
{
    public class OmaWarlock : SkeletonAxeThrower
    {
        protected override void Attack()
        {
            switch (SEnvir.Random.Next(5))
            {

                case 0:
                    AttackMagic(MagicType.AdamantineFireBall, Element.Fire, false, GetDC());
                    break;
                default:
                    AttackMagic(MagicType.FireBall, Element.Fire, false, GetDC() * 2 / 3);
                    break;
            }
        }
    }
}
