using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace ATVTW {

    public static class Common {

        public const int MAX_EXCLUDED_ANCILLARIES = 3;
        public const int MAX_ANTITRAITS = 20;
        public const int MAX_CULTURES = 7;
        public const int MAX_AFFECTS_EDCT = 11;

        public static ArrayList traits;
        public static ArrayList ancillaries;
        public static ArrayList traitTriggers;
        public static ArrayList ancillaryTriggers;

        public static void resetVariables() {
            traits = new ArrayList();
            traitTriggers = new ArrayList();
            ancillaries = new ArrayList();
            ancillaryTriggers = new ArrayList();
        }

        public static int findElement(object obj, ArrayList aList) {
            int location = -1;

            for(int i = 0; i < aList.Count; i += 1) {
                if(aList[i].Equals(obj)) {
                    location = i;
                    break;
                }
            }

            return location;
        }

        public static int findElementPartialMatch(string str, ArrayList aList, int startPosition) {
            int location = -1;

            for(int i = 0; i < aList.Count; i += 1) {
                if(((ATVTW)aList[(i + startPosition) % aList.Count]).name.ToLower().IndexOf(str.ToLower()) > -1) {
                    location = (i + startPosition) % aList.Count;
                    break;
                }
            }

            return location;
        }

        public static string testParse(string input, bool canBeNegative) {
            int tempInt;
            string output;

            try {
                tempInt = Int16.Parse(input);
                if(!canBeNegative && tempInt <= 0) {
                    output = "{" + input + "}";
                }else if(canBeNegative && tempInt == 0) {
                    output = "{" + input + "}";
                }else{
                    output = input;
                }
            } catch {
                output = "{" + input + "}";
            }//end try catch block

            return output;
        }//end testParse

        public static string removeComments(string inputString) {
            if(inputString.IndexOf(";") > -1) {
                inputString = inputString.Substring(0, inputString.IndexOf(";"));
            }
            return inputString;
        }

        public static bool readLine(ref StreamReader reader, ref string[] output, ref int lineNumber) {
            string inputLine;
            char[] DELIMS = {' ', '\t', ','};
            bool valid = false;

            //read until a non-empty line (after comments have been removed) or until the end of the file
            while((inputLine = reader.ReadLine()) != null && (inputLine = Common.removeComments(inputLine).Trim()) == ""){
                lineNumber += 1;
            }
            lineNumber += 1;

            if(inputLine != null){
                valid = true;
                output = inputLine.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries);
            }
            return valid;
        }//end readLine

        //return the inputToken unchanged if the character type is valid, return the {inputToken} otherwise
        public static string validCharacterType(string inputToken) {
            if(inputToken != "spy" && inputToken != "assassin" && inputToken != "diplomat" && inputToken != "general" && inputToken != "family" && inputToken != "admiral" && inputToken != "named character" && inputToken != "all") {
                inputToken = "{" + inputToken + "}";
            }
            return inputToken;
        }

        public static string validBaseCulture(string inputToken) {
            if(inputToken != "roman" && inputToken != "greek" && inputToken != "egyptian" && inputToken != "carthaginian" && inputToken != "eastern" && inputToken != "barbarian" && inputToken != "hun" && inputToken != "nomad") {
                inputToken = "{" + inputToken + "}";
            }
            return inputToken;
        }

        public static string validBaseReligion(string inputToken) {
            if ( inputToken != "pagan" && inputToken != "christianity" && inputToken != "zoroastrian" ) {
                inputToken = "{" + inputToken + "}";
            }
            return inputToken;
        }
    }
}