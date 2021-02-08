using XCENT.JobServer.Manager.App;

namespace POC_Splitter {
    partial class Form1 {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnTrigger = new System.Windows.Forms.Button();
            this.btnStep1 = new System.Windows.Forms.Button();
            this.btnStep2 = new System.Windows.Forms.Button();
            this.btnStep3 = new System.Windows.Forms.Button();
            this.btnShowParams = new System.Windows.Forms.Button();
            this.parameterContainer1 = new XCENT.JobServer.Manager.App.ParameterEditor();
            this.btnIsValid = new System.Windows.Forms.Button();
            this.txtLanding = new System.Windows.Forms.TextBox();
            this.lblLanding = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTrigger
            // 
            this.btnTrigger.Location = new System.Drawing.Point(13, 12);
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(75, 32);
            this.btnTrigger.TabIndex = 0;
            this.btnTrigger.Text = "Trigger";
            this.btnTrigger.UseVisualStyleBackColor = true;
            this.btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // btnStep1
            // 
            this.btnStep1.Location = new System.Drawing.Point(94, 12);
            this.btnStep1.Name = "btnStep1";
            this.btnStep1.Size = new System.Drawing.Size(32, 32);
            this.btnStep1.TabIndex = 1;
            this.btnStep1.Text = "1";
            this.btnStep1.UseVisualStyleBackColor = true;
            this.btnStep1.Click += new System.EventHandler(this.btnStep1_Click);
            // 
            // btnStep2
            // 
            this.btnStep2.Location = new System.Drawing.Point(132, 13);
            this.btnStep2.Name = "btnStep2";
            this.btnStep2.Size = new System.Drawing.Size(32, 32);
            this.btnStep2.TabIndex = 2;
            this.btnStep2.Text = "2";
            this.btnStep2.UseVisualStyleBackColor = true;
            this.btnStep2.Click += new System.EventHandler(this.btnStep2_Click);
            // 
            // btnStep3
            // 
            this.btnStep3.Location = new System.Drawing.Point(170, 13);
            this.btnStep3.Name = "btnStep3";
            this.btnStep3.Size = new System.Drawing.Size(32, 32);
            this.btnStep3.TabIndex = 3;
            this.btnStep3.Text = "3";
            this.btnStep3.UseVisualStyleBackColor = true;
            this.btnStep3.Click += new System.EventHandler(this.btnStep3_Click);
            // 
            // btnShowParams
            // 
            this.btnShowParams.Location = new System.Drawing.Point(313, 12);
            this.btnShowParams.Name = "btnShowParams";
            this.btnShowParams.Size = new System.Drawing.Size(122, 32);
            this.btnShowParams.TabIndex = 4;
            this.btnShowParams.Text = "Show Params";
            this.btnShowParams.UseVisualStyleBackColor = true;
            this.btnShowParams.Click += new System.EventHandler(this.btnShowParams_Click);
            // 
            // parameterContainer1
            // 
            this.parameterContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.parameterContainer1.Location = new System.Drawing.Point(13, 53);
            this.parameterContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.parameterContainer1.Name = "parameterContainer1";
            this.parameterContainer1.Size = new System.Drawing.Size(1169, 608);
            this.parameterContainer1.TabIndex = 8;
            // 
            // btnIsValid
            // 
            this.btnIsValid.Location = new System.Drawing.Point(441, 12);
            this.btnIsValid.Name = "btnIsValid";
            this.btnIsValid.Size = new System.Drawing.Size(122, 32);
            this.btnIsValid.TabIndex = 5;
            this.btnIsValid.Text = "Validate";
            this.btnIsValid.UseVisualStyleBackColor = true;
            this.btnIsValid.Click += new System.EventHandler(this.btnIsValid_Click);
            // 
            // txtLanding
            // 
            this.txtLanding.Location = new System.Drawing.Point(654, 13);
            this.txtLanding.Name = "txtLanding";
            this.txtLanding.Size = new System.Drawing.Size(270, 29);
            this.txtLanding.TabIndex = 7;
            // 
            // lblLanding
            // 
            this.lblLanding.AutoSize = true;
            this.lblLanding.Location = new System.Drawing.Point(582, 16);
            this.lblLanding.Name = "lblLanding";
            this.lblLanding.Size = new System.Drawing.Size(66, 21);
            this.lblLanding.TabIndex = 6;
            this.lblLanding.Text = "Landing";
            this.lblLanding.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 675);
            this.Controls.Add(this.lblLanding);
            this.Controls.Add(this.txtLanding);
            this.Controls.Add(this.btnIsValid);
            this.Controls.Add(this.btnShowParams);
            this.Controls.Add(this.btnStep3);
            this.Controls.Add(this.btnStep2);
            this.Controls.Add(this.btnStep1);
            this.Controls.Add(this.btnTrigger);
            this.Controls.Add(this.parameterContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ParameterEditor parameterContainer1;
        private System.Windows.Forms.Button btnTrigger;
        private System.Windows.Forms.Button btnStep1;
        private System.Windows.Forms.Button btnStep2;
        private System.Windows.Forms.Button btnStep3;
        private System.Windows.Forms.Button btnShowParams;
        private System.Windows.Forms.Button btnIsValid;
        private System.Windows.Forms.TextBox txtLanding;
        private System.Windows.Forms.Label lblLanding;
    }
}

