namespace XCENT.JobServer.Manager.App
{
    partial class ParameterValue
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
            this.btnClear = new System.Windows.Forms.Button();
            this.lblVariable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(0, 1);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(19, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "?";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // lblVariable
            // 
            this.lblVariable.AutoEllipsis = true;
            this.lblVariable.BackColor = System.Drawing.SystemColors.Control;
            this.lblVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVariable.Location = new System.Drawing.Point(80, 1);
            this.lblVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(177, 23);
            this.lblVariable.TabIndex = 3;
            this.lblVariable.Text = "label1";
            this.lblVariable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblVariable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblVariable_MouseDown);
            // 
            // ParameterValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblVariable);
            this.Controls.Add(this.btnClear);
            this.Name = "ParameterValue";
            this.Size = new System.Drawing.Size(336, 24);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblVariable;
    }
}
