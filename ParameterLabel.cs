using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POC_Spliiter
{
    public delegate void ShowHelpHandler( string Name );
    public delegate void SetFormulaHandler( string Name );

    public partial class ParameterLabel : UserControl
    {
        string _parameterName;
        int _margin;

        public event ShowHelpHandler ShowHelp;
        public event SetFormulaHandler SetFormula;

        public ParameterLabel( string name, string caption, bool isRequired, int height, int margin ) {
            InitializeComponent();

            Height = height;

            lblCaption.Text = caption;

            btnFormula.Click += OnFormulaClick;
            btnHelp.Click += OnHelpClick;

            _parameterName = name;
            _margin = margin;

            lblRequired.Visible = isRequired;

            lblCaption.AutoSize = true;
            lblCaption.AutoEllipsis = true;

            lblCaption.Click += OnCaptionClick;

        }

        protected override void OnClick( EventArgs e ) {
            base.OnClick( e );
            ( Tag as Control ).Focus();
        }

        private void OnCaptionClick( object sender, EventArgs e ) {
            (Tag as Control).Focus();
        }

        private void OnHelpClick( object sender, EventArgs e ) {
            ShowHelp?.Invoke( _parameterName );
        }

        private void OnFormulaClick( object sender, EventArgs e ) {
            SetFormula?.Invoke( _parameterName );
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );

            btnFormula.Top = _margin / 2;
            btnFormula.Height = Height - _margin;
            btnFormula.Width = btnFormula.Height;
            btnFormula.Left = Width - btnFormula.Width;

            btnHelp.Top = btnFormula.Top;
            btnHelp.Height = btnFormula.Height;
            btnHelp.Width = btnFormula.Width;
            btnHelp.Left = btnFormula.Left - _margin - btnHelp.Width;

            int captionLabelRight = btnHelp.Left - _margin;

            if ( lblRequired.Visible ) {
                lblRequired.Top = ( Height - lblRequired.Height ) / 2;
                lblRequired.Height = Height - _margin;
                lblRequired.Left = captionLabelRight - lblRequired.Width;
                captionLabelRight = lblRequired.Left;
            }

            lblCaption.Top = ( Height - lblCaption.Height ) / 2;
            lblCaption.Height = btnFormula.Height;
            lblCaption.MaximumSize = new Size( captionLabelRight, lblCaption.Height );
            lblCaption.Left = captionLabelRight - lblCaption.Width;

        }




    }
}
