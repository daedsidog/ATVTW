using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace ATVTW
{
    /// <summary>
	/// Summary description for frmATVTW.
	/// </summary>
	public class frmATVTW : System.Windows.Forms.Form
	{
		private bool parsedAncillaries = false;
		private bool parsedTraits = false;

		private ArrayList conditionTemplates;
		private ArrayList attributeTemplates;
		private ArrayList eventTemplates;

		private ArrayList traits;
		private ArrayList triggers;

		private ArrayList ancillaries;
		private ArrayList ancillaryTriggers;

		private bool bProcessingTraits;
		private bool bProcessingAncillaries;
		
		// init the interface helper vars
		private Trait currentTrait;
		private int currentTraitNumber;
		private int currentTriggerNumber;

		private Ancillary currentAncillary;
		private int currentAncillaryNumber;
		private int currentAncillaryTriggerNumber;

		private int currentLevelNumber;

		private int MAX_CHARACTER_TYPES;
		private int MAX_EXCLUDE_CULTURES;
		private int MAX_EXCLUDE_ANCILLARIES;
		private int MAX_ANTI_TRAITS;

        #region "Windows Form Designer Generated Declarations"

        private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage traitPage;
		private System.Windows.Forms.TabPage traitTriggerPage;
		private System.Windows.Forms.TabPage ancillaryPage;
		private System.Windows.Forms.TabPage ancillaryTriggerPage;
		private System.Windows.Forms.Button btnNextProblemTrait;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label lblLevelEffects;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Label lblLevelThreshhold;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label lblLevelEpithet;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label lblLevelLose;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label lblLevelGain;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label lblLevelEffectDesc;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label lblLevelDesc;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label lblLevelName;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblNoGoingBackLevel;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label lblExcludesCultures;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblAntiTraits;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblCharacters;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblTraitName;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblParseStatus;
		private System.Windows.Forms.Button btnFindProblemTriggerTrait;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button14;
		private System.Windows.Forms.Button button15;
		private System.Windows.Forms.Button button16;
		private System.Windows.Forms.Button button17;
		private System.Windows.Forms.Button button18;
		private System.Windows.Forms.Button button19;
		private System.Windows.Forms.Button button20;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label44;
		private System.Windows.Forms.Label lblTriggerTraitName;
		private System.Windows.Forms.Button button23;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Button button21;
		private System.Windows.Forms.Button button22;
		private System.Windows.Forms.Label lblTriggerConditions;
		private System.Windows.Forms.Label lblWhenToTest;
		private System.Windows.Forms.Label lblTriggerName;
		private System.Windows.Forms.Label lblTriggerAffectsName;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label lblTriggerAffectsChance;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label lblTriggerAffectsLevel;
		private System.Windows.Forms.Button button24;
		private System.Windows.Forms.Button button25;
		private System.Windows.Forms.TextBox txtTraitName;
		private System.Windows.Forms.Button btnTraitName;
		private System.Windows.Forms.Label lblHidden;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Button btnNextProblemAncillary;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Button button28;
		private System.Windows.Forms.Button button30;
		private System.Windows.Forms.Button button31;
		private System.Windows.Forms.Button button32;
		private System.Windows.Forms.Button button34;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.Label label58;
		private System.Windows.Forms.Button button37;
		private System.Windows.Forms.Label label60;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.Label lblTriggerStatus;
		private System.Windows.Forms.Button button29;
		private System.Windows.Forms.Button button33;
		private System.Windows.Forms.Label label65;
		private System.Windows.Forms.Button button38;
		private System.Windows.Forms.Button button39;
		private System.Windows.Forms.Label label67;
		private System.Windows.Forms.Button btnFindProblemTriggerAncillary;
		private System.Windows.Forms.Label label68;
		private System.Windows.Forms.Button button41;
		private System.Windows.Forms.Button button43;
		private System.Windows.Forms.Button button44;
		private System.Windows.Forms.Button button45;
		private System.Windows.Forms.Button button47;
		private System.Windows.Forms.Label label71;
		private System.Windows.Forms.Label label72;
		private System.Windows.Forms.Label label74;
		private System.Windows.Forms.Label label76;
		private System.Windows.Forms.Label label77;
		private System.Windows.Forms.Button button48;
		private System.Windows.Forms.Label label78;
		private System.Windows.Forms.Label label75;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtTraitsFilePath;
		private System.Windows.Forms.Button btnParseTraits;
		private System.Windows.Forms.Label label81;
		private System.Windows.Forms.Button btnParseAncillaries;
		private System.Windows.Forms.Label lblAncillaryStatus;
		private System.Windows.Forms.Label lblAncillaryTriggerStatus;
		private System.Windows.Forms.TextBox txtAncillaryPath;
		private System.Windows.Forms.Label lblExcludesAncillaries;
		private System.Windows.Forms.Label lblAncillaryName;
		private System.Windows.Forms.Label lblUniqueAncillary;
		private System.Windows.Forms.Label lblAncillaryEffects;
		private System.Windows.Forms.Label lblAncillaryEffectsDescription;
		private System.Windows.Forms.Label lblAncillaryDescription;
		private System.Windows.Forms.Label lblExcludesAncillaryCultures;
		private System.Windows.Forms.Label lblExcludesAncillaries2;
		private System.Windows.Forms.Label lblAncillaryEffectChance;
		private System.Windows.Forms.Label lblAncillaryEffectName;
		private System.Windows.Forms.Label lblAncillaryConditions;
		private System.Windows.Forms.Label lblAncillaryWhenTested;
		private System.Windows.Forms.Label lblAncillaryTriggerName;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label lblAncillaryImageFile;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.TabPage orphans;
		private System.Windows.Forms.TextBox txtOrphans;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Button button26;
		private System.Windows.Forms.TextBox txtOrphanStatus;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.TabPage options;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.CheckBox chkTraitAffect;
        private MenuStrip mnuMainMenu;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuExit;
        private ToolStripMenuItem mnuTools;
        private ToolStripMenuItem mnuToolsOptions;
        private ToolStripMenuItem mnuHelp;
        private ToolStripMenuItem mnuHelpAbout;
        private Button btnAncillarySearch;
        private TextBox txtAncillaryName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        #endregion

        public frmATVTW()
		{
			InitializeComponent();

			// init constants
			MAX_CHARACTER_TYPES = 10;
			MAX_EXCLUDE_CULTURES = 30;
			MAX_EXCLUDE_ANCILLARIES = 10;
			MAX_ANTI_TRAITS = 10;

			// init arrays
			conditionTemplates = new ArrayList();
			attributeTemplates = new ArrayList();
            eventTemplates = new ArrayList();

			// read conditions
			generateConditions();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmATVTW));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.traitPage = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTraitsFilePath = new System.Windows.Forms.TextBox();
            this.btnParseTraits = new System.Windows.Forms.Button();
            this.lblHidden = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btnTraitName = new System.Windows.Forms.Button();
            this.txtTraitName = new System.Windows.Forms.TextBox();
            this.btnNextProblemTrait = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.lblLevelEffects = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.lblLevelThreshhold = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lblLevelEpithet = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblLevelLose = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblLevelGain = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblLevelEffectDesc = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblLevelDesc = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lblLevelName = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lblNoGoingBackLevel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblExcludesCultures = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblAntiTraits = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCharacters = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTraitName = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblParseStatus = new System.Windows.Forms.Label();
            this.traitTriggerPage = new System.Windows.Forms.TabPage();
            this.button25 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.lblTriggerAffectsLevel = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblTriggerAffectsChance = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.button22 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.lblTriggerAffectsName = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnFindProblemTriggerTrait = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTriggerConditions = new System.Windows.Forms.Label();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.lblWhenToTest = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.lblTriggerName = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.lblTriggerTraitName = new System.Windows.Forms.Label();
            this.button23 = new System.Windows.Forms.Button();
            this.label46 = new System.Windows.Forms.Label();
            this.lblTriggerStatus = new System.Windows.Forms.Label();
            this.ancillaryPage = new System.Windows.Forms.TabPage();
            this.btnAncillarySearch = new System.Windows.Forms.Button();
            this.txtAncillaryName = new System.Windows.Forms.TextBox();
            this.lblAncillaryImageFile = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.txtAncillaryPath = new System.Windows.Forms.TextBox();
            this.btnParseAncillaries = new System.Windows.Forms.Button();
            this.lblUniqueAncillary = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btnNextProblemAncillary = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.lblAncillaryEffects = new System.Windows.Forms.Label();
            this.button28 = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button34 = new System.Windows.Forms.Button();
            this.lblAncillaryEffectsDescription = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.lblAncillaryDescription = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.lblExcludesAncillaryCultures = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.lblExcludesAncillaries2 = new System.Windows.Forms.Label();
            this.lblExcludesAncillaries = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.lblAncillaryName = new System.Windows.Forms.Label();
            this.button37 = new System.Windows.Forms.Button();
            this.label60 = new System.Windows.Forms.Label();
            this.lblAncillaryStatus = new System.Windows.Forms.Label();
            this.ancillaryTriggerPage = new System.Windows.Forms.TabPage();
            this.label75 = new System.Windows.Forms.Label();
            this.button29 = new System.Windows.Forms.Button();
            this.button33 = new System.Windows.Forms.Button();
            this.lblAncillaryEffectChance = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.button38 = new System.Windows.Forms.Button();
            this.button39 = new System.Windows.Forms.Button();
            this.lblAncillaryEffectName = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.btnFindProblemTriggerAncillary = new System.Windows.Forms.Button();
            this.label68 = new System.Windows.Forms.Label();
            this.lblAncillaryConditions = new System.Windows.Forms.Label();
            this.button41 = new System.Windows.Forms.Button();
            this.button43 = new System.Windows.Forms.Button();
            this.button44 = new System.Windows.Forms.Button();
            this.button45 = new System.Windows.Forms.Button();
            this.button47 = new System.Windows.Forms.Button();
            this.lblAncillaryWhenTested = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.lblAncillaryTriggerName = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.button48 = new System.Windows.Forms.Button();
            this.label78 = new System.Windows.Forms.Label();
            this.lblAncillaryTriggerStatus = new System.Windows.Forms.Label();
            this.orphans = new System.Windows.Forms.TabPage();
            this.label39 = new System.Windows.Forms.Label();
            this.txtOrphanStatus = new System.Windows.Forms.TextBox();
            this.button26 = new System.Windows.Forms.Button();
            this.label38 = new System.Windows.Forms.Label();
            this.txtOrphans = new System.Windows.Forms.TextBox();
            this.options = new System.Windows.Forms.TabPage();
            this.chkTraitAffect = new System.Windows.Forms.CheckBox();
            this.label47 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.mnuMainMenu = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.traitPage.SuspendLayout();
            this.traitTriggerPage.SuspendLayout();
            this.ancillaryPage.SuspendLayout();
            this.ancillaryTriggerPage.SuspendLayout();
            this.orphans.SuspendLayout();
            this.options.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.mnuMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.traitPage);
            this.tabControl1.Controls.Add(this.traitTriggerPage);
            this.tabControl1.Controls.Add(this.ancillaryPage);
            this.tabControl1.Controls.Add(this.ancillaryTriggerPage);
            this.tabControl1.Controls.Add(this.orphans);
            this.tabControl1.Controls.Add(this.options);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(700, 513);
            this.tabControl1.TabIndex = 58;
            // 
            // traitPage
            // 
            this.traitPage.Controls.Add(this.label2);
            this.traitPage.Controls.Add(this.txtTraitsFilePath);
            this.traitPage.Controls.Add(this.btnParseTraits);
            this.traitPage.Controls.Add(this.lblHidden);
            this.traitPage.Controls.Add(this.label19);
            this.traitPage.Controls.Add(this.btnTraitName);
            this.traitPage.Controls.Add(this.txtTraitName);
            this.traitPage.Controls.Add(this.btnNextProblemTrait);
            this.traitPage.Controls.Add(this.label11);
            this.traitPage.Controls.Add(this.lblLevelEffects);
            this.traitPage.Controls.Add(this.button11);
            this.traitPage.Controls.Add(this.button10);
            this.traitPage.Controls.Add(this.button3);
            this.traitPage.Controls.Add(this.button9);
            this.traitPage.Controls.Add(this.button8);
            this.traitPage.Controls.Add(this.button7);
            this.traitPage.Controls.Add(this.button6);
            this.traitPage.Controls.Add(this.lblLevelThreshhold);
            this.traitPage.Controls.Add(this.label24);
            this.traitPage.Controls.Add(this.lblLevelEpithet);
            this.traitPage.Controls.Add(this.label12);
            this.traitPage.Controls.Add(this.lblLevelLose);
            this.traitPage.Controls.Add(this.label14);
            this.traitPage.Controls.Add(this.lblLevelGain);
            this.traitPage.Controls.Add(this.label16);
            this.traitPage.Controls.Add(this.lblLevelEffectDesc);
            this.traitPage.Controls.Add(this.label18);
            this.traitPage.Controls.Add(this.lblLevelDesc);
            this.traitPage.Controls.Add(this.label20);
            this.traitPage.Controls.Add(this.label21);
            this.traitPage.Controls.Add(this.lblLevelName);
            this.traitPage.Controls.Add(this.button4);
            this.traitPage.Controls.Add(this.label10);
            this.traitPage.Controls.Add(this.button5);
            this.traitPage.Controls.Add(this.label9);
            this.traitPage.Controls.Add(this.lblNoGoingBackLevel);
            this.traitPage.Controls.Add(this.label8);
            this.traitPage.Controls.Add(this.lblExcludesCultures);
            this.traitPage.Controls.Add(this.label7);
            this.traitPage.Controls.Add(this.lblAntiTraits);
            this.traitPage.Controls.Add(this.label6);
            this.traitPage.Controls.Add(this.lblCharacters);
            this.traitPage.Controls.Add(this.label5);
            this.traitPage.Controls.Add(this.label3);
            this.traitPage.Controls.Add(this.lblTraitName);
            this.traitPage.Controls.Add(this.button2);
            this.traitPage.Controls.Add(this.label1);
            this.traitPage.Controls.Add(this.lblParseStatus);
            this.traitPage.Location = new System.Drawing.Point(4, 22);
            this.traitPage.Name = "traitPage";
            this.traitPage.Size = new System.Drawing.Size(692, 487);
            this.traitPage.TabIndex = 0;
            this.traitPage.Text = "Traits";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label2.Location = new System.Drawing.Point(40, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.TabIndex = 107;
            this.label2.Text = "Path to traits:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTraitsFilePath
            // 
            this.txtTraitsFilePath.Location = new System.Drawing.Point(147, 14);
            this.txtTraitsFilePath.Name = "txtTraitsFilePath";
            this.txtTraitsFilePath.Size = new System.Drawing.Size(446, 20);
            this.txtTraitsFilePath.TabIndex = 106;
            this.txtTraitsFilePath.Text = "C:/Program Files/Activision/Rome - Total War/Data/";
            // 
            // btnParseTraits
            // 
            this.btnParseTraits.Location = new System.Drawing.Point(600, 14);
            this.btnParseTraits.Name = "btnParseTraits";
            this.btnParseTraits.Size = new System.Drawing.Size(87, 21);
            this.btnParseTraits.TabIndex = 105;
            this.btnParseTraits.Text = "Parse Traits";
            this.btnParseTraits.Click += new System.EventHandler(this.btnParseTraits_Click);
            // 
            // lblHidden
            // 
            this.lblHidden.Location = new System.Drawing.Point(147, 180);
            this.lblHidden.Name = "lblHidden";
            this.lblHidden.Size = new System.Drawing.Size(246, 21);
            this.lblHidden.TabIndex = 104;
            this.lblHidden.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label19.Location = new System.Drawing.Point(27, 180);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(113, 20);
            this.label19.TabIndex = 103;
            this.label19.Text = "Hidden?";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTraitName
            // 
            this.btnTraitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.btnTraitName.Location = new System.Drawing.Point(373, 76);
            this.btnTraitName.Name = "btnTraitName";
            this.btnTraitName.Size = new System.Drawing.Size(54, 21);
            this.btnTraitName.TabIndex = 100;
            this.btnTraitName.Text = "Search";
            this.btnTraitName.Click += new System.EventHandler(this.btnTraitName_Click);
            // 
            // txtTraitName
            // 
            this.txtTraitName.Location = new System.Drawing.Point(147, 76);
            this.txtTraitName.Name = "txtTraitName";
            this.txtTraitName.Size = new System.Drawing.Size(226, 20);
            this.txtTraitName.TabIndex = 99;
            this.txtTraitName.Visible = false;
            // 
            // btnNextProblemTrait
            // 
            this.btnNextProblemTrait.BackColor = System.Drawing.Color.Coral;
            this.btnNextProblemTrait.Enabled = false;
            this.btnNextProblemTrait.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.btnNextProblemTrait.Location = new System.Drawing.Point(433, 42);
            this.btnNextProblemTrait.Name = "btnNextProblemTrait";
            this.btnNextProblemTrait.Size = new System.Drawing.Size(87, 27);
            this.btnNextProblemTrait.TabIndex = 98;
            this.btnNextProblemTrait.Text = "Next Problem";
            this.btnNextProblemTrait.UseVisualStyleBackColor = false;
            this.btnNextProblemTrait.Click += new System.EventHandler(this.btnNextProblemTrait_Click);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label11.Location = new System.Drawing.Point(27, 381);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 20);
            this.label11.TabIndex = 97;
            this.label11.Text = "Effects:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLevelEffects
            // 
            this.lblLevelEffects.Location = new System.Drawing.Point(147, 381);
            this.lblLevelEffects.Name = "lblLevelEffects";
            this.lblLevelEffects.Size = new System.Drawing.Size(246, 97);
            this.lblLevelEffects.TabIndex = 96;
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.YellowGreen;
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button11.Location = new System.Drawing.Point(7, 42);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(46, 20);
            this.button11.TabIndex = 95;
            this.button11.Text = "< Start";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button10.Location = new System.Drawing.Point(60, 42);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(40, 20);
            this.button10.TabIndex = 94;
            this.button10.Text = "< 20";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button3.Location = new System.Drawing.Point(107, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 20);
            this.button3.TabIndex = 93;
            this.button3.Text = "< 5";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button9.Location = new System.Drawing.Point(147, 42);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(33, 20);
            this.button9.TabIndex = 92;
            this.button9.Text = "< 1";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.YellowGreen;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button8.Location = new System.Drawing.Point(380, 42);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(47, 20);
            this.button8.TabIndex = 91;
            this.button8.Text = "End>";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button7.Location = new System.Drawing.Point(333, 42);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(40, 20);
            this.button7.TabIndex = 90;
            this.button7.Text = "20 >";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button6.Location = new System.Drawing.Point(293, 42);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(34, 20);
            this.button6.TabIndex = 89;
            this.button6.Text = "5 >";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // lblLevelThreshhold
            // 
            this.lblLevelThreshhold.Location = new System.Drawing.Point(147, 361);
            this.lblLevelThreshhold.Name = "lblLevelThreshhold";
            this.lblLevelThreshhold.Size = new System.Drawing.Size(246, 20);
            this.lblLevelThreshhold.TabIndex = 88;
            this.lblLevelThreshhold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label24.Location = new System.Drawing.Point(27, 361);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(113, 19);
            this.label24.TabIndex = 87;
            this.label24.Text = "Threshhold:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLevelEpithet
            // 
            this.lblLevelEpithet.Location = new System.Drawing.Point(147, 340);
            this.lblLevelEpithet.Name = "lblLevelEpithet";
            this.lblLevelEpithet.Size = new System.Drawing.Size(246, 21);
            this.lblLevelEpithet.TabIndex = 86;
            this.lblLevelEpithet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label12.Location = new System.Drawing.Point(27, 340);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 20);
            this.label12.TabIndex = 85;
            this.label12.Text = "Epithet:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLevelLose
            // 
            this.lblLevelLose.Location = new System.Drawing.Point(147, 319);
            this.lblLevelLose.Name = "lblLevelLose";
            this.lblLevelLose.Size = new System.Drawing.Size(246, 21);
            this.lblLevelLose.TabIndex = 84;
            this.lblLevelLose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label14.Location = new System.Drawing.Point(27, 319);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(113, 20);
            this.label14.TabIndex = 83;
            this.label14.Text = "Lose Message:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLevelGain
            // 
            this.lblLevelGain.Location = new System.Drawing.Point(147, 298);
            this.lblLevelGain.Name = "lblLevelGain";
            this.lblLevelGain.Size = new System.Drawing.Size(246, 21);
            this.lblLevelGain.TabIndex = 82;
            this.lblLevelGain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label16.Location = new System.Drawing.Point(27, 298);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(113, 20);
            this.label16.TabIndex = 81;
            this.label16.Text = "Gain Message:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLevelEffectDesc
            // 
            this.lblLevelEffectDesc.Location = new System.Drawing.Point(147, 277);
            this.lblLevelEffectDesc.Name = "lblLevelEffectDesc";
            this.lblLevelEffectDesc.Size = new System.Drawing.Size(246, 21);
            this.lblLevelEffectDesc.TabIndex = 80;
            this.lblLevelEffectDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label18.Location = new System.Drawing.Point(27, 277);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(113, 20);
            this.label18.TabIndex = 79;
            this.label18.Text = "Effects Description:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLevelDesc
            // 
            this.lblLevelDesc.Location = new System.Drawing.Point(147, 257);
            this.lblLevelDesc.Name = "lblLevelDesc";
            this.lblLevelDesc.Size = new System.Drawing.Size(246, 20);
            this.lblLevelDesc.TabIndex = 78;
            this.lblLevelDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label20.Location = new System.Drawing.Point(40, 257);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 19);
            this.label20.TabIndex = 77;
            this.label20.Text = "Description:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label21.Location = new System.Drawing.Point(53, 236);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(87, 20);
            this.label21.TabIndex = 76;
            this.label21.Text = "Level Name:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLevelName
            // 
            this.lblLevelName.ForeColor = System.Drawing.Color.Black;
            this.lblLevelName.Location = new System.Drawing.Point(147, 236);
            this.lblLevelName.Name = "lblLevelName";
            this.lblLevelName.Size = new System.Drawing.Size(246, 21);
            this.lblLevelName.TabIndex = 75;
            this.lblLevelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button4.Location = new System.Drawing.Point(7, 201);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(20, 28);
            this.button4.TabIndex = 74;
            this.button4.Text = "<";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label10.Location = new System.Drawing.Point(187, 208);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 21);
            this.label10.TabIndex = 73;
            this.label10.Text = "LEVELS";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button5.Location = new System.Drawing.Point(380, 201);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(20, 28);
            this.button5.TabIndex = 72;
            this.button5.Text = ">";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label9.Location = new System.Drawing.Point(187, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 20);
            this.label9.TabIndex = 71;
            this.label9.Text = "TRAITS";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNoGoingBackLevel
            // 
            this.lblNoGoingBackLevel.Location = new System.Drawing.Point(147, 159);
            this.lblNoGoingBackLevel.Name = "lblNoGoingBackLevel";
            this.lblNoGoingBackLevel.Size = new System.Drawing.Size(246, 21);
            this.lblNoGoingBackLevel.TabIndex = 70;
            this.lblNoGoingBackLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label8.Location = new System.Drawing.Point(27, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 20);
            this.label8.TabIndex = 69;
            this.label8.Text = "NoGoingBack Level:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExcludesCultures
            // 
            this.lblExcludesCultures.Location = new System.Drawing.Point(147, 139);
            this.lblExcludesCultures.Name = "lblExcludesCultures";
            this.lblExcludesCultures.Size = new System.Drawing.Size(246, 20);
            this.lblExcludesCultures.TabIndex = 68;
            this.lblExcludesCultures.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label7.Location = new System.Drawing.Point(27, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 20);
            this.label7.TabIndex = 67;
            this.label7.Text = "Excludes Cultures:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAntiTraits
            // 
            this.lblAntiTraits.Location = new System.Drawing.Point(147, 118);
            this.lblAntiTraits.Name = "lblAntiTraits";
            this.lblAntiTraits.Size = new System.Drawing.Size(246, 21);
            this.lblAntiTraits.TabIndex = 66;
            this.lblAntiTraits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label6.Location = new System.Drawing.Point(27, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 20);
            this.label6.TabIndex = 65;
            this.label6.Text = "Anti-Traits:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCharacters
            // 
            this.lblCharacters.Location = new System.Drawing.Point(147, 97);
            this.lblCharacters.Name = "lblCharacters";
            this.lblCharacters.Size = new System.Drawing.Size(246, 21);
            this.lblCharacters.TabIndex = 64;
            this.lblCharacters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label5.Location = new System.Drawing.Point(27, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 20);
            this.label5.TabIndex = 63;
            this.label5.Text = "Applies to Characters:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label3.Location = new System.Drawing.Point(53, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 20);
            this.label3.TabIndex = 62;
            this.label3.Text = "Trait Name:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTraitName
            // 
            this.lblTraitName.Location = new System.Drawing.Point(147, 76);
            this.lblTraitName.Name = "lblTraitName";
            this.lblTraitName.Size = new System.Drawing.Size(226, 21);
            this.lblTraitName.TabIndex = 61;
            this.lblTraitName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTraitName.Click += new System.EventHandler(this.lblTraitName_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button2.Location = new System.Drawing.Point(253, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(34, 20);
            this.button2.TabIndex = 60;
            this.button2.Text = "1 >";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label1.Location = new System.Drawing.Point(600, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 19);
            this.label1.TabIndex = 59;
            this.label1.Text = "Status:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblParseStatus
            // 
            this.lblParseStatus.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblParseStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblParseStatus.Location = new System.Drawing.Point(433, 76);
            this.lblParseStatus.Name = "lblParseStatus";
            this.lblParseStatus.Size = new System.Drawing.Size(254, 402);
            this.lblParseStatus.TabIndex = 58;
            this.lblParseStatus.Text = resources.GetString("lblParseStatus.Text");
            // 
            // traitTriggerPage
            // 
            this.traitTriggerPage.Controls.Add(this.button25);
            this.traitTriggerPage.Controls.Add(this.button24);
            this.traitTriggerPage.Controls.Add(this.lblTriggerAffectsLevel);
            this.traitTriggerPage.Controls.Add(this.label17);
            this.traitTriggerPage.Controls.Add(this.lblTriggerAffectsChance);
            this.traitTriggerPage.Controls.Add(this.label13);
            this.traitTriggerPage.Controls.Add(this.button22);
            this.traitTriggerPage.Controls.Add(this.button21);
            this.traitTriggerPage.Controls.Add(this.lblTriggerAffectsName);
            this.traitTriggerPage.Controls.Add(this.label15);
            this.traitTriggerPage.Controls.Add(this.btnFindProblemTriggerTrait);
            this.traitTriggerPage.Controls.Add(this.label4);
            this.traitTriggerPage.Controls.Add(this.lblTriggerConditions);
            this.traitTriggerPage.Controls.Add(this.button14);
            this.traitTriggerPage.Controls.Add(this.button15);
            this.traitTriggerPage.Controls.Add(this.button16);
            this.traitTriggerPage.Controls.Add(this.button17);
            this.traitTriggerPage.Controls.Add(this.button18);
            this.traitTriggerPage.Controls.Add(this.button19);
            this.traitTriggerPage.Controls.Add(this.button20);
            this.traitTriggerPage.Controls.Add(this.lblWhenToTest);
            this.traitTriggerPage.Controls.Add(this.label31);
            this.traitTriggerPage.Controls.Add(this.label32);
            this.traitTriggerPage.Controls.Add(this.lblTriggerName);
            this.traitTriggerPage.Controls.Add(this.label34);
            this.traitTriggerPage.Controls.Add(this.label35);
            this.traitTriggerPage.Controls.Add(this.label44);
            this.traitTriggerPage.Controls.Add(this.lblTriggerTraitName);
            this.traitTriggerPage.Controls.Add(this.button23);
            this.traitTriggerPage.Controls.Add(this.label46);
            this.traitTriggerPage.Controls.Add(this.lblTriggerStatus);
            this.traitTriggerPage.Location = new System.Drawing.Point(4, 22);
            this.traitTriggerPage.Name = "traitTriggerPage";
            this.traitTriggerPage.Size = new System.Drawing.Size(692, 487);
            this.traitTriggerPage.TabIndex = 1;
            this.traitTriggerPage.Text = "Trait Triggers";
            // 
            // button25
            // 
            this.button25.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button25.Location = new System.Drawing.Point(93, 166);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(54, 21);
            this.button25.TabIndex = 149;
            this.button25.Text = "<<<";
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // button24
            // 
            this.button24.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button24.Location = new System.Drawing.Point(347, 166);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(53, 21);
            this.button24.TabIndex = 148;
            this.button24.Text = ">>>";
            this.button24.Click += new System.EventHandler(this.button24_Click);
            // 
            // lblTriggerAffectsLevel
            // 
            this.lblTriggerAffectsLevel.Location = new System.Drawing.Point(313, 340);
            this.lblTriggerAffectsLevel.Name = "lblTriggerAffectsLevel";
            this.lblTriggerAffectsLevel.Size = new System.Drawing.Size(54, 138);
            this.lblTriggerAffectsLevel.TabIndex = 147;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label17.Location = new System.Drawing.Point(313, 319);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(42, 14);
            this.label17.TabIndex = 146;
            this.label17.Text = "Points";
            // 
            // lblTriggerAffectsChance
            // 
            this.lblTriggerAffectsChance.Location = new System.Drawing.Point(373, 340);
            this.lblTriggerAffectsChance.Name = "lblTriggerAffectsChance";
            this.lblTriggerAffectsChance.Size = new System.Drawing.Size(54, 138);
            this.lblTriggerAffectsChance.TabIndex = 145;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label13.Location = new System.Drawing.Point(367, 319);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 14);
            this.label13.TabIndex = 144;
            this.label13.Text = "% Chance";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button22
            // 
            this.button22.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button22.Location = new System.Drawing.Point(7, 111);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(20, 28);
            this.button22.TabIndex = 143;
            this.button22.Text = "<";
            this.button22.Click += new System.EventHandler(this.button22_Click);
            // 
            // button21
            // 
            this.button21.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button21.Location = new System.Drawing.Point(407, 111);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(20, 28);
            this.button21.TabIndex = 142;
            this.button21.Text = ">";
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // lblTriggerAffectsName
            // 
            this.lblTriggerAffectsName.Location = new System.Drawing.Point(93, 340);
            this.lblTriggerAffectsName.Name = "lblTriggerAffectsName";
            this.lblTriggerAffectsName.Size = new System.Drawing.Size(214, 138);
            this.lblTriggerAffectsName.TabIndex = 141;
            this.lblTriggerAffectsName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label15.Location = new System.Drawing.Point(187, 319);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 14);
            this.label15.TabIndex = 140;
            this.label15.Text = "Bestowed Trait Name:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnFindProblemTriggerTrait
            // 
            this.btnFindProblemTriggerTrait.BackColor = System.Drawing.Color.Coral;
            this.btnFindProblemTriggerTrait.Enabled = false;
            this.btnFindProblemTriggerTrait.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.btnFindProblemTriggerTrait.Location = new System.Drawing.Point(433, 42);
            this.btnFindProblemTriggerTrait.Name = "btnFindProblemTriggerTrait";
            this.btnFindProblemTriggerTrait.Size = new System.Drawing.Size(134, 27);
            this.btnFindProblemTriggerTrait.TabIndex = 139;
            this.btnFindProblemTriggerTrait.Text = "Find Problem Trigger";
            this.btnFindProblemTriggerTrait.UseVisualStyleBackColor = false;
            this.btnFindProblemTriggerTrait.Click += new System.EventHandler(this.btnFindProblemTriggerTrait_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label4.Location = new System.Drawing.Point(7, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 28);
            this.label4.TabIndex = 138;
            this.label4.Text = "Granted Under Conditions:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTriggerConditions
            // 
            this.lblTriggerConditions.Location = new System.Drawing.Point(93, 194);
            this.lblTriggerConditions.Name = "lblTriggerConditions";
            this.lblTriggerConditions.Size = new System.Drawing.Size(334, 118);
            this.lblTriggerConditions.TabIndex = 137;
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.YellowGreen;
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button14.Location = new System.Drawing.Point(7, 42);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(46, 20);
            this.button14.TabIndex = 136;
            this.button14.Text = "< Start";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button11_Click);
            // 
            // button15
            // 
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button15.Location = new System.Drawing.Point(60, 42);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(40, 20);
            this.button15.TabIndex = 135;
            this.button15.Text = "< 20";
            this.button15.Click += new System.EventHandler(this.button10_Click);
            // 
            // button16
            // 
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button16.Location = new System.Drawing.Point(107, 42);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(33, 20);
            this.button16.TabIndex = 134;
            this.button16.Text = "< 5";
            this.button16.Click += new System.EventHandler(this.button3_Click);
            // 
            // button17
            // 
            this.button17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button17.Location = new System.Drawing.Point(147, 42);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(33, 20);
            this.button17.TabIndex = 133;
            this.button17.Text = "< 1";
            this.button17.Click += new System.EventHandler(this.button9_Click);
            // 
            // button18
            // 
            this.button18.BackColor = System.Drawing.Color.YellowGreen;
            this.button18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button18.Location = new System.Drawing.Point(380, 42);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(47, 20);
            this.button18.TabIndex = 132;
            this.button18.Text = "End>";
            this.button18.UseVisualStyleBackColor = false;
            this.button18.Click += new System.EventHandler(this.button8_Click);
            // 
            // button19
            // 
            this.button19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button19.Location = new System.Drawing.Point(333, 42);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(40, 20);
            this.button19.TabIndex = 131;
            this.button19.Text = "20 >";
            this.button19.Click += new System.EventHandler(this.button7_Click);
            // 
            // button20
            // 
            this.button20.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button20.Location = new System.Drawing.Point(293, 42);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(34, 20);
            this.button20.TabIndex = 130;
            this.button20.Text = "5 >";
            this.button20.Click += new System.EventHandler(this.button6_Click);
            // 
            // lblWhenToTest
            // 
            this.lblWhenToTest.Location = new System.Drawing.Point(153, 166);
            this.lblWhenToTest.Name = "lblWhenToTest";
            this.lblWhenToTest.Size = new System.Drawing.Size(187, 21);
            this.lblWhenToTest.TabIndex = 119;
            this.lblWhenToTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label31.Location = new System.Drawing.Point(7, 166);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(80, 20);
            this.label31.TabIndex = 118;
            this.label31.Text = "When Tested:";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label32.Location = new System.Drawing.Point(7, 146);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(80, 20);
            this.label32.TabIndex = 117;
            this.label32.Text = "Trigger Name:";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTriggerName
            // 
            this.lblTriggerName.ForeColor = System.Drawing.Color.Black;
            this.lblTriggerName.Location = new System.Drawing.Point(93, 146);
            this.lblTriggerName.Name = "lblTriggerName";
            this.lblTriggerName.Size = new System.Drawing.Size(334, 20);
            this.lblTriggerName.TabIndex = 116;
            this.lblTriggerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label34.Location = new System.Drawing.Point(113, 118);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(214, 21);
            this.label34.TabIndex = 114;
            this.label34.Text = "TRIGGERS BESTOWING THIS TRAIT";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label35.Location = new System.Drawing.Point(187, 42);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(60, 20);
            this.label35.TabIndex = 112;
            this.label35.Text = "TRAITS";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label44
            // 
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label44.Location = new System.Drawing.Point(53, 76);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(87, 20);
            this.label44.TabIndex = 103;
            this.label44.Text = "Trait Name:";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTriggerTraitName
            // 
            this.lblTriggerTraitName.Location = new System.Drawing.Point(147, 76);
            this.lblTriggerTraitName.Name = "lblTriggerTraitName";
            this.lblTriggerTraitName.Size = new System.Drawing.Size(253, 21);
            this.lblTriggerTraitName.TabIndex = 102;
            this.lblTriggerTraitName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button23
            // 
            this.button23.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button23.Location = new System.Drawing.Point(253, 42);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(34, 20);
            this.button23.TabIndex = 101;
            this.button23.Text = "1 >";
            this.button23.Click += new System.EventHandler(this.button2_Click);
            // 
            // label46
            // 
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label46.Location = new System.Drawing.Point(600, 49);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(83, 19);
            this.label46.TabIndex = 100;
            this.label46.Text = "Status:";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTriggerStatus
            // 
            this.lblTriggerStatus.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblTriggerStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblTriggerStatus.Location = new System.Drawing.Point(433, 76);
            this.lblTriggerStatus.Name = "lblTriggerStatus";
            this.lblTriggerStatus.Size = new System.Drawing.Size(254, 402);
            this.lblTriggerStatus.TabIndex = 99;
            this.lblTriggerStatus.Text = "OK";
            // 
            // ancillaryPage
            // 
            this.ancillaryPage.Controls.Add(this.btnAncillarySearch);
            this.ancillaryPage.Controls.Add(this.txtAncillaryName);
            this.ancillaryPage.Controls.Add(this.lblAncillaryImageFile);
            this.ancillaryPage.Controls.Add(this.label27);
            this.ancillaryPage.Controls.Add(this.label81);
            this.ancillaryPage.Controls.Add(this.txtAncillaryPath);
            this.ancillaryPage.Controls.Add(this.btnParseAncillaries);
            this.ancillaryPage.Controls.Add(this.lblUniqueAncillary);
            this.ancillaryPage.Controls.Add(this.label23);
            this.ancillaryPage.Controls.Add(this.btnNextProblemAncillary);
            this.ancillaryPage.Controls.Add(this.label25);
            this.ancillaryPage.Controls.Add(this.lblAncillaryEffects);
            this.ancillaryPage.Controls.Add(this.button28);
            this.ancillaryPage.Controls.Add(this.button30);
            this.ancillaryPage.Controls.Add(this.button31);
            this.ancillaryPage.Controls.Add(this.button32);
            this.ancillaryPage.Controls.Add(this.button34);
            this.ancillaryPage.Controls.Add(this.lblAncillaryEffectsDescription);
            this.ancillaryPage.Controls.Add(this.label40);
            this.ancillaryPage.Controls.Add(this.lblAncillaryDescription);
            this.ancillaryPage.Controls.Add(this.label42);
            this.ancillaryPage.Controls.Add(this.label49);
            this.ancillaryPage.Controls.Add(this.lblExcludesAncillaryCultures);
            this.ancillaryPage.Controls.Add(this.label53);
            this.ancillaryPage.Controls.Add(this.lblExcludesAncillaries2);
            this.ancillaryPage.Controls.Add(this.lblExcludesAncillaries);
            this.ancillaryPage.Controls.Add(this.label58);
            this.ancillaryPage.Controls.Add(this.lblAncillaryName);
            this.ancillaryPage.Controls.Add(this.button37);
            this.ancillaryPage.Controls.Add(this.label60);
            this.ancillaryPage.Controls.Add(this.lblAncillaryStatus);
            this.ancillaryPage.Location = new System.Drawing.Point(4, 22);
            this.ancillaryPage.Name = "ancillaryPage";
            this.ancillaryPage.Size = new System.Drawing.Size(692, 487);
            this.ancillaryPage.TabIndex = 2;
            this.ancillaryPage.Text = "Ancillaries";
            // 
            // btnAncillarySearch
            // 
            this.btnAncillarySearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.btnAncillarySearch.Location = new System.Drawing.Point(373, 76);
            this.btnAncillarySearch.Name = "btnAncillarySearch";
            this.btnAncillarySearch.Size = new System.Drawing.Size(54, 21);
            this.btnAncillarySearch.TabIndex = 158;
            this.btnAncillarySearch.Text = "Search";
            this.btnAncillarySearch.Click += new System.EventHandler(this.btnAncillarySearch_Click);
            // 
            // txtAncillaryName
            // 
            this.txtAncillaryName.Location = new System.Drawing.Point(146, 76);
            this.txtAncillaryName.Name = "txtAncillaryName";
            this.txtAncillaryName.Size = new System.Drawing.Size(226, 20);
            this.txtAncillaryName.TabIndex = 157;
            this.txtAncillaryName.Visible = false;
            // 
            // lblAncillaryImageFile
            // 
            this.lblAncillaryImageFile.Location = new System.Drawing.Point(147, 97);
            this.lblAncillaryImageFile.Name = "lblAncillaryImageFile";
            this.lblAncillaryImageFile.Size = new System.Drawing.Size(246, 21);
            this.lblAncillaryImageFile.TabIndex = 156;
            this.lblAncillaryImageFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label27.Location = new System.Drawing.Point(27, 97);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(113, 20);
            this.label27.TabIndex = 155;
            this.label27.Text = "Image Filename:";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label81
            // 
            this.label81.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label81.Location = new System.Drawing.Point(33, 14);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(100, 20);
            this.label81.TabIndex = 152;
            this.label81.Text = "Path to ancillaries:";
            this.label81.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAncillaryPath
            // 
            this.txtAncillaryPath.Location = new System.Drawing.Point(140, 14);
            this.txtAncillaryPath.Name = "txtAncillaryPath";
            this.txtAncillaryPath.Size = new System.Drawing.Size(440, 20);
            this.txtAncillaryPath.TabIndex = 151;
            this.txtAncillaryPath.Text = "C:/Program Files/Activision/Rome - Total War/Data/";
            // 
            // btnParseAncillaries
            // 
            this.btnParseAncillaries.Location = new System.Drawing.Point(587, 14);
            this.btnParseAncillaries.Name = "btnParseAncillaries";
            this.btnParseAncillaries.Size = new System.Drawing.Size(100, 21);
            this.btnParseAncillaries.TabIndex = 150;
            this.btnParseAncillaries.Text = "Parse Ancillaries";
            this.btnParseAncillaries.Click += new System.EventHandler(this.btnParseAncillaries_Click);
            // 
            // lblUniqueAncillary
            // 
            this.lblUniqueAncillary.Location = new System.Drawing.Point(147, 201);
            this.lblUniqueAncillary.Name = "lblUniqueAncillary";
            this.lblUniqueAncillary.Size = new System.Drawing.Size(246, 21);
            this.lblUniqueAncillary.TabIndex = 149;
            this.lblUniqueAncillary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label23.Location = new System.Drawing.Point(27, 201);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(113, 20);
            this.label23.TabIndex = 148;
            this.label23.Text = "Is Unique Ancillary?";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNextProblemAncillary
            // 
            this.btnNextProblemAncillary.BackColor = System.Drawing.Color.Coral;
            this.btnNextProblemAncillary.Enabled = false;
            this.btnNextProblemAncillary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.btnNextProblemAncillary.Location = new System.Drawing.Point(433, 42);
            this.btnNextProblemAncillary.Name = "btnNextProblemAncillary";
            this.btnNextProblemAncillary.Size = new System.Drawing.Size(87, 27);
            this.btnNextProblemAncillary.TabIndex = 145;
            this.btnNextProblemAncillary.Text = "Next Problem";
            this.btnNextProblemAncillary.UseVisualStyleBackColor = false;
            this.btnNextProblemAncillary.Click += new System.EventHandler(this.btnNextProblemAncillary_Click);
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label25.Location = new System.Drawing.Point(27, 263);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(113, 20);
            this.label25.TabIndex = 144;
            this.label25.Text = "Effects:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAncillaryEffects
            // 
            this.lblAncillaryEffects.Location = new System.Drawing.Point(147, 263);
            this.lblAncillaryEffects.Name = "lblAncillaryEffects";
            this.lblAncillaryEffects.Size = new System.Drawing.Size(246, 98);
            this.lblAncillaryEffects.TabIndex = 143;
            // 
            // button28
            // 
            this.button28.BackColor = System.Drawing.Color.YellowGreen;
            this.button28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button28.Location = new System.Drawing.Point(7, 42);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(46, 20);
            this.button28.TabIndex = 142;
            this.button28.Text = "< Start";
            this.button28.UseVisualStyleBackColor = false;
            this.button28.Click += new System.EventHandler(this.button28_Click);
            // 
            // button30
            // 
            this.button30.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button30.Location = new System.Drawing.Point(60, 42);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(33, 20);
            this.button30.TabIndex = 140;
            this.button30.Text = "< 5";
            this.button30.Click += new System.EventHandler(this.button30_Click);
            // 
            // button31
            // 
            this.button31.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button31.Location = new System.Drawing.Point(100, 42);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(33, 20);
            this.button31.TabIndex = 139;
            this.button31.Text = "< 1";
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // button32
            // 
            this.button32.BackColor = System.Drawing.Color.YellowGreen;
            this.button32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button32.Location = new System.Drawing.Point(380, 42);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(47, 20);
            this.button32.TabIndex = 138;
            this.button32.Text = "End>";
            this.button32.UseVisualStyleBackColor = false;
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // button34
            // 
            this.button34.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button34.Location = new System.Drawing.Point(340, 42);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(33, 20);
            this.button34.TabIndex = 136;
            this.button34.Text = "5 >";
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // lblAncillaryEffectsDescription
            // 
            this.lblAncillaryEffectsDescription.Location = new System.Drawing.Point(147, 243);
            this.lblAncillaryEffectsDescription.Name = "lblAncillaryEffectsDescription";
            this.lblAncillaryEffectsDescription.Size = new System.Drawing.Size(246, 20);
            this.lblAncillaryEffectsDescription.TabIndex = 127;
            this.lblAncillaryEffectsDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label40.Location = new System.Drawing.Point(27, 243);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(113, 20);
            this.label40.TabIndex = 126;
            this.label40.Text = "Effects Description:";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAncillaryDescription
            // 
            this.lblAncillaryDescription.Location = new System.Drawing.Point(147, 222);
            this.lblAncillaryDescription.Name = "lblAncillaryDescription";
            this.lblAncillaryDescription.Size = new System.Drawing.Size(246, 21);
            this.lblAncillaryDescription.TabIndex = 125;
            this.lblAncillaryDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label42
            // 
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label42.Location = new System.Drawing.Point(40, 222);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(100, 20);
            this.label42.TabIndex = 124;
            this.label42.Text = "Description:";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label49
            // 
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label49.Location = new System.Drawing.Point(173, 42);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(94, 20);
            this.label49.TabIndex = 118;
            this.label49.Text = "ANCILLARIES";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExcludesAncillaryCultures
            // 
            this.lblExcludesAncillaryCultures.Location = new System.Drawing.Point(147, 159);
            this.lblExcludesAncillaryCultures.Name = "lblExcludesAncillaryCultures";
            this.lblExcludesAncillaryCultures.Size = new System.Drawing.Size(246, 42);
            this.lblExcludesAncillaryCultures.TabIndex = 115;
            // 
            // label53
            // 
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label53.Location = new System.Drawing.Point(27, 159);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(113, 20);
            this.label53.TabIndex = 114;
            this.label53.Text = "Excludes Cultures:";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExcludesAncillaries2
            // 
            this.lblExcludesAncillaries2.Location = new System.Drawing.Point(147, 118);
            this.lblExcludesAncillaries2.Name = "lblExcludesAncillaries2";
            this.lblExcludesAncillaries2.Size = new System.Drawing.Size(246, 41);
            this.lblExcludesAncillaries2.TabIndex = 113;
            // 
            // lblExcludesAncillaries
            // 
            this.lblExcludesAncillaries.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.lblExcludesAncillaries.Location = new System.Drawing.Point(27, 118);
            this.lblExcludesAncillaries.Name = "lblExcludesAncillaries";
            this.lblExcludesAncillaries.Size = new System.Drawing.Size(113, 20);
            this.lblExcludesAncillaries.TabIndex = 112;
            this.lblExcludesAncillaries.Text = "Excludes Ancillaries:";
            this.lblExcludesAncillaries.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label58
            // 
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label58.Location = new System.Drawing.Point(53, 76);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(87, 20);
            this.label58.TabIndex = 109;
            this.label58.Text = "Ancillary Name:";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAncillaryName
            // 
            this.lblAncillaryName.Location = new System.Drawing.Point(147, 76);
            this.lblAncillaryName.Name = "lblAncillaryName";
            this.lblAncillaryName.Size = new System.Drawing.Size(226, 21);
            this.lblAncillaryName.TabIndex = 108;
            this.lblAncillaryName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAncillaryName.Click += new System.EventHandler(this.lblAncillaryName_Click);
            // 
            // button37
            // 
            this.button37.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button37.Location = new System.Drawing.Point(300, 42);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(33, 20);
            this.button37.TabIndex = 107;
            this.button37.Text = "1 >";
            this.button37.Click += new System.EventHandler(this.button37_Click);
            // 
            // label60
            // 
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label60.Location = new System.Drawing.Point(600, 49);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(83, 19);
            this.label60.TabIndex = 106;
            this.label60.Text = "Status:";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAncillaryStatus
            // 
            this.lblAncillaryStatus.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblAncillaryStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblAncillaryStatus.Location = new System.Drawing.Point(433, 76);
            this.lblAncillaryStatus.Name = "lblAncillaryStatus";
            this.lblAncillaryStatus.Size = new System.Drawing.Size(254, 402);
            this.lblAncillaryStatus.TabIndex = 105;
            this.lblAncillaryStatus.Text = resources.GetString("lblAncillaryStatus.Text");
            // 
            // ancillaryTriggerPage
            // 
            this.ancillaryTriggerPage.Controls.Add(this.label75);
            this.ancillaryTriggerPage.Controls.Add(this.button29);
            this.ancillaryTriggerPage.Controls.Add(this.button33);
            this.ancillaryTriggerPage.Controls.Add(this.lblAncillaryEffectChance);
            this.ancillaryTriggerPage.Controls.Add(this.label65);
            this.ancillaryTriggerPage.Controls.Add(this.button38);
            this.ancillaryTriggerPage.Controls.Add(this.button39);
            this.ancillaryTriggerPage.Controls.Add(this.lblAncillaryEffectName);
            this.ancillaryTriggerPage.Controls.Add(this.label67);
            this.ancillaryTriggerPage.Controls.Add(this.btnFindProblemTriggerAncillary);
            this.ancillaryTriggerPage.Controls.Add(this.label68);
            this.ancillaryTriggerPage.Controls.Add(this.lblAncillaryConditions);
            this.ancillaryTriggerPage.Controls.Add(this.button41);
            this.ancillaryTriggerPage.Controls.Add(this.button43);
            this.ancillaryTriggerPage.Controls.Add(this.button44);
            this.ancillaryTriggerPage.Controls.Add(this.button45);
            this.ancillaryTriggerPage.Controls.Add(this.button47);
            this.ancillaryTriggerPage.Controls.Add(this.lblAncillaryWhenTested);
            this.ancillaryTriggerPage.Controls.Add(this.label71);
            this.ancillaryTriggerPage.Controls.Add(this.label72);
            this.ancillaryTriggerPage.Controls.Add(this.lblAncillaryTriggerName);
            this.ancillaryTriggerPage.Controls.Add(this.label74);
            this.ancillaryTriggerPage.Controls.Add(this.label76);
            this.ancillaryTriggerPage.Controls.Add(this.label77);
            this.ancillaryTriggerPage.Controls.Add(this.button48);
            this.ancillaryTriggerPage.Controls.Add(this.label78);
            this.ancillaryTriggerPage.Controls.Add(this.lblAncillaryTriggerStatus);
            this.ancillaryTriggerPage.Location = new System.Drawing.Point(4, 22);
            this.ancillaryTriggerPage.Name = "ancillaryTriggerPage";
            this.ancillaryTriggerPage.Size = new System.Drawing.Size(692, 487);
            this.ancillaryTriggerPage.TabIndex = 3;
            this.ancillaryTriggerPage.Text = "Ancillary Triggers";
            // 
            // label75
            // 
            this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label75.Location = new System.Drawing.Point(173, 42);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(94, 20);
            this.label75.TabIndex = 181;
            this.label75.Text = "ANCILLARIES";
            this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button29
            // 
            this.button29.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button29.Location = new System.Drawing.Point(93, 166);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(54, 21);
            this.button29.TabIndex = 180;
            this.button29.Text = "<<<";
            this.button29.Click += new System.EventHandler(this.button29_Click);
            // 
            // button33
            // 
            this.button33.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button33.Location = new System.Drawing.Point(347, 166);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(53, 21);
            this.button33.TabIndex = 179;
            this.button33.Text = ">>>";
            this.button33.Click += new System.EventHandler(this.button33_Click);
            // 
            // lblAncillaryEffectChance
            // 
            this.lblAncillaryEffectChance.Location = new System.Drawing.Point(373, 340);
            this.lblAncillaryEffectChance.Name = "lblAncillaryEffectChance";
            this.lblAncillaryEffectChance.Size = new System.Drawing.Size(54, 138);
            this.lblAncillaryEffectChance.TabIndex = 176;
            // 
            // label65
            // 
            this.label65.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label65.Location = new System.Drawing.Point(367, 319);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(60, 14);
            this.label65.TabIndex = 175;
            this.label65.Text = "% Chance";
            this.label65.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button38
            // 
            this.button38.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button38.Location = new System.Drawing.Point(7, 111);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(20, 28);
            this.button38.TabIndex = 174;
            this.button38.Text = "<";
            this.button38.Click += new System.EventHandler(this.button38_Click);
            // 
            // button39
            // 
            this.button39.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button39.Location = new System.Drawing.Point(407, 111);
            this.button39.Name = "button39";
            this.button39.Size = new System.Drawing.Size(20, 28);
            this.button39.TabIndex = 173;
            this.button39.Text = ">";
            this.button39.Click += new System.EventHandler(this.button39_Click);
            // 
            // lblAncillaryEffectName
            // 
            this.lblAncillaryEffectName.Location = new System.Drawing.Point(153, 340);
            this.lblAncillaryEffectName.Name = "lblAncillaryEffectName";
            this.lblAncillaryEffectName.Size = new System.Drawing.Size(214, 138);
            this.lblAncillaryEffectName.TabIndex = 172;
            this.lblAncillaryEffectName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label67
            // 
            this.label67.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label67.Location = new System.Drawing.Point(213, 319);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(154, 14);
            this.label67.TabIndex = 171;
            this.label67.Text = "Bestowed Ancillary Name:";
            this.label67.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnFindProblemTriggerAncillary
            // 
            this.btnFindProblemTriggerAncillary.BackColor = System.Drawing.Color.Coral;
            this.btnFindProblemTriggerAncillary.Enabled = false;
            this.btnFindProblemTriggerAncillary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.btnFindProblemTriggerAncillary.Location = new System.Drawing.Point(433, 42);
            this.btnFindProblemTriggerAncillary.Name = "btnFindProblemTriggerAncillary";
            this.btnFindProblemTriggerAncillary.Size = new System.Drawing.Size(134, 27);
            this.btnFindProblemTriggerAncillary.TabIndex = 170;
            this.btnFindProblemTriggerAncillary.Text = "Find Problem Trigger";
            this.btnFindProblemTriggerAncillary.UseVisualStyleBackColor = false;
            this.btnFindProblemTriggerAncillary.Click += new System.EventHandler(this.btnFindProblemTriggerAncillary_Click);
            // 
            // label68
            // 
            this.label68.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label68.Location = new System.Drawing.Point(7, 194);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(80, 28);
            this.label68.TabIndex = 169;
            this.label68.Text = "Granted Under Conditions:";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAncillaryConditions
            // 
            this.lblAncillaryConditions.Location = new System.Drawing.Point(93, 194);
            this.lblAncillaryConditions.Name = "lblAncillaryConditions";
            this.lblAncillaryConditions.Size = new System.Drawing.Size(334, 118);
            this.lblAncillaryConditions.TabIndex = 168;
            // 
            // button41
            // 
            this.button41.BackColor = System.Drawing.Color.YellowGreen;
            this.button41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button41.Location = new System.Drawing.Point(7, 42);
            this.button41.Name = "button41";
            this.button41.Size = new System.Drawing.Size(46, 20);
            this.button41.TabIndex = 167;
            this.button41.Text = "< Start";
            this.button41.UseVisualStyleBackColor = false;
            this.button41.Click += new System.EventHandler(this.button41_Click);
            // 
            // button43
            // 
            this.button43.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button43.Location = new System.Drawing.Point(60, 42);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(33, 20);
            this.button43.TabIndex = 165;
            this.button43.Text = "< 5";
            this.button43.Click += new System.EventHandler(this.button43_Click);
            // 
            // button44
            // 
            this.button44.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button44.Location = new System.Drawing.Point(100, 42);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(33, 20);
            this.button44.TabIndex = 164;
            this.button44.Text = "< 1";
            this.button44.Click += new System.EventHandler(this.button44_Click);
            // 
            // button45
            // 
            this.button45.BackColor = System.Drawing.Color.YellowGreen;
            this.button45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button45.Location = new System.Drawing.Point(380, 42);
            this.button45.Name = "button45";
            this.button45.Size = new System.Drawing.Size(47, 20);
            this.button45.TabIndex = 163;
            this.button45.Text = "End>";
            this.button45.UseVisualStyleBackColor = false;
            this.button45.Click += new System.EventHandler(this.button45_Click);
            // 
            // button47
            // 
            this.button47.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button47.Location = new System.Drawing.Point(340, 42);
            this.button47.Name = "button47";
            this.button47.Size = new System.Drawing.Size(33, 20);
            this.button47.TabIndex = 161;
            this.button47.Text = "5 >";
            this.button47.Click += new System.EventHandler(this.button47_Click);
            // 
            // lblAncillaryWhenTested
            // 
            this.lblAncillaryWhenTested.Location = new System.Drawing.Point(153, 166);
            this.lblAncillaryWhenTested.Name = "lblAncillaryWhenTested";
            this.lblAncillaryWhenTested.Size = new System.Drawing.Size(187, 21);
            this.lblAncillaryWhenTested.TabIndex = 160;
            this.lblAncillaryWhenTested.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label71
            // 
            this.label71.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label71.Location = new System.Drawing.Point(7, 166);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(80, 20);
            this.label71.TabIndex = 159;
            this.label71.Text = "When Tested:";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label72
            // 
            this.label72.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label72.Location = new System.Drawing.Point(7, 146);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(80, 20);
            this.label72.TabIndex = 158;
            this.label72.Text = "Trigger Name:";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAncillaryTriggerName
            // 
            this.lblAncillaryTriggerName.ForeColor = System.Drawing.Color.Black;
            this.lblAncillaryTriggerName.Location = new System.Drawing.Point(93, 146);
            this.lblAncillaryTriggerName.Name = "lblAncillaryTriggerName";
            this.lblAncillaryTriggerName.Size = new System.Drawing.Size(334, 20);
            this.lblAncillaryTriggerName.TabIndex = 157;
            this.lblAncillaryTriggerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label74
            // 
            this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label74.Location = new System.Drawing.Point(93, 118);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(260, 21);
            this.label74.TabIndex = 156;
            this.label74.Text = "TRIGGERS BESTOWING THIS ANCILLARY";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label76
            // 
            this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label76.Location = new System.Drawing.Point(27, 76);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(113, 20);
            this.label76.TabIndex = 154;
            this.label76.Text = "Ancillary Name:";
            this.label76.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label77
            // 
            this.label77.Location = new System.Drawing.Point(147, 76);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(253, 21);
            this.label77.TabIndex = 153;
            this.label77.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button48
            // 
            this.button48.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button48.Location = new System.Drawing.Point(300, 42);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(33, 20);
            this.button48.TabIndex = 152;
            this.button48.Text = "1 >";
            this.button48.Click += new System.EventHandler(this.button48_Click);
            // 
            // label78
            // 
            this.label78.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label78.Location = new System.Drawing.Point(600, 49);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(83, 19);
            this.label78.TabIndex = 151;
            this.label78.Text = "Status:";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAncillaryTriggerStatus
            // 
            this.lblAncillaryTriggerStatus.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblAncillaryTriggerStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblAncillaryTriggerStatus.Location = new System.Drawing.Point(433, 76);
            this.lblAncillaryTriggerStatus.Name = "lblAncillaryTriggerStatus";
            this.lblAncillaryTriggerStatus.Size = new System.Drawing.Size(254, 402);
            this.lblAncillaryTriggerStatus.TabIndex = 150;
            this.lblAncillaryTriggerStatus.Text = "OK";
            // 
            // orphans
            // 
            this.orphans.Controls.Add(this.label39);
            this.orphans.Controls.Add(this.txtOrphanStatus);
            this.orphans.Controls.Add(this.button26);
            this.orphans.Controls.Add(this.label38);
            this.orphans.Controls.Add(this.txtOrphans);
            this.orphans.Location = new System.Drawing.Point(4, 22);
            this.orphans.Name = "orphans";
            this.orphans.Size = new System.Drawing.Size(692, 487);
            this.orphans.TabIndex = 5;
            this.orphans.Text = "Orphans";
            // 
            // label39
            // 
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label39.Location = new System.Drawing.Point(13, 409);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(227, 21);
            this.label39.TabIndex = 101;
            this.label39.Text = "Status";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOrphanStatus
            // 
            this.txtOrphanStatus.AcceptsReturn = true;
            this.txtOrphanStatus.AcceptsTab = true;
            this.txtOrphanStatus.Location = new System.Drawing.Point(13, 430);
            this.txtOrphanStatus.Multiline = true;
            this.txtOrphanStatus.Name = "txtOrphanStatus";
            this.txtOrphanStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOrphanStatus.Size = new System.Drawing.Size(674, 48);
            this.txtOrphanStatus.TabIndex = 100;
            // 
            // button26
            // 
            this.button26.BackColor = System.Drawing.Color.Coral;
            this.button26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.button26.Location = new System.Drawing.Point(600, 7);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(87, 28);
            this.button26.TabIndex = 99;
            this.button26.Text = "Find Orphans";
            this.button26.UseVisualStyleBackColor = false;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // label38
            // 
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label38.Location = new System.Drawing.Point(13, 21);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(227, 21);
            this.label38.TabIndex = 72;
            this.label38.Text = "ORPHANED TRAITS AND ANCILLARIES";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOrphans
            // 
            this.txtOrphans.AcceptsReturn = true;
            this.txtOrphans.AcceptsTab = true;
            this.txtOrphans.Location = new System.Drawing.Point(13, 42);
            this.txtOrphans.Multiline = true;
            this.txtOrphans.Name = "txtOrphans";
            this.txtOrphans.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOrphans.Size = new System.Drawing.Size(674, 360);
            this.txtOrphans.TabIndex = 0;
            // 
            // options
            // 
            this.options.Controls.Add(this.chkTraitAffect);
            this.options.Controls.Add(this.label47);
            this.options.Controls.Add(this.label43);
            this.options.Controls.Add(this.label41);
            this.options.Location = new System.Drawing.Point(4, 22);
            this.options.Name = "options";
            this.options.Size = new System.Drawing.Size(692, 487);
            this.options.TabIndex = 6;
            this.options.Text = "Options";
            // 
            // chkTraitAffect
            // 
            this.chkTraitAffect.Checked = true;
            this.chkTraitAffect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTraitAffect.Location = new System.Drawing.Point(73, 69);
            this.chkTraitAffect.Name = "chkTraitAffect";
            this.chkTraitAffect.Size = new System.Drawing.Size(14, 14);
            this.chkTraitAffect.TabIndex = 75;
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(93, 69);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(274, 21);
            this.label47.TabIndex = 74;
            this.label47.Text = "Check the current Affect dictionary for Affect validity";
            // 
            // label43
            // 
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label43.Location = new System.Drawing.Point(40, 42);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(60, 20);
            this.label43.TabIndex = 73;
            this.label43.Text = "TRAITS";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label41.Location = new System.Drawing.Point(13, 14);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(180, 21);
            this.label41.TabIndex = 72;
            this.label41.Text = "CONFIGURATION OPTIONS";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label37);
            this.tabPage1.Controls.Add(this.label36);
            this.tabPage1.Controls.Add(this.label33);
            this.tabPage1.Controls.Add(this.label30);
            this.tabPage1.Controls.Add(this.label29);
            this.tabPage1.Controls.Add(this.label28);
            this.tabPage1.Controls.Add(this.label26);
            this.tabPage1.Controls.Add(this.label22);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(692, 487);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "About";
            // 
            // label37
            // 
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label37.Location = new System.Drawing.Point(127, 111);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(340, 21);
            this.label37.TabIndex = 7;
            this.label37.Text = "or visit the Scriptorium at www.totalwar.org";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label36
            // 
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label36.Location = new System.Drawing.Point(127, 90);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(340, 21);
            this.label36.TabIndex = 6;
            this.label36.Text = "Tamur, at tamur@cicero.modwest..com";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label33.Location = new System.Drawing.Point(7, 90);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(113, 21);
            this.label33.TabIndex = 5;
            this.label33.Text = "Send Comments To:";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label30.Location = new System.Drawing.Point(127, 69);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(340, 21);
            this.label30.TabIndex = 4;
            this.label30.Text = "November 2007";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label29.Location = new System.Drawing.Point(7, 69);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(113, 21);
            this.label29.TabIndex = 3;
            this.label29.Text = "Last Modified:";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label28.Location = new System.Drawing.Point(127, 49);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(340, 20);
            this.label28.TabIndex = 2;
            this.label28.Text = "August 2006";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label26.Location = new System.Drawing.Point(27, 49);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(93, 20);
            this.label26.TabIndex = 1;
            this.label26.Text = "Created:";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ));
            this.label22.Location = new System.Drawing.Point(13, 14);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(360, 21);
            this.label22.TabIndex = 0;
            this.label22.Text = "ATV-TW, Version 0.92";
            // 
            // mnuMainMenu
            // 
            this.mnuMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuTools,
            this.mnuHelp});
            this.mnuMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMainMenu.Name = "mnuMainMenu";
            this.mnuMainMenu.Size = new System.Drawing.Size(856, 24);
            this.mnuMainMenu.TabIndex = 59;
            this.mnuMainMenu.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(35, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4 ) ) );
            this.mnuExit.Size = new System.Drawing.Size(143, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuToolsOptions});
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(44, 20);
            this.mnuTools.Text = "&Tools";
            // 
            // mnuToolsOptions
            // 
            this.mnuToolsOptions.Enabled = false;
            this.mnuToolsOptions.Name = "mnuToolsOptions";
            this.mnuToolsOptions.ShortcutKeys = ( ( System.Windows.Forms.Keys )( ( System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O ) ) );
            this.mnuToolsOptions.Size = new System.Drawing.Size(158, 22);
            this.mnuToolsOptions.Text = "&Options";
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(40, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Enabled = false;
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(114, 22);
            this.mnuHelpAbout.Text = "&About";
            // 
            // frmATVTW
            // 
            this.AcceptButton = this.btnParseTraits;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(856, 608);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.mnuMainMenu);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ( ( System.Drawing.Icon )( resources.GetObject("$this.Icon") ) );
            this.MainMenuStrip = this.mnuMainMenu;
            this.Name = "frmATVTW";
            this.Text = "ATV-TW - Ancillary/Trait Validator for Rome: Total War - Version 0.92";
            this.tabControl1.ResumeLayout(false);
            this.traitPage.ResumeLayout(false);
            this.traitPage.PerformLayout();
            this.traitTriggerPage.ResumeLayout(false);
            this.ancillaryPage.ResumeLayout(false);
            this.ancillaryPage.PerformLayout();
            this.ancillaryTriggerPage.ResumeLayout(false);
            this.orphans.ResumeLayout(false);
            this.orphans.PerformLayout();
            this.options.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.mnuMainMenu.ResumeLayout(false);
            this.mnuMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmATVTW());
		}

		private void btnParseTraits_Click(object sender, System.EventArgs e)
		{
			lblParseStatus.Text = "Parsing file...";
			lblParseStatus.Refresh();

			// read current path from interface
			string tempPath = txtTraitsFilePath.Text;
			tempPath += "export_descr_character_traits.txt";
			bool allOK = false;
			TextReader reader = null;

			Trait tempTrait = new Trait();
			TraitLevel tempLevel = new TraitLevel();
			traits = new ArrayList();
            triggers = new ArrayList();
			Trigger tempTrigger = new Trigger();
			TriggerAffect tempAffect = new TriggerAffect();
			ArrayList tempTrigCond = new ArrayList();
			ArrayList tempTrigAffects = new ArrayList();
			ArrayList tempLevelEffects = new ArrayList();
            ArrayList tempAL;
			
			bProcessingTraits = true;
			bool affectFound;

			// open traits file for reading
			try{
				reader = new StreamReader(tempPath);
				allOK = true;	
			}catch{
				lblParseStatus.Text = "File not found: " + tempPath + "  Please correct the path and try again.";
			}

			if (allOK){
				// set global "did it" var
				parsedTraits = true;

				string newLine = "";

				// loop through and fill up all objects
				while ((newLine = reader.ReadLine()) != null)
				{
					// dump empty white space
					newLine.Trim();

					// check for the section swap
                    if (newLine.IndexOf("Trigger") > -1) //TRIGGER DATA STARTS HERE
						bProcessingTraits = false;

					// check for comment lines, skip processing if found
					if (newLine.IndexOf(";") < 0 && newLine != "")
					{
						// big switch for trait or trigger section
						if (bProcessingTraits)
						{
							// process each line individually
							if (newLine.IndexOf("Trait ") > -1){
								// this is a new trait
								// first add the latest level to the current trait, since it has not been packed in yet
								if (tempTrait.name != ""){
									// add the last effect to the level
									tempLevel.effects = tempLevelEffects;

									// now add the level to the trait
									tempTrait.levels.Add(tempLevel);

									// and start up a new level, just to clear that info for later
									tempLevel = new TraitLevel();

									// now pack this trait into the traits array, and start a new one
									traits.Add(tempTrait);
								}//end if tempTrait.name != ""

								tempTrait = new Trait();

								// grab name
								tempAL = ProcessStringArray(newLine,1,7);
								tempTrait.name = tempAL[0].ToString();

								lblParseStatus.Text = "Processing trait " + tempTrait.name;
								lblParseStatus.Refresh();
							}else if (newLine.IndexOf("Characters") > -1){
								// process this for character types it applies to
								tempTrait.characterTypes = ProcessStringArray(newLine,1,MAX_CHARACTER_TYPES);
							}else if (newLine.IndexOf("ExcludeCultures") > -1){
								// process excluded cultures, same as character processing
								tempTrait.excludeCultures = ProcessStringArray(newLine,1,MAX_EXCLUDE_CULTURES);
							}else if (newLine.IndexOf("Hidden") > -1){
								// this trait is a hidden trait
								tempAL = ProcessStringArray(newLine,1,5);
								tempTrait.hiddenTrait = true;
							}else if (newLine.IndexOf("NoGoingBackLevel") > -1){
								// process this for the single int that should be on this line
								tempAL = ProcessStringArray(newLine,1,5);

								// because a non-int value here is a definite no-no, we catch and break if there is a problem
                                tempTrait.noGoingBackLevel = Common.testParse(tempAL[0].ToString(), false);
							}else if (newLine.IndexOf("AntiTraits") > -1){
								// process for antitraits
								tempTrait.antiTraits = ProcessStringArray(newLine,1,MAX_ANTI_TRAITS);
							}else if (newLine.IndexOf("Level") > -1){
								// we hit a new level, pack up the old one if there is one
								if (tempLevel.name != ""){
									tempLevel.effects = tempLevelEffects;
									tempTrait.levels.Add(tempLevel);
								}//end if (tempLevel.name != "")

								tempLevel = new TraitLevel();

								// get level name
								tempAL = ProcessStringArray(newLine,1,5);
								tempLevel.name = tempAL[0].ToString();

								// and then initialise the temp effects array
								tempLevelEffects = new ArrayList();
							}else if (newLine.IndexOf("EffectsDescription") > -1){
								// pull out effectsdesc name
								tempAL = ProcessStringArray(newLine,1,5);
								tempLevel.effectsDescName = tempAL[0].ToString();
							}else if (newLine.IndexOf("Description") > -1){
								// pull out the description name
								tempAL = ProcessStringArray(newLine,1,5);
								tempLevel.descriptionName = tempAL[0].ToString();
							}else if (newLine.IndexOf("LoseMessage") > -1){
								tempAL = ProcessStringArray(newLine,1,5);
								tempLevel.loseMessageName = tempAL[0].ToString();
							}else if (newLine.IndexOf("GainMessage") > -1){
								tempAL = ProcessStringArray(newLine,1,5);
								tempLevel.gainMessageName = tempAL[0].ToString();
							}else if (newLine.IndexOf("Epithet") > -1){
								tempAL = ProcessStringArray(newLine,1,5);
								tempLevel.epithetName = tempAL[0].ToString();
							}else if (newLine.IndexOf("Threshold") > -1){
								// process this for threshhold int
								tempAL = ProcessStringArray(newLine,1,5);

								// because a negative or non-int value here is a definite no-no, we catch it
                                tempLevel.threshhold = Common.testParse(tempAL[0].ToString(), false);
							}else if (newLine.IndexOf("Effect") > -1){
								// this is one of possibly many effects, validate and store these in a temp array to be packed with the level later
                                affectFound = false;
								
								// now parse line for effect info and pack it into the new object
								tempAL = ProcessStringArray(newLine,1,5);
                                //Effect tempEffect = new Effect(tempAL);// 
                                Effect tempEffect = new Effect();
			
                                tempEffect.name = tempAL[0].ToString();

								//************************* validity checks **************************//
								// check effect name for validity if the user wants to
								if (chkTraitAffect.Checked){
									for (int x=0;x<attributeTemplates.Count;x++){
										Attribute tempAtt = (Attribute) attributeTemplates[x];
										if (tempEffect.name == tempAtt.name){
											affectFound = true;
											break;
										}//end if
									}//end for

									if (!affectFound){
                                        tempEffect.name = "{" + tempEffect.name + "}";
									}//end if
								}//end if

								// because a non-int value here is a definite no-no, we catch and break if there is a problem
                                tempEffect.level = Common.testParse(tempAL[1].ToString(), true);

								tempLevelEffects.Add(tempEffect);
							}
						}else{
							if (newLine.IndexOf("Trigger") > -1){
								// into the trigger section
								if (tempTrait.name != ""){
									// pack up the last trait
									tempTrait.levels.Add(tempLevel);

									// now pack this trait into the traits array
									traits.Add(tempTrait);

									// and initialise so we don't get fooled into adding it again next time round
									tempTrait = new Trait();
								}

								// Now start up the triggers.
								// Add the latest conditions and affects to the current trigger, since it
								//	has not been packed in yet.
								if (tempTrigger.name != "")
								{
									// copy conditions & affects into trigger object, because pass-by-value is the only
									//		option for arraylists
									for (int n=0;n<tempTrigCond.Count;n++)
										tempTrigger.conditions.Add(tempTrigCond[n]);

									for (int n=0;n<tempTrigAffects.Count;n++)
										tempTrigger.affectsList.Add(tempTrigAffects[n]);

									// clear the temp arrays
                                    tempTrigAffects.Clear();
                                    tempTrigCond.Clear();
									
									// pack this into the triggers list
									triggers.Add(tempTrigger);
								}

								tempTrigger = new Trigger();

								// grab name
								tempAL = ProcessStringArray(newLine,1,5);
								tempTrigger.name = tempAL[0].ToString();

								lblParseStatus.Text = "Processing trigger " + tempTrigger.name;
								lblParseStatus.Refresh();
							}else if (newLine.IndexOf("WhenToTest") > -1){
								tempAL = ProcessStringArray(newLine,1,5);
                                tempTrigger.triggerEvent = getEvent(tempAL[0].ToString());
							}else if (newLine.IndexOf(" Condition ") > -1 || newLine.IndexOf(" and ") > -1){
								int adjustedStart = 0;
								Condition tempCondition = new Condition();

								tempAL = ProcessStringArray(newLine,1,6);
								
								// put everything in the right bin, and make sure to shift for the "not" if there is one
								if (tempAL.Count > 1 && tempAL[adjustedStart].ToString() == "not"){
									// adjust param reading start to position 2, and set name & boolean value
									adjustedStart = 2;
									tempCondition.name = tempAL[1].ToString();
									tempCondition.boolValue = false;
								}else{
									// just set name & adjust start to 1
									tempCondition.name = tempAL[0].ToString();
									adjustedStart = 1;
								}//end if

								// stuff param values into condition
								for (int x=adjustedStart;x<tempAL.Count;x++)
									tempCondition.parameters.Add(tempAL[x]);

								// and add this condition to the temp array so it can be added to the 
								tempTrigCond.Add(tempCondition);
							}else if (newLine.IndexOf("Affects") > -1){
								tempAffect = new TriggerAffect();

								tempAL = ProcessStringArray(newLine,1,5);
								tempAffect.traitName = tempAL[0].ToString();
								
								// try blocks for threshhold and chance
                                tempAffect.traitThreshhold = Common.testParse(tempAL[1].ToString(), true);

                                tempAffect.chance = Common.testParse(tempAL[3].ToString(), false);

								tempTrigAffects.Add(tempAffect);
							}//end if trigger
						}//end if bProcessingTraits
					}//end if (not a comment or not a blank line)
				}//end while (newLine = reader.readLine() != null)

				// close the reader
				reader.Close();

                //place last trigger into triggers array object
                if (tempTrigger.name != "")
                {
                    // copy conditions & affects into trigger object, because pass-by-value is the only
                    //		option for arraylists
                    for (int n = 0; n < tempTrigCond.Count; n++)
                        tempTrigger.conditions.Add(tempTrigCond[n]);

                    for (int n = 0; n < tempTrigAffects.Count; n++)
                        tempTrigger.affectsList.Add(tempTrigAffects[n]);

                    // clear the temp arrays
                    tempTrigAffects.Clear();
                    tempTrigCond.Clear();

                    // pack this into the triggers list
                    triggers.Add(tempTrigger);
                }

				//if (bFound)
			    lblParseStatus.Text = "Finished, all traits and trait triggers read successfully.";

				// enable find-problem and find-problem-trigger buttons
				btnNextProblemTrait.Enabled = true;
				btnFindProblemTriggerTrait.Enabled = true;

				// init the helper vars
				currentTraitNumber = 0; currentLevelNumber = 0;

				// we've got everything read, now initialise the first readout
				refreshTrait(true);
			}
		}

		private void button49_Click(object sender, System.EventArgs e)
		{
			// reroute this to the parse button action on the Ancillaries tab page
			this.btnParseAncillaries_Click(sender,e);
		}

		// refills trait info boxes with current trait info
		// calls refreshTrigger() to refill trigger info for the new trait if redoTrigger is true
                private void refreshTrait(bool redoTrigger) {
                    // check for valid trait number
                    if ( currentTraitNumber > -1 && currentTraitNumber < traits.Count ) {
                        // reset all text colouring
                        resetTraitTabColours();

                        string tempString = "", spacerString = "";

                        // grab the current trait (for global use, this is used in several other places)
                        currentTrait = ( Trait )traits[currentTraitNumber];

                        // and check it!
                        currentTrait = checkTrait(currentTrait);

                        // start filling in boxes
                        lblTraitName.Text = currentTrait.name;

                        if ( currentTrait.hiddenTrait )
                            lblHidden.Text = "Yes";
                        else
                            lblHidden.Text = "No";

                        lblTriggerTraitName.Text = currentTrait.name;

                        setLabelError(currentTrait.noGoingBackLevel, lblNoGoingBackLevel);

                        // the complex ones require reconstruction
                        for ( int n = 0; n < currentTrait.characterTypes.Count; n++ ) {
                            tempString += spacerString + currentTrait.characterTypes[n].ToString();
                            spacerString = ", ";
                        }
                        lblCharacters.Text = tempString;

                        spacerString = ""; tempString = "";
                        for ( int n = 0; n < currentTrait.antiTraits.Count; n++ ) {
                            tempString += spacerString + currentTrait.antiTraits[n].ToString();
                            spacerString = ", ";
                        }

                        if ( tempString.IndexOf("{") > -1 ) {
                            lblAntiTraits.ForeColor = Color.Red;
                        }

                lblAntiTraits.Text = tempString;

                spacerString = ""; tempString = "";
                for ( int n = 0; n < currentTrait.excludeCultures.Count; n++ ) {
                    tempString += spacerString + currentTrait.excludeCultures[n].ToString();
                    spacerString = ", ";
                }
                lblExcludesCultures.Text = tempString;

                // now because we need to be precise, set the level to 0 and refresh level info
                currentLevelNumber = 0;
                refreshLevel();

                if ( redoTrigger )
                    refreshTrigger(true, 0, true, true);
            }
        }

		// refills ancillary info boxes with current ancillary info
		// calls refreshAncillaryTrigger() to refill ancillary trigger info for the new ancillary if redoTrigger is true
		private void refreshAncillary(bool redoTrigger)
		{
		    Effect tempEffect;
		
			// first check for a valid ancillary number
			if (currentAncillaryNumber > -1 && currentAncillaryNumber < ancillaries.Count)
			{
				// reset colouring
				
				// grab the current ancillary (for global use, this is used in several other places)
				currentAncillary = (Ancillary) ancillaries[currentAncillaryNumber];
		
				// and check it!
				currentAncillary = checkAncillary(currentAncillary);

				// helper vars
				string tempString = "", spacerString = "";
				string thisName = currentAncillary.name;
				string thisDesc = currentAncillary.descriptionTag;
				string thisEffDesc = currentAncillary.effectsDescriptionTag;

				// start filling in boxes

				lblAncillaryImageFile.Text = currentAncillary.imageFilename;
                setLabelError(currentAncillary.name, lblAncillaryName);
                setLabelError(currentAncillary.effectsDescriptionTag, lblAncillaryEffectsDescription);
                setLabelError(currentAncillary.descriptionTag, lblAncillaryDescription);

				if (currentAncillary.isUniqueAncillary)
					lblUniqueAncillary.Text = "Yes";
				else
					lblUniqueAncillary.Text = "No";

				label77.Text = currentAncillary.name;										// label for triggers page

				// the complex ones require reconstruction
				for (int n=0;n<currentAncillary.excludedAncillaries.Count;n++)
				{
					tempString += spacerString + currentAncillary.excludedAncillaries[n].ToString();
					spacerString = ", ";
				}

                setLabelError(tempString, lblExcludesAncillaries2);
				
				spacerString = ""; tempString = "";
				for (int n=0;n<currentAncillary.excludedCultures.Count;n++)
				{
					tempString += spacerString + currentAncillary.excludedCultures[n].ToString();
					spacerString = ", ";
				}
				lblExcludesAncillaryCultures.Text = tempString;

				// effects, first clear, then add new
				lblAncillaryEffects.Text = "";

				for (int n=0;n<currentAncillary.effects.Count;n++)
				{
					tempEffect = (Effect) currentAncillary.effects[n];
					lblAncillaryEffects.Text += tempEffect.name + " ";

					if (Int16.Parse(tempEffect.level) > 0)
						lblAncillaryEffects.Text += "+" + tempEffect.level.ToString();
                    else if ( Int16.Parse(tempEffect.level) < 0 )
						lblAncillaryEffects.Text += "-" + tempEffect.level.ToString();
					else
						lblAncillaryEffects.Text += tempEffect.level.ToString();

					lblLevelEffects.Text += System.Environment.NewLine;
				}
                if ( lblLevelEffects.Text.IndexOf("{") > -1 ) {
                    lblLevelEffects.ForeColor = Color.Red;
                }

				if (redoTrigger)
					refreshAncillaryTrigger(true,0,true,true);
			}
		}

		// checks a trait for correct info
		private Trait checkTrait(Trait toCheck)
		{
			TextReader reader = null;

			// first we check the name & description against the export_VnVs.txt file
			string tempPath = txtTraitsFilePath.Text;
			tempPath += "text/export_VnVs.txt";
			bool allOK = false;

			try
			{
				reader = new StreamReader(tempPath);
				allOK = true;
			}
			catch
			{
				lblParseStatus.Text = "File not found: " + tempPath + "  Please correct the path and try again.";
			}

			if (allOK)
			{
				string levelName = "", levelDesc = "", levelEffDesc = "", levelGain = "", levelLose = "", levelEpithet = "";
				string newLine = "";
				string tempName = "";
				bool foundName = false, foundDesc = false, foundEffDesc = false, foundGain = false, foundLose = false, foundEpithet = false;
				bool checkOK = false;
				TraitLevel tempLevel = null;
				Trait tempTrait = null;

				//---------------------- CHECK TRAIT INFO -----------------//
				// anti-traits
				for (int n=0;n<toCheck.antiTraits.Count;n++)
				{
					checkOK = false;

					tempName = toCheck.antiTraits[n].ToString();
					// now loop through all traits and see if this actually exists
					for (int x=0;x<traits.Count;x++)
					{
						tempTrait = (Trait) traits[x];
						if (tempTrait.name == tempName)
						{
							checkOK = true;
							break;
						}
					}

					if (!checkOK && tempName.IndexOf("{") < 0)
					{
						tempName = "{" + tempName + "}";
						toCheck.antiTraits[n] = tempName;
					}
				}

				if (!toCheck.hiddenTrait)
				{
					//---------------------- CHECK LEVEL INFO -----------------//
					// we're checking levels, not traits here, so start looping
					for (int n=0;n<toCheck.levels.Count;n++)
					{
						// reset found vars
						foundName = false; foundDesc = false; foundEffDesc = false; foundGain = false; foundLose = false; foundEpithet = false;

						// pull out level for easy access
						tempLevel = (TraitLevel) toCheck.levels[n];

						// pull to-check vals from the level
						if (!tempLevel.name.StartsWith("{"))
						{
							levelName = "{" + tempLevel.name + "}";
							levelDesc = "{" + tempLevel.descriptionName + "}";
							levelEffDesc = "{" + tempLevel.effectsDescName + "}";
							levelGain = "{" + tempLevel.gainMessageName + "}";
							levelLose = "{" + tempLevel.loseMessageName + "}";
							levelEpithet = "{" + tempLevel.epithetName + "}";
						}

						// we try-blocked this earlier with the same path, so we assume all will be ok here as well
						reader = new StreamReader(tempPath);

						// don't worry about empties
						if (levelGain == "{}")
							foundGain = true;
						if (levelLose == "{}")
							foundLose = true;
						if (levelEpithet == "{}")
							foundEpithet = true;

						// loop through and check values
						while ((newLine = reader.ReadLine()) != null)
						{
							// skip comments
							if (newLine.IndexOf("¬") < 0)
							{
								if (!foundName)
									if (newLine.IndexOf(levelName) > -1)
										foundName = true;

								if (!foundDesc)
									if (newLine.IndexOf(levelDesc) > -1)
										foundDesc = true;

								if (!foundEffDesc)
									if (newLine.IndexOf(levelEffDesc) > -1)
										foundEffDesc = true;

								if (!foundGain)
									if (newLine.IndexOf(levelGain) > -1)
										foundGain = true;

								if (!foundLose)
									if (newLine.IndexOf(levelLose) > -1)
										foundLose = true;

								if (!foundEpithet)
									if (newLine.IndexOf(levelEpithet) > -1)
										foundEpithet = true;

								if (foundName && foundDesc && foundEffDesc && foundGain && foundLose && foundEpithet)
									break;
							}
						}

						if (!foundName)
							tempLevel.name = levelName;

						if (!foundDesc)
							tempLevel.descriptionName = levelDesc;

						if (!foundEffDesc)
							tempLevel.effectsDescName = levelEffDesc;

						if (!foundGain)
							tempLevel.gainMessageName = levelGain;

						if (!foundLose)
							tempLevel.loseMessageName = levelLose;

						if (!foundEpithet)
							tempLevel.epithetName = levelEpithet;

						toCheck.levels[n] = tempLevel;
					}
				}
                reader.Close();
			}

			return toCheck;
		}
		// checks an ancillary for correct info
		private Ancillary checkAncillary(Ancillary toCheck)
		{
			// reader for the text stream
			TextReader reader = null;

			// first we check the name & description against the export_VnVs.txt file
			string tempPath = txtAncillaryPath.Text;
			tempPath += "text/export_ancillaries.txt";
			bool allOK = false;

			try
			{
				reader = new StreamReader(tempPath);
				allOK = true;	
			}
			catch
			{
				lblParseStatus.Text = "File not found: " + tempPath + "  Please correct the path and try again.";
			}

			if (allOK)
			{
				string levelName = "", levelDesc = "", levelEffDesc = "";
				string newLine = "";
				string tempName = "";
				bool foundName = false, foundDesc = false, foundEffDesc = false;
				bool checkOK = false;
				Ancillary tempAncillary = null;

				//---------------------- CHECK ANCILLARY INFO -----------------//
				// excluded ancillaries
				for (int n=0;n<toCheck.excludedAncillaries.Count;n++)
				{
					checkOK = false;

					tempName = toCheck.excludedAncillaries[n].ToString();
					// now loop through all ancillaries and see if this actually exists
					for (int x=0;x<ancillaries.Count;x++)
					{
						tempAncillary = (Ancillary) ancillaries[x];
						if (tempAncillary.name == tempName)
						{
							checkOK = true;
							break;
						}
					}

					if (!checkOK && tempName.IndexOf("{") < 0)
					{
						tempName = "{" + tempName + "}";
						toCheck.excludedAncillaries[n] = tempName;
					}
				}

				// pull to-check vals from the level
				if (!toCheck.name.StartsWith("{"))
				{
					levelName = "{" + toCheck.name + "}";
					levelDesc = "{" + toCheck.descriptionTag + "}";
					levelEffDesc = "{" + toCheck.effectsDescriptionTag + "}";
				}

				// we try-blocked this earlier with the same path, so we assume all will be ok here as well
				reader = new StreamReader(tempPath);

				// loop through and check values
				while ((newLine = reader.ReadLine()) != null)
				{
					// skip comments
					if (newLine.IndexOf("¬") < 0)
					{
						if (!foundName)
							if (newLine.IndexOf(levelName) > -1)
								foundName = true;

						if (!foundDesc)
							if (newLine.IndexOf(levelDesc) > -1)
								foundDesc = true;

						if (!foundEffDesc)
							if (newLine.IndexOf(levelEffDesc) > -1)
								foundEffDesc = true;

						if (foundName && foundDesc && foundEffDesc)
							break;
					}
				}

				// permanently save the marked-as-bad version
				if (!foundName)
					toCheck.name = levelName;

				if (!foundDesc)
					toCheck.descriptionTag = levelDesc;

				if (!foundEffDesc)
					toCheck.effectsDescriptionTag = levelEffDesc;

                reader.Close();
			}

			return toCheck;
		}


		// checks for problem ancillary triggers
		private void checkAncillaryTriggers()
		{
			// helper vars
			Trigger tempTrig = null;
			TriggerAffect tempAff = null;
			Ancillary tempAnc = null;
			bool foundMatch = false;
			bool conditionsOK = true;
			bool paramsOK = true;
			int n = 0;

			for (n=0;n<ancillaryTriggers.Count;n++)
			{
				tempTrig = (Trigger) ancillaryTriggers[n];
				lblAncillaryTriggerStatus.Text = "Checking trigger " + tempTrig.name;
				lblAncillaryTriggerStatus.Refresh();

				// loop to check affects list
                for ( int x = 0; x < tempTrig.acquireAncillaryList.Count; x++ )
				{
					tempAff = (TriggerAffect) tempTrig.acquireAncillaryList[x];

					// note that for ancillaries, traitName is actually the ancillaryName. This was re-used
					//	for convenience and, unfortunately, was not renamed due to the number of references
					//	this property had.
					foundMatch = false;

                    // and now we test against every ancillary till we find a match
					for (int q=0;q<ancillaries.Count;q++)
					{
						tempAnc = (Ancillary) ancillaries[q];
                        if ( tempAff.traitName == tempAnc.name )
						{
							// we found this one, stop looking
							foundMatch = true;
							break;
						}
					}

                    if ( !foundMatch ) {
                        // we fell through without matching, this is a bad trigger!
                        lblAncillaryTriggerStatus.Text = "Bad trigger found. This trigger bestows the " + tempAff.traitName + " ancillary, which does not exist." + Environment.NewLine + Environment.NewLine + "To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
                        currentAncillaryTriggerNumber = n;
                        refreshAncillaryTrigger(false, 0, false, false);
                        currentAncillaryNumber = -1;
                        refreshAncillary(false);
                        break;
                    } else {
                        if ( tempAff.chance.IndexOf("{") > -1 ) {
                            lblAncillaryTriggerStatus.Text = "Bad trigger found. This trigger has a non-integer chance of bestowing the " + tempAff.traitName + " ancillary." + Environment.NewLine + Environment.NewLine + "To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
                            foundMatch = false;
                            break;
                        }
                    }
				}

                if ( !foundMatch ) {
                    break; //if there's a problem with the AcquireAncillary line break;
                }

				// need to check Condition syntax
				Condition testTemplate = null, testCurrent = null;
				lblTriggerStatus.Text = "Testing conditions";
				lblTriggerStatus.Refresh();

				paramsOK = true;

				// this is identical to the Trait Trigger check for condition syntax, see that function for comments
				for (int q=0;q<tempTrig.conditions.Count && paramsOK;q++)
				{
					foundMatch = false;
					testCurrent = (Condition) tempTrig.conditions[q];
					lblAncillaryTriggerStatus.Text = "Testing conditions: finding match for " + testCurrent.name;
					lblAncillaryTriggerStatus.Refresh();

					for (int x=0;x<conditionTemplates.Count && !foundMatch;x++)
					{
						testTemplate = (Condition) conditionTemplates[x];
						if (testTemplate.name == testCurrent.name)
						{
							foundMatch = true;
							lblAncillaryTriggerStatus.Text = "Testing conditions: testing parameters for " + testCurrent.name;
							lblAncillaryTriggerStatus.Refresh();

							for (int y=0;y<testTemplate.parameters.Count;y++)
							{
								if (testTemplate.parameters[y].ToString() == "logic token")
								{
									if (testCurrent.parameters.Count > y)
									{
										if (!(testCurrent.parameters[y].ToString() == "=" ||
											testCurrent.parameters[y].ToString() == "<" ||
											testCurrent.parameters[y].ToString() == ">" ||
											testCurrent.parameters[y].ToString() == "<=" ||
											testCurrent.parameters[y].ToString() == ">="))
										{
											paramsOK = false;
											y++;
											lblAncillaryTriggerStatus.Text = "Bad parameter found for " + testCurrent.name + ", parameter " + testCurrent.parameters[y].ToString() + " is invalid, should be a valid logic token. To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
											lblAncillaryTriggerStatus.Refresh();
											break;
										}
									}
									else
									{
										paramsOK = false;
										y++;
										lblAncillaryTriggerStatus.Text = "Missing parameter for "  + testCurrent.name + ", parameter #" + y.ToString() + " should be a " + testTemplate.parameters[y-1].ToString() + ". To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
										lblAncillaryTriggerStatus.Refresh();
										break;
									}
								}
								else if (testTemplate.parameters[y].ToString() == "test value")
								{
									if (testCurrent.parameters.Count > y)
									{
										try
										{
											float testCast = float.Parse(testCurrent.parameters[y].ToString());
										}
										catch
										{
											paramsOK = false;
											lblAncillaryTriggerStatus.Text = "Bad parameter found for " + testCurrent.name + ", parameter " + testCurrent.parameters[y].ToString() + " is invalid, should be a valid test value. To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
											lblAncillaryTriggerStatus.Refresh();
											break;
										}
									}
									else
									{
										paramsOK = false;
										y++;
										lblAncillaryTriggerStatus.Text = "Missing parameter for "  + testCurrent.name + ", parameter #" + y.ToString() + " should be a " + testTemplate.parameters[y-1].ToString() + ". To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
										lblAncillaryTriggerStatus.Refresh();
										break;
									}
								}
								else if (testTemplate.parameters[y].ToString() != "")
								{
									if (!(testCurrent.parameters.Count > y))
									{
										paramsOK = false;
										y++;
										lblAncillaryTriggerStatus.Text = "Missing parameter for "  + testCurrent.name + ", parameter #" + y.ToString() + " should be a " + testTemplate.parameters[y-1].ToString() + ". To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
										lblAncillaryTriggerStatus.Refresh();
										break;
									}
								}
								else
								{
									break;
								}
							} // END Y loop
						}
					} // END X loop
					if (!foundMatch)
					{
                        lblAncillaryTriggerStatus.Text = "Cound not find match for condition " + testCurrent.name + "." + Environment.NewLine + Environment.NewLine + "To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
						lblAncillaryTriggerStatus.Refresh();
						break;
					}
				}// END Q loop

				if (!foundMatch || !paramsOK)
				{
					// just break here, we took care of informing the user in the inner loop
					break;
				}
			} //END N loop

			// if there is a problem, fall through. Otherwise inform user that everything is ok
			if (foundMatch && conditionsOK && paramsOK)
			{
				// we got all the way through with no problems, inform
				lblAncillaryTriggerStatus.Text = "All ancillary triggers OK.";
			}

			// set current trigger number and refresh all
			currentAncillaryTriggerNumber = n;
			refreshAncillaryTrigger(false,0,false,false);
			currentAncillaryNumber = -1;
			refreshAncillary(false);
		}


		// checks for problem trait triggers
		private void checkTraitTriggers()
		{
			// helper vars
			Trigger tempTrig = null;
			TriggerAffect tempAff = null;
			Trait tempTrait = null;
			string testName = "";
			bool foundMatch = false;
			bool conditionsOK = true;
			bool paramsOK = true;
			int n = 0;

			for (n=0;n<triggers.Count;n++)
			{
				tempTrig = (Trigger) triggers[n];
				lblTriggerStatus.Text = "Checking trigger " + tempTrig.name;
				lblTriggerStatus.Refresh();

				// loop to check affects list
				for (int x=0;x<tempTrig.affectsList.Count;x++)
				{
					tempAff = (TriggerAffect) tempTrig.affectsList[x];

					testName = tempAff.traitName;

					foundMatch = false;
					// and now we test against every trait till we find a match
					for (int q=0;q<traits.Count;q++)
					{
						tempTrait = (Trait) traits[q];
						if (testName == tempTrait.name)
						{
							// we found this one, stop looking
							foundMatch = true;
							break;
						}
					}

                    if ( !foundMatch ) {
                        // we fell through without matching, this is a bad trigger!
                        lblTriggerStatus.Text = "Bad trigger found. This trigger bestows the " + testName + " trait, which does not exist." + Environment.NewLine + Environment.NewLine + "To continue the search, correct the problem and reparse the trait file, then hit the 'Find Problem Trigger' button again.";
                        currentTriggerNumber = n;
                        refreshTrigger(false, 0, false, false);
                        currentTraitNumber = -1;
                        refreshTrait(false);
                        break;
                    } else {
                        if ( tempAff.chance.IndexOf("{") > -1 ) {
                            lblTriggerStatus.Text = "Bad trigger found. This trigger has a non-integer chance of bestowing the " + tempAff.traitName + " trait." + Environment.NewLine + Environment.NewLine + "To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
                            foundMatch = false;
                            break;
                        }
                        if ( tempAff.traitThreshhold.IndexOf("{") > -1 ) {
                            lblTriggerStatus.Text = "Bad trigger found. This trigger bestows a non-integer number of points to the " + tempAff.traitName + " trait." + Environment.NewLine + Environment.NewLine + "To continue the search, correct the problem and reparse the ancillary file, then hit the 'Find Problem Trigger' button again.";
                            foundMatch = false;
                            break;
                        }
                    }
				}

				// need to check Condition syntax
				Condition testTemplate = null, testCurrent = null;
				lblTriggerStatus.Text = "Testing conditions";
				lblTriggerStatus.Refresh();

				paramsOK = true;

				// the Q loop runs through all conditions in THIS trigger, the X loop runs
				// through all condition TEMPLATES to find a match and thus check for correct syntax,
				// and finally the Y loop runs through all parameters in the current trigger condition
				for (int q=0;q<tempTrig.conditions.Count && paramsOK;q++)
				{
					foundMatch = false;
					testCurrent = (Condition) tempTrig.conditions[q];
					lblTriggerStatus.Text = "Testing conditions: finding match for " + testCurrent.name;
					lblTriggerStatus.Refresh();

					for (int x=0;x<conditionTemplates.Count && !foundMatch;x++)
					{
						testTemplate = (Condition) conditionTemplates[x];

						// check for a matching name
						if (testTemplate.name == testCurrent.name)
						{
							// reset foundmatch and notify of current action
							foundMatch = true;
							lblTriggerStatus.Text = "Testing conditions: testing parameters for " + testCurrent.name;
							lblTriggerStatus.Refresh();

							// check parameters for correct type
							// don't check for correct value quite yet, may do this later in a limited way
							for (int y=0;y<testTemplate.parameters.Count;y++)
							{
								if (testTemplate.parameters[y].ToString() == "logic token")
								{
									// check to make sure there isn't an empty parameter in the test case
									if (testCurrent.parameters.Count > y)
									{
										if (!(testCurrent.parameters[y].ToString() == "=" ||
											testCurrent.parameters[y].ToString() == "<" ||
											testCurrent.parameters[y].ToString() == ">" ||
											testCurrent.parameters[y].ToString() == "<=" ||
											testCurrent.parameters[y].ToString() == ">="))
										{
											// logic token check failed
											paramsOK = false;
											y++;
											lblTriggerStatus.Text = "Bad parameter found for " + testCurrent.name + ", parameter " + testCurrent.parameters[y].ToString() + " is invalid, should be a valid logic token. To continue, fix the problem in the traits file, and re-parse the file.";
											lblTriggerStatus.Refresh();
											break;
										}
									}
									else
									{
										paramsOK = false;
										y++;
										lblTriggerStatus.Text = "Missing parameter for "  + testCurrent.name + ", parameter #" + y.ToString() + " should be a " + testTemplate.parameters[y-1].ToString() + ". To continue, fix the problem in the traits file, and re-parse the file.";
										lblTriggerStatus.Refresh();
										break;
									}
								}
								else if (testTemplate.parameters[y].ToString() == "test value")
								{
									// check to make sure there isn't an empty parameter in the test case
									if (testCurrent.parameters.Count > y)
									{
										// try casting this to a float. if it fails, it's a bad test value
										try
										{
											float testCast = float.Parse(testCurrent.parameters[y].ToString());
										}
										catch
										{
											// cast failed
											paramsOK = false;
											lblTriggerStatus.Text = "Bad parameter found for " + testCurrent.name + ", parameter " + testCurrent.parameters[y].ToString() + " is invalid, should be a valid test value. To continue, fix the problem in the traits file, and re-parse the file.";
											lblTriggerStatus.Refresh();
											break;
										}
									}
									else
									{
										paramsOK = false;
										y++;
										lblTriggerStatus.Text = "Missing parameter for "  + testCurrent.name + ", parameter #" + y.ToString() + " should be a " + testTemplate.parameters[y-1].ToString() + ". To continue, fix the problem in the traits file, and re-parse the file.";
										lblTriggerStatus.Refresh();
										break;
									}
								}
								else if (testTemplate.parameters[y].ToString() != "")
								{
									// there is some parameter but we haven't spec'd it yet. Just check to make sure
									//	that there is some sort of value in the condition being checked
									if (!(testCurrent.parameters.Count > y))
									{
										paramsOK = false;
										y++;
										lblTriggerStatus.Text = "Missing parameter for "  + testCurrent.name + ", parameter #" + y.ToString() + " should be a " + testTemplate.parameters[y-1].ToString() + ". To continue, fix the problem in the traits file, and re-parse the file.";
										lblTriggerStatus.Refresh();
										break;
									}
								}
								else
								{
									// template is empty, we can break
									break;
								}
							} // END Y loop
						}
					} // END X loop
					if (!foundMatch)
					{
						// couldn't find this condition
						lblTriggerStatus.Text = "Cound not find match for condition " + testCurrent.name + ". To continue, correct this problem and reparse the file.";
						lblTriggerStatus.Refresh();
						break;
					}
				}//end for (q<conditions.Count)

				if (!foundMatch || !paramsOK)
				{
					// just break here, we took care of informing the user in the inner loop
					break;
				}
			} //end for (n<triggers.Count)

			// if there is a problem, just fall through. Otherwise inform user that everything is ok
            if ( foundMatch && conditionsOK && paramsOK ) {
                // we got all the way through with no problems, inform
                lblTriggerStatus.Text = "All trait triggers OK.";
                currentTriggerNumber = -1;
                refreshTrigger(false, 0, false, false);
                currentTraitNumber = 0;
                refreshTrait(false);
            } else {

                // set current trigger number and refresh all
                currentTriggerNumber = n;
                refreshTrigger(false, 0, false, false);
                currentTraitNumber = -1;
                refreshTrait(false);
            }
		}


		// refills level info boxes with current level info
		private void refreshLevel()
		{
			Effect tempEffect = null;

			// reset label colouring
            resetTraitTabLevelColours();

			// first get the level
			if (currentLevelNumber < currentTrait.levels.Count && currentLevelNumber > -1){
				TraitLevel tempLevel = (TraitLevel) currentTrait.levels[currentLevelNumber];

                //string tempString = tempLevel.name;

                setLabelError(tempLevel.name, lblLevelName);
                setLabelError(tempLevel.descriptionName, lblLevelDesc);
                setLabelError(tempLevel.effectsDescName, lblLevelEffectDesc);
                setLabelError(tempLevel.gainMessageName, lblLevelGain);
                setLabelError(tempLevel.loseMessageName, lblLevelLose);
                setLabelError(tempLevel.epithetName, lblLevelEpithet);
                setLabelError(tempLevel.threshhold, lblLevelThreshhold);

				// effects, first clear, then add new
				lblLevelEffects.Text = "";

                for ( int n = 0; n < tempLevel.effects.Count; n++ ) {
                    tempEffect = ( Effect )tempLevel.effects[n];
                    lblLevelEffects.Text += tempEffect.name + " ";

                    if ( Int16.Parse(tempEffect.level) > 0 )
                        lblLevelEffects.Text += "+" + tempEffect.level.ToString();
                    else if ( Int16.Parse(tempEffect.level) < 0 )
                        lblLevelEffects.Text += "-" + tempEffect.level.ToString();
                    else
                        lblLevelEffects.Text += tempEffect.level.ToString();

                    lblLevelEffects.Text += System.Environment.NewLine;
                }
                if ( lblLevelEffects.Text.IndexOf("{") > -1 ) {
                    lblLevelEffects.ForeColor = Color.Red;
                }
			}else{
				lblParseStatus.Text = "Error Encountered: invalid level (" + currentLevelNumber.ToString() + ") attempted on the " + currentTrait.name + " trait.";
			}
		}

		// refreshes trigger info for traits
		private void refreshTrigger(bool findTrigger, int searchStartIndex, bool searchForward, bool blankStatus)
		{
			// clear labels
			lblTriggerConditions.Text = "";
			lblTriggerAffectsName.Text = "";
			lblTriggerAffectsChance.Text = "";
			lblTriggerAffectsLevel.Text = "";
			lblTriggerName.Text = "";
			lblWhenToTest.Text = "";

			// helper vars
			TriggerAffect trigAffect = null;

			// if needed, find a trigger that gives this trait
			if (findTrigger)
				currentTriggerNumber = findTriggerForTrait(searchStartIndex, searchForward);

			// make sure we aren't out of range
			if (currentTriggerNumber > triggers.Count - 1)
				currentTriggerNumber = triggers.Count - 1;

            if ( currentTriggerNumber == -1 ) {
                if ( blankStatus ) {
                    // inform the user that there was no matching trigger found
                    if (( searchStartIndex > 0 && searchForward)||(searchStartIndex < triggers.Count - 1 && !searchForward))
                        lblTriggerStatus.Text = "There was no further trigger found bestowing this trait. You can loop back or forward through the existing triggers.";
                    else
                        lblTriggerStatus.Text = "There was no trigger found bestowing this trait.";
                }
            } else {
                // blank the status box, unless blankStatus is false
                if ( blankStatus )
                    lblTriggerStatus.Text = "";

                // now pull out trigger info
                Trigger newTrig = ( Trigger )triggers[currentTriggerNumber];

                resetTraitTriggerTabColours();

                lblTriggerName.Text = newTrig.name;

                lblWhenToTest.Text = newTrig.triggerEvent.eventName;
                if ( newTrig.triggerEvent.eventName.IndexOf("{") > -1 ) {
                    lblWhenToTest.ForeColor = Color.Red;
                }

                // loop to fill up conditions
                for ( int n = 0; n < newTrig.conditions.Count; n++ ) {
                    string construct = "";
                    Condition tempCond = ( Condition )newTrig.conditions[n];

                    construct = tempCond.name;
                    for ( int i = 0; i < tempCond.parameters.Count; i++ )
                        construct += " " + tempCond.parameters[i].ToString();

                    lblTriggerConditions.Text += construct + Environment.NewLine;
                }
                if ( lblTriggerConditions.Text.IndexOf("{") > -1 ) {
                    lblTriggerConditions.ForeColor = Color.Red;
                }

                // loop to fill up affects
                for ( int n = 0; n < newTrig.affectsList.Count; n++ ) {
                    // pull out affect
                    trigAffect = ( TriggerAffect )newTrig.affectsList[n];

                    // now add this info to the right labels
                    lblTriggerAffectsName.Text += trigAffect.traitName + Environment.NewLine;
                    lblTriggerAffectsLevel.Text += trigAffect.traitThreshhold + Environment.NewLine;
                    lblTriggerAffectsChance.Text += trigAffect.chance + Environment.NewLine;
                }
                if ( lblTriggerAffectsName.Text.IndexOf("{") > -1 ) {
                    lblTriggerAffectsName.ForeColor = Color.Red;
                }
                if ( lblTriggerAffectsLevel.Text.IndexOf("{") > -1 ) {
                    lblTriggerAffectsLevel.ForeColor = Color.Red;
                }
                if ( lblTriggerAffectsChance.Text.IndexOf("{") > -1 ) {
                    lblTriggerAffectsChance.ForeColor = Color.Red;
                }
            }
		}

		// refreshes trigger info for ancillaries
		private void refreshAncillaryTrigger(bool findTrigger, int searchStartIndex, bool searchForward, bool blankStatus)
		{
			// clear labels
			lblAncillaryConditions.Text = "";
			lblAncillaryTriggerName.Text = "";
			lblAncillaryEffectName.Text = "";
			lblAncillaryEffectChance.Text = "";
			lblAncillaryWhenTested.Text = "";

			// helper vars
			TriggerAffect trigAffect = null;

			// if needed, find a trigger that gives this ancillary
			if (findTrigger)
				currentAncillaryTriggerNumber = findTriggerForAncillary(searchStartIndex, searchForward);

			// make sure we aren't out of range
			if (currentAncillaryTriggerNumber > ancillaryTriggers.Count - 1)
				currentAncillaryTriggerNumber = ancillaryTriggers.Count - 1;

			if (currentAncillaryTriggerNumber == -1)
			{
				// inform the user that there was no matching trigger found
				if ((searchStartIndex > 0 && searchForward)|| (searchStartIndex < triggers.Count -1 || !searchForward))
					lblAncillaryTriggerStatus.Text = "There was no further trigger found bestowing this trait. You can loop back or forward through the existing triggers.";
				else
					lblAncillaryTriggerStatus.Text = "There was no trigger found bestowing this trait.";
			}
			else
			{
				// blank the status box unless blankStatus is false
				if (blankStatus)
					lblAncillaryTriggerStatus.Text = "";

				// now pull out trigger info
				Trigger newTrig = (Trigger) ancillaryTriggers[currentAncillaryTriggerNumber];

                // reset colouring
                resetAncillaryTriggerTabColours();

                lblAncillaryTriggerName.Text = newTrig.name;

				lblAncillaryWhenTested.Text = newTrig.triggerEvent.eventName;
                if ( newTrig.triggerEvent.eventName.IndexOf("{") > -1 ) {
                    lblAncillaryWhenTested.ForeColor = Color.Red;
                }

				// loop to fill up conditions
				for (int n=0;n<newTrig.conditions.Count;n++)
				{
					string construct = "";
					Condition tempCond = (Condition) newTrig.conditions[n];

					construct = tempCond.name;
					for (int x=0;x<tempCond.parameters.Count;x++)
						construct += " " + tempCond.parameters[x].ToString();

					lblAncillaryConditions.Text += construct + Environment.NewLine;
				}
                if ( lblAncillaryConditions.Text.IndexOf("{") > -1 ) {
                    lblAncillaryConditions.ForeColor = Color.Red;
                }

				// loop to fill up affects
				for (int n=0;n<newTrig.acquireAncillaryList.Count;n++)
				{
					// pull out affect
					trigAffect = (TriggerAffect) newTrig.acquireAncillaryList[n];

					// now add this info to the right labels
					lblAncillaryEffectName.Text += trigAffect.traitName + Environment.NewLine;
					lblAncillaryEffectChance.Text += trigAffect.chance + Environment.NewLine;
				}
                if ( lblAncillaryEffectName.Text.IndexOf("{") > -1 ) {
                    lblAncillaryEffectName.ForeColor = Color.Red;
                }
                if ( lblAncillaryEffectChance.Text.IndexOf("{") > -1 ) {
                    lblAncillaryEffectChance.ForeColor = Color.Red;
                }
			}
		}

		// finds a trigger that bestows the trait set by currentTraitNumber. If a values is passed
		//	in to searchStartIndex, this limits the search to only triggers in the triggers
		//	ArrayList which have an index greater than this passed-in value.
		//
		// Returns -1 if no match was found.
		private int findTriggerForTrait(int searchStartIndex, bool searchForward)
		{
			// catch any out-of-range values
			if (searchStartIndex < -1 || searchStartIndex > triggers.Count)
			{
				// set to the appropriate "beginning" value
				if (searchForward)
					searchStartIndex = 0;
				else
					searchStartIndex = triggers.Count-1;
			}

			// helper vars
			Trigger tempTrig = null;
			TriggerAffect tempAff = null;

			// grab current trait
			Trait newTrait = (Trait) traits[currentTraitNumber];

			// set the appropriate incrementer
			int incr = 0;
			if (searchForward)
				incr = 1;
			else
				incr = -1;

			// loop through all triggers until one is found that matches the affect name
			for (int n=searchStartIndex;n<triggers.Count && n>-1;n+=incr)
			{
				tempTrig = (Trigger) triggers[n];
				for (int x=0;x<tempTrig.affectsList.Count;x++)
				{
					tempAff = (TriggerAffect) tempTrig.affectsList[x];
					if (tempAff.traitName == newTrait.name)
					{
						// we found one, simply return the index of the trigger
						return n;
					}
				}
			}

			// no matching trigger found, return -1
			return -1;
		}

		// same search as findTriggerForTrait, except this is for ancillaries
		//
		// Returns -1 if no match was found.
		private int findTriggerForAncillary(int searchStartIndex, bool searchForward)
		{
			// catch any out-of-range values
			if (searchStartIndex < -1 || searchStartIndex > ancillaries.Count)
			{
				// set to the appropriate "beginning" value
				if (searchForward)
					searchStartIndex = 0;
				else
					searchStartIndex = ancillaries.Count-1;
			}

			// helper vars
			Trigger tempTrig = null;
			TriggerAffect tempAff = null;

			// grab current trait
			Ancillary newAncillary = (Ancillary) ancillaries[currentAncillaryNumber];

			// set the appropriate incrementer
			int incr = 0;
			if (searchForward)
				incr = 1;
			else
				incr = -1;

			// loop through all triggers until one is found that matches the affect name
			for (int n=searchStartIndex;n<ancillaryTriggers.Count && n>-1;n+=incr)
			{
				tempTrig = (Trigger) ancillaryTriggers[n];
				for (int x=0;x<tempTrig.acquireAncillaryList.Count;x++)
				{
					tempAff = (TriggerAffect) tempTrig.acquireAncillaryList[x];
					if (tempAff.traitName == newAncillary.name)
					{
						// we found one, simply return the index of the trigger
						return n;
					}
				}
			}

			// no matching trigger found, return -1
			return -1;
		}


		private ArrayList ProcessStringArray(string toProcess, int firstValuePosition, int maxValuePosition)
		{
			// take out double-spaces, remove tabs, trim, remove commas
			toProcess = toProcess.Replace('\t',' ');
			toProcess = toProcess.Trim();
			toProcess = toProcess.Replace(",","");

			// and now, due to the lack of regex as in Perl, we will replace double-spaces with single ten times
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");
			toProcess = toProcess.Replace("  "," ");

			// split line
			string[] tempList = toProcess.Split(' ');
			ArrayList tempArray = new ArrayList();

			if (maxValuePosition > tempList.GetUpperBound(0))
				maxValuePosition = tempList.GetUpperBound(0) + 1;

			// at this point, tempList[firstValuePosition] should have the first data we want to save, so loop from there
			for (int n=firstValuePosition; n<maxValuePosition; n++)
			{
				if (tempList[n] != "")
					tempArray.Add(tempList[n]);
				else
					break;
			}

			return tempArray;
		}

		public int findTraitPosition(string traitName)
		{
			Trait findTemp = null;

			for (int n=0;n<traits.Count;n++)
			{
				findTemp = (Trait) traits[n];
				if (findTemp.name == traitName)
					return n;
			}

			return -1;
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber++;

			if (currentTraitNumber > traits.Count - 1)
				currentTraitNumber--;
			else
				refreshTrait(true);
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber += 5;

			if (currentTraitNumber > traits.Count - 1)
				currentTraitNumber -= 5;
			else
				refreshTrait(true);
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber += 20;

			if (currentTraitNumber > traits.Count - 1)
				currentTraitNumber -= 20;
			else
				refreshTrait(true);
		}

		private void button8_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber = traits.Count - 1;
			refreshTrait(true);
		}

		private void button9_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber--;

			if (currentTraitNumber < 0)
				currentTraitNumber++;
			else
				refreshTrait(true);
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber -= 5;

			if (currentTraitNumber < 0)
				currentTraitNumber += 5;
			else
				refreshTrait(true);
		}

		private void button10_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber -= 20;

			if (currentTraitNumber < 0)
				currentTraitNumber += 20;
			else
				refreshTrait(true);
		}

		private void button11_Click(object sender, System.EventArgs e)
		{
			currentTraitNumber = 0;
			refreshTrait(true);
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			currentLevelNumber++;

			// this requires a bit of work to check validity
			if (currentLevelNumber == currentTrait.levels.Count)
				currentLevelNumber--;
			else
				refreshLevel();
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			currentLevelNumber--;

			if (currentLevelNumber < 0)
				currentLevelNumber++;
			else
				refreshLevel();
		}

		private void lblTraitName_Click(object sender, System.EventArgs e)
		{
			// UI changes to get the textbox showing and label hidden
			lblTraitName.Hide();
			txtTraitName.Text = lblTraitName.Text;
            txtTraitName.SelectAll();
			txtTraitName.Show();
		}
		
		// search for a trait name that matches what was typed into txtTraitName
        private void btnTraitName_Click(object sender, System.EventArgs e) {
            // helper vars
            Trait tempTrait = null;
            bool changeMade = false;

            if ( !parsedTraits ) {
                //do nothing
                lblParseStatus.Text = "To search for a trait, parse the traits by pressing the Parse Traits button.";
            } else if ( txtTraitName.Visible ) {
                // search. We start from the currentTraitNumber+1 because otherwise similar traits would never show up.
                for ( int n = 0; n < traits.Count; n++ ) {
                    tempTrait = ( Trait )traits[(currentTraitNumber + 1+ n) % traits.Count];

                    if ( tempTrait.name.IndexOf(txtTraitName.Text) > -1 ) {
                        // found a match, set current trait and skip to end
                        lblParseStatus.Text = "Matching trait found for string '" + txtTraitName.Text + "'.";
                        currentTraitNumber = ( currentTraitNumber + 1 + n ) % traits.Count;
                        changeMade = true;
                        break;
                    }
                }

                if ( !changeMade )
                    lblParseStatus.Text = "No matching trait found for string '" + txtTraitName.Text + "'.";

                // finally show the label, and refresh
                lblTraitName.Show();
                txtTraitName.Hide();
                refreshTrait(true);
            } else {
                lblTraitName_Click(sender, e);
            }
        }

        private void lblAncillaryName_Click(object sender, EventArgs e) {
            lblAncillaryName.Hide();
            txtAncillaryName.Text = lblAncillaryName.Text;
            txtAncillaryName.SelectAll();
            txtAncillaryName.Show();
        }

        private void btnAncillarySearch_Click(object sender, EventArgs e) {
            // helper vars
            Ancillary tempAncillary = null;
            bool changeMade = false;

            if ( !parsedAncillaries ) {
                //do nothing
                 lblAncillaryStatus.Text = "To search for an ancillary, parse the ancillaries by pressing the Parse Ancillaries button.";
            } else if ( txtAncillaryName.Visible ) {
                // search. We start from the currentTraitNumber+1 because otherwise similar ancillaries would never show up.
                for ( int n = 0; n < ancillaries.Count; n++ ) {
                    tempAncillary = ( Ancillary )ancillaries[( currentAncillaryNumber + 1 + n ) % ancillaries.Count];

                    if ( tempAncillary.name.IndexOf(txtAncillaryName.Text) > -1 ) {
                        // found a match, set current trait and skip to end
                        lblAncillaryStatus.Text = "Matching ancillary found for string '" + txtAncillaryName.Text + "'.";
                        currentAncillaryNumber = ( currentAncillaryNumber + 1 + n ) % ancillaries.Count;
                        changeMade = true;
                        break;
                    }
                }

                if ( !changeMade )
                    lblAncillaryStatus.Text = "No matching ancillary found for string '" + txtAncillaryName.Text + "'.";

                // finally show the label, and refresh
                lblAncillaryName.Show();
                txtAncillaryName.Hide();
                refreshAncillary(true);
            } else {
                lblAncillaryName_Click(sender, e);
            }

        }

		// find the next problem in any trait, starting from current, if any
        private void btnNextProblemTrait_Click(object sender, System.EventArgs e)
		{
            bool problem = false;
            Trait newTrait = null;
            TraitLevel newLevel = null;
            Effect newEffect = null;
            string temp = "";
            int n = 0;	// very important n, this will set currentTraitNumber at the end if a problem is found
            int l = 0;	// also very important, this will set currentLevel for refresh

            // first reset current number if it is bad
			if (currentTraitNumber < 0)
				currentTraitNumber = 0;
			else if (currentTraitNumber > traits.Count-1)
				currentTraitNumber = traits.Count-1;

			// inform user we're starting
			lblParseStatus.Text = "Scanning data for problem areas...";
			lblParseStatus.Refresh();

			// start looping from the current trait
			for(n = currentTraitNumber; n < traits.Count && !problem; n += 1)
			{
				// reset problem trigger
				//problem = false;

				// pull out trait
				newTrait = (Trait) traits[n];

				// inform the user what we're doing
				lblParseStatus.Text = "Scanning data for problem areas... checking trait " + newTrait.name;
				lblParseStatus.Refresh();

				// run it through the trait checker
				newTrait = checkTrait(newTrait);

				// check every single-value property of the trait first...
				if(newTrait.name.IndexOf("{") > -1){
					problem = true;
				}

                // check multi-value properties
                // antitraits
                for(int i = 0; !problem && i < newTrait.antiTraits.Count; i++){
                    temp = newTrait.antiTraits[i].ToString();
                    if ( temp.IndexOf("{") > -1 ) {
                        problem = true;
                    }
                }

                // character types
                for( int i = 0; !problem && i < newTrait.characterTypes.Count; i++ ) {
                    temp = newTrait.characterTypes[i].ToString();
                    if ( temp.IndexOf("{") > -1 ) {
                        problem = true;
                    }
                }

                // NoGoingBackLevel
                if(!problem && newTrait.noGoingBackLevel.IndexOf("{") > -1){
                    problem = true;
                }

                // excluded cultures
                for ( int i = 0; !problem && i < newTrait.excludeCultures.Count; i += 1 ) {
                    temp = newTrait.excludeCultures[i].ToString();
                    if ( temp.IndexOf("{") > -1 ) {
                        problem = true;
                        break;
                    }
                }

                // if we got here, all is ok since the break would take us out of this loop.
                // now check levels
                for ( l = 0; !problem && l < newTrait.levels.Count; l++ ) {
                    // pull out the level
                    newLevel = ( TraitLevel )newTrait.levels[l];

                    // inform the user what's going on
                    lblParseStatus.Text = "Scanning data for problem areas... checking trait " + newTrait.name + ", level " + newLevel.name;
                    lblParseStatus.Refresh();

                    // check single-value properties
                    if ( !problem && newLevel.name.IndexOf("{") > -1 ) {
                        problem = true;
                    }

                    if ( !problem && newLevel.effectsDescName.IndexOf("{") > -1 ) {
                        problem = true;
                    }
                    if ( !problem && newLevel.descriptionName.IndexOf("{") > -1 ) {
                        problem = true;
                    }
                    if ( !problem && newLevel.epithetName.IndexOf("{") > -1 ) {
                        problem = true;
                    }
                    if ( !problem && newLevel.gainMessageName.IndexOf("{") > -1 ) {
                        problem = true;
                    }
                    if ( !problem && newLevel.loseMessageName.IndexOf("{") > -1 ) {
                        problem = true;
                    }

                    // now check multi-value properties of the level
                    // just effects
                    for ( int i = 0; !problem && i < newLevel.effects.Count; i += 1 ) {
                        newEffect = ( Effect )newLevel.effects[i];
                        if ( newEffect.name.IndexOf("{") > -1 || newEffect.level.IndexOf("{") > -1 ) {
                            problem = true;
                            break;
                        }//end if (temp.indexOf("{") > -1)
                    }//end for i < newLevel.effects.Count
                }//end for l < newTrait.levels.Count				
			}//end for n < traits.Count

			if (problem)
			{
				// stop here and display
                lblParseStatus.Text = lblParseStatus.Text + System.Environment.NewLine + System.Environment.NewLine + "Problem found, halting. To continue, correct the problem and re-parse with the Parse Traits button, or skip to the next trait and press the Next Problem button." + System.Environment.NewLine + System.Environment.NewLine + "If there is no indication of something wrong, it may be that a line has a mistaken semicolon. Check both export_descr_character_traits and export_VnVs.";
                currentTraitNumber = n > 0 ? n - 1 : n - 1; //reduce n by 1 to account for last check of the traits.Count for loop
				refreshTrait(true);
                currentLevelNumber = l > 0 ? l - 1 : 0; //reduce l by 1 to account for last check of the newTrait.levels.Count for loop
				refreshLevel();
			}
			else
			{
				lblParseStatus.Text = "No problems found, reading last trait.";
				currentTraitNumber = traits.Count - 1;
				refreshTrait(true);
			}
		}

		private void button21_Click(object sender, System.EventArgs e)
		{
			refreshTrigger(true,currentTriggerNumber+1,true,true);
		}

		private void button22_Click(object sender, System.EventArgs e)
		{
			refreshTrigger(true,currentTriggerNumber-1,false,true);
		}

		private void button24_Click(object sender, System.EventArgs e)
		{
			// utility values
			Trigger newTrig = null;
			Trait tempTrait = null;

			// search for the next trigger tested under the current WhenToTest value

			// first, grab the current WhenToTest value
			string testing = lblWhenToTest.Text;

			if (testing != "")
			{
				// now find the next trigger matching this condition
				for (int n=currentTriggerNumber+1;n<triggers.Count;n++)
				{
					newTrig = (Trigger) triggers[n];

					if (newTrig.triggerEvent.eventName == testing)
					{
						// we found our match
						currentTriggerNumber = n;
						refreshTrigger(false,0,false,true);
						
						// now we need to refresh the trait info as well
						TriggerAffect newAff = (TriggerAffect) newTrig.affectsList[0];
						string traitToGrab = newAff.traitName;

						// loop and find this trait
						for (int x=0;x<traits.Count;x++)
						{
							tempTrait = (Trait) traits[x];
							if (tempTrait.name == traitToGrab)
							{
								// hurrah, we found it
								currentTraitNumber = x;
								refreshTrait(false);
								break;
							}
						}

						break;
					}
				}
			}
		}

		private void button25_Click(object sender, System.EventArgs e)
		{
			// utility values
			Trigger newTrig = null;
			Trait tempTrait = null;

			// search for the next trigger tested under the current WhenToTest value

			// first, grab the current WhenToTest value
			string testing = lblWhenToTest.Text;

			if (testing != "")
			{
				// now find the next trigger matching this condition
				for (int n=currentTriggerNumber-1;n>-1;n--)
				{
					newTrig = (Trigger) triggers[n];

					if (newTrig.triggerEvent.eventName == testing)
					{
						// we found our match
						currentTriggerNumber = n;
						refreshTrigger(false,0,false,true);
						
						// now we need to refresh the trait info as well
						TriggerAffect newAff = (TriggerAffect) newTrig.affectsList[0];
						string traitToGrab = newAff.traitName;

						// loop and find this trait
						for (int x=0;x<traits.Count;x++)
						{
							tempTrait = (Trait) traits[x];
							if (tempTrait.name == traitToGrab)
							{
								// hurrah, we found it
								currentTraitNumber = x;
								refreshTrait(false);
								break;
							}
						}

						break;
					}
				}
			}
		}

		private void button42_Click(object sender, System.EventArgs e)
		{
			// reroute this to the parse button action on the Traits tab page
			this.btnParseTraits_Click(sender,e);
		}

		private void btnParseAncillaries_Click(object sender, System.EventArgs e)
		{
			lblAncillaryStatus.Text = "Parsing file...";
			lblAncillaryStatus.Refresh();

			// read current path from interface
			string tempPath = txtAncillaryPath.Text;
			tempPath += "export_descr_ancillaries.txt";
			bool allOK = false;
			TextReader reader = null;

			Ancillary tempAncillary = new Ancillary();
			Effect tempEffect = new Effect();
			ancillaries = new ArrayList();
			ancillaryTriggers = new ArrayList();
			Trigger tempTrigger = new Trigger();
			TriggerAffect tempAffect = new TriggerAffect();
			ArrayList tempTrigCond = new ArrayList();
			ArrayList tempTrigAffects = new ArrayList();
			ArrayList tempTrigAcquireAncillaries = new ArrayList();
            ArrayList tempAL;
			
			bProcessingAncillaries = true;
			
			// open traits file for reading
			try{
				reader = new StreamReader(tempPath);
				allOK = true;	
			}catch{
				lblParseStatus.Text = "File not found: " + tempPath + "  Please correct the path and try again.";
			}//end try block

			if (allOK){
				// set global "did it" var
				parsedAncillaries = true;
			
				string newLine = "";

				// loop through and fill up all objects
				while ((newLine = reader.ReadLine()) != null)
				{
					// dump empty white space
					newLine.Trim();

					// check for the section swap
                    if (newLine.IndexOf("Trigger") > -1) //ANCILLARY TRIGGERS START HERE
						bProcessingAncillaries = false;

					// check for comment lines, skip processing if found
					if (newLine.IndexOf(";") < 0 && newLine != "")
					{
						// big switch for trait or trigger section
						if (bProcessingAncillaries)
						{
							// process each line individually
							if (newLine.IndexOf("Ancillary ") > -1)
							{
								// this is a new ancillary, pack the current ancillary into the ancillary array
								//	and start a new one
								if (tempAncillary.name != "")
									ancillaries.Add(tempAncillary);

								tempAncillary = new Ancillary();

								// grab name
								tempAL = ProcessStringArray(newLine,1,7);
								tempAncillary.name = tempAL[0].ToString();

								lblAncillaryStatus.Text = "Processing ancillary " + tempAncillary.name;
								lblAncillaryStatus.Refresh();
							}
							else if (newLine.IndexOf("ExcludedAncillaries") > -1)
							{
								// process this for character types it applies to
								tempAncillary.excludedAncillaries = ProcessStringArray(newLine,1,MAX_EXCLUDE_ANCILLARIES);
							}
							else if (newLine.IndexOf("ExcludeCultures") > -1)
							{
								// process excluded cultures, same as character processing
								tempAncillary.excludedCultures = ProcessStringArray(newLine,1,MAX_EXCLUDE_CULTURES);
							}
							else if (newLine.IndexOf("Unique") > -1)
							{
								// this trait is a hidden trait
								tempAncillary.isUniqueAncillary = true;
							}
							else if (newLine.IndexOf("EffectsDescription") > -1)
							{
								// pull out effectsdesc name
								tempAL = ProcessStringArray(newLine,1,5);
								tempAncillary.effectsDescriptionTag = tempAL[0].ToString();
							}
							else if (newLine.IndexOf("Description") > -1)
							{
								// pull out the description name
								tempAL = ProcessStringArray(newLine,1,5);
								tempAncillary.descriptionTag = tempAL[0].ToString();
							}
							else if (newLine.IndexOf("Image") > -1)
							{
								// pull out the image name
								tempAL = ProcessStringArray(newLine,1,5);
								tempAncillary.imageFilename = tempAL[0].ToString();

								// *******
								// removed validity check due to serious directory issues, Sept 2006
								// *******
							}
							else if (newLine.IndexOf("Effect") > -1)
							{
								// this is one of possibly many effects, validate and then store these in a temp array to be packed with the level later
						
								// first create new temp effect
                                bool affectFound = false;
								tempEffect = new Effect();

								// now parse line for effect info and pack it into the new object
								tempAL = ProcessStringArray(newLine,1,5);
								tempEffect.name = tempAL[0].ToString();

								//************************* validity checks **************************//
								// check effect name for validity
								for (int i=0; i<attributeTemplates.Count; i++){
									Attribute tempAtt = (Attribute) attributeTemplates[i];
									if (tempEffect.name == tempAtt.name){
										affectFound = true;
										break;
									}//end if (effectName == attributeName)
								}//end for all attributes

								if (!affectFound){
									tempEffect.name = "{" + tempEffect.name + "}";
								}//end if (!affectFound)

								// because a non-int value here is a definite no-no, we catch and break if there is a problem
                                tempEffect.level = Common.testParse(tempAL[1].ToString(), true);
								tempAncillary.effects.Add(tempEffect);
							}
						}
						else
						{
							if (newLine.IndexOf("Trigger") > -1)
							{
								// into the trigger section
								if (tempAncillary.name != "")
								{
									// pack this ancillary into the ancillaries array
									ancillaries.Add(tempAncillary);

									// and initialise so we don't get fooled into adding it again next time round
									tempAncillary = new Ancillary();
								}

								// Now start up the triggers.
								// Add the latest conditions and affects to the current trigger, since it
								//	has not been packed in yet.
								if (tempTrigger.name != "")
								{
									// copy conditions & affects & acquired ancillaries into trigger object, because pass-by-value is the only
									//		option for arraylists
									for (int n=0;n<tempTrigCond.Count;n++)
										tempTrigger.conditions.Add(tempTrigCond[n]);

									for (int n=0;n<tempTrigAffects.Count;n++)
										tempTrigger.affectsList.Add(tempTrigAffects[n]);

                                    for (int n = 0; n < tempTrigAcquireAncillaries.Count; n++)
                                        tempTrigger.acquireAncillaryList.Add(tempTrigAcquireAncillaries[n]);
                                    
                                    // clear the temp arrays
									tempTrigCond = new ArrayList();
									tempTrigAffects = new ArrayList();
									tempTrigAcquireAncillaries = new ArrayList();

									// pack this into the triggers list
                                    if ( tempTrigger.acquireAncillaryList.Count > 0 ) {
                                        ancillaryTriggers.Add(tempTrigger);
                                    }
									
									//if traits have been parsed && if there was an affected granted by the trigger, add the trigger to the list of trait triggers
									if( parsedTraits && tempTrigger.affectsList.Count > 0){
									    triggers.Add(tempTrigger);
									}
								}

								tempTrigger = new Trigger();

								// grab name
								tempAL = ProcessStringArray(newLine,1,5);
								tempTrigger.name = tempAL[0].ToString();

								lblAncillaryStatus.Text = "Processing trigger " + tempTrigger.name;
								lblAncillaryStatus.Refresh();
							}
							else if (newLine.IndexOf("WhenToTest") > -1)
							{
								tempAL = ProcessStringArray(newLine,1,5);
								tempTrigger.triggerEvent = getEvent(tempAL[0].ToString());
							}
							else if (newLine.IndexOf("Condition") > -1 || newLine.IndexOf(" and ") > -1)
							{
								int adjustedStart = 0;
								Condition tempCondition = new Condition();

								tempAL = ProcessStringArray(newLine,1,6);
								
								// put everything in the right bin, and make sure to shift for the "not" if there is one
								if (tempAL.Count > 1 && tempAL[adjustedStart].ToString() == "not")
								{
									// adjust param reading start to position 2, and set name & boolean value
									adjustedStart = 2;
									tempCondition.name = tempAL[1].ToString();
									tempCondition.boolValue = false;
								}
								else
								{
									// just set name & adjust start to 1
									tempCondition.name = tempAL[0].ToString();
									adjustedStart = 1;
								}

								// stuff param values into condition
								for (int x=adjustedStart;x<tempAL.Count;x++)
									tempCondition.parameters.Add(tempAL[x]);

								// and add this condition to the temp array so it can be added to the 
								tempTrigCond.Add(tempCondition);
							}							
							else if (newLine.IndexOf("AcquireAncillary") > -1)
							{
								tempAffect = new TriggerAffect();

								tempAL = ProcessStringArray(newLine,1,5);
								tempAffect.traitName = tempAL[0].ToString();

                                tempAffect.chance = Common.testParse(tempAL[2].ToString(), false);

                                tempTrigAcquireAncillaries.Add(tempAffect);
                            }
                            else if (newLine.IndexOf("Affects") > -1)
                            {
                                tempAffect = new TriggerAffect();

                                tempAL = ProcessStringArray(newLine, 1, 5);
                                tempAffect.traitName = tempAL[0].ToString();

                                tempAffect.traitThreshhold = Common.testParse(tempAL[1].ToString(), true);

                                tempAffect.chance = Common.testParse(tempAL[3].ToString(), false);

                                tempTrigAffects.Add(tempAffect);
                            }//end if trigger
						}
					}
				}

				// close the reader
				reader.Close();

                // Add the latest conditions and affects to the current trigger, since it
                //	has not been packed in yet.
                if (tempTrigger.name != ""){
                    // copy conditions & affects into trigger object, because pass-by-value is the only
                    //		option for arraylists
                    for (int n = 0; n < tempTrigCond.Count; n++)
                        tempTrigger.conditions.Add(tempTrigCond[n]);

                    for (int n = 0; n < tempTrigAffects.Count; n++)
                        tempTrigger.affectsList.Add(tempTrigAffects[n]);

                    for (int n = 0; n < tempTrigAcquireAncillaries.Count; n++)
                        tempTrigger.acquireAncillaryList.Add(tempTrigAcquireAncillaries[n]);                        

                    // clear the temp arrays
                    tempTrigCond = new ArrayList();
                    tempTrigAffects = new ArrayList();
                    tempTrigAcquireAncillaries = new ArrayList();

                    // pack this into the ancillary triggers list if at least one ancillary is acquired
                    if ( tempTrigger.acquireAncillaryList.Count > 0 ) {
                        ancillaryTriggers.Add(tempTrigger);
                    }

                    //if traits have been parsed && if there was an affected granted by the trigger, add the trigger to the list of trait triggers
                    if ( parsedTraits && tempTrigger.affectsList.Count > 0 ) {
                        triggers.Add(tempTrigger);
                    }
                }

				
				lblAncillaryStatus.Text = "Finished, all ancillaries and ancillary triggers read successfully.";

				// make the find-problem and find-problem-trigger buttons live
				btnNextProblemAncillary.Enabled = true;
				btnFindProblemTriggerAncillary.Enabled = true;

				// init the helper vars
				currentAncillaryNumber = 0; 
				currentAncillaryTriggerNumber = 0;

				// we've got everything read, now initialise the first readout
				refreshAncillary(true);
			}
		}

		private void btnFindProblemTriggerAncillary_Click(object sender, System.EventArgs e)
		{
			checkAncillaryTriggers();
		}

		private void btnNextProblemAncillary_Click(object sender, System.EventArgs e)
		{
            bool problem = false;
            Ancillary newAncillary = null;
            Effect newEffect = null;
            string temp = "";
            int n = 0;	// very important n, this will set currentTraitNumber at the end if a problem is found

            // reset current number if it's bad
			if (currentAncillaryNumber < 0)
				currentAncillaryNumber = 0;
			else if (currentAncillaryNumber > ancillaries.Count-1)
				currentAncillaryNumber = ancillaries.Count-1;

			// inform the user we're starting
			lblAncillaryStatus.Text = "Scanning data for problem areas...";
			lblAncillaryStatus.Refresh();

			// start looping from the current ancillary
			for (n=currentAncillaryNumber;n<ancillaries.Count;n++)
			{
				// reset problem trigger
				problem = false;

				// pull out trait
				newAncillary = (Ancillary) ancillaries[n];

				// inform the user what we're doing
				lblAncillaryStatus.Text = "Scanning data for problem areas... checking ancillary " + newAncillary.name;
				lblAncillaryStatus.Refresh();

				// run it through the trait checker
				newAncillary = checkAncillary(newAncillary);

				// check every single-value property of the trait first...
				if (newAncillary.name.IndexOf("{") > -1){
					problem = true;
					break;
				}

				// check multi-value properties
				// excluded ancillaries
                for ( int i = 0; !problem && i < newAncillary.excludedAncillaries.Count; i++ ){
					temp = newAncillary.excludedAncillaries[i].ToString();
					if (temp.IndexOf("{") > -1){
						problem = true;
						break;
					}
				}

				// excluded cultures
                for ( int i = 0; !problem && i < newAncillary.excludedCultures.Count; i++ ){
					temp = newAncillary.excludedCultures[i].ToString();
					if (temp.IndexOf("{") > -1){
						problem = true;
						break;
					}
				}
			
				if (newAncillary.effectsDescriptionTag.IndexOf("{") > -1){
					problem = true;
					break;
				}

				if (newAncillary.descriptionTag.IndexOf("{") > -1){
					problem = true;
					break;
				}

                // check effects
                for ( int i = 0; !problem && i < newAncillary.effects.Count; i += 1 ) {
                    newEffect = ( Effect )newAncillary.effects[i];
                    if ( newEffect.name.IndexOf("{") > -1 || newEffect.level.IndexOf("{") > -1 ) {
                        problem = true;
                        break;
                    }//end if (temp.indexOf("{") > -1)
                }//end for i < newLevel.effects.Count

				if (problem)
					break;
			}

			if (problem)
			{
				// stop here and display
				lblAncillaryStatus.Text = lblAncillaryStatus.Text + System.Environment.NewLine + System.Environment.NewLine + "Problem found, halting. To continue, correct the problem and re-parse with the Parse Ancillaries button, or skip to the next ancillary and press the Next Problem button." + System.Environment.NewLine + System.Environment.NewLine + "If there is no indication of something wrong, it may be that a line has a mistaken semicolon. Check both export_descr_ancillaries.txt and export_ancillaries.txt.";
				currentAncillaryNumber = n;
				refreshAncillary(true);
			}
			else
			{
				lblAncillaryStatus.Text = "No problems found, reading last trait.";
				currentAncillaryNumber = ancillaries.Count - 1;
				refreshAncillary(true);
			}
		}

		private void button37_Click(object sender, System.EventArgs e)
		{
			currentAncillaryNumber++;

			if (currentAncillaryNumber > ancillaries.Count - 1)
				currentAncillaryNumber--;
			else
				refreshAncillary(true);
		}

		private void button31_Click(object sender, System.EventArgs e)
		{
			currentAncillaryNumber--;

			if (currentAncillaryNumber < 0)
				currentAncillaryNumber++;
			else
				refreshAncillary(true);
		}

		private void button34_Click(object sender, System.EventArgs e)
		{
			currentAncillaryNumber += 5;

			if (currentAncillaryNumber > ancillaries.Count - 1)
				currentAncillaryNumber -= 5;
			else
				refreshAncillary(true);
		}

		private void button30_Click(object sender, System.EventArgs e)
		{
			currentAncillaryNumber -= 5;

			if (currentAncillaryNumber < 0)
				currentAncillaryNumber += 5;
			else
				refreshAncillary(true);
		}

		private void button28_Click(object sender, System.EventArgs e)
		{
			currentAncillaryNumber = 0;
			refreshAncillary(true);
		}

		private void button32_Click(object sender, System.EventArgs e)
		{
			currentAncillaryNumber = ancillaries.Count - 1;
			refreshAncillary(true);
		}

		private void button48_Click(object sender, System.EventArgs e)
		{
			button37_Click(sender,e);
		}

		private void button44_Click(object sender, System.EventArgs e)
		{
			button31_Click(sender,e);
		}

		private void button47_Click(object sender, System.EventArgs e)
		{
			button34_Click(sender,e);
		}

		private void button43_Click(object sender, System.EventArgs e)
		{
			button30_Click(sender,e);
		}

		private void button45_Click(object sender, System.EventArgs e)
		{
			button32_Click(sender,e);
		}

		private void button41_Click(object sender, System.EventArgs e)
		{
			button28_Click(sender,e);
		}

		private void button39_Click(object sender, System.EventArgs e)
		{
			refreshAncillaryTrigger(true,currentAncillaryTriggerNumber+1,true,true);
		}

		private void button38_Click(object sender, System.EventArgs e)
		{
			refreshAncillaryTrigger(true,currentAncillaryTriggerNumber-1,true,true);
		}

		private void button33_Click(object sender, System.EventArgs e)
		{
			// utility values
			Trigger newTrig = null;
			Ancillary tempAncillary = null;
			bool foundMatch = false;

			// clear status readout
			lblAncillaryTriggerStatus.Text = "";

			// search for the next trigger tested under the current WhenToTest value

			// first, grab the current WhenToTest value
			string testing = lblAncillaryWhenTested.Text;

			if (testing != "")
			{
				// now find the next trigger matching this condition
				for (int n=currentAncillaryTriggerNumber+1;n<ancillaryTriggers.Count;n++)
				{
					newTrig = (Trigger) ancillaryTriggers[n];

					if (newTrig.triggerEvent.eventName == testing)
					{
						// we found our match
						currentAncillaryTriggerNumber = n;
						refreshAncillaryTrigger(false,0,false,true);
						
						// now we need to refresh the ancillary info as well
						TriggerAffect newAff = (TriggerAffect) newTrig.acquireAncillaryList[0];
						string ancillaryToGrab = newAff.traitName;

						// loop and find this ancillary
						for (int x=0;x<ancillaries.Count;x++)
						{
							tempAncillary = (Ancillary) ancillaries[x];
							if (tempAncillary.name == ancillaryToGrab)
							{
								// hurrah, we found it
								currentAncillaryNumber = x;
								refreshAncillary(false);
								break;
							}
						}

						foundMatch = true;
						break;
					}
				}

				// if we got here and didn't find anything, inform the user
				if (!foundMatch)
					lblAncillaryTriggerStatus.Text = "No further trigger found which is evaluated at this time.";
			}
		}

		private void button29_Click(object sender, System.EventArgs e)
		{
			// utility values
			Trigger newTrig = null;
			Ancillary tempAncillary = null;
			bool foundMatch = false;

			// clear status readout
			lblAncillaryTriggerStatus.Text = "";

			// search for the previous trigger tested under the current WhenToTest value

			// first, grab the current WhenToTest value
			string testing = lblAncillaryWhenTested.Text;

			if (testing != "")
			{
				// now find the next trigger matching this condition
				for (int n=currentAncillaryTriggerNumber-1;n>-1;n--)
				{
					newTrig = (Trigger) ancillaryTriggers[n];

					if (newTrig.triggerEvent.eventName == testing)
					{
						// we found our match
						currentAncillaryTriggerNumber = n;
						refreshAncillaryTrigger(false,0,false,true);

						// now we need to refresh the ancillary info as well
						TriggerAffect newAff = (TriggerAffect) newTrig.acquireAncillaryList[0];
						string ancillaryToGrab = newAff.traitName;

						// loop and find this ancillary
						for (int x=0;x<ancillaries.Count;x++)
						{
							tempAncillary = (Ancillary) ancillaries[x];
							if (tempAncillary.name == ancillaryToGrab)
							{
								// hurrah, we found it
								currentAncillaryNumber = x;
								refreshAncillary(false);
								break;
							}
						}

						foundMatch = true;
						break;
					}
				}

				// if we got here and we didn't find one, inform the user
				if (!foundMatch)
					lblAncillaryTriggerStatus.Text = "No previous trigger found which is evaluated at this time.";
			}
		}

		private void btnFindProblemTriggerTrait_Click(object sender, System.EventArgs e)
		{
			checkTraitTriggers();
		}

		private void generateConditions()
		{
			// catalogue attributes, from 
			addAttribute("Ambush","int");
			addAttribute("Attack","int");
			addAttribute("BattleSurgery","percent");
			addAttribute("BodyguardValour","int");
			addAttribute("BribeResistance","percent");
			addAttribute("CavalryCommand","int");
			addAttribute("Combat_V_Barbarian","int");
			addAttribute("Combat_V_Carthaginian","int");
			addAttribute("Combat_V_Eastern","int");
			addAttribute("Combat_V_Egyptian","int");
			addAttribute("Combat_V_Greek","int");
			addAttribute("Combat_V_Roman","int");
			addAttribute("Combat_V_Slave","int");
			addAttribute("Combat_V_Nomad","int");
			addAttribute("Combat_V_Hun","int");
			addAttribute("Command","int");
			addAttribute("Construction","int");
			addAttribute("Defence","int");
			addAttribute("Electability","int");
			addAttribute("Farming","int");
			addAttribute("Fertility","int");
			addAttribute("GrainTrading","int");
			addAttribute("Health","int");
			addAttribute("HitPoints","int");
			addAttribute("InfantryCommand","int");
			addAttribute("Influence","int");
			addAttribute("Law","int");
			addAttribute("LineOfSight","int");
			addAttribute("Looting","percent");
			addAttribute("Management","int");
			addAttribute("Mining","percent");
			addAttribute("MovementPoints","percent");
			addAttribute("NavalCommand","int");
			addAttribute("Negotiation","int");
			addAttribute("NightBattle","int");
			addAttribute("PersonalSecurity","int");
			addAttribute("PublicSecurity","int");
			addAttribute("SiegeAttack","int");
			addAttribute("SiegeDefence","int");
			addAttribute("SiegeEngineering","int");
			addAttribute("SlaveTrading","int");
			addAttribute("Squalor","int");
			addAttribute("TaxCollection","percent");
			addAttribute("Trading","percent");
			addAttribute("TrainingAgents","percent");
			addAttribute("TrainingAnimalUnits","percent");
			addAttribute("TrainingUnits","percent");
			addAttribute("TroopMorale","int");
			addAttribute("Unrest","int");
			addAttribute("PopularStanding","percent");
			addAttribute("SenateStanding","percent");
			addAttribute("LocalPopularity","int");
			addAttribute("Loyalty","int");
			addAttribute("Bribery", "int");
			addAttribute("Subterfuge", "int");
			addAttribute("christianity", "int");
			addAttribute("pagan","int");
			addAttribute("zoroastrian","int");

			// catalogue conditions and associated syntax, from JeromeGrasdyke's posting about Sept 10, 2005
			addCondition("I_InBattle");
			addCondition("WonBattle");
            addCondition("I_WonBattle", "faction type");
			addCondition("Routs");
			addCondition("Ally_Routs");
            addCondition("GeneralHPLostRatioinBattle", "logic token", "test value");
            addCondition("GeneralNumKillsInBattle", "logic token", "test value");
			addCondition("GeneralFoughtInCombat");
            addCondition("PercentageOfArmyKilled", "logic token", "test value");
            addCondition("I_PercentageOfArmyKilled", "alliance index", "army index", "logic token", "percentage");
            addCondition("PercentageEnemyKilled", "logic token", "test value");
            addCondition("PercentageBodyguardKilled", "logic token", "test value");
            addCondition("PercentageRoutedOffField", "logic token", "test value");
            addCondition("NumKilledGenerals", "logic token", "test value");
            addCondition("PercentageUnitCategory", "unit category", "logic token", "test value");
            addCondition("NumFriendsInBattle", "logic token", "test value");
            addCondition("NumEnemiesInBattle", "logic token", "test value");
            addCondition("GeneralFoughtFaction", "faction type");
            addCondition("GeneralFoughtCulture", "culture type");
            addCondition("I_ConflictType", "conflict type");
			addCondition("IsNightBattle");
            addCondition("BattleSuccess", "logic token", "success type");
            addCondition("BattleOdds", "logic token", "test value");
            addCondition("WasAttacker");
            addCondition("I_BattleAiAttacking");
			addCondition("I_BattleAiAttackingSettlement");
			addCondition("I_BattleAiDefendingSettlement");
			addCondition("I_BattleAiDefendingHill");
			addCondition("I_BattleAiDefendingCrossing");
			addCondition("I_BattleAiScouting");
			addCondition("I_BattleIsRiverBattle");
			addCondition("I_BattleIsSiegeBattle");
			addCondition("I_BattleIsSallyOutBattle");
			addCondition("I_BattleIsFortBattle");
            addCondition("I_BattleAttackerNumSiegeEngines", "seige engine class", "logic token", "test value");
            addCondition("I_BattleAttackerNumArtilleryCanPenetrateWalls", "logic token", "test value");
            addCondition("I_BattleDefenderNumNonMissileUnitsOnWalls", "logic token", "test value");
            addCondition("I_BattleDefenderNumMissileUnitsOnWalls", "logic token", "test value");
			addCondition("I_BattleSettlementWallsBreached");
			addCondition("I_BattleSettlementGateDestroyed");
            addCondition("I_BattleSettlementTowerDefence", "tower defence type");
            addCondition("I_BattleSettlementGateDefence", "gate defence type");
            addCondition("I_BattleSettlementFortificationLevel", "logic token", "test value");
            addCondition("BattleBuildingType", "building type", "logic token", "test value");
            addCondition("I_BattleSettlementGateStrength", "gate strength", "logic token", "test value");
            addCondition("I_BattleNumberOfRiverCrossings", "logic token", "test value");
			addCondition("BattlePlayerUnitClass","logic token","unit class");
            addCondition("BattleEnemyUnitClass", "logic token", "unit class");
            addCondition("BattlePlayerUnitCategory", "logic token", "unit category");
            addCondition("BattleEnemyUnitCategory", "logic token", "unit category");
            addCondition("BattlePlayerUnitSiegeEngineClass", "logic token", "seige engine category");
            addCondition("BattleEnemyUnitSiegeEngineClass", "logic token", "seige engine category");
            addCondition("BattlePlayerUnitOnWalls");
            addCondition("BattleEnemyUnitOnWalls");
            addCondition("BattlePlayerCurrentFormation", "logic token", "formation");
            addCondition("BattleEnemyCurrentFormation", "logic token", "formation");
            addCondition("BattlePlayerUnitCloseFormation");
            addCondition("BattleEnemyUnitCloseFormation");
            addCondition("BattlePlayerUnitSpecialAbilitySupported", "logic token", "special ability");
            addCondition("BattleEnemyUnitSpecialAbilitySupported", "logic token", "special ability");
            addCondition("BattlePlayerUnitSpecialAbilityActive");
            addCondition("BattleEnemyUnitSpecialAbilityActive");
            addCondition("BattlePlayerMountClass", "logic token", "mount class");
            addCondition("BattleEnemyMountClass", "logic token", "mount class");
            addCondition("BattlePlayerUnitMeleeStrength", "logic token", "test value");
            addCondition("BattleEnemyUnitMeleeStrength", "logic token", "test value");
            addCondition("BattlePlayerUnitMissileStrength", "logic token", "test value");
            addCondition("BattleEnemyUnitMissileStrength", "logic token", "test value");
            addCondition("BattlePlayerUnitSpecialFormation", "logic token", "formation");
            addCondition("BattleEnemyUnitSpecialFormation", "logic token", "formation");
            addCondition("BattlePlayerUnitEngaged");
            addCondition("BattleEnemyUnitEngaged");
            addCondition("BattlePlayerActionStatus", "logic token", "action status");
            addCondition("BattleEnemyActionStatus", "logic token", "action status");
            addCondition("BattlePlayerUnitMovingFast");
			addCondition("BattleEnemyUnitMovingFast");
            addCondition("BattleRangeOfAttack", "logic token", "test value");
            addCondition("BattleDirectionOfAttack", "logic token", "test value");
            addCondition("BattleIsMeleeAttack");
            addCondition("I_BattlePlayerArmyPercentageOfUnitClass", "unit class", "logic token", "percentage");
            addCondition("I_BattleEnemyArmyPercentageOfUnitClass", "unit class", "logic token", "percentage");
            addCondition("I_BattlePlayerArmyPercentageOfUnitCategory", "unit category", "logic token", "percentage");
            addCondition("I_BattleEnemyArmyPercentageOfUnitCategory", "unit category", "logic token", "percentage");
            addCondition("I_BattlePlayerArmyPercentageOfMountClass", "mount class", "logic token", "percentage");
            addCondition("I_BattleEnemyArmyPercentageOfMountClass", "mount class", "logic token", "percentage");
            addCondition("I_BattlePlayerArmyPercentageOfClassAndCategory", "unit class", "unit category", "logic token", "percentage");
            addCondition("I_BattleEnemyArmyPercentageOfClassAndCategory", "unit class", "unit category", "logic token", "percentage");
            addCondition("I_BattlePlayerArmyPercentageOfSpecialAbility", "special ability", "logic token", "percentage");
            addCondition("I_BattleEnemyArmyPercentageOfSpecialAbility", "special ability", "logic token", "percentage");
            addCondition("I_BattlePlayerArmyPercentageCanHide", "logic token", "percentage");
            addCondition("I_BattleEnemyArmyPercentageCanHide", "logic token", "percentage");
            addCondition("I_BattlePlayerArmyPercentageCanSwim", "logic token", "percentage");
            addCondition("I_BattleEnemyArmyPercentageCanSwim", "logic token", "percentage");
            addCondition("I_BattlePlayerArmyIsAttacker");
            addCondition("I_BattlePlayerAllianceOddsInFavour", "logic token", "test value");
            addCondition("I_BattlePlayerAllianceOddsAgainst", "logic token", "test value");
            addCondition("TotalSiegeWeapons", "logic token", "test value");
            addCondition("I_BattleStarted");
            addCondition("I_IsUnitMoveFastSet", "unit label");
            addCondition("I_IsUnitMoving", "unit label");
            addCondition("I_IsUnitIdle", "unit label");
            addCondition("I_IsUnitRouting", "unit label");
            addCondition("I_IsUnitUnderFire", "unit label");
            addCondition("I_IsUnitEngaged", "unit label");
            addCondition("I_IsUnitEngagedWithUnit", "unit label", "unit label");
            addCondition("I_UnitFormation", "unit label", "logic token", "formation type");
            addCondition("I_PercentageUnitKilled", "unit label", "logic token", "percentage");
            addCondition("I_UnitPercentageAmmoLeft", "unit label", "logic token", "percentage");
            addCondition("I_UnitDistanceFromPosition", "unit label", "position", "position", "logic token", "test value");
            addCondition("I_UnitDistanceFromLine", "unit label", "location", "location", "logic token", "test value");
            addCondition("I_UnitDistanceFromUnit", "unit label", "unit label", "logic token", "test value");
            addCondition("I_UnitInRangeOfUnit", "unit label", "unit label");
            addCondition("I_UnitDestroyed", "unit label");
            addCondition("I_UnitEnemyUnitInRadius", "unit label", "test value");
            addCondition("I_IsUnitGroupMoving", "group label");
            addCondition("I_IsUnitGroupEngaged", "group label");
            addCondition("I_IsUnitGroupIdle", "group label");
            addCondition("I_IsUnitGroupDestroyed", "group label");
            addCondition("I_PercentageUnitGroupKilled", "group label", "logic token", "percentage");
            addCondition("I_UnitGroupFormation", "group label", "logic token", "formation");
            addCondition("I_UnitGroupDistanceFromPosition", "group label", "position", "logic token", "distance");
            addCondition("I_UnitGroupDistanceFromGroup", "group label", "group label", "logic token", "distance");
            addCondition("I_UnitGroupInRangeOfUnit", "group label", "unit label");
            addCondition("I_UnitInRangeOfUnitGroup", "unit label", "group label");
            addCondition("I_UnitGroupInRangeOfUnitGroup", "group label", "group label");
            addCondition("I_PlayerInRangeOfUnitGroup", "group label");
            addCondition("I_PlayerInRangeOfUnit", "unit label");
            addCondition("I_UnitTypeSelected", "unit type");
            addCondition("UnitType", "unit type");
            addCondition("I_UnitSelected", "unit label");
            addCondition("I_MultipleUnitsSelected");
            addCondition("I_SpecificUnitsSelected", "unit label", "optional unit label", "optional unit label", "optional unit label", "optional unit label");
            addCondition("I_IsCameraZoomingToUnit", "unit label");
            addCondition("I_BattleEnemyArmyPercentageOfMatchingUnits", "unit match type", "logic token", "test value");
            addCondition("I_BattleEnemyArmyNumberOfMatchingUnits", "unit match type", "logic token", "test value");
            addCondition("I_BattlePlayerArmyPercentageOfMatchingUnits", "unit match type", "logic token", "test value");
            addCondition("I_BattlePlayerArmyNumberOfMatchingUnits", "unit match type", "logic token", "test value");
            addCondition("Trait", "trait name", "logic token", "level");
            addCondition("FatherTrait", "trait name", "logic token", "level");
            addCondition("FactionLeaderTrait", "trait name", "logic token", "level");
            addCondition("Attribute", "attribute name", "logic token", "level");
            addCondition("RemainingMPPercentage", "logic token", "test value");
            addCondition("I_RemainingMPPercentage", "character name", "logic token", "test value");
            addCondition("I_CharacterCanMove", "character name");
            addCondition("NoActionThisTurn");
            addCondition("AgentType", "logic token", "character type");
            addCondition("TrainedAgentType", "logic token", "character type");
            addCondition("DisasterType", "disaster type");
            addCondition("CultureType", "culture type");
            addCondition("OriginalFactionType", "original faction type");
            addCondition("OriginalCultureType", "original culture type");
            addCondition("IsGeneral");
            addCondition("IsAdmiral");
            addCondition("EndedInSettlement");
            addCondition("IsFactionLeader");
            addCondition("IsFactionHeir");
            addCondition("IsMarried");
            addCondition("AtSea");
            addCondition("InEnemyLands");
            addCondition("InBarbarianLands");
            addCondition("InUncivilisedLands");
            addCondition("IsBesieging");
            addCondition("IsUnderSiege");
            addCondition("I_WithdrawsBeforeBattle");
            addCondition("EndedInEnemyZOC");
            addCondition("AdviseAction", "logic token", "action");
            addCondition("I_CharacterTypeNearCharacterType", "faction", "character type", "distance", "faction", "character type");
            addCondition("I_CharacterTypeNearTile", "faction", "character type", "distance", "test value", "test value");
            addCondition("FactionType", "faction type");
            addCondition("TargetFactionType", "faction type");
            addCondition("FactionCultureType", "culture type");
            addCondition("TargetFactionCultureType", "culture type");
            addCondition("TrainedUnitCategory", "unit category");
            addCondition("UnitCategory", "logic token", "unit category");
            addCondition("SenateMissionTimeRemaining", "logic token", "test value");
            addCondition("MedianTaxLevel", "logic token", "tax level");
            addCondition("ModeTaxLevel", "logic token", "tax level");
            addCondition("I_ModeTaxLevel", "faction", "logic token", "tax level");
            addCondition("MissionSuccessLevel", "logic token", "success level");
            addCondition("MissionSucceeded");
            addCondition("MissionFactionTargetType", "faction type");
            addCondition("MissionCultureTargetType", "culture type");
            addCondition("DiplomaticStanceFromCharacter", "faction type", "logic token", "stance");
            addCondition("DiplomaticStanceFromFaction", "faction type", "logic token", "stance");
            addCondition("FactionHasAllies");
            addCondition("LosingMoney");
            addCondition("I_LosingMoney", "faction type");
            addCondition("SupportCostsPercentage", "logic token", "test value");
            addCondition("Treasury", "logic token", "test value");
            addCondition("OnAWarFooting");
            addCondition("I_FactionBesieging", "faction type");
            addCondition("I_FactionBesieged", "faction type");
            addCondition("I_NumberOfSettlements", "faction type", "logic token", "test value");
            addCondition("I_NumberOfHeirs", "faction type", "logic token", "test value");
            addCondition("I_FactionNearTile", "faction type", "distance", "test value", "test value");
            addCondition("SettlementsTaken", "logic token", "test value");
            addCondition("BattlesFought", "logic token", "test value");
            addCondition("BattlesWon", "logic token", "test value");
            addCondition("BattlesLost", "logic token", "test value");
            addCondition("DefensiveSiegesFought", "logic token", "test value");
            addCondition("DefensiveSiegesWon", "logic token", "test value");
            addCondition("OffensiveSiegesFought", "logic token", "test value");
            addCondition("OffensiveSiegesWon", "logic token", "test value");
            addCondition("FactionwideAncillaryExists", "ancillary name");
            addCondition("RandomPercent", "logic token", "test value");
            addCondition("TrueCondition");
            addCondition("WorldwideAncillaryExists", "ancillary name");
            addCondition("SettlementName", "settlement name");
            addCondition("GovernorBuildingExists", "logic token", "building level");
            addCondition("SettlementBuildingExists", "logic token", "building level");
            addCondition("BuildingFinishedByGovernor", "logic token", "building level");
            addCondition("SettlementBuildingFinished", "logic token", "building level");
            addCondition("GovernorPlugInExists", "logic token", "building level");
            addCondition("GovernorPlugInFinished", "logic token", "building level");
            addCondition("GovernorTaxLevel", "logic token", "tax level");
            addCondition("SettlementTaxLevel", "logic token", "tax level");
            addCondition("GovernorInResidence");
            addCondition("GovernorLoyaltyLevel", "logic token", "loyalty level");
            addCondition("SettlementLoyaltyLevel", "logic token", "loyalty level");
            addCondition("RiotRisk", "logic token", "test value");
            addCondition("BuildingQueueIdleDespiteCash");
            addCondition("TrainingQueueIdleDespiteCash");
            addCondition("I_SettlementExists", "settlement name");
            addCondition("I_SettlementOwner", "settlement name", "logic token", "faction type");
            addCondition("AdviseFinancialBuild", "logic token", "build type");
            addCondition("AdviseBuild", "logic token", "build type");
            addCondition("AdviseRecruit", "logic token", "unit type");
            addCondition("SettlementPopulationMaxedOut");
            addCondition("SettlementPopulationTooLow");
            addCondition("SettlementAutoManaged", "logic token", "automanage type");
            addCondition("PercentageOfPopulationInGarrison", "logic token", "test value");
            addCondition("GarrisonToPopulationRatio", "logic token", "test value");
            addCondition("HealthPercentage", "logic token", "test value");
            addCondition("SettlementHasPlague");
            addCondition("IsFortGarrisoned");
            addCondition("IsSettlementGarrisoned");
            addCondition("IsSettlementRioting");
            addCondition("I_NumberUnitsInSettlement", "settlement name", "unit type", "logic token", "test value");
            addCondition("CharacterIsLocal");
            addCondition("TargetCharacterIsLocal");
            addCondition("SettlementIsLocal");
            addCondition("TargetSettlementIsLocal");
            addCondition("RegionIsLocal");
            addCondition("TargetRegionIsLocal");
            addCondition("ArmyIsLocal");
            addCondition("TargetArmyIsLocal");
            addCondition("FactionIsLocal");
            addCondition("I_LocalFaction", "faction type");
            addCondition("TargetFactionIsLocal");
            addCondition("I_TurnNumber", "logic token", "test value");
            addCondition("I_MapName", "file path");
            addCondition("I_ThreadCount", "test name", "logic token", "test value");
            addCondition("I_IsTriggerTrue", "test name");
            addCondition("IncomingMessageType", "message name");
            addCondition("I_AdvisorVerbosity", "logic token", "test value");
            addCondition("I_AdvisorVisible");
            addCondition("I_CharacterSelected", "character name");
            addCondition("I_AgentSelected", "agent type");
            addCondition("I_SettlementSelected", "settlement name");
            addCondition("ShortcutTriggered", "element name", "function name");
            addCondition("I_AdvancedStatsScrollIsOpen");
            addCondition("ButtonPressed", "button name");
            addCondition("ScrollOpened", "scroll name");
            addCondition("ScrollClosed", "scroll name");
            addCondition("ScrollAdviceRequested", "scroll name");
            addCondition("I_CompareCounter", "script name", "logic token", "test value");
            addCondition("I_TimerElapsed", "timer name", "logic token", "test value");
            addCondition("I_SoundPlaying", "sound name");
            addCondition("I_AdvisorSpeechPlaying");
            addCondition("IsBesieging");
            addCondition("IsUnderSeige");

            //catalog events from the docudemon files
            eventTemplates.Add(new Event("PreBattle"));
            eventTemplates.Add(new Event("PreBattleWithdrawl"));
            eventTemplates.Add(new Event("PostBattle"));
            eventTemplates.Add(new Event("HireMercenaries"));
            eventTemplates.Add(new Event("GeneralCaptureResidence"));
            eventTemplates.Add(new Event("GeneralCaptureWonder"));
            eventTemplates.Add(new Event("GeneralCaptureSettlement"));
            eventTemplates.Add(new Event("LeaderDestroyedFaction"));
            eventTemplates.Add(new Event("CharacterDamagedByDisaster"));
            eventTemplates.Add(new Event("GeneralAssaultResidence"));
            eventTemplates.Add(new Event("OfferedForAdoption"));
            eventTemplates.Add(new Event("LesserGeneralOfferedForAdoption"));
            eventTemplates.Add(new Event("OfferedForMarriage"));
            eventTemplates.Add(new Event("BrotherAdopted"));
            eventTemplates.Add(new Event("BecomesFactionLeader"));
            eventTemplates.Add(new Event("BecomesFactionHeir"));
            eventTemplates.Add(new Event("BecomeQuaestor"));
            eventTemplates.Add(new Event("BecomeAedile"));
            eventTemplates.Add(new Event("BecomePraetor"));
            eventTemplates.Add(new Event("BecomesConsul"));
            eventTemplates.Add(new Event("BecomeCensor"));
            eventTemplates.Add(new Event("BecomePontifexMaximus"));
            eventTemplates.Add(new Event("CeasedFactionLeader"));
            eventTemplates.Add(new Event("CeasedFactionHeir"));
            eventTemplates.Add(new Event("CeasedQuaestor"));
            eventTemplates.Add(new Event("CeasedAedile"));
            eventTemplates.Add(new Event("CeasedPraetor"));
            eventTemplates.Add(new Event("CeasedConsul"));
            eventTemplates.Add(new Event("CeasedCensor"));
            eventTemplates.Add(new Event("CeasedPontifexMaximus"));
            eventTemplates.Add(new Event("LostLegionaryEagle"));
            eventTemplates.Add(new Event("CapturedLegionaryEagle"));
            eventTemplates.Add(new Event("RecapturedLegionaryEagle"));
            eventTemplates.Add(new Event("SenateExposure"));
            eventTemplates.Add(new Event("QuaestorInvestigationMinor"));
            eventTemplates.Add(new Event("QuaestorInvestigation"));
            eventTemplates.Add(new Event("QuaestorInvestigationMajor"));
            eventTemplates.Add(new Event("Birth"));
            eventTemplates.Add(new Event("CharacterComesOfAge"));
            eventTemplates.Add(new Event("CharacterMarries"));
            eventTemplates.Add(new Event("CharacterBecomesAFather"));
            eventTemplates.Add(new Event("CharacterTurnStart"));
            eventTemplates.Add(new Event("CharacterTurnEnd"));
            eventTemplates.Add(new Event("CharacterTurnEndInSettlement"));
            eventTemplates.Add(new Event("GeneralDevestatesTile"));
            eventTemplates.Add(new Event("SpyMission"));
            eventTemplates.Add(new Event("ExecutesASpyOnAMission"));
            eventTemplates.Add(new Event("LeaderOrderedSpyingMission"));
            eventTemplates.Add(new Event("AssassinationMission"));
            eventTemplates.Add(new Event("ExecutesAnAssassinOnAMission"));
            eventTemplates.Add(new Event("LeaderOrderedAssassinationMission"));
            eventTemplates.Add(new Event("SufferAssassinationAttempt"));
            eventTemplates.Add(new Event("SabotageMission"));
            eventTemplates.Add(new Event("LeaderOrderedSabotage"));
            eventTemplates.Add(new Event("BriberyMission"));
            eventTemplates.Add(new Event("LeaderOrderedBribery"));
            eventTemplates.Add(new Event("AcceptBribe"));
            eventTemplates.Add(new Event("RefuseBribe"));
            eventTemplates.Add(new Event("Insurrection"));
            eventTemplates.Add(new Event("DiplomacyMission"));
            eventTemplates.Add(new Event("LeaderOrderedDiplomacy"));
            eventTemplates.Add(new Event("LeaderSenateMissionSuccess"));
            eventTemplates.Add(new Event("LeaderSenateMissionFailure"));
            eventTemplates.Add(new Event("NewAdmiralCreated"));
            eventTemplates.Add(new Event("GovernorUnitTrained"));
            eventTemplates.Add(new Event("GovernorBuildingCompleted"));
            eventTemplates.Add(new Event("GovernorPlugInCompleted"));
            eventTemplates.Add(new Event("AgentCreated"));
            eventTemplates.Add(new Event("GovernorAgentCreated"));
            eventTemplates.Add(new Event("GovernorBuildingDestroyed"));
            eventTemplates.Add(new Event("GovernorCityRiots"));
            eventTemplates.Add(new Event("GovernorCityRebels"));
            eventTemplates.Add(new Event("GovernorThrowGames"));
            eventTemplates.Add(new Event("GovernorThrowRaces"));
            eventTemplates.Add(new Event("EnslavePopulation"));
            eventTemplates.Add(new Event("ExterminatePopulation"));
            eventTemplates.Add(new Event("CharacterSelected"));
            eventTemplates.Add(new Event("MultiTurnMove"));
            eventTemplates.Add(new Event("CharacterPanelOpen"));



		}

		private void addCondition(string condName, string param1, string param2, string param3, string param4, string param5)
		{
            Condition template = new Condition(condName);

			if (param1 != null)
				template.parameters.Add(param1);

			if (param2 != null)
				template.parameters.Add(param2);

			if (param3 != null)
				template.parameters.Add(param3);
			
			if (param4 != null)
				template.parameters.Add(param4);

			if (param5 != null)
				template.parameters.Add(param5);

			this.conditionTemplates.Add(template);
		}

        private void addCondition(string condName, string param1, string param2, string param3, string param4){
            addCondition(condName, param1, param2, param3, param4, null);
        }

        private void addCondition(string condName, string param1, string param2, string param3){
            addCondition(condName, param1, param2, param3, null, null);
        }
        private void addCondition(string condName, string param1, string param2){
            addCondition(condName, param1, param2, null, null, null);
        }
        private void addCondition(string condName, string param1){
            addCondition(condName, param1, null, null, null, null);
        }
        private void addCondition(string condName){
            addCondition(condName, null, null, null, null, null);
        }

		private void addAttribute(string attName, string attChangeType)
		{
			Attribute template = new Attribute();

			template.name = attName;
			if (attChangeType != "")
				template.changeType = attChangeType;
			else
				template.changeType = "none";

			this.attributeTemplates.Add(template);
		}

		private void button26_Click(object sender, System.EventArgs e)
		{
			// clear any previous results
			txtOrphans.Text = "";

			// identify orphaned traits and ancillaries (i.e. traits and ancillaries that have no associated triggers)
			// we've got all trigger and all trait/ancillary information if the user has parsed both files
			if (parsedTraits || parsedAncillaries)
			{
				txtOrphanStatus.Text = "Working...";

				Trait tempTrait = null;
				Ancillary tempAnc = null;
				Trigger tempTrig = null;
				TriggerAffect tempAff = null;

				// traits
				if (parsedTraits)
				{
					for (int n=0;n<traits.Count;n++)
					{
						bool foundMatch = false;
						tempTrait = (Trait) traits[n];

						for (int y=0;y<triggers.Count;y++)
						{
							tempTrig = (Trigger) triggers[y];
							for (int x=0;x<tempTrig.affectsList.Count;x++)
							{
								tempAff = (TriggerAffect) tempTrig.affectsList[x];

								// note that for ancillaries, traitName is actually the ancillaryName. This was re-used
								//	for convenience and, unfortunately, was not renamed due to the number of references
								//	this property had.
								if (tempTrait.name == tempAff.traitName)
								{
									// we found a match
									foundMatch = true;
									break;
								}

							}

							if (foundMatch)
								break;
						}

						if (!foundMatch)
						{
							// notify that we checked all triggers and did not find this ancillary
							txtOrphans.Text = txtOrphans.Text + Environment.NewLine + "ORPHANED TRAIT: " + tempTrait.name;
							txtOrphans.Refresh();
						}
					}

					txtOrphanStatus.Text = txtOrphanStatus.Text + Environment.NewLine + "Checked traits.";
				}

				// ancillaries
				if (parsedAncillaries)
				{
					for (int n=0;n<ancillaries.Count;n++)
					{
						bool foundMatch = false;
						tempAnc = (Ancillary) ancillaries[n];

						for (int y=0;y<ancillaryTriggers.Count;y++)
						{
							tempTrig = (Trigger) ancillaryTriggers[y];
							for (int x=0;x<tempTrig.affectsList.Count;x++)
							{
								tempAff = (TriggerAffect) tempTrig.affectsList[x];

								// note that for ancillaries, traitName is actually the ancillaryName. This was re-used
								//	for convenience and, unfortunately, was not renamed due to the number of references
								//	this property had.
								if (tempAnc.name == tempAff.traitName)
								{
									// we found a match
									foundMatch = true;
									break;
								}

							}

							if (foundMatch)
								break;
						}

						if (!foundMatch)
						{
							// notify that we checked all triggers and did not find this ancillary
							txtOrphans.Text = txtOrphans.Text + Environment.NewLine + "ORPHANED ANCILLARY: " + tempAnc.name;
							txtOrphans.Refresh();
						}
					}

					txtOrphanStatus.Text = txtOrphanStatus.Text + Environment.NewLine + "Checked ancillaries.";
				}

				txtOrphanStatus.Text = txtOrphanStatus.Text + Environment.NewLine + "Check complete.";
			}
			else
			{
				txtOrphanStatus.Text = "This function requires that you parse either the traits file, or the ancillaries file, or both. Please parse at least one, then return here again.";
			}
		}

        private void resetTraitTabColours() {
            lblTraitName.ForeColor = Color.Black;
            lblCharacters.ForeColor = Color.Black;
            lblAntiTraits.ForeColor = Color.Black;
            lblExcludesCultures.ForeColor = Color.Black;
            lblNoGoingBackLevel.ForeColor = Color.Black;
        }

        private void resetTraitTabLevelColours() {
            lblLevelName.ForeColor = Color.Black;
            lblLevelDesc.ForeColor = Color.Black;
            lblLevelEffectDesc.ForeColor = Color.Black; ;
            lblLevelGain.ForeColor = Color.Black; ;
            lblLevelLose.ForeColor = Color.Black; ;
            lblLevelEpithet.ForeColor = Color.Black;
            lblLevelThreshhold.ForeColor = Color.Black;
            lblLevelEffects.ForeColor = Color.Black;
        }

        private void resetTraitTriggerTabColours() {
            lblTriggerConditions.ForeColor = Color.Black;
            lblTriggerAffectsChance.ForeColor = Color.Black;
            lblTriggerAffectsLevel.ForeColor = Color.Black;
            lblTriggerAffectsName.ForeColor = Color.Black;
            lblWhenToTest.ForeColor = Color.Black;
        }

        private void resetAncillaryTabColours() {
            lblAncillaryName.ForeColor = Color.Black;
            lblAncillaryDescription.ForeColor = Color.Black;
            lblAncillaryEffectsDescription.ForeColor = Color.Black;
            lblExcludesAncillaries2.ForeColor = Color.Black;
            lblExcludesAncillaryCultures.ForeColor = Color.Black;
            lblAncillaryEffects.ForeColor = Color.Black;
        }

        private void resetAncillaryTriggerTabColours() {
            lblAncillaryConditions.ForeColor = System.Drawing.Color.Black;
            lblAncillaryTriggerName.ForeColor = System.Drawing.Color.Black;
            lblAncillaryEffectName.ForeColor = System.Drawing.Color.Black;
            lblAncillaryEffectChance.ForeColor = System.Drawing.Color.Black;
            lblAncillaryWhenTested.ForeColor = System.Drawing.Color.Black;
        }

        private void setLabelError(string input, Label lbl) {
            if ( input.StartsWith("{") ) {
                // this entry has an error, change the text of the corresponding label red
                input = input.Replace("{", "");
                input = input.Replace("}", "");
                lbl.ForeColor = System.Drawing.Color.Red;
            }
            lbl.Text = input;
        }

        private void mnuExit_Click(object sender, EventArgs e) {
            this.Close();
        }//end mnuExit_Click

        private Event getEvent(string eventToken) {
            Event tempEvent;
            bool foundEvent = false;

            for ( int i = 0; i < eventTemplates.Count; i += 1 ) {
                tempEvent = ( Event )eventTemplates[i];
                if ( tempEvent.eventName == eventToken ) {
                    foundEvent = true;
                }
            }

            if ( !foundEvent ) {
                eventToken = "{" + eventToken + "}";
            }

            return new Event(eventToken);
        }
    }
}
