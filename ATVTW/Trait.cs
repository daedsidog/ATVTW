using System;
using System.Collections;
using System.IO;

namespace ATVTW {
    /// <summary>
    /// Trait represents a single trait. This is Not the same as a trait level, which is actually applied to the individual.
    /// TraitLevel objects have a Trait object embedded in them.
    /// </summary>
    class Trait : ATVTW {
        //public string name;
        public ArrayList characterTypes;
        public ArrayList excludeCultures;
        public string noGoingBackLevel;
        public ArrayList antiTraits;
        public ArrayList levels;				// arraylist of TraitLevel objects
        public ArrayList triggers;
        public bool hiddenTrait;

        public Trait() : this("") {
        }

        public Trait(string traitName)
            : base(traitName) {
            characterTypes = new ArrayList();
            excludeCultures = new ArrayList();
            antiTraits = new ArrayList();
            levels = new ArrayList();
            triggers = new ArrayList();
            noGoingBackLevel = "";

        }

        public bool readTrait(ref StreamReader reader, ref string errorMsg, ref string[] tokenizedInput, ArrayList enums, ArrayList attributes, ref int lineNumber) {
            bool valid = true;
            bool readRequired = true;
            errorMsg = Environment.NewLine;

            //check if the trait name has already been taken, don't check the first trait name
            if(Common.traits.Count > 0 && Common.findElement(name, Common.traits) != -1) {
                name = "{" + name + "}";
            }
            //check for characters line
            if(!Common.readLine(ref reader, ref tokenizedInput, ref lineNumber)) {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
            } else if(tokenizedInput[0] != "Characters") {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: Expecting Characters - Found " + tokenizedInput[0];
            } else {
                for(int i = 1; i < tokenizedInput.Length; i += 1) {
                    if(tokenizedInput[i] != "named" || i == tokenizedInput.Length - 1){
                        characterTypes.Add(Common.validCharacterType(tokenizedInput[i]));
                    } else {
                        characterTypes.Add(Common.validCharacterType(tokenizedInput[i] + " " + tokenizedInput[i+1]));
                        i += 1;
                    }
                }
            }

            //read the next line and check for optional hidden tag
            if(valid) {
                if(readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber)) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if(tokenizedInput[0] != "Hidden") {
                    readRequired = false;
                } else if(tokenizedInput.Length != 1) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Hidden";
                } else {
                    hiddenTrait = true;
                    readRequired = true;
                }
            }

            //parse the optional ExcludesCulture line, reading a line if necessary
            if(valid) {
                if(readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber)) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if(tokenizedInput[0] != "ExcludeCultures") {
                    readRequired = false;
                } else if(tokenizedInput.Length > Common.MAX_CULTURES + 1){
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many cultures listed, the maximum number of cultures is " + Common.MAX_CULTURES;
                } else {
                    for(int i = 1; i < tokenizedInput.Length; i += 1) {
                        excludeCultures.Add(Common.validBaseCulture(tokenizedInput[i]));
                    }
                    readRequired = true;
                }
            }

            //parse the optional NoGoingBackLevel line, reading a line if necessary
            if(valid) {
                if(readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber)) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if(tokenizedInput[0] != "NoGoingBackLevel") {
                    readRequired = false;
                } else if(tokenizedInput.Length != 2) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for NoGoingBackLevel";
                } else {
                    noGoingBackLevel = Common.testParse(tokenizedInput[1], false);
                    readRequired = true;
                }
            }

            //parse the optional AntiTraits line, reading a line if necessary
            if(valid) {
                if(readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber)) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if(tokenizedInput[0] != "AntiTraits") {
                    readRequired = false;
                }else if(tokenizedInput.Length < 2){
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: No antitraits listed on AntiTrait line";
                } else {
                    for(int i = 1; i < tokenizedInput.Length; i += 1) {
                        antiTraits.Add(tokenizedInput[i]);
                    }
                    readRequired = true;
                }
            }

            //parse the name of the first level for the trait, reading a line if necessary
            if(valid) {
                if(readRequired && !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber)) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else {
                    while(valid && tokenizedInput[0] == "Level") {
                        errorMsg += "Parsing level " + tokenizedInput[1] + " . . ." + Environment.NewLine + Environment.NewLine;
                            
                        if(tokenizedInput.Length != 2) {
                            errorMsg = "Line " + lineNumber + " - Error: Too many arguments for Level";
                            valid = false;
                        } else {
                            Level tempLevel = new Level(tokenizedInput[1]);
                            valid = tempLevel.readLevel(ref reader, ref errorMsg, ref tokenizedInput, enums, attributes, ref lineNumber);
                            if(valid) {
                                this.levels.Add(tempLevel);
                            }//end if (!validInput)
                        }//end if (tokenizedInput.Length != 2)
                    }//end while(valid && tokenizedInput[0] == "Level")
                }//end reading levels for current trait
            }

            return valid;
        }//end readTrait
    }
}