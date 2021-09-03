using System;
using System.Collections;
using System.IO;

namespace ATVTW {
    /// <summary>
    /// Summary description for Ancillary.
    /// </summary>
    class Ancillary : ATVTW {
        public string imageFilename;
        public string descriptionName;
        public string effectsDescName;
        public bool uniqueAncillary;

        public ArrayList excludedAncillaries;
        public ArrayList excludeCultures;
        public ArrayList effects;  //arraylist of Effect objects for effect lines
        public ArrayList religiousBelief; //arraylist of Effect objects pertaining to religious_beliefs
        public ArrayList religiousOrder; //arraylist of Effect objects pertaining to religious_order

        public Ancillary() : this("") { }

        public Ancillary(string name)
            : base(name) {

            uniqueAncillary = false;
            excludedAncillaries = new ArrayList();
            excludeCultures = new ArrayList();
            effects = new ArrayList();
            religiousBelief = new ArrayList();
            religiousOrder = new ArrayList();
        }

        public bool readAncillary(ref StreamReader reader, ref string errorMsg, ref string[] tokenizedInput, ArrayList enums, ArrayList attributes, ref int lineNumber) {
            bool valid = true;
            bool readRequired = true;
            errorMsg = Environment.NewLine;
            string aString = name;

            //check to see if the ancillary name, is in export_ancillaries.txt
            if(Common.findElement(name, enums) == -1) {
                name = "{" + name + "}";
            }

            //check for duplicate ancillary names
            if(Common.ancillaries.Count > 0 && Common.findElement(name, Common.ancillaries) > -1) {
                name = "(" + name + ")";
            }

            //parse the Image line
            if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
            } else if(tokenizedInput[0] != "Image") {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: Expecting Image Found - " + tokenizedInput[0];
            } else if(tokenizedInput.Length != 2) {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Image";
            } else {
                imageFilename = tokenizedInput[1];
            }//end parse Image

            //read the next line and check for optional unique tag
            if(valid) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if(tokenizedInput[0] != "Unique") {
                    readRequired = false;
                } else if(tokenizedInput.Length != 1) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Unique";
                } else {
                    uniqueAncillary = true;
                    readRequired = true;
                }
            }//end parsing Unique

            //parse the optional ExcludedAncillaries line, reading a line if necessary
            if(valid) {
                if ( readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if(tokenizedInput[0] != "ExcludedAncillaries") {
                    readRequired = false;
                } else if(tokenizedInput.Length < 2) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: No excluded ancillaries listed on ExcludedAncillaries line";
                } else {
                    readRequired = true;
                    for(int i = 1; i < tokenizedInput.Length; i += 1) {
                        excludedAncillaries.Add(tokenizedInput[i]);
                    }
                }
            }//end parsing ExcludedAncillaries

            //parse the optional ExcludeCulture line, reading a line if necessary
            if(valid) {
                if ( readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if(tokenizedInput[0] != "ExcludeCultures") {
                    readRequired = false;
                } else {
                    for(int i = 1; i < tokenizedInput.Length; i += 1) {
                        excludeCultures.Add(Common.validBaseCulture(tokenizedInput[i]));
                    }
                    readRequired = true;
                }
            }//end parsing ExcludeCultures

            //parse the level description
            if(valid){
                if ( readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                }else if(tokenizedInput[0] != "Description"){
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Expecting Description - Found " + tokenizedInput[0];
                }else{
                    if( tokenizedInput.Length != 2 ) {
                        valid = false;
                        errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Description";
                    }else{
                        descriptionName = tokenizedInput[1];

                        //check for the level description being in export_VnVs.txt, if it isn't mark the level name
                        if(Common.findElement(descriptionName, enums) == -1){
                            descriptionName = "{" + descriptionName + "}";
                        }
                    }
                }
            }//end parsing description

            //parse the level effectdescription
            if(valid) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else if(tokenizedInput[0] != "EffectsDescription") {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Expecting EffectsDescription - Found " + tokenizedInput[0];
                } else {
                    if(tokenizedInput.Length != 2) {
                        valid = false;
                        errorMsg += "Line " + lineNumber + " - Error: Too many arguments for EffectsDescription";
                    } else {
                        effectsDescName = tokenizedInput[1];

                        //check for the level effectsdescription being in export_ancillaries.txt, if it isn't mark the level name
                        if(Common.findElement(effectsDescName, enums) == -1) {
                            effectsDescName = "{" + effectsDescName + "}";
                        }
                    }
                }
            }//end parsing effectsdescription

            //parse the levels effects lines, do a initial read line if necessary
            if(valid) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else {
                    while(valid && (tokenizedInput[0] == "Effect" || tokenizedInput[0] == "Religious_Belief")) {
                        if(tokenizedInput.Length != 3) {
                            valid = false;
                            errorMsg += "Line " + lineNumber + " - Error: Too many arguments for " + tokenizedInput[0] + " " + tokenizedInput[1];
                        } else {
                            tokenizedInput[2] = Common.testParse(tokenizedInput[2], true);
                            if(tokenizedInput[0] == "Effect"){
                                if ( Common.findElement(tokenizedInput[1], attributes) == -1 ) {
                                    tokenizedInput[1] = "{" + tokenizedInput[1] + "}";
                                }
                                effects.Add(new Effect(tokenizedInput[1], tokenizedInput[2]));
                            } else if(tokenizedInput[0] == "Religious_Belief") {
                                tokenizedInput[1] = Common.validBaseReligion(tokenizedInput[1]);
                                religiousBelief.Add(new Effect(tokenizedInput[1], tokenizedInput[2]));
                            } else {
                                tokenizedInput[1] = Common.validBaseReligion(tokenizedInput[1]);
                                religiousOrder.Add(new Effect(tokenizedInput[1], tokenizedInput[2]));
                            }
                            if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                                valid = false;
                                errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                            }
                        }
                    }
                }
            }//end parsing Effects lines

            return valid;
        }
    }
}