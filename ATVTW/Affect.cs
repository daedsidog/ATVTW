using System;

namespace ATVTW
{
	/// <summary>
	/// Summary description for TriggerAffect.
	/// </summary>
	class Affect : ATVTW
	{
		public string points;
		public string chance;

		public Affect() : this("","",""){}

        public Affect(string name, string chance): this(name,"",chance) { }

        public Affect(string name, string points, string chance) : base(name) {
            this.chance = chance;
            this.points = points;
        }

        public override string ToString() {
            return name.PadRight(30) + points.PadLeft(5) + chance.PadLeft(11);            
        }

        public bool hasError() {
            return name.IndexOf("{") > -1 || points.IndexOf("{") > -1 || chance.IndexOf("{") > -1;
        }
	}
}
