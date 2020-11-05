namespace POC_Spliiter
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
            this.lblRequired = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFormula
            // 
            this.btnFormula.Location = new System.Drawing.Point(296, 3);
            this.btnFormula.Name = "btnFormula";
            this.btnFormula.Size = new System.Drawing.Size(28, 23);
            this.btnFormula.TabIndex = 0;
            this.btnFormula.Text = "=";
            this.btnFormula.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(264, 3);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(26, 23);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.Text = "?";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // lblCaption
            // 
            this.lblCaption.AutoEllipsis = true;
            this.lblCaption.Location = new System.Drawing.Point(3, 3);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(177, 23);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "label1";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblRequired.Location = new System.Drawing.Point(223, 8);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(11, 13);
            this.lblRequired.TabIndex = 3;
            this.lblRequired.Text = "*";
            this.lblRequired.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ParameterLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Controls.Add(this.lblRequired);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnFormula);
            this.Name = "ParameterLabel";
            this.Size = new System.Drawing.Size(324, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFormula;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblRequired;
    }
}
