namespace db_access
{
    partial class FieldSelector
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFieldName = new System.Windows.Forms.Label();
            this.cbInclude = new System.Windows.Forms.CheckBox();
            this.cbFrom = new System.Windows.Forms.ComboBox();
            this.cbTo = new System.Windows.Forms.ComboBox();
            this.lbFrom = new System.Windows.Forms.Label();
            this.lbTo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFieldName
            // 
            this.lblFieldName.AutoSize = true;
            this.lblFieldName.Location = new System.Drawing.Point(18, 9);
            this.lblFieldName.Name = "lblFieldName";
            this.lblFieldName.Size = new System.Drawing.Size(35, 13);
            this.lblFieldName.TabIndex = 0;
            this.lblFieldName.Text = "label1";
            this.lblFieldName.Click += new System.EventHandler(this.label1_Click);
            // 
            // cbInclude
            // 
            this.cbInclude.AutoSize = true;
            this.cbInclude.Checked = true;
            this.cbInclude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInclude.Location = new System.Drawing.Point(51, 25);
            this.cbInclude.Name = "cbInclude";
            this.cbInclude.Size = new System.Drawing.Size(86, 17);
            this.cbInclude.TabIndex = 1;
            this.cbInclude.Text = "Include Field";
            this.cbInclude.UseVisualStyleBackColor = true;
            // 
            // cbFrom
            // 
            this.cbFrom.FormattingEnabled = true;
            this.cbFrom.Location = new System.Drawing.Point(175, 21);
            this.cbFrom.Name = "cbFrom";
            this.cbFrom.Size = new System.Drawing.Size(121, 21);
            this.cbFrom.TabIndex = 2;
            // 
            // cbTo
            // 
            this.cbTo.FormattingEnabled = true;
            this.cbTo.Location = new System.Drawing.Point(319, 21);
            this.cbTo.Name = "cbTo";
            this.cbTo.Size = new System.Drawing.Size(121, 21);
            this.cbTo.TabIndex = 3;
            // 
            // lbFrom
            // 
            this.lbFrom.AutoSize = true;
            this.lbFrom.Location = new System.Drawing.Point(172, 5);
            this.lbFrom.Name = "lbFrom";
            this.lbFrom.Size = new System.Drawing.Size(30, 13);
            this.lbFrom.TabIndex = 4;
            this.lbFrom.Text = "From";
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(316, 5);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(20, 13);
            this.lbTo.TabIndex = 5;
            this.lbTo.Text = "To";
            // 
            // FieldSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbTo);
            this.Controls.Add(this.lbFrom);
            this.Controls.Add(this.cbTo);
            this.Controls.Add(this.cbFrom);
            this.Controls.Add(this.cbInclude);
            this.Controls.Add(this.lblFieldName);
            this.Name = "FieldSelector";
            this.Size = new System.Drawing.Size(484, 52);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblFieldName;
        public System.Windows.Forms.CheckBox cbInclude;
        public System.Windows.Forms.ComboBox cbFrom;
        public System.Windows.Forms.ComboBox cbTo;
        private System.Windows.Forms.Label lbFrom;
        private System.Windows.Forms.Label lbTo;
    }
}
