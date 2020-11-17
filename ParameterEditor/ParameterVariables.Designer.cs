namespace POC_Splitter
{
    partial class ParameterVariables
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.chkIncludeInputVariables = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            this.SuspendLayout();
            // 
            // chkIncludeInputVariables
            // 
            this.chkIncludeInputVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIncludeInputVariables.AutoSize = true;
            this.chkIncludeInputVariables.Location = new System.Drawing.Point(12, 444);
            this.chkIncludeInputVariables.Name = "chkIncludeInputVariables";
            this.chkIncludeInputVariables.Size = new System.Drawing.Size(134, 17);
            this.chkIncludeInputVariables.TabIndex = 1;
            this.chkIncludeInputVariables.Text = "Include Input Variables";
            this.chkIncludeInputVariables.UseVisualStyleBackColor = true;
            this.chkIncludeInputVariables.CheckedChanged += new System.EventHandler(this.chkIncludeInputVariables_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(713, 444);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(632, 444);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 32);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // dgvVariables
            // 
            this.dgvVariables.AllowUserToAddRows = false;
            this.dgvVariables.AllowUserToDeleteRows = false;
            this.dgvVariables.AllowUserToResizeRows = false;
            this.dgvVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dgvVariables.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Location = new System.Drawing.Point(12, 12);
            this.dgvVariables.MultiSelect = false;
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.RowHeadersVisible = false;
            this.dgvVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVariables.ShowCellErrors = false;
            this.dgvVariables.ShowCellToolTips = false;
            this.dgvVariables.ShowEditingIcon = false;
            this.dgvVariables.ShowRowErrors = false;
            this.dgvVariables.Size = new System.Drawing.Size(776, 426);
            this.dgvVariables.TabIndex = 5;
            this.dgvVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_CellDoubleClick);
            // 
            // ParameterVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 486);
            this.Controls.Add(this.dgvVariables);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkIncludeInputVariables);
            this.Name = "ParameterVariables";
            this.Text = "ParameterVariables";
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkIncludeInputVariables;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridView dgvVariables;
    }
}