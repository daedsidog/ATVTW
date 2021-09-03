using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace ATVTW {
    struct TriggerLabels {
        public Label nameLabel;
        public Label eventLabel;
        public ListBox conditionListBox;
        public ListBox affectsListBox;
        public ListBox acquireAncillaryListBox;

        public TriggerLabels(ref Label name, ref Label eventName, ref ListBox conditions, ref ListBox affects, ListBox acquireAncillaries) {
            nameLabel = name;
            eventLabel = eventName;
            conditionListBox = conditions;
            affectsListBox = affects;
            acquireAncillaryListBox = acquireAncillaries;
        }
    };

    public partial class frmATVTW : Form {
        private int currentTrait;
        private int currentLevel;
        private int currentTraitTrigger;
        private int currentAncillary;
        private int currentAncillaryTrigger;

        private bool stopForWarnings = true;
        private bool finishedCurrentItem;
        private bool checkedFirstTrait;
        private bool checkedFirstAncillary;
        private bool checkedFirstTraitTrigger;
        private bool checkedFirstAncillaryTrigger;
        private bool parsedFiles;

        private TriggerLabels traitTriggerLabels;
        private TriggerLabels ancillaryTriggerLabels;

        private const int TRAIT_TAB_INDEX = 0;
        private const int TRAIT_TRIGGER_TAB_INDEX = 1;
        private const int ANCILLARY_TAB_INDEX = 2;
        private const int ANCILLARY_TRIGGER_TAB_INDEX = 3;
        private const int DESCR_STRAT_TAB_INDEX = 4;
        private const int ORPHANS_TAB_INDEX = 5;

        public frmATVTW() {
            InitializeComponent();
            traitTriggerLabels = new TriggerLabels(ref lblTraitTriggerName, ref lblTraitEventName, ref lstTraitConditions, ref lstTraitAffects, null);
            ancillaryTriggerLabels = new TriggerLabels(ref lblAncillaryTriggerName, ref lblAncillaryEventName, ref lstAncillaryConditions, ref lstAncillaryAffects, lstAncillaryAcqurieAncillary);
            lstAncillaryAcqurieAncillary.BackColor = BackColor;
            lstAncillaryAffects.BackColor = BackColor;
            lstAncillaryConditions.BackColor = BackColor;
            lstAncillaryEffects.BackColor = BackColor;
            lstAntiTraits.BackColor = BackColor;
            lstLevelEffects.BackColor = BackColor;
            lstTraitAffects.BackColor = BackColor;
            lstTraitConditions.BackColor = BackColor;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.Run(new frmATVTW());
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e) {
            AboutATVTW frm1 = new AboutATVTW();
            frm1.ShowDialog();
        }

        private void mnuFileExit_Click(object sender, EventArgs e) {
            this.Close();
        }//end mnuFileExit_Click

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if(parsedFiles) {
                bool valid = false;
                switch(tabControl1.SelectedIndex) {
                    case TRAIT_TAB_INDEX:
                        setButtonsEnabledProperty(currentTrait, Common.traits);
                        checkedFirstTrait = false;
                        valid = checkTrait(false) & checkLevel(false);
                        goto case -2;
                    case TRAIT_TRIGGER_TAB_INDEX:
                        setButtonsEnabledProperty(currentTraitTrigger, Common.traitTriggers);
                        checkedFirstTraitTrigger = false;
                        valid = checkTrigger(false, currentTraitTrigger, Common.traitTriggers, true);
                        goto case -2;
                    case ANCILLARY_TAB_INDEX:
                        setButtonsEnabledProperty(currentAncillary, Common.ancillaries);
                        checkedFirstAncillary = false;
                        valid = checkAncillary(false);
                        goto case -2;
                    case ANCILLARY_TRIGGER_TAB_INDEX:
                        setButtonsEnabledProperty(currentAncillaryTrigger, Common.ancillaryTriggers);
                        checkedFirstAncillaryTrigger = false;
                        valid = checkTrigger(false, currentAncillaryTrigger, Common.ancillaryTriggers, false);
                        goto case -2;
                    case ORPHANS_TAB_INDEX:
                        lblParseStatus.Text = "Click the \"Find Orphans\" button to list all traits and ancillaries which cannot be obtained through triggers.";
                        btnNextProblem.Text = "Find Orphans";
                        goto default;
                    case DESCR_STRAT_TAB_INDEX:
                        lblParseStatus.Text = "Click the \"Parse DS\" button to list all traits and ancillaries in the chosen descr_strat.txt file that aren't defined in EDCT and EDA respectively.";
                        btnNextProblem.Text = "Parse DS";
                        goto default;
                    case -2:
                        lblParseStatus.Text = "Click the \"Next Problem\" button to find the next error or warning.\n\n" + lblParseStatus.Text;
                        break;
                    default:
                        disableAllButtons();
                        break;
                }

                if(valid){
                    lblParseStatus.Text += Environment.NewLine + "No errors or warning found.";
                }

            }
        }

        private void cboFilePath_SelectedIndexChanged(object sender, EventArgs e) {
            DialogResult result;
            if((cboFilePath.SelectedIndex != -1) && (cboFilePath.SelectedItem.ToString() == "<Browse>")) {
                result = fbdATFolderFinder.ShowDialog();
                if(result == DialogResult.OK) {
                    cboFilePath.Items.Insert(cboFilePath.SelectedIndex, fbdATFolderFinder.SelectedPath + "\\");
                    cboFilePath.SelectedIndex -= 1;
                }//end if (result==OK)
            }//end if ((SelectedIndex != -1) && (user chose <Browse>))
        }//end cboFilePath_SelectedIndexChanged

        private void frmATVTW_Load(object sender, EventArgs e){
            cboFilePath.SelectedIndex = 0;
        }

        private void btnParseFiles_Click(object sender, EventArgs e) {
            //read the current path from the interface
            string path = GetPath();
            string errorMessage;
            ArrayList traitEnums;
            ArrayList ancillaryEnums;

            if ( !File.Exists(path + "export_descr_character_traits.txt") ) {
                lblParseStatus.Text = "File not found: " + path + "export_descr_character_traits.txt. Please correct the path and try again.";
            } else if ( !File.Exists(path + "export_descr_ancillaries.txt") ) {
                lblParseStatus.Text = "File not found: " + path + "export_descr_ancillaries.txt. Please correct the path and try again.";
            } else if ( !File.Exists(path + "text\\export_VnVs.txt") ) {
                lblParseStatus.Text = "File not found: " + path + "text\\export_VnVs.txt. Please correct the path and try again.";
            } else if ( !File.Exists(path + "text\\export_ancillaries.txt") ) {
                lblParseStatus.Text = "File not found: " + path + "text\\export_ancillaries.txt. Please correct the path and try again.";
            } else if (!readEnums(path + "text\\export_VnVs.txt" , out traitEnums, out errorMessage)){
                lblParseStatus.Text = "Error in file: " + path + "text\\export_ancillaries.txt" + Environment.NewLine + Environment.NewLine + errorMessage; 
            } else if(!readEnums(path + "text\\export_ancillaries.txt", out ancillaryEnums, out errorMessage)) {
                lblParseStatus.Text = "Error in file: " + path + "text\\export_ancillaries.txt" + Environment.NewLine + Environment.NewLine + errorMessage;
            } else {
                //all files are present we can now begin parsing everything

                //first we need to read in the enum files
                ArrayList conditions = new ArrayList();
                ArrayList events = new ArrayList();
                ArrayList attributes = new ArrayList();
                string[] tokenizedInput = null;
                bool validInput = true;
                string errMsg = null;
                int lineNumber = 0;

                StreamReader reader = null;

                //empties any previous traits/ancillaries/triggers required and is ready to process new ones
                Common.resetVariables();

                //generate the attribute, condition, and event lists used to check for errors
                generateConditions(ref attributes, ref conditions, ref events);

                lblParseStatus.Text = "Parsing file export_descr_character_traits.txt . . ." + Environment.NewLine;
                lblParseStatus.Refresh();

                reader = new StreamReader(path + "export_descr_character_traits.txt");
                validInput = Common.readLine(ref reader, ref tokenizedInput, ref lineNumber);

                //if the file isn't empty try to parse the traits
                if(!validInput) {
                    lblParseStatus.Text = "Error: export_descr_character_traits.txt is an empty file";
                } else {
                    while(validInput && tokenizedInput[0] == "Trait") {
                        lblParseStatus.Text = "Parsing trait " + tokenizedInput[1] + " . . ." + Environment.NewLine;
                        lblParseStatus.Refresh();

                        if(tokenizedInput.Length == 2) {
                            Trait tempTrait = new Trait(tokenizedInput[1]);
                            validInput = tempTrait.readTrait(ref reader, ref errMsg, ref tokenizedInput, traitEnums, attributes, ref lineNumber);
                            if(!validInput) {
                                lblParseStatus.Text += errMsg;
                            } else {
                                Common.traits.Add(tempTrait);
                            }//end if (!validInput)
                        } else {
                            lblParseStatus.Text += Environment.NewLine + "Line " + lineNumber + " - Error: Too many arguments for Trait";
                            validInput = false;
                        }//end if the current trait has the correct number of tokens
                    }//end while there are more traits
                }//end if the file contains valid input

                //if the traits parsed successfully, try to parse the trait triggers
                if(validInput) {
                    validInput = parseTriggers(false, ref reader, tokenizedInput, events, conditions, attributes, ref lineNumber);
                    reader.Close();
                }//end parsing of trait triggers

                //if the trait triggers parsed successfully, try to parse the ancillaries
                if(validInput) {
                    lblParseStatus.Text = "Parsing file export_descr_ancillaries.txt . . ." + Environment.NewLine;
                    lblParseStatus.Refresh();

                    lineNumber = 0;
                    reader = new StreamReader(path + "export_descr_ancillaries.txt");
                    validInput = Common.readLine(ref reader, ref tokenizedInput, ref lineNumber);
                    if(!validInput) {
                        lblParseStatus.Text = "Error: export_descr_ancillaries.txt is an empty file";
                    } else {
                        while(validInput && tokenizedInput[0] == "Ancillary") {
                            lblParseStatus.Text = "Parsing ancillary " + tokenizedInput[1] + " . . ." + Environment.NewLine;
                            lblParseStatus.Refresh();

                            if(tokenizedInput.Length == 2) {
                                Ancillary tempAncillary = new Ancillary(tokenizedInput[1]);
                                validInput = tempAncillary.readAncillary(ref reader, ref errMsg, ref tokenizedInput, ancillaryEnums, attributes, ref lineNumber);
                                if(!validInput) {
                                    lblParseStatus.Text += errMsg;
                                } else {
                                    Common.ancillaries.Add(tempAncillary);
                                }//end if (!validInput)
                            } else {
                                lblParseStatus.Text += Environment.NewLine + "Line " + lineNumber + " - Error: Too many arguments for Ancillary";
                                validInput = false;
                            }//end if current ancillary has the right number of tokens
                        }//end while there are more ancillaries
                    }//end if (!validInput)
                }//end if (validInput)

                //if the ancillaries parsed successfully, try to parse the ancillary triggers
                if(validInput) {
                    validInput = parseTriggers(true, ref reader, tokenizedInput, events, conditions, attributes, ref lineNumber);
                }//end parsing of ancillary triggers
                reader.Close();

                if(validInput) {
                    //checks to make sure anti-traits are valid traits, and that anti-traits work in both directions
                    checkAntiTraits();
                    checkExcludedAncillaries();
                    btnNextProblem.Enabled = true;
                    btnSearch.Enabled = true;
                    txtSearchBox.Enabled = true;
                    resetTraitTestingVariables();
                    checkedFirstTraitTrigger = false;
                    checkedFirstAncillary = false;
                    checkedFirstAncillaryTrigger = false;
                    parsedFiles = true;
                    currentTrait = 0;
                    currentAncillaryTrigger = 0;
                    currentAncillary = 0;
                    currentTraitTrigger = 0;
                    this.AcceptButton = btnNextProblem;
                    refreshTrait(true);
                    refreshAncillary();
                    refreshTrigger(false, currentTraitTrigger, ref traitTriggerLabels, Common.traitTriggers);
                    refreshTrigger(true, currentAncillaryTrigger, ref ancillaryTriggerLabels, Common.ancillaryTriggers);
                    txtDescrStrat.Text = "";
                    txtOrphans.Text = "";
                    if(tabControl1.SelectedIndex == ORPHANS_TAB_INDEX) {
                        btnNextProblem.Text = "Find Orphans";
                        lblParseStatus.Text = "Click the \"Find Orphans\" button to list all traits and ancillaries which cannot be obtained through triggers.";
                    } else if(tabControl1.SelectedIndex == DESCR_STRAT_TAB_INDEX) {
                        lblParseStatus.Text = "Click the \"Parse DS\" button to list all traits and ancillaries in the chosen descr_strat.txt file that aren't defined in EDCT and EDA respectively.";
                        btnNextProblem.Text = "Parse DS";
                    } else {
                        moveListing(0);
                        lblParseStatus.Text = "Click the \"Next Problem\" button to find the next error or warning.\n\n" + lblParseStatus.Text;
                    }
                    lblParseStatus.Text = "Traits, Ancillaries and Triggers were successfully parsed.\n\n" + lblParseStatus.Text;
                }
            }

        }//end btnParseFiles_Click

        private string GetPath() {
            string path;
            if(cboFilePath.SelectedIndex == -1) {
                path = cboFilePath.Text;
            } else {
                path = cboFilePath.SelectedItem.ToString();
            }
            return path;
        }

        /* Function readEnums
         * Input: path to the file containitng the enumerations
         * Output: returns an ArrayList which contains all the enumerations from the file
         */
        private bool readEnums(string path, out ArrayList aList, out string errorMessage) {
            string inputLine;
            StreamReader reader = new StreamReader(path);
            int startIndex, endIndex, lineNumber = 0;
            bool valid = true;
            aList = new ArrayList();
            
            //while there are more lines in the file
            errorMessage = null;
            while(!reader.EndOfStream && valid) {
                inputLine = Common.removeComments(reader.ReadLine());
                lineNumber += 1;
                startIndex = inputLine.IndexOf("{");
                if(startIndex > -1) {
                    endIndex = inputLine.IndexOf("}");
                    if(endIndex > startIndex){
                        aList.Add(inputLine.Substring(startIndex + 1, endIndex - startIndex - 1));
                    }else{
                        valid = false;
                        errorMessage = "Line " + lineNumber + ": Error in file " + path;
                    }
                }
            }
            reader.Close();
            return valid;
        }//end readEnums

        /* Function: parseTriggers
         * Input: boolean value indicating if trait or ancillary triggers are being read, a streamreader to read the input, a string array with current line of input from the streamreader, 
         *        three arraylists one with valid conditions, one with valid events, one with valid attributes
         * Output: a boolean value indicating if the triggers were successfully parsed, an output parameter will return an error message if parsing was unsuccessful
         * */
        private bool parseTriggers(bool parsingAncillaries, ref StreamReader reader, string[] tokenizedInput, ArrayList events, ArrayList conditions, ArrayList attributes, ref int lineNumber) {
            bool validInput = true;
            string errMsg = "";

            while(validInput && tokenizedInput[0] == "Trigger") {
                lblParseStatus.Text = "Parsing trigger " + tokenizedInput[1] + " . . ." + Environment.NewLine;
                lblParseStatus.Refresh();

                if ( tokenizedInput.Length == 2 ) {
                    Trigger tempTrigger = new Trigger(tokenizedInput[1]);
                    validInput = tempTrigger.readTrigger(parsingAncillaries, ref reader, ref errMsg, ref tokenizedInput, events, conditions, attributes, ref lineNumber);
                    if(!validInput) {
                        lblParseStatus.Text += errMsg;
                    } else {
                        if(parsingAncillaries) {
                            Common.ancillaryTriggers.Add(tempTrigger);
                        } else {
                            Common.traitTriggers.Add(tempTrigger);
                        }
                    }//end if (!validInput)
                } else {
                    lblParseStatus.Text = Environment.NewLine + "Line " + lineNumber + " - Error parsing trigger " + tokenizedInput[1] + ": Too many arguments for Trigger";
                    validInput = false;
                }
            }
            if(validInput && !reader.EndOfStream && tokenizedInput[0] != "" && tokenizedInput[0] != "FakeEffect" ){
                lblParseStatus.Text += Environment.NewLine + "Line " + lineNumber + " - Error parsing " + (parsingAncillaries ? "EDA" : "EDCT") + ": Expected End of File, Found " + tokenizedInput[0];
                validInput = false;
            }
            return validInput;
        }

        #region Functions for setting up the lists of events, attributes and conditions
        private void generateConditions(ref ArrayList attributes, ref ArrayList conditions, ref ArrayList events) {
            // catalogue attributes, from 
            attributes.Add("Ambush");
            attributes.Add("Attack");
            attributes.Add("BattleSurgery");
            attributes.Add("BodyguardValour");
            attributes.Add("BribeResistance");
            attributes.Add("CavalryCommand");
            attributes.Add("Combat_V_Barbarian");
            attributes.Add("Combat_V_Carthaginian");
            attributes.Add("Combat_V_Eastern");
            attributes.Add("Combat_V_Egyptian");
            attributes.Add("Combat_V_Greek");
            attributes.Add("Combat_V_Roman");
            attributes.Add("Combat_V_Slave");
            attributes.Add("Combat_V_Nomad");
            attributes.Add("Combat_V_Hun");
            attributes.Add("Command");
            attributes.Add("Construction");
            attributes.Add("Defence");
            attributes.Add("Electability");
            attributes.Add("Farming");
            attributes.Add("Fertility");
            attributes.Add("GrainTrading");
            attributes.Add("Health");
            attributes.Add("HitPoints");
            attributes.Add("InfantryCommand");
            attributes.Add("Influence");
            attributes.Add("Law");
            attributes.Add("LineOfSight");
            attributes.Add("Looting");
            attributes.Add("Management");
            attributes.Add("Mining");
            attributes.Add("MovementPoints");
            attributes.Add("NavalCommand");
            attributes.Add("Negotiation");
            attributes.Add("NightBattle");
            attributes.Add("PersonalSecurity");
            attributes.Add("PublicSecurity");
            attributes.Add("SiegeAttack");
            attributes.Add("SiegeDefence");
            attributes.Add("SiegeEngineering");
            attributes.Add("SlaveTrading");
            attributes.Add("Squalor");
            attributes.Add("TaxCollection");
            attributes.Add("Trading");
            attributes.Add("TrainingAgents");
            attributes.Add("TrainingAnimalUnits");
            attributes.Add("TrainingUnits");
            attributes.Add("TroopMorale");
            attributes.Add("Unrest");
            attributes.Add("PopularStanding");
            attributes.Add("SenateStanding");
            attributes.Add("LocalPopularity");
            attributes.Add("Loyalty");
            attributes.Add("Bribery");
            attributes.Add("Subterfuge");
            
            // catalogue conditions and associated syntax, from JeromeGrasdyke's posting about Sept 10, 2005
            addCondition(ref conditions, "I_InBattle");
            addCondition(ref conditions, "WonBattle");
            addCondition(ref conditions, "I_WonBattle", "faction type");
            addCondition(ref conditions, "Routs");
            addCondition(ref conditions, "Ally_Routs");
            addCondition(ref conditions, "GeneralHPLostRatioinBattle", "logic token", "double value");
            addCondition(ref conditions, "GeneralNumKillsInBattle", "logic token", "non-negative int");
            addCondition(ref conditions, "GeneralFoughtInCombat");
            addCondition(ref conditions, "PercentageOfArmyKilled", "logic token", "double value");
            addCondition(ref conditions, "I_PercentageOfArmyKilled", "alliance index", "army index", "logic token", "double value");
            addCondition(ref conditions, "PercentageEnemyKilled", "logic token", "double value");
            addCondition(ref conditions, "PercentageBodyguardKilled", "logic token", "double value");
            addCondition(ref conditions, "PercentageRoutedOffField", "logic token", "double value");
            addCondition(ref conditions, "NumKilledGenerals", "logic token", "non-negative int");
            addCondition(ref conditions, "PercentageUnitCategory", "unit category", "logic token", "double value");
            addCondition(ref conditions, "NumFriendsInBattle", "logic token", "non-negative int");
            addCondition(ref conditions, "NumEnemiesInBattle", "logic token", "non-negative int");
            addCondition(ref conditions, "GeneralFoughtFaction", "faction type");
            addCondition(ref conditions, "GeneralFoughtCulture", "culture type");
            addCondition(ref conditions, "I_ConflictType", "conflict type");
            addCondition(ref conditions, "IsNightBattle");
            addCondition(ref conditions, "BattleSuccess", "logic token", "success type");
            addCondition(ref conditions, "BattleOdds", "logic token", "double value");
            addCondition(ref conditions, "WasAttacker");
            addCondition(ref conditions, "I_BattleAiAttacking");
            addCondition(ref conditions, "I_BattleAiAttackingSettlement");
            addCondition(ref conditions, "I_BattleAiDefendingSettlement");
            addCondition(ref conditions, "I_BattleAiDefendingHill");
            addCondition(ref conditions, "I_BattleAiDefendingCrossing");
            addCondition(ref conditions, "I_BattleAiScouting");
            addCondition(ref conditions, "I_BattleIsRiverBattle");
            addCondition(ref conditions, "I_BattleIsSiegeBattle");
            addCondition(ref conditions, "I_BattleIsSallyOutBattle");
            addCondition(ref conditions, "I_BattleIsFortBattle");
            addCondition(ref conditions, "I_BattleAttackerNumSiegeEngines", "seige engine class", "logic token", "non-negative int");
            addCondition(ref conditions, "I_BattleAttackerNumArtilleryCanPenetrateWalls", "logic token", "non-negative int");
            addCondition(ref conditions, "I_BattleDefenderNumNonMissileUnitsOnWalls", "logic token", "non-negative int");
            addCondition(ref conditions, "I_BattleDefenderNumMissileUnitsOnWalls", "logic token", "non-negative int");
            addCondition(ref conditions, "I_BattleSettlementWallsBreached");
            addCondition(ref conditions, "I_BattleSettlementGateDestroyed");
            addCondition(ref conditions, "I_BattleSettlementTowerDefence", "tower defence type");
            addCondition(ref conditions, "I_BattleSettlementGateDefence", "gate defence type");
            addCondition(ref conditions, "I_BattleSettlementFortificationLevel", "logic token", "wall level");
            addCondition(ref conditions, "BattleBuildingType", "logic token", "building type");
            addCondition(ref conditions, "I_BattleSettlementGateStrength", "logic token", "gate strength");
            addCondition(ref conditions, "I_BattleNumberOfRiverCrossings", "logic token", "non-negative int");
            addCondition(ref conditions, "BattlePlayerUnitClass", "logic token", "unit class");
            addCondition(ref conditions, "BattleEnemyUnitClass", "logic token", "unit class");
            addCondition(ref conditions, "BattlePlayerUnitCategory", "logic token", "unit category");
            addCondition(ref conditions, "BattleEnemyUnitCategory", "logic token", "unit category");
            addCondition(ref conditions, "BattlePlayerUnitSiegeEngineClass", "logic token", "seige engine category");
            addCondition(ref conditions, "BattleEnemyUnitSiegeEngineClass", "logic token", "seige engine category");
            addCondition(ref conditions, "BattlePlayerUnitOnWalls");
            addCondition(ref conditions, "BattleEnemyUnitOnWalls");
            addCondition(ref conditions, "BattlePlayerCurrentFormation", "logic token", "formation");
            addCondition(ref conditions, "BattleEnemyCurrentFormation", "logic token", "formation");
            addCondition(ref conditions, "BattlePlayerUnitCloseFormation");
            addCondition(ref conditions, "BattleEnemyUnitCloseFormation");
            addCondition(ref conditions, "BattlePlayerUnitSpecialAbilitySupported", "logic token", "special ability");
            addCondition(ref conditions, "BattleEnemyUnitSpecialAbilitySupported", "logic token", "special ability");
            addCondition(ref conditions, "BattlePlayerUnitSpecialAbilityActive");
            addCondition(ref conditions, "BattleEnemyUnitSpecialAbilityActive");
            addCondition(ref conditions, "BattlePlayerMountClass", "logic token", "mount class");
            addCondition(ref conditions, "BattleEnemyMountClass", "logic token", "mount class");
            addCondition(ref conditions, "BattlePlayerUnitMeleeStrength", "logic token", "double value");
            addCondition(ref conditions, "BattleEnemyUnitMeleeStrength", "logic token", "double value");
            addCondition(ref conditions, "BattlePlayerUnitMissileStrength", "logic token", "double value");
            addCondition(ref conditions, "BattleEnemyUnitMissileStrength", "logic token", "double value");
            addCondition(ref conditions, "BattlePlayerUnitSpecialFormation", "logic token", "formation");
            addCondition(ref conditions, "BattleEnemyUnitSpecialFormation", "logic token", "formation");
            addCondition(ref conditions, "BattlePlayerUnitEngaged");
            addCondition(ref conditions, "BattleEnemyUnitEngaged");
            addCondition(ref conditions, "BattlePlayerActionStatus", "logic token", "action status");
            addCondition(ref conditions, "BattleEnemyActionStatus", "logic token", "action status");
            addCondition(ref conditions, "BattlePlayerUnitMovingFast");
            addCondition(ref conditions, "BattleEnemyUnitMovingFast");
            addCondition(ref conditions, "BattleRangeOfAttack", "logic token", "double value");
            addCondition(ref conditions, "BattleDirectionOfAttack", "logic token", "attack direction");
            addCondition(ref conditions, "BattleIsMeleeAttack");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageOfUnitClass", "unit class", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageOfUnitClass", "unit class", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageOfUnitCategory", "unit category", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageOfUnitCategory", "unit category", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageOfMountClass", "mount class", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageOfMountClass", "mount class", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageOfClassAndCategory", "unit class", "unit category", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageOfClassAndCategory", "unit class", "unit category", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageOfSpecialAbility", "special ability", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageOfSpecialAbility", "special ability", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageCanHide", "hide type", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageCanHide", "hide type", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageCanSwim", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageCanSwim", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyIsAttacker");
            addCondition(ref conditions, "I_BattlePlayerAllianceOddsInFavour", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerAllianceOddsAgainst", "logic token", "double value");
            addCondition(ref conditions, "TotalSiegeWeapons", "logic token", "non-negative int");
            addCondition(ref conditions, "I_BattleStarted");
            addCondition(ref conditions, "I_IsUnitMoveFastSet", "unit label");
            addCondition(ref conditions, "I_IsUnitMoving", "unit label");
            addCondition(ref conditions, "I_IsUnitIdle", "unit label");
            addCondition(ref conditions, "I_IsUnitRouting", "unit label");
            addCondition(ref conditions, "I_IsUnitUnderFire", "unit label");
            addCondition(ref conditions, "I_IsUnitEngaged", "unit label");
            addCondition(ref conditions, "I_IsUnitEngagedWithUnit", "unit label", "unit label");
            addCondition(ref conditions, "I_UnitFormation", "unit label", "logic token", "formation type");
            addCondition(ref conditions, "I_PercentageUnitKilled", "unit label", "logic token", "double value");
            addCondition(ref conditions, "I_UnitPercentageAmmoLeft", "unit label", "logic token", "double value");
            addCondition(ref conditions, "I_UnitDistanceFromPosition", "unit label", "int value", "int value", "logic token", "double value");
            addCondition(ref conditions, "I_UnitDistanceFromLine", "unit label", "location", "location", "logic token", "double value");
            addCondition(ref conditions, "I_UnitDistanceFromUnit", "unit label", "unit label", "logic token", "double value");
            addCondition(ref conditions, "I_UnitInRangeOfUnit", "unit label", "unit label");
            addCondition(ref conditions, "I_UnitDestroyed", "unit label");
            addCondition(ref conditions, "I_UnitEnemyUnitInRadius", "unit label", "double value");
            addCondition(ref conditions, "I_IsUnitGroupMoving", "group label");
            addCondition(ref conditions, "I_IsUnitGroupEngaged", "group label");
            addCondition(ref conditions, "I_IsUnitGroupIdle", "group label");
            addCondition(ref conditions, "I_IsUnitGroupDestroyed", "group label");
            addCondition(ref conditions, "I_PercentageUnitGroupKilled", "group label", "logic token", "double value");
            addCondition(ref conditions, "I_UnitGroupFormation", "group label", "logic token", "formation");
            addCondition(ref conditions, "I_UnitGroupDistanceFromPosition", "group label", "int value", "int value", "logic token", "double value");
            addCondition(ref conditions, "I_UnitGroupDistanceFromGroup", "group label", "group label", "logic token", "double value");
            addCondition(ref conditions, "I_UnitGroupInRangeOfUnit", "group label", "unit label");
            addCondition(ref conditions, "I_UnitInRangeOfUnitGroup", "unit label", "group label");
            addCondition(ref conditions, "I_UnitGroupInRangeOfUnitGroup", "group label", "group label");
            addCondition(ref conditions, "I_PlayerInRangeOfUnitGroup", "group label");
            addCondition(ref conditions, "I_PlayerInRangeOfUnit", "unit label");
            addCondition(ref conditions, "I_UnitTypeSelected", "unit type");
            addCondition(ref conditions, "UnitType", "unit type");
            addCondition(ref conditions, "I_UnitSelected", "unit label");
            addCondition(ref conditions, "I_MultipleUnitsSelected");
            addCondition(ref conditions, "I_SpecificUnitsSelected", "unit label", "optional unit label", "optional unit label", "optional unit label", "optional unit label");
            addCondition(ref conditions, "I_IsCameraZoomingToUnit", "unit label");
            addCondition(ref conditions, "I_BattleEnemyArmyPercentageOfMatchingUnits", "unit match type", "logic token", "double value");
            addCondition(ref conditions, "I_BattleEnemyArmyNumberOfMatchingUnits", "unit match type", "logic token", "non-negative int");
            addCondition(ref conditions, "I_BattlePlayerArmyPercentageOfMatchingUnits", "unit match type", "logic token", "double value");
            addCondition(ref conditions, "I_BattlePlayerArmyNumberOfMatchingUnits", "unit match type", "logic token", "non-negative int");
            addCondition(ref conditions, "Trait", "trait name", "logic token", "non-negative int");
            addCondition(ref conditions, "FatherTrait", "trait name", "logic token", "non-negative int");
            addCondition(ref conditions, "FactionLeaderTrait", "trait name", "logic token", "non-negative int");
            addCondition(ref conditions, "Attribute", "attribute name", "logic token", "int value");
            addCondition(ref conditions, "WorldwideAncillaryExists", "ancillary name");
            addCondition(ref conditions, "FactionwideAncillaryExists", "ancillary name");
            addCondition(ref conditions, "RemainingMPPercentage", "logic token", "double value");
            addCondition(ref conditions, "I_RemainingMPPercentage", "character name", "logic token", "double value");
            addCondition(ref conditions, "I_CharacterCanMove", "character name");
            addCondition(ref conditions, "NoActionThisTurn");
            addCondition(ref conditions, "AgentType", "logic token", "character type");
            addCondition(ref conditions, "TrainedAgentType", "logic token", "character type");
            addCondition(ref conditions, "DisasterType", "disaster type");
            addCondition(ref conditions, "CultureType", "culture type");
            addCondition(ref conditions, "OriginalFactionType", "faction type");
            addCondition(ref conditions, "OriginalCultureType", "culture type");
            addCondition(ref conditions, "IsGeneral");
            addCondition(ref conditions, "IsAdmiral");
            addCondition(ref conditions, "EndedInSettlement");
            addCondition(ref conditions, "IsFactionLeader");
            addCondition(ref conditions, "IsFactionHeir");
            addCondition(ref conditions, "IsMarried");
            addCondition(ref conditions, "AtSea");
            addCondition(ref conditions, "InEnemyLands");
            addCondition(ref conditions, "InBarbarianLands");
            addCondition(ref conditions, "InUncivilisedLands");
            addCondition(ref conditions, "IsBesieging");
            addCondition(ref conditions, "IsUnderSiege");
            addCondition(ref conditions, "I_WithdrawsBeforeBattle");
            addCondition(ref conditions, "EndedInEnemyZOC");
            addCondition(ref conditions, "AdviseAction", "logic token", "action");
            addCondition(ref conditions, "I_CharacterTypeNearCharacterType", "faction", "character type", "non-negative int", "faction", "character type");
            addCondition(ref conditions, "I_CharacterTypeNearTile", "faction", "character type", "non-negative int", "non-negative int", "non-negative int");
            addCondition(ref conditions, "FactionType", "faction type");
            addCondition(ref conditions, "TargetFactionType", "faction type");
            addCondition(ref conditions, "FactionCultureType", "culture type");
            addCondition(ref conditions, "TargetFactionCultureType", "culture type");
            addCondition(ref conditions, "TrainedUnitCategory", "unit category");
            addCondition(ref conditions, "UnitCategory", "logic token", "unit category");
            addCondition(ref conditions, "SenateMissionTimeRemaining", "logic token", "non-negative int");
            addCondition(ref conditions, "MedianTaxLevel", "logic token", "tax level");
            addCondition(ref conditions, "ModeTaxLevel", "logic token", "tax level");
            addCondition(ref conditions, "I_ModeTaxLevel", "faction type", "logic token", "tax level");
            addCondition(ref conditions, "MissionSuccessLevel", "logic token", "success level");
            addCondition(ref conditions, "MissionSucceeded");
            addCondition(ref conditions, "MissionFactionTargetType", "faction type");
            addCondition(ref conditions, "MissionCultureTargetType", "culture type");
            addCondition(ref conditions, "DiplomaticStanceFromCharacter", "faction type", "logic token", "stance");
            addCondition(ref conditions, "DiplomaticStanceFromFaction", "faction type", "logic token", "stance");
            addCondition(ref conditions, "FactionHasAllies");
            addCondition(ref conditions, "LosingMoney");
            addCondition(ref conditions, "I_LosingMoney", "faction type");
            addCondition(ref conditions, "SupportCostsPercentage", "logic token", "double value");
            addCondition(ref conditions, "Treasury", "logic token", "int value");
            addCondition(ref conditions, "OnAWarFooting");
            addCondition(ref conditions, "I_FactionBesieging", "faction type");
            addCondition(ref conditions, "I_FactionBesieged", "faction type");
            addCondition(ref conditions, "I_NumberOfSettlements", "faction type", "logic token", "non-negative int");
            addCondition(ref conditions, "I_NumberOfHeirs", "faction type", "logic token", "non-negative int");
            addCondition(ref conditions, "I_FactionNearTile", "faction type", "non-negative int", "non-negative int", "non-negative int");
            addCondition(ref conditions, "SettlementsTaken", "logic token", "non-negative int");
            addCondition(ref conditions, "BattlesFought", "logic token", "non-negative int");
            addCondition(ref conditions, "BattlesWon", "logic token", "non-negative int");
            addCondition(ref conditions, "BattlesLost", "logic token", "non-negative int");
            addCondition(ref conditions, "DefensiveSiegesFought", "logic token", "non-negative int");
            addCondition(ref conditions, "DefensiveSiegesWon", "logic token", "non-negative int");
            addCondition(ref conditions, "OffensiveSiegesFought", "logic token", "non-negative int");
            addCondition(ref conditions, "OffensiveSiegesWon", "logic token", "non-negative int");
            addCondition(ref conditions, "RandomPercent", "logic token", "non-negative int");
            addCondition(ref conditions, "TrueCondition");
            addCondition(ref conditions, "SettlementName", "settlement name");
            addCondition(ref conditions, "GovernorBuildingExists", "logic token", "building level");
            addCondition(ref conditions, "SettlementBuildingExists", "logic token", "building level");
            addCondition(ref conditions, "BuildingFinishedByGovernor", "logic token", "building level");
            addCondition(ref conditions, "SettlementBuildingFinished", "logic token", "building level");
            addCondition(ref conditions, "GovernorPlugInExists", "logic token", "building level");
            addCondition(ref conditions, "GovernorPlugInFinished", "logic token", "building level");
            addCondition(ref conditions, "GovernorTaxLevel", "logic token", "tax level");
            addCondition(ref conditions, "SettlementTaxLevel", "logic token", "tax level");
            addCondition(ref conditions, "GovernorInResidence");
            addCondition(ref conditions, "GovernorLoyaltyLevel", "logic token", "loyalty level");
            addCondition(ref conditions, "SettlementLoyaltyLevel", "logic token", "loyalty level");
            addCondition(ref conditions, "RiotRisk", "logic token", "non-negative int");
            addCondition(ref conditions, "BuildingQueueIdleDespiteCash");
            addCondition(ref conditions, "TrainingQueueIdleDespiteCash");
            addCondition(ref conditions, "I_SettlementExists", "settlement name");
            addCondition(ref conditions, "I_SettlementOwner", "settlement name", "logic token", "faction type");
            addCondition(ref conditions, "AdviseFinancialBuild", "logic token", "build type");
            addCondition(ref conditions, "AdviseBuild", "logic token", "build type");
            addCondition(ref conditions, "AdviseRecruit", "logic token", "unit type");
            addCondition(ref conditions, "SettlementPopulationMaxedOut");
            addCondition(ref conditions, "SettlementPopulationTooLow");
            addCondition(ref conditions, "SettlementAutoManaged", "logic token", "automanage type");
            addCondition(ref conditions, "PercentageOfPopulationInGarrison", "logic token", "double value");
            addCondition(ref conditions, "GarrisonToPopulationRatio", "logic token", "double value");
            addCondition(ref conditions, "HealthPercentage", "logic token", "double value");
            addCondition(ref conditions, "SettlementHasPlague");
            addCondition(ref conditions, "IsFortGarrisoned");
            addCondition(ref conditions, "IsSettlementGarrisoned");
            addCondition(ref conditions, "IsSettlementRioting");
            addCondition(ref conditions, "I_NumberUnitsInSettlement", "settlement name", "unit type", "logic token", "non-negative int");
            addCondition(ref conditions, "CharacterIsLocal");
            addCondition(ref conditions, "TargetCharacterIsLocal");
            addCondition(ref conditions, "SettlementIsLocal");
            addCondition(ref conditions, "TargetSettlementIsLocal");
            addCondition(ref conditions, "RegionIsLocal");
            addCondition(ref conditions, "TargetRegionIsLocal");
            addCondition(ref conditions, "ArmyIsLocal");
            addCondition(ref conditions, "TargetArmyIsLocal");
            addCondition(ref conditions, "FactionIsLocal");
            addCondition(ref conditions, "I_LocalFaction", "faction type");
            addCondition(ref conditions, "TargetFactionIsLocal");
            addCondition(ref conditions, "I_TurnNumber", "logic token", "non-negative int");
            addCondition(ref conditions, "I_MapName", "file path");
            addCondition(ref conditions, "I_ThreadCount", "test name", "logic token", "non-negative int");
            addCondition(ref conditions, "I_IsTriggerTrue", "test name");
            addCondition(ref conditions, "IncomingMessageType", "message name");
            addCondition(ref conditions, "I_AdvisorVerbosity", "logic token", "int value");
            addCondition(ref conditions, "I_AdvisorVisible");
            addCondition(ref conditions, "I_CharacterSelected", "character name");
            addCondition(ref conditions, "I_AgentSelected", "character type");
            addCondition(ref conditions, "I_SettlementSelected", "settlement name");
            addCondition(ref conditions, "ShortcutTriggered", "element name", "function name");
            addCondition(ref conditions, "I_AdvancedStatsScrollIsOpen");
            addCondition(ref conditions, "ButtonPressed", "button name");
            addCondition(ref conditions, "ScrollOpened", "scroll name");
            addCondition(ref conditions, "ScrollClosed", "scroll name");
            addCondition(ref conditions, "ScrollAdviceRequested", "scroll name");
            addCondition(ref conditions, "I_CompareCounter", "script name", "logic token", "int value");
            addCondition(ref conditions, "I_TimerElapsed", "timer name", "logic token", "int value");
            addCondition(ref conditions, "I_SoundPlaying", "sound name");
            addCondition(ref conditions, "I_AdvisorSpeechPlaying");
            addCondition(ref conditions, "IsBesieging");
            addCondition(ref conditions, "IsUnderSeige");

            //catalog events from the docudemon files
            events.Add("PreBattle");
            events.Add("PreBattleWithdrawal");
            events.Add("PostBattle");
            events.Add("BattleGeneralKilled");
            events.Add("BattleGeneralRouted");
            events.Add("HireMercenaries");
            events.Add("GeneralCaptureResidence");
            events.Add("GeneralCaptureWonder");
            events.Add("GeneralCaptureSettlement");
            events.Add("LeaderDestroyedFaction");
            events.Add("CharacterDamagedByDisaster");
            events.Add("GeneralAssaultResidence");
            events.Add("OfferedForAdoption");
            events.Add("LesserGeneralOfferedForAdoption");
            events.Add("OfferedForMarriage");
            events.Add("BrotherAdopted");
            events.Add("BecomesFactionLeader");
            events.Add("BecomesFactionHeir");
            events.Add("BecomeQuaestor");
            events.Add("BecomeAedile");
            events.Add("BecomePraetor");
            events.Add("BecomeConsul");
            events.Add("BecomeCensor");
            events.Add("BecomePontifexMaximus");
            events.Add("CeasedFactionLeader");
            events.Add("CeasedFactionHeir");
            events.Add("CeasedQuaestor");
            events.Add("CeasedAedile");
            events.Add("CeasedPraetor");
            events.Add("CeasedConsul");
            events.Add("CeasedCensor");
            events.Add("CeasedPontifexMaximus");
            events.Add("LostLegionaryEagle");
            events.Add("CapturedLegionaryEagle");
            events.Add("RecapturedLegionaryEagle");
            events.Add("SenateExposure");
            events.Add("QuaestorInvestigationMinor");
            events.Add("QuaestorInvestigation");
            events.Add("QuaestorInvestigationMajor");
            events.Add("Birth");
            events.Add("CharacterComesOfAge");
            events.Add("CharacterMarries");
            events.Add("CharacterBecomesAFather");
            events.Add("CharacterTurnStart");
            events.Add("CharacterTurnEnd");
            events.Add("CharacterTurnEndInSettlement");
            events.Add("GeneralDevastatesTile");
            events.Add("SpyMission");
            events.Add("ExecutesASpyOnAMission");
            events.Add("LeaderOrderedSpyingMission");
            events.Add("AssassinationMission");
            events.Add("ExecutesAnAssassinOnAMission");
            events.Add("LeaderOrderedAssassination");
            events.Add("SufferAssassinationAttempt");
            events.Add("SabotageMission");
            events.Add("LeaderOrderedSabotage");
            events.Add("BriberyMission");
            events.Add("LeaderOrderedBribery");
            events.Add("AcceptBribe");
            events.Add("RefuseBribe");
            events.Add("Insurrection");
            events.Add("DiplomacyMission");
            events.Add("LeaderOrderedDiplomacy");
            events.Add("LeaderSenateMissionSuccess");
            events.Add("LeaderSenateMissionFailure");
            events.Add("NewAdmiralCreated");
            events.Add("GovernorUnitTrained");
            events.Add("GovernorBuildingCompleted");
            events.Add("GovernorPlugInCompleted");
            events.Add("AgentCreated");
            events.Add("GovernorAgentCreated");
            events.Add("GovernorBuildingDestroyed");
            events.Add("GovernorCityRiots");
            events.Add("GovernorCityRebels");
            events.Add("GovernorThrowGames");
            events.Add("GovernorThrowRaces");
            events.Add("EnslavePopulation");
            events.Add("ExterminatePopulation");
            events.Add("CharacterSelected");
            events.Add("MultiTurnMove");
            events.Add("CharacterPanelOpen");
            events.Add("SettlementTurnEnd");
            events.Add("SettlementTurnStart");
            events.Add("UnitTrained");
            events.Add("BuildingCompleted");
            events.Add("PlugInCompleted");
            events.Add("BuildingDestroyed");
            events.Add("CityRiots");
            events.Add("CityRebels");
            events.Add("UngarrisonedSettlement");
            events.Add("CitySacked");
        }

        private void addCondition(ref ArrayList conditions, string condName, string param1, string param2, string param3, string param4, string param5) {
            ConditionTemplate template = new ConditionTemplate(condName);

            if(param1 != null)
                template.parameters.Add(param1);

            if(param2 != null)
                template.parameters.Add(param2);

            if(param3 != null)
                template.parameters.Add(param3);

            if(param4 != null)
                template.parameters.Add(param4);

            if(param5 != null)
                template.parameters.Add(param5);

            conditions.Add(template);
        }

        private void addCondition(ref ArrayList conditions, string condName, string param1, string param2, string param3, string param4) {
            addCondition(ref conditions, condName, param1, param2, param3, param4, null);
        }

        private void addCondition(ref ArrayList conditions, string condName, string param1, string param2, string param3) {
            addCondition(ref conditions, condName, param1, param2, param3, null, null);
        }

        private void addCondition(ref ArrayList conditions, string condName, string param1, string param2) {
            addCondition(ref conditions, condName, param1, param2, null, null, null);
        }

        private void addCondition(ref ArrayList conditions, string condName, string param1) {
            addCondition(ref conditions, condName, param1, null, null, null, null);
        }

        private void addCondition(ref ArrayList conditions, string condName) {
            addCondition(ref conditions, condName, null, null, null, null, null);
        }
        #endregion

        private void refreshTrait(bool resetLevelNumber) {
            if(currentTrait > -1 && currentTrait < Common.traits.Count) {
                //reset the colours of the trait tab (not including level colours)
                resetTraitTabColours();

                //grab current trait
                Trait tempTrait = (Trait)Common.traits[currentTrait];
                string spacer = ", ";
                bool antiTraitWarning = false;
                bool antiTraitError = false;

                //and check it!
                //currentTrait = checkTrait(currentTrait);

                setLabelError(tempTrait.name, ref lblTraitName, "{", "}");
                setLabelError(tempTrait.noGoingBackLevel, ref lblNoGoingBackLevel, "{", "}");

                if(tempTrait.hiddenTrait) {
                    lblHidden.Text = "Yes";
                } else {
                    lblHidden.Text = "No";
                }

                lblCharacters.Text = tempTrait.characterTypes[0].ToString();
                if(tempTrait.characterTypes.Count > 1) {
                    lblCharacters.Text += "[";
                }
                for(int i = 1; i < tempTrait.characterTypes.Count; i += 1) {
                    lblCharacters.Text += spacer + tempTrait.characterTypes[i].ToString();
                }
                if(tempTrait.characterTypes.Count > 1) {
                    lblCharacters.Text += "]";
                    lblCharacters.ForeColor = Color.Blue;
                }
                if(lblCharacters.Text.IndexOf("{") > -1) {
                    lblCharacters.ForeColor = Color.Red;
                }


                lblTraitsExcludesCultures.Text = tempTrait.excludeCultures.Count > 0 ? tempTrait.excludeCultures[0].ToString() : "";
                for(int i = 1; i < tempTrait.excludeCultures.Count; i += 1) {
                    lblTraitsExcludesCultures.Text += spacer + tempTrait.excludeCultures[i].ToString();
                }
                if(lblTraitsExcludesCultures.Text.IndexOf("{") > -1) {
                    lblTraitsExcludesCultures.ForeColor = Color.Red;
                }

                string tempStr = "";
                if(tempTrait.antiTraits.Count > 0) {
                    tempStr = tempTrait.antiTraits[0].ToString();
                    if(hasError(tempTrait.antiTraits[0].ToString())) antiTraitError = true;
                    if(hasWarning(tempTrait.antiTraits[0].ToString())) antiTraitWarning = true;
                }
                lstAntiTraits.Items.Clear();
                for(int i = 1; i < tempTrait.antiTraits.Count; i += 1) {
                    tempStr += spacer + tempTrait.antiTraits[i].ToString();
                    if(hasError(tempTrait.antiTraits[i].ToString())) antiTraitError = true;
                    if(hasWarning(tempTrait.antiTraits[i].ToString())) antiTraitWarning = true;
                }
                if(tempTrait.antiTraits.Count > Common.MAX_ANTITRAITS) {
                    tempStr = "{" + tempStr + "}";
                    antiTraitError = true;
                }
                lstAntiTraits.Items.Add(tempStr);
                if(antiTraitWarning){
                    lstAntiTraits.ForeColor = Color.Blue;
                }
                if(antiTraitError){
                    lstAntiTraits.ForeColor = Color.Red;
                }

                if(resetLevelNumber) {
                    currentLevel = 0;
                }
                refreshLevel();
                setButtonsEnabledProperty(currentTrait, Common.traits);
            }
        }//end refreshTrait()

        private void refreshLevel() {
            //if there's no level reset the labels
            if(((Trait)Common.traits[currentTrait]).levels.Count == 0) {
                lblLevelName.Text = "";
                lblLevelDesc.Text = "";
                lblLevelEffectDesc.Text = "";
                lblLevelGain.Text = "";
                lblLevelLose.Text = "";
                lblLevelEpithet.Text = "";
                lblLevelThreshhold.Text = "";
                lstLevelEffects.Items.Clear();
            } else {
                //if there is a level populate the fields
                if(currentLevel > -1 && currentLevel < ((Trait)Common.traits[currentTrait]).levels.Count) {
                    Effect tempEffect;
                    Level tempLevel;

                    //reset the colours for the level entries
                    resetTraitTabLevelColours();

                    tempLevel = (Level)((Trait)Common.traits[currentTrait]).levels[currentLevel];
                    setLabelError(tempLevel.name, ref lblLevelName, "{", "}");
                    setLabelError(tempLevel.descriptionName, ref lblLevelDesc, "{", "}");
                    setLabelError(tempLevel.effectsDescName, ref lblLevelEffectDesc, "{", "}");
                    setLabelError(tempLevel.gainMessageName, ref lblLevelGain, "{", "}");
                    setLabelError(tempLevel.loseMessageName, ref lblLevelLose, "{", "}");
                    setLabelError(tempLevel.epithetName, ref lblLevelEpithet, "{", "}");
                    setLabelError(tempLevel.threshhold, ref lblLevelThreshhold, "{", "}");

                    //effects, first clear, then add new
                    lstLevelEffects.Items.Clear();
                    for(int i = 0; i < tempLevel.effects.Count; i += 1){
                        tempEffect = (Effect)tempLevel.effects[i];
                        lstLevelEffects.Items.Add(tempEffect);
                        if(tempEffect.name.IndexOf("{") > -1 || tempEffect.pointsAllocated.IndexOf("{") > -1){
                            lstLevelEffects.ForeColor = Color.Red;
                        }
                    }

                    //religious_beliefs, first clear, then add new
                    lblLevelReligiousBeliefs.Text = "";
                    for(int i = 0; i < tempLevel.religiousBelief.Count; i += 1){
                        tempEffect = ( Effect )tempLevel.religiousBelief[i];
                        lblLevelReligiousBeliefs.Text += tempEffect + Environment.NewLine;
                    }
                    if(lblLevelReligiousBeliefs.Text.IndexOf("{") > -1){
                        lblLevelReligiousBeliefs.ForeColor = Color.Red;
                    }

                    //religious_order, first clear, then add new
                    lblLevelReligiousOrder.Text = "";
                    for(int i = 0; i < tempLevel.religiousOrder.Count; i += 1) {
                        tempEffect = (Effect)tempLevel.religiousOrder[i];
                        lblLevelReligiousOrder.Text += tempEffect + Environment.NewLine;
                    }
                    if(lblLevelReligiousOrder.Text.IndexOf("{") > -1) {
                        lblLevelReligiousOrder.ForeColor = Color.Red;
                    }
                }//end if (currentLevel < ((Traits)Common.traits[currentTrait]).levels.Count)
            }//end if ((Traits)Common.traits[currentTrait]).levels.Count == 0)
        }//end refreshLevel()

        private void refreshAncillary() {
            if(currentAncillary > -1 && currentAncillary < Common.ancillaries.Count) {
                Ancillary tempAncillary;
                Effect tempEffect;
                string spacer = ", ";
                bool excludesAncillaryWarning = false;
                bool excludesAncillaryError = false;

                //reset the label colours of the ancillary tab
                resetAncillaryTabColours();

                tempAncillary = (Ancillary)Common.ancillaries[currentAncillary];
                if(hasError(tempAncillary.name, "(", ")")) {
                    if(hasError(tempAncillary.name.Substring(1, tempAncillary.name.Length - 2))) {
                        setLabelError(tempAncillary.name, ref lblAncillaryName, "({", "})");
                    } else {
                        setLabelError(tempAncillary.name, ref lblAncillaryName, "(", ")");
                    }
                } else if(hasError(tempAncillary.name)) {
                    setLabelError(tempAncillary.name, ref lblAncillaryName, "{", "}");
                } else {
                    lblAncillaryName.Text = tempAncillary.name;
                }

                setLabelError(tempAncillary.descriptionName, ref lblAncillaryDescription, "{", "}");
                setLabelError(tempAncillary.effectsDescName, ref lblAncillaryEffectsDescription, "{", "}");

                //fill in the image name
                lblAncillaryImageFile.Text = tempAncillary.imageFilename;

                //fill in if the ancillary is unique or not
                if(tempAncillary.uniqueAncillary) {
                    lblUniqueAncillary.Text = "Yes";
                } else {
                    lblUniqueAncillary.Text = "No";
                }

                //fill in the excluded ancillaries list
                lblExcludesAncillaries.Text = "";
                if(tempAncillary.excludedAncillaries.Count > 0) {
                    lblExcludesAncillaries.Text = tempAncillary.excludedAncillaries[0].ToString();
                    if(hasError(tempAncillary.excludedAncillaries[0].ToString())) excludesAncillaryError = true;
                    if(hasWarning(tempAncillary.excludedAncillaries[0].ToString())) excludesAncillaryWarning = true;
                }

                for(int i = 1; i < tempAncillary.excludedAncillaries.Count; i += 1) {
                    lblExcludesAncillaries.Text += spacer + tempAncillary.excludedAncillaries[i].ToString();
                    if(hasError(tempAncillary.excludedAncillaries[i].ToString())) excludesAncillaryError = true;
                    if(hasWarning(tempAncillary.excludedAncillaries[i].ToString())) excludesAncillaryWarning = true;
                }

                if(tempAncillary.excludedAncillaries.Count > Common.MAX_EXCLUDED_ANCILLARIES) {
                    lblExcludesAncillaries.Text = "{" + lblExcludesAncillaries.Text + "}";
                    excludesAncillaryError = true;
                }

                if(excludesAncillaryWarning) {
                    lblExcludesAncillaries.ForeColor = Color.Blue;
                }

                if(excludesAncillaryError) {
                    lblExcludesAncillaries.ForeColor = Color.Red;
                }

                //fill in the excluded cultures list
                lblAncillaryExcludeCultures.Text = tempAncillary.excludeCultures.Count > 0 ? tempAncillary.excludeCultures[0].ToString() : "";
                for(int i = 1; i < tempAncillary.excludeCultures.Count; i += 1) {
                    lblAncillaryExcludeCultures.Text += spacer + tempAncillary.excludeCultures[i].ToString();
                }
                if(lblAncillaryExcludeCultures.Text.IndexOf("{") > -1) {
                    lblAncillaryExcludeCultures.ForeColor = Color.Red;
                }

                //effects, first clear, then add new
                lstAncillaryEffects.Items.Clear();
                for ( int i = 0; i < tempAncillary.effects.Count; i += 1 ) {
                    tempEffect = ( Effect )tempAncillary.effects[i];
                    lstAncillaryEffects.Items.Add(tempEffect);
                    if ( tempEffect.name.IndexOf("{") > -1 || tempEffect.pointsAllocated.IndexOf("{") > -1 ) {
                        lstAncillaryEffects.ForeColor = Color.Red;
                    }
                }

                //religious_beliefs, first clear, then add new
                lblAncillaryReligiousBelief.Text = "";
                for ( int i = 0; i < tempAncillary.religiousBelief.Count; i += 1 ) {
                    tempEffect = ( Effect )tempAncillary.religiousBelief[i];
                    lblAncillaryReligiousBelief.Text += tempEffect + Environment.NewLine;
                }
                if(lblAncillaryReligiousBelief.Text.IndexOf("{") > -1) {
                    lblAncillaryReligiousBelief.ForeColor = Color.Red;
                }

                //religious_order, first clear, then add new
                lblAncillaryReligiousOrder.Text = "";
                for(int i = 0; i < tempAncillary.religiousOrder.Count; i += 1) {
                    tempEffect = (Effect)tempAncillary.religiousOrder[i];
                    lblAncillaryReligiousOrder.Text += tempEffect + Environment.NewLine;
                }
                if(lblAncillaryReligiousOrder.Text.IndexOf("{") > -1) {
                    lblAncillaryReligiousOrder.ForeColor = Color.Red;
                }

                setButtonsEnabledProperty(currentAncillary, Common.ancillaries);
            }
        }

        private void refreshTrigger(bool refreshAncillary, int currentPosition, ref TriggerLabels labels, ArrayList triggers) {
            if(currentPosition > -1 && currentPosition < triggers.Count) {
                Trigger tempTrigger = (Trigger)triggers[currentPosition];
                Condition tempCond;
                Affect tempAffect;

                //refreshing the triggers controls colour on the tab
                refreshTriggerTabColour(labels);

                //set the enabled property on the buttons
                setButtonsEnabledProperty(currentPosition, triggers);

                //fill in the event and name labels
                setLabelError(tempTrigger.name, ref labels.nameLabel, "(", ")");
                setLabelError(tempTrigger.triggerEvent, ref labels.eventLabel, "{", "}");

                //clear the listbox fill in the conditions
                labels.conditionListBox.Items.Clear();
                for(int i = 0; i < tempTrigger.conditions.Count; i += 1) {
                    tempCond = (Condition)tempTrigger.conditions[i];
                    labels.conditionListBox.Items.Add(tempCond);
                    if(!tempCond.hasNoErrors()){
                        labels.conditionListBox.ForeColor = Color.Red;
                    }
                }//end for (i<tempTrigger.conditions.Count)

                //fill in the affects labels
                labels.affectsListBox.Items.Clear();
                for(int i = 0; i < tempTrigger.affectsList.Count; i += 1) {
                    tempAffect = (Affect)tempTrigger.affectsList[i];
                    labels.affectsListBox.Items.Add(tempAffect);
                    if(tempAffect.hasError()){
                        labels.affectsListBox.ForeColor = Color.Red;
                    }
                }

                if(!refreshAncillary) {
                    //if in EDCT there are more than Common.MAX_AFFECTS_EDCT affects lines
                    if(tempTrigger.affectsList.Count > Common.MAX_AFFECTS_EDCT) {
                        labels.affectsListBox.ForeColor = Color.Red;
                    }
                }else{ //if (refreshAncillary){
                    //fill in the affects labels with any acquiredAncillaries, should be none for traits if the parser is working correctly
                    labels.acquireAncillaryListBox.Items.Clear();
                    for(int i = 0; i < tempTrigger.acquireAncillaryList.Count; i += 1){
                        tempAffect = ( Affect )tempTrigger.acquireAncillaryList[i];
                        labels.acquireAncillaryListBox.Items.Add(tempAffect);
                        if(tempAffect.hasError()){
                            labels.acquireAncillaryListBox.ForeColor = Color.Red;
                        }
                    }
                }

            }//end if currentPosition is valid
        }


        private void refreshTriggerTabColour(TriggerLabels labels) {
            if(labels.acquireAncillaryListBox != null){
                labels.acquireAncillaryListBox.ForeColor = Color.Black;
            }
            labels.affectsListBox.ForeColor = Color.Black;
            labels.conditionListBox.ForeColor = Color.Black;
            labels.eventLabel.ForeColor = Color.Black;
            labels.nameLabel.ForeColor = Color.Black;
        }

        private void resetTraitTabColours() {
            lblTraitName.ForeColor = Color.Black;
            lblCharacters.ForeColor = Color.Black;
            lblTraitsExcludesCultures.ForeColor = Color.Black;
            lblNoGoingBackLevel.ForeColor = Color.Black;
            lstAntiTraits.ForeColor = Color.Black;
        }

        private void resetTraitTabLevelColours() {
            lblLevelName.ForeColor = Color.Black;
            lblLevelDesc.ForeColor = Color.Black;
            lblLevelEffectDesc.ForeColor = Color.Black;
            lblLevelGain.ForeColor = Color.Black;
            lblLevelLose.ForeColor = Color.Black;
            lblLevelEpithet.ForeColor = Color.Black;
            lblLevelThreshhold.ForeColor = Color.Black;
            lblLevelReligiousBeliefs.ForeColor = Color.Black;
            lblLevelReligiousOrder.ForeColor = Color.Black;
            lstLevelEffects.ForeColor = Color.Black;
        }

        private void resetAncillaryTabColours() {
            lblAncillaryName.ForeColor = Color.Black;
            lblAncillaryDescription.ForeColor = Color.Black;
            lblAncillaryEffectsDescription.ForeColor = Color.Black;
            lblExcludesAncillaries.ForeColor = Color.Black;
            lblAncillaryExcludeCultures.ForeColor = Color.Black;
            lblAncillaryReligiousBelief.ForeColor = Color.Black;
            lblAncillaryReligiousOrder.ForeColor = Color.Black;
            lstAncillaryEffects.ForeColor = Color.Black;
        }

        private void setLabelError(string input, ref Label lbl, string openP, string closeP) {
            if(hasError(input, openP, closeP)) {
                // this entry has an error, change the text of the corresponding label red
                input = input.Substring(openP.Length, input.Length - openP.Length - closeP.Length);
                lbl.ForeColor = System.Drawing.Color.Red;
            }
            lbl.Text = input;
        }

        //sets the buttons if the trait tab is the active tab
        private void setButtonsEnabledProperty(int currentPosition, ArrayList aList) {
            btnStart.Enabled = currentPosition >= 1;
            btnBackward1.Enabled = btnStart.Enabled;
            btnBackward20.Enabled = currentPosition >= 20;
            btnBackward5.Enabled = currentPosition >= 5;
            btnEnd.Enabled = currentPosition < (aList.Count - 1);
            btnForward1.Enabled = btnEnd.Enabled;
            btnForward20.Enabled = currentPosition < (aList.Count - 20);
            btnForward5.Enabled = currentPosition < (aList.Count - 5);
            enableMainControls();
            if(tabControl1.SelectedIndex == TRAIT_TAB_INDEX) {
                setLevelButtons();
            }
        }
        private void enableMainControls() {
            btnSearch.Enabled = true;
            txtSearchBox.Enabled = true;
            btnNextProblem.Text = "Next Problem";
        }

        //disable all buttons if the orphans tab is the active tab
        private void disableAllButtons() {
            btnStart.Enabled = false;
            btnBackward1.Enabled = false;
            btnBackward20.Enabled = false;
            btnBackward5.Enabled = false;
            btnEnd.Enabled = false;
            btnForward1.Enabled = false;
            btnForward20.Enabled = false;
            btnForward5.Enabled = false;
            btnSearch.Enabled = false;
            txtSearchBox.Enabled = false;
        }

        private void setLevelButtons() {
            btnPrevLevel.Enabled = currentLevel > 0;
            btnNextLevel.Enabled = currentLevel < ((Trait)Common.traits[currentTrait]).levels.Count - 1;
        }

        private void btnForward_Click(object sender, EventArgs e) {
            Button item = (Button)sender;
            int increase = Int32.Parse(item.Name.Substring(item.Name.Length - 1));
            moveListing(increase);
        }

        private void btnForward20_Click(object sender, EventArgs e) {
            moveListing(20);
        }

        private void btnBackward_Click(object sender, EventArgs e) {
            Button item = (Button)sender;
            int decrease = -1 * Int32.Parse(item.Name.Substring(item.Name.Length - 1));
            moveListing(decrease);
        }

        private void btnBackward20_Click(object sender, EventArgs e) {
            moveListing(-20);
        }

        private void btnStart_Click(object sender, EventArgs e) {
            if(tabControl1.SelectedIndex == TRAIT_TAB_INDEX) {
                moveListing(-currentTrait);
            } else if(tabControl1.SelectedIndex == TRAIT_TRIGGER_TAB_INDEX) {
                moveListing(-currentTraitTrigger);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TAB_INDEX) {
                moveListing(-currentAncillary);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TRIGGER_TAB_INDEX) {
                moveListing(-currentAncillaryTrigger);
            } else {
                //do nothing, button should be disabled
            }
        }

        private void btnEnd_Click(object sender, EventArgs e) {
            if(tabControl1.SelectedIndex == TRAIT_TAB_INDEX) {
                moveListing(Common.traits.Count - currentTrait - 1);
            } else if(tabControl1.SelectedIndex == TRAIT_TRIGGER_TAB_INDEX) {
                moveListing(Common.traitTriggers.Count - currentTraitTrigger - 1);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TAB_INDEX) {
                moveListing(Common.ancillaries.Count - currentAncillary - 1);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TRIGGER_TAB_INDEX) {
                moveListing(Common.ancillaryTriggers.Count - currentAncillaryTrigger - 1);
            } else {
                //do nothing, button should be disabled
            }
        }

        private void moveListing(int relativeChangePosition) {
            bool valid = false;
            if(tabControl1.SelectedIndex == TRAIT_TAB_INDEX) {
                currentTrait += relativeChangePosition;
                resetTraitTestingVariables();
                refreshTrait(true);
                valid = checkTrait(false) & checkLevel(false);
                checkedFirstTrait = true;
                finishedCurrentItem = currentLevel == ((Trait)Common.traits[currentTrait]).levels.Count - 1;
            } else if(tabControl1.SelectedIndex == TRAIT_TRIGGER_TAB_INDEX) {
                currentTraitTrigger += relativeChangePosition;
                checkedFirstTraitTrigger = false;
                refreshTrigger(false, currentTraitTrigger, ref traitTriggerLabels, Common.traitTriggers);
                valid = checkTrigger(false, currentTraitTrigger, Common.traitTriggers, true);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TAB_INDEX) {
                currentAncillary += relativeChangePosition;
                checkedFirstAncillary = false;
                refreshAncillary();
                valid = checkAncillary(false);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TRIGGER_TAB_INDEX) {
                currentAncillaryTrigger += relativeChangePosition;
                checkedFirstAncillaryTrigger = false;
                refreshTrigger(true, currentAncillaryTrigger, ref ancillaryTriggerLabels, Common.ancillaryTriggers);
                valid = checkTrigger(false, currentAncillaryTrigger, Common.ancillaryTriggers, false);
            } else {
                lblParseStatus.Text = "";
            }
            if(valid) {
                lblParseStatus.Text += Environment.NewLine + "No errors or warning found.";
            }
        }

        private void btnNextLevel_Click(object sender, EventArgs e) {
            currentLevel += 1;
            moveLevel();
        }

        private void btnPrevLevel_Click(object sender, EventArgs e) {
            currentLevel -= 1;
            moveLevel();
        }

        private void moveLevel() {
            refreshLevel();
            setLevelButtons();
            if(checkTrait(false) & checkLevel(false)) {
                lblParseStatus.Text += Environment.NewLine + "No errors or warning found.";
            }
        }

        private void checkAntiTraits() {
            Trait tempTraitA;
            Trait tempTraitB;
            int tempInt;

            for(int i = 0; i < Common.traits.Count; i += 1) {
                tempTraitA = (Trait)Common.traits[i];
                for(int j = 0; j < tempTraitA.antiTraits.Count; j += 1) {
                    tempInt = Common.findElement(tempTraitA.antiTraits[j], Common.traits);
                    if(tempInt == -1) {
                        tempTraitA.antiTraits[j] = "{" + tempTraitA.antiTraits[j] + "}";
                    } else {
                        tempTraitB = (Trait)Common.traits[tempInt];
                        tempInt = Common.findElement(tempTraitA.name, tempTraitB.antiTraits);
                        if(tempInt == -1) {
                            tempTraitA.antiTraits[j] = "[" + tempTraitA.antiTraits[j] + "]";
                        }//end if (tempInt == -1)
                    }//end if (tempInt == -1)
                }//end for (j<tempTraitA.antiTraits.Count)
            }//end for (i<Common.traits.Count)
        }//end checkAntiTraits

        private void checkExcludedAncillaries() {
            Ancillary tempAncillaryA;
            Ancillary tempAncillaryB;
            int tempInt;

            for(int i = 0; i < Common.ancillaries.Count; i += 1) {
                tempAncillaryA = (Ancillary)Common.ancillaries[i];
                for(int j = 0; j < tempAncillaryA.excludedAncillaries.Count; j += 1) {
                    tempInt = Common.findElement(tempAncillaryA.excludedAncillaries[j], Common.ancillaries);
                    if(tempInt == -1) {
                        tempAncillaryA.excludedAncillaries[j] = "{" + tempAncillaryA.excludedAncillaries[j] + "}";
                    } else {
                        tempAncillaryB = (Ancillary)Common.ancillaries[tempInt];
                        tempInt = Common.findElement(tempAncillaryA.name, tempAncillaryB.excludedAncillaries);
                        if(tempInt == -1) {
                            tempAncillaryA.excludedAncillaries[j] = "[" + tempAncillaryA.excludedAncillaries[j] + "]";
                        }//end if (tempInt == -1)
                    }//end if (tempInt == -1)
                }//end for (j<tempAncillaryA.excludedAncillaries.Count)
            }//end for (i<Common.ancillaries.Count)
        }//end checkExcludedAncillaries

        private string removeErrorMarks(string input) {
            string aString = input;
            if(hasError(aString, "(", ")")) aString = aString.Substring(1, aString.Length - 2);
            if(hasError(aString)) aString = aString.Substring(1, aString.Length - 2);
            if(hasWarning(aString)) aString = aString.Substring(1, aString.Length - 2);
            return aString;
        }

        private bool hasError(string input) {
            return hasError(input, "{", "}");
        }

        private bool hasError(string input, string startChar, string endChar) {
            return input.StartsWith(startChar) && input.EndsWith(endChar);
        }

        private bool hasWarning(string input) {
            return hasError(input, "[", "]");
        }

        private bool checkTrait(bool nextProblem) {
            Trait aTrait = (Trait)Common.traits[currentTrait];
            bool valid = true;

            lblParseStatus.Text = "Checking trait " + removeErrorMarks(aTrait.name) + " . . ." + Environment.NewLine;
            lblParseStatus.Refresh();

            if(finishedCurrentItem || !checkedFirstTrait) {
                //check for duplicate trait names
                if(hasError(aTrait.name)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Trait " + removeErrorMarks(aTrait.name) + " already exists." + Environment.NewLine;
                }

                //check for invalid character types && later warn of having multiple character types
                for(int i = 0; i < aTrait.characterTypes.Count; i += 1) {
                    if(hasError(aTrait.characterTypes[i].ToString())) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: Invalid Character Type: " + removeErrorMarks(aTrait.characterTypes[i].ToString()) + Environment.NewLine;
                    }
                }
                if(((nextProblem && stopForWarnings) || !nextProblem) && (aTrait.characterTypes.Count > 1)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Warning: Multiple Character Types Found - All Characters Types after the first will not acquire this trait." + Environment.NewLine;
                }

                //check for invalid cultures (this is based on default culture names)
                for(int i = 0; i < aTrait.excludeCultures.Count; i += 1) {
                    if(hasError(aTrait.excludeCultures[i].ToString())) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: Invalid Culture: " + removeErrorMarks(aTrait.excludeCultures[i].ToString()) + Environment.NewLine;
                    }
                }

                //check for invalid noGoingBackLevel
                if(hasError(aTrait.noGoingBackLevel)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Invalid NoGoingBackLevel: " + removeErrorMarks(aTrait.noGoingBackLevel.ToString()) + Environment.NewLine;
                }

                //check for antiTraits that aren't valid traits, and if warnings are on for antiTraits that don't work in both directions
                if(aTrait.antiTraits.Count > Common.MAX_ANTITRAITS) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Maximun AntiTraits allowed is " + Common.MAX_ANTITRAITS + "." + Environment.NewLine;
                }
                for(int i = 0; i < aTrait.antiTraits.Count; i += 1) {
                    if(hasError(aTrait.antiTraits[i].ToString())) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: AntiTrait " + removeErrorMarks(aTrait.antiTraits[i].ToString()) + " is not defined." + Environment.NewLine;
                    }
                    if(((nextProblem && stopForWarnings) || !nextProblem) && (hasWarning(aTrait.antiTraits[i].ToString()))) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Warning: AntiTrait " + removeErrorMarks(aTrait.antiTraits[i].ToString()) + " does not work in both directions." + Environment.NewLine;
                    }
                }
            }
            return valid;
        }

        private bool checkLevel(bool nextProblem) {
            bool valid = true;
            Level aLevel;
            Effect anEffect;

            if(currentLevel < ((Trait)Common.traits[currentTrait]).levels.Count) {
                aLevel = (Level)((Trait)Common.traits[currentTrait]).levels[currentLevel];

                lblParseStatus.Text += Environment.NewLine + "Checking level " + removeErrorMarks(aLevel.name) + " . . ." + Environment.NewLine;
                lblParseStatus.Refresh();

                //check that level name is defined in the VnV file
                if(hasError(aLevel.name)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Level " + removeErrorMarks(aLevel.name) + " is not defined in export_VnVs.txt." + Environment.NewLine;
                }

                //check that the description is defined in the VnV file
                if(hasError(aLevel.descriptionName)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: DescriptionName " + removeErrorMarks(aLevel.descriptionName) + " is not defined in export_VnVs.txt." + Environment.NewLine;
                }

                //check that the effectsDescription is defined in the VnV file
                if(hasError(aLevel.effectsDescName)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: EffectsDescription " + removeErrorMarks(aLevel.effectsDescName) + " is not defined in export_VnVs.txt." + Environment.NewLine;
                }

                //check that the gainMessage is defined in the VnV file
                if(hasError(aLevel.gainMessageName)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: GainMessage " + removeErrorMarks(aLevel.gainMessageName) + " is not defined in export_VnVs.txt." + Environment.NewLine;
                }

                //check that the loseMessage is defined in the VnV file
                if(hasError(aLevel.loseMessageName)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: LoseMessage " + removeErrorMarks(aLevel.loseMessageName) + " is not defined in export_VnVs.txt." + Environment.NewLine;
                }

                //check that the epithet is defined in the VnV file
                if(hasError(aLevel.epithetName)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Epithet " + removeErrorMarks(aLevel.epithetName) + " is not defined in export_VnVs.txt." + Environment.NewLine;
                }

                //check that the threshold is a valid integer, and is greater than the threshold from any previous level
                if(hasError(aLevel.threshhold)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Threshold " + removeErrorMarks(aLevel.threshhold) + " is not a positive integer greater than 0." + Environment.NewLine;
                }

                //check each affect line and report if the attribute is a valid attribute and if the number is a non-zero integer
                for(int i = 0; i < aLevel.effects.Count; i += 1) {
                    anEffect = (Effect)aLevel.effects[i];
                    
                    if(hasError(anEffect.name)) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: Attribute " + removeErrorMarks(anEffect.name) + " is not a valid attribute." + Environment.NewLine;
                    }
                    if(hasError(anEffect.pointsAllocated)) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: For attribute " + removeErrorMarks(anEffect.name) + " the points allocated " + removeErrorMarks(anEffect.pointsAllocated) + " is not a non-zero integer." + Environment.NewLine;
                    }
                }

                //check each religious belief line and report if the religion is a valid religion, and if the number is non-zero integer
                for(int i = 0; i < aLevel.religiousBelief.Count; i += 1){
                    anEffect = ( Effect )aLevel.religiousBelief[i];
                    
                    if (hasError(anEffect.name)) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: Religion " + removeErrorMarks(anEffect.name) + " is not a valid religions." + Environment.NewLine;
                    }
                    if (hasError(anEffect.pointsAllocated)) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: For religion " + removeErrorMarks(anEffect.name) + " the points allocated " + removeErrorMarks(anEffect.pointsAllocated) + " is not a non-zero integer." + Environment.NewLine;
                    }
                }

                //check each religious order line and report if the religion is a valid religion, and if the number is non-zero integer
                for(int i = 0; i < aLevel.religiousOrder.Count; i += 1) {
                    anEffect = (Effect)aLevel.religiousOrder[i];
                    
                    if(hasError(anEffect.name)) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: Religion " + removeErrorMarks(anEffect.name) + " is not a valid religions." + Environment.NewLine;
                    }
                    if(hasError(anEffect.pointsAllocated)) {
                        valid = false;
                        lblParseStatus.Text += Environment.NewLine + "Error: For religion " + removeErrorMarks(anEffect.name) + " the points allocated " + removeErrorMarks(anEffect.pointsAllocated) + " is not a non-zero integer." + Environment.NewLine;
                    }
                }
            }
            return valid;
        }

        private bool checkAncillary(bool nextProblem) {
            bool valid = true;
            Ancillary anAncillary;
            Effect anEffect;
            string aString;

            anAncillary = (Ancillary)Common.ancillaries[currentAncillary];

            lblParseStatus.Text = "Checking ancillary " + removeErrorMarks(anAncillary.name) + " . . ." + Environment.NewLine;
            lblParseStatus.Refresh();

            //check that the ancillary is not a duplicate ancillary
            aString = anAncillary.name;
            if(hasError(aString, "(", ")")) {
                aString = aString.Substring(1, aString.Length - 2);
                valid = false;
                lblParseStatus.Text += Environment.NewLine + "Error: Ancillary " + removeErrorMarks(anAncillary.name) + " already exists." + Environment.NewLine;
            }

            //check that ancillary name is defined in the VnV file
            if(hasError(aString)) {
                valid = false;
                lblParseStatus.Text += Environment.NewLine + "Error: Ancillary " + removeErrorMarks(anAncillary.name) + " is not defined in export_ancillaries.txt." + Environment.NewLine;
            }
            //check that the description is defined in the VnV file
            if(hasError(anAncillary.descriptionName)) {
                valid = false;
                lblParseStatus.Text += Environment.NewLine + "Error: Description " + removeErrorMarks(anAncillary.descriptionName) + " is not defined in export_ancillaries.txt." + Environment.NewLine;
            }

            //check that the effectsDescription is defined in the VnV file
            if(hasError(anAncillary.effectsDescName)) {
                valid = false;
                lblParseStatus.Text += Environment.NewLine + "Error: EffectsDescription " + removeErrorMarks(anAncillary.effectsDescName) + " is not defined in export_ancillaries.txt." + Environment.NewLine;
            }

            //check for antiTraits that aren't valid traits, and if warnings are on for antiTraits that don't work in both directions
            if(anAncillary.excludedAncillaries.Count > Common.MAX_EXCLUDED_ANCILLARIES) {
                valid = false;
                lblParseStatus.Text += Environment.NewLine + "Error: Maximun ExcludedAncillaries allowed is " + Common.MAX_EXCLUDED_ANCILLARIES + "." + Environment.NewLine;
            }
            for(int i = 0; i < anAncillary.excludedAncillaries.Count; i += 1) {
                if(hasError(anAncillary.excludedAncillaries[i].ToString())) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: ExcludedAncillaries " + removeErrorMarks(anAncillary.excludedAncillaries[i].ToString()) + " is not defined." + Environment.NewLine;
                }
                if(((nextProblem && stopForWarnings) || !nextProblem) && (hasWarning(anAncillary.excludedAncillaries[i].ToString()))) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Warning: ExcludedAncillaries " + removeErrorMarks(anAncillary.excludedAncillaries[i].ToString()) + " does not work in both directions." + Environment.NewLine;
                }
            }

            //check for invalid cultures (this is based on default culture names
            for(int i = 0; i < anAncillary.excludeCultures.Count; i += 1) {
                if(hasError(anAncillary.excludeCultures[i].ToString())) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Invalid Culture: " + removeErrorMarks(anAncillary.excludeCultures[i].ToString()) + Environment.NewLine;
                }
            }

            //check each affect line and report if the attribute is a valid attribute and if the number is a non-zero integer
            for(int i = 0; i < anAncillary.effects.Count; i += 1) {
                anEffect = (Effect)anAncillary.effects[i];

                if(hasError(anEffect.name)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Attribute " + removeErrorMarks(anEffect.name) + " is not a valid attribute." + Environment.NewLine;
                }
                if(hasError(anEffect.pointsAllocated)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: For attribute " + removeErrorMarks(anEffect.name) + " the points allocated " + removeErrorMarks(anEffect.pointsAllocated) + " is not a non-zero integer." + Environment.NewLine;
                }
            }

            //check each religious belief line and report if the religion is a valid religion, and if the number is non-zero integer
            for(int i = 0; i < anAncillary.religiousBelief.Count; i += 1) {
                anEffect = (Effect)anAncillary.religiousBelief[i];

                if(hasError(anEffect.name)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Religion " + removeErrorMarks(anEffect.name) + " is not a valid religions." + Environment.NewLine;
                }
                if(hasError(anEffect.pointsAllocated)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: For religion " + removeErrorMarks(anEffect.name) + " the points allocated " + removeErrorMarks(anEffect.pointsAllocated) + " is not a non-zero integer." + Environment.NewLine;
                }
            }

            //check each religious order line and report if the religion is a valid religion, and if the number is non-zero integer
            for(int i = 0; i < anAncillary.religiousOrder.Count; i += 1) {
                anEffect = (Effect)anAncillary.religiousOrder[i];

                if(hasError(anEffect.name)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: Religion " + removeErrorMarks(anEffect.name) + " is not a valid religions." + Environment.NewLine;
                }
                if(hasError(anEffect.pointsAllocated)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: For religion " + removeErrorMarks(anEffect.name) + " the points allocated " + removeErrorMarks(anEffect.pointsAllocated) + " is not a non-zero integer." + Environment.NewLine;
                }
            }
            return valid;
        }

        private bool checkTrigger(bool nextProblem, int currentPosition, ArrayList aList, bool parsingTraits) {
            bool valid = true;
            Trigger aTrigger;
            Condition aCondition;
            Affect anAffect;

            aTrigger = (Trigger)aList[currentPosition];

            lblParseStatus.Text = "Checking trigger " + aTrigger.name + " . . ." + Environment.NewLine;
            lblParseStatus.Refresh();

            if(hasError(aTrigger.name, "(", ")")) {
                valid = false;
                lblParseStatus.Text += Environment.NewLine + "Error: Trigger " + aTrigger.name.Substring(1, aTrigger.name.Length - 2) + " already exists." + Environment.NewLine;
            }

            if(hasError(aTrigger.triggerEvent)) {
                valid = false;
                lblParseStatus.Text += Environment.NewLine + "Error: Event " + aTrigger.triggerEvent.Substring(1, aTrigger.triggerEvent.Length - 2) + " is either an invalid event or an event that doesn't export a character record." + Environment.NewLine;
            }

            for(int i = 0; i < aTrigger.conditions.Count; i += 1) {
                aCondition = (Condition)aTrigger.conditions[i];
                String name = aCondition.name;

                //check if the condition matches a defined condition
                if(hasError(aCondition.name)) {
                    valid = false;
                    name = name.Substring(1, name.Length - 2);
                    lblParseStatus.Text += Environment.NewLine + "Error: Condition " + name + " is not defined." + Environment.NewLine;
                }else{
                    for(int j = 0; j < aCondition.parameters.Count; j += 1) {
                        string aParameter = aCondition.parameters[j].ToString();
                        if(hasError(aParameter)) {
                            valid = false;
                            lblParseStatus.Text += Environment.NewLine + "Error: For condition " + name + ", " + aParameter.Substring(1, aParameter.Length - 2) + " is not a valid '" + aCondition.parameterNames[j].ToString() + "'." + Environment.NewLine;
                        }//end if (tempParam.IndexOf"{") > -1)
                    }//end for all parameters
                }//end if condition is a define condition - used to determine whether or not to check parameters
            }//end for all conditions

            if(parsingTraits && aTrigger.affectsList.Count > Common.MAX_AFFECTS_EDCT) {
                lblParseStatus.Text += Environment.NewLine + "Error: Trigger " + removeErrorMarks(aTrigger.name) + " has too many affects lines. In EDCT the maximum is 11.";
            }

            for(int i = 0; i < aTrigger.affectsList.Count; i += 1) {
                anAffect = (Affect)aTrigger.affectsList[i];
                string name = anAffect.name;

                if(hasError(anAffect.name)) {
                    valid = false;
                    name = name.Substring(1, name.Length - 2);
                    lblParseStatus.Text += Environment.NewLine + "Error: Trait " + name + " is not defined." + Environment.NewLine;
                }

                if(hasError(anAffect.points)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: For trait " + name + "  the points allocated " + anAffect.points.Substring(1, anAffect.points.Length - 2) + " is not a non-zero integer." + Environment.NewLine;

                }

                if(hasError(anAffect.chance)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: For trait " + name + "  the percentage chance " + anAffect.chance.Substring(1, anAffect.chance.Length - 2) + " is not a positive integer." + Environment.NewLine;

                }
            }

            for(int i = 0; i < aTrigger.acquireAncillaryList.Count; i += 1) {
                anAffect = (Affect)aTrigger.acquireAncillaryList[i];
                string name = anAffect.name;

                if(hasError(anAffect.name)) {
                    valid = false;
                    name = name.Substring(1, name.Length - 2);
                    lblParseStatus.Text += Environment.NewLine + "Error: Ancillary " + name + " is not defined." + Environment.NewLine;
                }

                if(hasError(anAffect.chance)) {
                    valid = false;
                    lblParseStatus.Text += Environment.NewLine + "Error: For ancillary " + name + "  the percentage chance " + anAffect.chance.Substring(1, anAffect.chance.Length - 2) + " is not a positive integer." + Environment.NewLine;
                }
            }

            return valid;
        }

        private void btnNextProblem_Click(object sender, EventArgs e) {
            if(tabControl1.SelectedIndex == TRAIT_TAB_INDEX) {
                validateTraits();
            } else if(tabControl1.SelectedIndex == TRAIT_TRIGGER_TAB_INDEX) {
                validateTriggers(false, ref checkedFirstTraitTrigger, ref currentTraitTrigger, ref Common.traitTriggers, ref traitTriggerLabels);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TAB_INDEX) {
                validateAncillaries();
            } else if(tabControl1.SelectedIndex == ANCILLARY_TRIGGER_TAB_INDEX) {
                validateTriggers(true, ref checkedFirstAncillaryTrigger, ref currentAncillaryTrigger, ref Common.ancillaryTriggers, ref ancillaryTriggerLabels);
            }else if (tabControl1.SelectedIndex == DESCR_STRAT_TAB_INDEX){
                parseDescrStrat();
            } else { //if (tabControl1.SelectedIndex == ORPHANS_TAB_INDEX){
                findOrphans();
            }
        }

        private void parseDescrStrat() {
            OpenFileDialog ofd = new OpenFileDialog();
            string[] tokenizedInput = null;
            int lineNumber = 0;

            //clear previous results
            txtDescrStrat.Text = "";
            ofd.InitialDirectory = GetPath() + @"world\maps\";
            ofd.Filter = "Descr_Strat.txt|descr_strat.txt";
            ofd.Title = "Select the descr_strat to parse";
            if(ofd.ShowDialog() == DialogResult.OK) {
                StreamReader reader = new StreamReader(ofd.OpenFile());
                lblParseStatus.Text = "Working . . .";
                while(!reader.EndOfStream) {
                    if(Common.readLine(ref reader, ref tokenizedInput, ref lineNumber)) {
                        switch(tokenizedInput[0]) {
                            case "faction":
                                lblParseStatus.Text += Environment.NewLine + "Processing faction " + tokenizedInput[1];
                                break;
                            case "traits":
                                for(int i = 1; i < tokenizedInput.Length - 1; i += 2) {
                                    if(!Common.traits.Contains(tokenizedInput[i])) {
                                        txtDescrStrat.Text += "Line " + lineNumber + " - Error: Trait " + tokenizedInput[i] + " is not defined." + Environment.NewLine;
                                    } else {
                                        int levelNumber;

                                        if(!int.TryParse(tokenizedInput[i + 1], out levelNumber) || levelNumber < 1) {
                                            txtDescrStrat.Text += "Line " + lineNumber + " - Error: For Trait " + tokenizedInput[i] + " expected a positive integer level number, found: " + tokenizedInput[i+1] + Environment.NewLine;
                                        } else {
                                            int index = Common.traits.IndexOf(tokenizedInput[i]);
                                            Trait aTrait = (Trait)Common.traits[index];
                                            if(levelNumber > aTrait.levels.Count) {
                                                txtDescrStrat.Text += "Line " + lineNumber + " - Error: For Trait " + tokenizedInput[i] + " the level number " + tokenizedInput[i + 1] + " is greater than the max level (" + aTrait.levels.Count + ")" + Environment.NewLine;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "ancillaries":
                                for(int i = 1; i < tokenizedInput.Length - 1; i += 1) {
                                    if(Common.findElement(tokenizedInput[i], Common.ancillaries) == -1) {
                                        txtDescrStrat.Text += "Line " + lineNumber + " - Error: Ancillary " + tokenizedInput[i] + " is not defined." + Environment.NewLine;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                lblParseStatus.Text += Environment.NewLine + ". . . Descr_Strat Processed";
                reader.Close();
            }
        }

        private void validateTraits() {
            bool valid = true;

            while(valid && currentTrait < Common.traits.Count - 1) {
                //if all the level of the previous trait have been checked . . .
                if(checkedFirstTrait) {
                    if(finishedCurrentItem) {
                        currentLevel = 0;
                        currentTrait += 1;
                    } else {
                        currentLevel += 1;
                    }
                }
                valid = checkTrait(true);
                checkedFirstTrait = true;
                finishedCurrentItem = false;
                for(; valid && currentLevel < ((Trait)Common.traits[currentTrait]).levels.Count; currentLevel += 1) {
                    valid = checkLevel(true) && valid;
                }//end for currentLevel has
                finishedCurrentItem = currentLevel == ((Trait)Common.traits[currentTrait]).levels.Count;
            }//end while(valid && currentTrait < Common.traits.Count)
            if(valid) {
                currentTrait = 0;
                currentLevel = 0;
                lblParseStatus.Text = "Finishing checking traits . . . no errors found.";
            } else {
                currentLevel = currentLevel == 0 ? 0 : currentLevel - 1;
            }
            refreshTrait(false);
        }

        private void validateAncillaries() {
            bool valid = true;

            while(valid && currentAncillary < Common.ancillaries.Count - 1) {
                if(checkedFirstAncillary) {
                    currentAncillary += 1;
                }
                valid = checkAncillary(true);
                checkedFirstAncillary = true;
            }
            if(valid) {
                currentAncillary = 0;
                lblParseStatus.Text = "Finished checking ancillaries . . . no errors found.";
            }
            refreshAncillary();
        }

        private void validateTriggers(bool checkingAncillaries, ref bool checkedFirst, ref int currentPosition, ref ArrayList aList, ref TriggerLabels labels) {
            bool valid = true;

            while(valid && currentPosition < aList.Count - 1) {
                if(checkedFirst) {
                    currentPosition += 1;
                }
                valid = checkTrigger(true, currentPosition, aList, !checkingAncillaries);
                checkedFirst = true;
            }
            if(valid) {
                currentPosition = 0;
                lblParseStatus.Text = "Finished checking " + (checkingAncillaries ? "ancillary" : "trait") + " triggers . . . no errors found.";
            }
            refreshTrigger(checkingAncillaries, currentPosition, ref labels, aList);
        }

        private void resetTraitTestingVariables() {
            checkedFirstTrait = false;
            finishedCurrentItem = false;
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            bool valid = false;
            int position;
            if(tabControl1.SelectedIndex == TRAIT_TAB_INDEX) {
                position = Common.findElementPartialMatch(txtSearchBox.Text, Common.traits, currentTrait + 1);
                if(position == -1) {
                    lblParseStatus.Text = "No matching trait found for string " + txtSearchBox.Text + ".";
                } else {
                    currentTrait = position;
                    resetTraitTestingVariables();
                    refreshTrait(true);
                    valid = checkTrait(false) & checkLevel(false);
                }
            } else if(tabControl1.SelectedIndex == TRAIT_TRIGGER_TAB_INDEX) {
                valid = searchTriggers(false, ref checkedFirstTraitTrigger, ref currentTraitTrigger, traitTriggerLabels, Common.traitTriggers);
            } else if(tabControl1.SelectedIndex == ANCILLARY_TAB_INDEX) {
                position = Common.findElementPartialMatch(txtSearchBox.Text, Common.ancillaries, currentAncillary + 1);
                if(position == -1) {
                    lblParseStatus.Text = "No matching ancillary found for string " + txtSearchBox.Text + ".";
                } else {
                    currentAncillary = position;
                    checkedFirstAncillary = false;
                    refreshAncillary();
                    valid = checkAncillary(false);
                }
            } else if(tabControl1.SelectedIndex == ANCILLARY_TRIGGER_TAB_INDEX) {
                valid = searchTriggers(true, ref checkedFirstAncillaryTrigger, ref currentAncillaryTrigger, ancillaryTriggerLabels, Common.ancillaryTriggers);
            } else {
                //do nothing, button should be disabled
            }
            if(valid) {
                lblParseStatus.Text += Environment.NewLine + "No errors or warning found.";
            }
        }

        private bool searchTriggers(bool refreshAncillary, ref bool checkFirstTrigger, ref int currentPosition, TriggerLabels labels, ArrayList aList) {
            bool valid;
            int position = Common.findElementPartialMatch(txtSearchBox.Text, aList, currentPosition + 1);
            if ( position == -1 ) {
                valid = false;
                lblParseStatus.Text = "No matching trigger found for string " + txtSearchBox.Text + ".";
            } else {
                currentPosition = position;
                checkFirstTrigger = false;
                refreshTrigger(refreshAncillary, currentPosition, ref labels, aList);
                valid = checkTrigger(false, currentPosition, aList, !refreshAncillary);
            }
            return valid;
        }

        private void mnuViewTabs_Click(object sender, EventArgs e) {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            tabControl1.SelectedIndex = Int16.Parse(item.Name.Substring(item.Name.Length - 1));
        }

        private void findOrphans() {
            // clear any previous results
            txtOrphans.Text = "";

            // identify orphaned traits and ancillaries (i.e. traits and ancillaries that have no associated triggers)
            // we've got all trigger and all trait/ancillary information if the user has parsed both files
            Trait tempTrait = null;
            Ancillary tempAnc = null;
            Trigger tempTrig = null;
            string aString;

            lblParseStatus.Text = "Working...";

            // traits
            for(int i = 0; i < Common.traits.Count; i += 1) {
                bool foundMatch = false;
                tempTrait = (Trait)Common.traits[i];
                aString = removeErrorMarks(tempTrait.name);

                for(int j = 0; j < Common.traitTriggers.Count; j += 1) {
                    tempTrig = (Trigger)Common.traitTriggers[j];
                    if(findItemInTrigger(aString, ref tempTrig.affectsList)) {
                        foundMatch = true;
                        break;
                    }
                }

                for(int j = 0; !foundMatch && j < Common.ancillaryTriggers.Count; j += 1) {
                    tempTrig = (Trigger)Common.ancillaryTriggers[j];
                    if(findItemInTrigger(aString, ref tempTrig.affectsList)) {
                        foundMatch = true;
                        break;
                    }
                }

                if(!foundMatch) {
                    // notify that we checked all triggers and did not find this trait
                    txtOrphans.Text = txtOrphans.Text + "ORPHANED TRAIT: " + aString + Environment.NewLine;
                    txtOrphans.Refresh();
                }
            }

            lblParseStatus.Text += Environment.NewLine + "Checked traits.";

            // ancillaries
            for(int i = 0; i < Common.ancillaries.Count; i += 1) {
                bool foundMatch = false;
                tempAnc = (Ancillary)Common.ancillaries[i];
                aString = removeErrorMarks(tempAnc.name);

                for(int j = 0; j < Common.ancillaryTriggers.Count; j += 1) {
                    tempTrig = (Trigger)Common.ancillaryTriggers[j];
                    if(findItemInTrigger(aString, ref tempTrig.acquireAncillaryList)) {
                        // we found a match
                        foundMatch = true;
                        break;
                    }
                }

                if(!foundMatch) {
                    // notify that we checked all triggers and did not find this ancillary
                    txtOrphans.Text = txtOrphans.Text + "ORPHANED ANCILLARY: " + aString + Environment.NewLine;
                    txtOrphans.Refresh();
                }
            }

            lblParseStatus.Text = lblParseStatus.Text + Environment.NewLine + "Checked ancillaries.";
            lblParseStatus.Text = lblParseStatus.Text + Environment.NewLine + "Check complete.";
        }

        private bool findItemInTrigger(string nameToFind, ref ArrayList affectsList) {
            bool foundMatch = false;
            Affect tempAffect;

            for(int i = 0; i < affectsList.Count; i += 1) {
                tempAffect = (Affect)affectsList[i];

                if(nameToFind == tempAffect.name) {
                    // we found a match
                    foundMatch = true;
                    break;
                }
            }
            return foundMatch;

        }

        private void lstAffects_MouseDoubleClick(Object sender, EventArgs e) {
            ListBox tempBox = ( ListBox )sender;
            bool valid = false;
            if(tempBox.Items.Count > 0){
                Affect tempAffect = (Affect)tempBox.SelectedItem;
                int tempInt = Common.findElement(tempAffect.name, Common.traits);
                if(tempInt > -1){
                    currentTrait = tempInt;
                    refreshTrait(true);
                    tabControl1.SelectedIndex = TRAIT_TAB_INDEX;
                    checkedFirstTrait = false;
                    valid = checkTrait(false) & checkLevel(false);
                }

                if(valid){
                    lblParseStatus.Text += Environment.NewLine + "No errors or warning found.";
                }

            }
        }

        private void lstAncillaryAcquireAncillary_MouseDoubleClick(Object sender, EventArgs e) {
            ListBox tempBox = ( ListBox )sender;
            bool valid = false;
            if ( tempBox.Items.Count > 0 ) {
                Affect tempAffect = ( Affect )tempBox.SelectedItem;
                int tempInt = Common.findElement(tempAffect.name, Common.ancillaries);
                if ( tempInt > -1 ) {
                    currentAncillary = tempInt;
                    refreshAncillary();
                    tabControl1.SelectedIndex = ANCILLARY_TAB_INDEX;
                    checkedFirstAncillary = false;
                    valid = checkAncillary(false);
                }

                if ( valid ) {
                    lblParseStatus.Text += Environment.NewLine + "No errors or warning found.";
                }

            }
        }

        private void mnuOptions_Click(object sender, EventArgs e) {
            frmOptions opt = new frmOptions();
            bool warnings = stopForWarnings;

            if(opt.ShowDialog(ref warnings) == DialogResult.OK){
                stopForWarnings = warnings;
            }
        }
    }
}