using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public static class FightsRepository
    {
        private static List<Fight> fights = new List<Fight> {
            new Fight(1, "Dino", "Joshua","Dino", 135000),
            new Fight(2, "Mike Tyson", "Dino","Dino", 250000),
            new Fight(3, "Jelena", "Connor McGregor", "Jelena",500000) 
        };

        public static IEnumerable<Fight> GetFights()
        {
            return fights;
        }
        public static Fight GetFight(int id)
        {
            return fights.Find(fight=>fight.Id==id);
        }
        public static void AddFight(Fight fight)
        {
            fights.Add(fight);
        }
        public static bool UpdateFight(Fight fight)
        {
          
            int fightUpdateIndex = fights.FindIndex(f => f.Id == fight.Id);
            if (fightUpdateIndex != -1)
            {
                fights[fightUpdateIndex].FirstFigher = fight.FirstFigher;
                fights[fightUpdateIndex].SecondFighter = fight.SecondFighter;
                fights[fightUpdateIndex].Winner = fight.Winner;
                fights[fightUpdateIndex].Viewers = fight.Viewers;
                return true;
            }return false;

        }
        public static void DeleteFight(int id)
        {
            fights.Remove(GetFight(id));
        }
    }
}