using System;

namespace Server.Models
{
    public class Damagers
    {
        public PlayerObject attackers
        {
            get; set;
        }
        public int damage
        {
            get; set;
        }
        public DateTime inCombat
        {
            get; set;
        }

        public Damagers()
        {
            attackers = null;
            damage = 0;
            inCombat = new DateTime();
        }
    }
}