using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApi.Models
{
    [DataContract]
    public class Fight
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FirstFigher { get; set; }
        [DataMember]
        public string SecondFighter { get; set; }
        [DataMember]
        public string Winner { get; set; }
        [DataMember]
        public int Viewers { get; set; }

        public Fight(int id, string firstFighter, string secondFighter, string winner, int viewers)
        {
            Id = id;
            FirstFigher = firstFighter;
            SecondFighter = secondFighter;
            Winner = winner;
            Viewers = viewers;
        }
    }
}