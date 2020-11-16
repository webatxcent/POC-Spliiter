namespace POC_Splitter
{
    partial class ParameterLabel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnFormula = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.lblCaption = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFormula
            // 
            this.btnFormula.FlatAppearance.BorderSize = 0;
            this.btnFormula.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormula.Location = new System.Drawing.Point(296, 3);
            this.btnFormula.Margin = new System.Windows.Forms.Padding(0);
            this.btnFormula.Name = "btnFormula";
            this.btnFormula.Size = new System.Drawing.Size(28, 23);
            this.btnFormula.TabIndex = 0;
            this.btnFormula.TabStop = false;
            this.btnFormula.Text = "=";
            this.btnFormula.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Location = new System.Drawing.Point(264, 3);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(26, 23);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.TabStop = false;
            this.btnHelp.Text = "?";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // lblCaption
            // 
            this.lblCaption.AutoEllipsis = true;
            this.lblCaption.Location = new System.Drawing.Point(3, 3);
            this.lblCaption.Margin = new System.Windows.Forms.Padding(0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(177, 23);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "label1";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ParameterLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnFormula);
            this.Name = "ParameterLabel";
            this.Size = new System.Drawing.Size(324, 32);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFormula;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label lblCaption;
    }
}
