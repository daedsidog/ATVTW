using System;
using System.Collections;
using System.IO;

namespace ATVTW
{
	/// <summary>
	/// Summary description for Trigger.
	/// </summary>
	class Trigger : ATVTW
	{

		public string triggerEvent;          // these WhenToTest event

		public ArrayList conditions;		// these are simply strings, no testing involved

		public ArrayList affectsList;		// a trigger Affect should be spelled Effect, this is
											//	what trait the trigger applies to, how many points that
											//	trait gains, and with what percentage chance it happens

        public ArrayList acquireAncillaryList; // a trigger AcquireAncillary is what ancillary the trigger
                                               // applies to, and with what percentage chance it happens

		public Trigger():this(""){}

        public Trigger(string name) : base(name){
			conditions = new ArrayList();
			affectsList = new ArrayList();
			acquireAncillaryList = new ArrayList();
            triggerEvent = "";
		}

        public bool readTrigger(bool parsingAncillaries, ref StreamReader reader, ref string errorMsg, ref string[] tokenizedInput, ArrayList events, ArrayList conditions, ArrayList attributes, ref int lineNumber){
            const int TRAIT_CHANCE_POS = 4;
            const int TRAIT_POINTS_POS = 2;
            const int ANCILLARY_CHANCE_POS = 3;

            bool valid = true;
            bool conditionsFound = false;
            int offset;
            bool notValue;
            errorMsg = Environment.NewLine;

            //check for duplicate trigger names
            if((parsingAncillaries && Common.ancillaryTriggers.Count > 0 && Common.findElement(name, Common.ancillaryTriggers) > -1 ) || (!parsingAncillaries && Common.traitTriggers.Count > 0 && Common.findElement(name, Common.traitTriggers) > -1 ) ) {
                name = "(" + name + ")";
            }

            //parse the event line
            if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
            }else if(tokenizedInput[0] != "WhenToTest"){
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: Expecting WhenToTest Found - " + tokenizedInput[0];
            } else if ( tokenizedInput.Length != 2 ) {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: Too many arguments for WhenToTest";
            } else {
                triggerEvent = tokenizedInput[1];
                if ( Common.findElement(triggerEvent, events) == -1 ) {
                    triggerEvent = "{" + triggerEvent + "}";
                }
            }

            if ( valid ) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of File Found!";
                } else if ( tokenizedInput[0] != "Condition" ) {
                    conditionsFound = false;
                } else {
                    conditionsFound = true;
                    if(tokenizedInput.Length > 2) {
                        notValue = tokenizedInput[1] == "not";
                    } else {
                        notValue = false;
                    }
                    offset = notValue ? 1 : 0;
                    Condition tempCondition = new Condition(tokenizedInput[1+offset], notValue);
                    int tempInt;
                    for ( int i = 2 + offset; i < tokenizedInput.Length; i += 1 ) {
                        tempCondition.parameters.Add(tokenizedInput[i]);
                    }

                    //check if the condition is a defined condition, if it is check the parameters
                    tempInt = Common.findElement(tempCondition.name, conditions);
                    if(tempInt == -1){
                        tempCondition.name = "{" + tempCondition.name + "}";
                    } else {
                        valid = ((ConditionTemplate)conditions[tempInt]).matchesTemplate(tempCondition, ref errorMsg, attributes, ref lineNumber);
                    }
                    this.conditions.Add(tempCondition);
                }
            }

            if ( valid && conditionsFound ) {
                if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                } else {
                    while ( valid && tokenizedInput[0] == "and" ) {
                        if(tokenizedInput.Length > 2) {
                            notValue = tokenizedInput[1] == "not";
                        } else {
                            notValue = false;
                        }
                        offset = notValue ? 1 : 0;

                        Condition tempCondition = new Condition(tokenizedInput[1 + offset], notValue);
                        int tempInt;

                        for ( int i = 2 + offset; i < tokenizedInput.Length; i += 1 ) {
                            tempCondition.parameters.Add(tokenizedInput[i]);
                        }

                        //check if the condition is a defined condition, if it is check the parameters
                        tempInt = Common.findElement(tempCondition.name, conditions);
                        if ( tempInt == -1 ) {
                            tempCondition.name = "{" + tempCondition.name + "}";
                        } else {
                            valid =((ConditionTemplate)conditions[tempInt]).matchesTemplate(tempCondition, ref errorMsg, attributes, ref lineNumber);
                        }
                        this.conditions.Add(tempCondition);

                        if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                            valid = false;
                            errorMsg += "Line " + lineNumber + " - Error: End of file found.";
                        }//end if !Common.readline
                    }//end while reading more conditions
                }//end if !Common.readline
            }//end if valid && conditionsFound

            //parse the levels effects lines, do a initial read line if necessary
            while ( valid && ( ( parsingAncillaries && tokenizedInput[0] == "AcquireAncillary" ) || tokenizedInput[0] == "Affects" ) ) {
                if ( tokenizedInput[0] == "AcquireAncillary" && tokenizedInput.Length != 4 ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for AcquireAncillary " + tokenizedInput[1];
                } else if ( tokenizedInput[0] == "Affects" && tokenizedInput.Length != 5 ) {
                    valid = false;
                    errorMsg += "Line " + lineNumber + " - Error: Too many arguments for Affects " + tokenizedInput[1];
                } else {
                    if ( tokenizedInput[0] == "AcquireAncillary" ) {
                        if ( Common.findElement(tokenizedInput[1], Common.ancillaries) == -1 ) {
                            tokenizedInput[1] = "{" + tokenizedInput[1] + "}";
                        }
                        tokenizedInput[ANCILLARY_CHANCE_POS] = Common.testParse(tokenizedInput[ANCILLARY_CHANCE_POS], false);
                        acquireAncillaryList.Add(new Affect(tokenizedInput[1], tokenizedInput[ANCILLARY_CHANCE_POS]));
                    } else{//if(tokenizedInput[0] == "Affects") {
                        if ( Common.findElement(tokenizedInput[1], Common.traits) == -1 ) {
                            tokenizedInput[1] = "{" + tokenizedInput[1] + "}";
                        }
                        tokenizedInput[TRAIT_POINTS_POS] = Common.testParse(tokenizedInput[TRAIT_POINTS_POS], true);
                        tokenizedInput[TRAIT_CHANCE_POS] = Common.testParse(tokenizedInput[TRAIT_CHANCE_POS], false);
                        affectsList.Add(new Affect(tokenizedInput[1], tokenizedInput[TRAIT_POINTS_POS], tokenizedInput[TRAIT_CHANCE_POS]));
                    }
                    if ( !Common.readLine(ref reader, ref tokenizedInput, ref lineNumber) ) {
                        break;
                    }
                }
            }//end parsing Effects lines

            return valid;
        }
	}
}
