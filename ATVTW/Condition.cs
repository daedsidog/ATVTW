using System;
using System.Collections;

namespace ATVTW {
    /// <summary>
    /// Summary description for Condition.
    /// </summary>
    class Condition : ATVTW {
        public ArrayList parameters;
        public ArrayList parameterNames;
        public bool boolValue;

        public Condition() : this("", false) { }

        public Condition(string name, bool notToken)
            : base(name) {
            parameters = new ArrayList();
            parameterNames = new ArrayList();
            boolValue = notToken;
        }

        public override string ToString() {
            string result = name;

            for(int i = 0; i < parameters.Count; i += 1){
                result += " " + parameters[i];
            }

            if(boolValue){
                result = "not " + result;
            }

            return result;
        }

        public bool hasNoErrors() {
            bool valid = true;
            if ( name.IndexOf("{") > -1 ) {
                valid = false;
            }else{
                for(int i = 0; i < parameters.Count && ( valid = parameters[i].ToString().IndexOf("{") == -1 ); i += 1);
            }

            return valid;
        }
    }
}