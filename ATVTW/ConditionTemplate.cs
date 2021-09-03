using System;
using System.Collections;
using System.Text;

namespace ATVTW {
    class ConditionTemplate : ATVTW {
        public ArrayList parameters;
        public bool boolValue;

        public ConditionTemplate() : this("") { }

        public ConditionTemplate(string name) : base(name) {
            parameters = new ArrayList();
            boolValue = true;
        }

        public bool comparable(Condition testCond) {
            return testCond.name == name;
        }

        public bool matchesTemplate(Condition testCond, ref string errorMsg, ArrayList attributes, ref int lineNumber) {
            bool result = true;
            bool valid = true;
            string token;

            if(testCond.parameters.Count != parameters.Count && name != "I_SpecificUnitsSelected") {
                valid = false;
                errorMsg += "Line " + lineNumber + " - Error: Invalid number of parameters for condition " + name;
            } else {
                for(int i = 0; i < parameters.Count; i += 1) {
                    testCond.parameterNames.Add(parameters[i]);
                    token = testCond.parameters[i].ToString();
                    switch(parameters[i].ToString()) {
                        case "logic token":
                            result = testLogicToken(token);
                            break;
                        case "double value":
                            result = testDoubleParse(token);
                            break;
                        case "int value":
                            result = testIntParse(token);
                            break;
                        case "non-negative int":
                            result = testNonNegativeParse(token);
                            break;
                        case "trait name":
                            result = Common.findElement(token, Common.traits) > -1;
                            break;
                        case "ancillary name":
                            result = Common.findElement(token, Common.ancillaries) > -1;
                            break;
                        case "attribute name":
                            result = Common.findElement(token, attributes) > -1;
                            break;
                        case "success type":
                            result = testSuccessTypeToken(token);
                            break;
                        case "siege engine class":
                            result = testSiegeEngineClass(token);
                            break;
                        case "tower defence type":
                            result = testTowerDefenceType(token);
                            break;
                        case "gate defence type":
                            result = testGateDefenceType(token);
                            break;
                        case "unit category":
                            result = testUnitCategory(token);
                            break;
                        case "culture type":
                            result = testCultureType(token);
                            break;
                        case "faction type":
                            result = testFactionType(token);
                            break;
                        case "conflict type":
                            result = testConflictType(token);
                            break;
                        case "wall level":
                            result = testWallLevel(token);
                            break;
                        case "gate strength":
                            result = testGateStrength(token);
                            break;
                        case "unit class":
                            result = testUnitClass(token);
                            break;
                        case "mount class":
                            result = testMountClass(token);
                            break;
                        case "hide type":
                            result = testHideType(token);
                            break;
                        case "character type":
                            result = testCharacterType(token);
                            break;
                        case "event type":
                            result = testEventType(token);
                            break;
                        case "tax level":
                            result = testTaxLevel(token);
                            break;
                        case "success level":
                            result = testSuccessLevel(token);
                            break;
                        case "loyalty level":
                            result = testLoyaltyLevel(token);
                            break;
                        case "attack direction":
                            result = testAttackDirection(token);
                            break;
                        case "stance":
                            result = testStance(token);
                            break;
                        default:
                            //for testing purposes to see what tokens aren't tested to decide if they should be
                            //testCond.parameters[i] = "[" + testCond.parameters[i] + "]";

                            //do nothing - the token isn't tested
                            result = true;
                            break;
                    }//end switch(parameters[i].ToString())

                    if(!result) {
                        testCond.parameters[i] = "{" + token + "}";
                    }//end if (!result)
                }//end for
            }//end if

            return valid;
        }

        private bool testLogicToken(string token) {
            return token == ">" || token == "<" || token == "=" || token == "<=" || token == ">=";
        }

        private bool testDoubleParse(string token) {
            bool result = true;
            try {
                double.Parse(token);
            } catch {
                result = false;
            }
            return result;
        }

        private bool testIntParse(string token) {
            bool result = true;
            try {
                int.Parse(token);
            } catch {
                result = false;
            }
            return result;
        }

        private bool testNonNegativeParse(string token) {
            bool result = true;
            try {
                int tempInt = int.Parse(token);
                result = tempInt >= 0;
            } catch {
                result = false;
            }
            return result;
        }

        private bool testSuccessTypeToken(string token) {
            return token == "close" || token == "average" || token == "clear" || token == "crushing";
        }

        private bool testSiegeEngineClass(string token) {
            return token == "tower" || token == "ram" || token == "ladder" || token == "sap_point";
        }

        private bool testTowerDefenceType(string token) {
            return token == "arrow_tower" || token == "ballista_tower" || token == "none";
        }

        private bool testGateDefenceType(string token) {
            return token == "hot_sand" || token == "burning_oil" || token == "none";
        }

        private bool testUnitCategory(string token) {
            return token == "infantry" || token == "cavalry" || token == "siege" || token == "non_combatant" || token == "ship" || token == "handler";
        }

        private bool testCultureType(string token) {
            return token == "roman" || token == "greek" || token == "eastern" || token == "egyptian" || token == "carthaginian" || token == "barbarian" || token == "hun" || token == "nomad";
        }

        private bool testFactionType(string token) {
            return token == "romans_brutii" || token == "romans_scipii" || token == "romans_julii" || token == "romans_senate" || token == "gauls" || token == "spain" || token == "carthage"
                || token == "egypt" || token == "pontus" || token == "armenia" || token == "parthia" || token == "macedon" || token == "germans" || token == "scythia" || token == "greek_cities"
                || token == "numidia" || token == "thrace" || token == "britons" || token == "dacia" || token == "seleucid" || token == "slave" || token == "huns" || token == "goths"
                || token == "sarmatians" || token == "vandals" || token == "franks" || token == "saxons" || token == "celts" || token == "sassanids" || token == "roxolani" || token == "ostrogoths"
                || token == "romano_british" || token == "slavs" || token == "burgundii" || token == "berbers" || token == "alemanni" || token == "empire_east" || token == "empire_west"
                || token == "empire_east_rebels" || token == "empire_west_rebels" || token == "lombardi";
        }

        private bool testConflictType(string token) {
            return token == "SuccessfulAmbush" || token == "FailedAmbush" || token == "Normal" || token == "Siege" || token == "SallyBesieger" || token == "Naval" || token == "Withdraw";
        }

        private bool testWallLevel(string token) {
            return token == "none" || token == "0" || token == "1" || token == "2" || token == "3" || token == "4";
        }

        private bool testGateStrength(string token) {
            return token == "0" || token == "1" || token == "2";
        }

        private bool testUnitClass(string token) {
            return token == "heavy" || token == "light" || token == "skirmish" || token == "spearman" || token == "missle";
        }

        private bool testMountClass(string token) {
            return token == "horse" || token == "camel" || token == "elephant" || token == "chariot";
        }

        private bool testHideType(string token) {
            return token == " hide_forest" || token == "hide_improved_forest" || token == "hide_long_grass" || token == "hide_anywhere";
        }

        private bool testCharacterType(string token) {
            return token == "family" || token == "general" || token == "named character" || token == "spy" || token == "assassin" || token == "diplomat" || token == "admiral";
        }

        private bool testEventType(string token) {
            return token == "earthquake" || token == "fire" || token == "flood" || token == "horde" || token == "riot" || token == "storm" || token == "volcano";
        }

        private bool testTaxLevel(string token) {
            return token == "tax_low" || token == "tax_normal" || token == "tax_high" || token == "tax_extortionate";
        }

        private bool testSuccessLevel(string token) {
            return token == "not_successful" || token == "slightly_successful" || token == "partly_successful" || token == "highly_successful";
        }

        private bool testLoyaltyLevel(string token) {
            return token == "loyalty_revolting" || token == "loyalty_rioting" || token == "loyalty_disillusioned" || token == "loyalty_content" || token == "loyalty_happy";
        }

        private bool testAttackDirection(string token) {
            return token == "front" || token == "flank" || token == "rear";
        }

        private bool testStance(string token) {
            return token == "Allied" || token == "Suspicious" || token == "Neutral" || token == "Hostile" || token == "AtWar";
        }
    }
}