using System;
using System.Collections;
using System.IO;

namespace ATVTW {
    /// <summary>
    /// Summary description for TraitLevel.
    /// </summary>
    public class Level {
        public string name;
        public string descriptionName;
        public string effectsDescName;
        public string loseMessageName;
        public string gainMessageName;
        public string epithetName;

        public string threshhold;
        public ArrayList effects;				// arraylist of Effect objects
        public ArrayList religiousBelief;             // arraylist of Effect object pertaining to religious_beliefs
        public ArrayList religiousOrder;             // arraylist of Effect object pertaining to religious_beliefs

        public Level() : this("") { }

        public Level(string levelName) {
            name = levelName;
            effects = new ArrayList();
            religiousBelief = new ArrayList();
            religiousOrder = new ArrayList();
        }

        public bool readLevel(ref StreamReader reader, ref string errorMsg, ref string[] tokenizedInput, ArrayList enums, ArrayList attributes, ref int lineNumber) {
            bool valid = true;
            bool readRequired = true;
            errorMsg += Environment.NewLine;

            //check for the level name being in export_VnVs.txt, if it isn't mark the level name
            if(Common.findElement(name, enums) == -1) {
                name = "{" + name + "}";
            }

            //parse the level description
            if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: End of file found.";
            } else if(tokenizedInput[0] != "Description") {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: Expecting Description - Found " + tokenizedInput[0];
            } else {
                if(tokenizedInput.Length != 2) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Description";
                } else {
                    descriptionName = tokenizedInput[1];

                    //check for the level description being in export_VnVs.txt, if it isn't mark the level name
                    if(Common.findElement(descriptionName, enums) == -1) {
                        descriptionName = "{" + descriptionName + "}";
                    }
                }
            }//end parse Description

            //parse the level effectdescription
            if(valid) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else if(tokenizedInput[0] != "EffectsDescription") {
                    valid = false;
                    errorMsg += lineNumber + " - Error: Expecting EffectsDescription - Found " + tokenizedInput[0];
                } else {
                    if(tokenizedInput.Length != 2) {
                        valid = false;
                        errorMsg += "Line " + lineNumber + " - Error: Too many arguments for EffectsDescription";
                    } else {
                        effectsDescName = tokenizedInput[1];

                        //check for the level effectsdescription being in export_VnVs.txt, if it isn't mark the level name
                        if(Common.findElement(effectsDescName, enums) == -1) {
                            effectsDescName = "{" + effectsDescName + "}";
                        }
                    }
                }
            }//end parse EffectsDescription

            //parse the level gainmessage, reading a line if necessary
            if(valid) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else if(tokenizedInput[0] != "GainMessage") {
                    readRequired = false;
                    gainMessageName = "";
                } else if(tokenizedInput.Length != 2) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for GainMessage";
                } else {
                    readRequired = true;
                    gainMessageName = tokenizedInput[1];

                    //check for the level gainmessage being in export_VnVs.txt, if it isn't mark the level name
                    if(Common.findElement(gainMessageName, enums) == -1) {
                        gainMessageName = "{" + gainMessageName + "}";
                    }
                }
            }//end parse GainMessage

            //parse the level losemessage, reading a line if necessary
            if(valid) {
                if ( readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else if(tokenizedInput[0] != "LoseMessage") {
                    readRequired = false;
                    loseMessageName = "";
                } else if(tokenizedInput.Length != 2) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for LoseMessage";
                } else {
                    readRequired = true;
                    loseMessageName = tokenizedInput[1];
                    if(Common.findElement(loseMessageName, enums) == -1) {
                        loseMessageName = "{" + loseMessageName + "}";
                    }
                }
            }//end parse LoseMessage

            //parse the level epithet, reading a line if necessary
            if(valid) {
                if ( readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else if(tokenizedInput[0] != "Epithet") {
                    readRequired = false;
                    epithetName = "";
                } else if(tokenizedInput.Length != 2) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Epithet";
                } else {
                    readRequired = true;
                    epithetName = tokenizedInput[1];
                    if(Common.findElement(epithetName, enums) == -1) {
                        epithetName = "{" + epithetName + "}";
                    }
                }
            }//end parse Epithet

            //parse the level threshold, reading a line if necessary
            if(valid) {
                if ( readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else if(tokenizedInput[0] != "Threshold") {
                    valid = false;
                    errorMsg += lineNumber + " - Error: Expecting Threshold - Found " + tokenizedInput[0];
                } else if(tokenizedInput.Length != 2) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Threshold";
                } else {
                    threshhold = Common.testParse(tokenizedInput[1], false);
                }
            }//end parse Threshold

            //parse the levels effects lines, do a initial read line if necessary
            if(valid) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else {
                    while(valid && (tokenizedInput[0] == "Effect" || tokenizedInput[0] == "Religious_Belief"  || tokenizedInput[0] == "Religious_Order")) {
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
            }//end parse Effect lines

            return valid;
        }
    }
}