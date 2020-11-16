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
            this.parameterContainer1 = new POC_Splitter.ParameterContainer();
            this.btnTrigger = new System.Windows.Forms.Button();
            this.btnStep1 = new System.Windows.Forms.Button();
            this.btnStep2 = new System.Windows.Forms.Button();
            this.btnStep3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.parameterContainer1.TabIndex = 0;
            // 
            // btnTrigger
            // 
            this.btnTrigger.Location = new System.Drawing.Point(13, 12);
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(75, 32);
            this.btnTrigger.TabIndex = 1;
            this.btnTrigger.Text = "Trigger";
            this.btnTrigger.UseVisualStyleBackColor = true;
            this.btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // btnStep1
            // 
            this.btnStep1.Location = new System.Drawing.Point(94, 12);
            this.btnStep1.Name = "btnStep1";
            this.btnStep1.Size = new System.Drawing.Size(32, 32);
            this.btnStep1.TabIndex = 2;
            this.btnStep1.Text = "1";
            this.btnStep1.UseVisualStyleBackColor = true;
            this.btnStep1.Click += new System.EventHandler(this.btnStep1_Click);
            // 
            // btnStep2
            // 
            this.btnStep2.Location = new System.Drawing.Point(132, 13);
            this.btnStep2.Name = "btnStep2";
            this.btnStep2.Size = new System.Drawing.Size(32, 32);
            this.btnStep2.TabIndex = 3;
            this.btnStep2.Text = "2";
            this.btnStep2.UseVisualStyleBackColor = true;
            this.btnStep2.Click += new System.EventHandler(this.btnStep2_Click);
            // 
            // btnStep3
            // 
            this.btnStep3.Location = new System.Drawing.Point(170, 12);
            this.btnStep3.Name = "btnStep3";
            this.btnStep3.Size = new System.Drawing.Size(32, 32);
            this.btnStep3.TabIndex = 4;
            this.btnStep3.Text = "3";
            this.btnStep3.UseVisualStyleBackColor = true;
            this.btnStep3.Click += new System.EventHandler(this.btnStep3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 675);
            this.Controls.Add(this.btnStep3);
            this.Controls.Add(this.btnStep2);
            this.Controls.Add(this.btnStep1);
            this.Controls.Add(this.btnTrigger);
            this.Controls.Add(this.parameterContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ParameterContainer parameterContainer1;
        private System.Windows.Forms.Button btnTrigger;
        private System.Windows.Forms.Button btnStep1;
        private System.Windows.Forms.Button btnStep2;
        private System.Windows.Forms.Button btnStep3;
    }
}

