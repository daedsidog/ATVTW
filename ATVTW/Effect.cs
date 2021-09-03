using System;
using System.Collections;

namespace ATVTW {
    /// <summary>
    /// Summary description for Effect.
    /// </summary>
    public class Effect {
        public string name;
        public string pointsAllocated;

        public Effect() : this("", "0") { }

        public Effect(string name, string pointsAllocated) {
            this.name = name;
            this.pointsAllocated = pointsAllocated;
        }

        public override string ToString() {
            string result = name + " ";
            short tempShort;

            if(!Int16.TryParse(pointsAllocated, out tempShort) || tempShort < 0){
                result += pointsAllocated;
            }else{
                result += "+" + pointsAllocated;
            }
 
            return result;
        }
    }
}