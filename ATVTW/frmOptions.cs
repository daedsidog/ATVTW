using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ATVTW {
    public partial class frmOptions : Form {
        public frmOptions() {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public DialogResult ShowDialog(ref bool warnings) {
            chkWarnings.Checked = warnings;
            ShowDialog();
            warnings = chkWarnings.Checked;
            return DialogResult;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}