using System;
using System.Collections.Generic;
using System.Text;

namespace ATVTW {

    class ATVTW {

        #region Fields

        public string name;

        #endregion

        #region Constructors

        public ATVTW() : this("") { }

        public ATVTW(string name) {
            this.name = name;
        }

        #endregion

        #region Operators

        public static bool operator ==(ATVTW a, ATVTW b) {
            return a.Equals(b);
        }

        public static bool operator ==(string a, ATVTW b) {
            return b.Equals(a);
        }

        public static bool operator ==(ATVTW b, string a) {
            return a.Equals(b);
        }

        public static bool operator !=(ATVTW a, ATVTW b) {
            return !a.Equals(b);
        }

        public static bool operator !=(string a, ATVTW b) {
            return !b.Equals(a);
        }

        public static bool operator !=(ATVTW b, string a) {
            return !a.Equals(b);
        }

        #endregion

        #region Overridden Methods

        public override bool Equals(Object obj) {
            bool equal = false;
            if(obj is ATVTW) {
                equal = ((ATVTW)obj).name == name;
            }
            if(obj is string) {
                equal = CleanName() == (string)obj;
            }
            return equal;
        }

        public override int GetHashCode() {
            return name.GetHashCode();
        }

        public override string ToString() {
            return name;
        }

        #endregion

        #region Private Methods

        private string CleanName() {
            string aString = name;

            if(aString.StartsWith("(") && aString.EndsWith(")")) aString = aString.Substring(1, aString.Length - 2);
            if(aString.StartsWith("[") && aString.EndsWith("]")) aString = aString.Substring(1, aString.Length - 2);
            if(aString.StartsWith("{") && aString.EndsWith("}")) aString = aString.Substring(1, aString.Length - 2);

            return aString;
        }

        #endregion
    }
}


