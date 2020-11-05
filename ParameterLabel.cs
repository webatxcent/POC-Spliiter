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
        bool _isRequired;

        public event ShowHelpHandler ShowHelp;
        public event SetFormulaHandler SetFormula;

        public ParameterLabel( string name, string caption, bool isRequired, int margin ) {
            InitializeComponent();

            lblCaption.Text = caption;

            btnFormula.Click += OnFormulaClick;
            btnHelp.Click += OnHelpClick;

            _parameterName = name;
            _margin = margin;

            lblCaption.AutoSize = true;
            lblCaption.AutoEllipsis = true;
            _isRequired = isRequired;

            lblCaption.Click += OnCaptionClick;
        }

        protected override void OnParentChanged( EventArgs e ) {
            if ( Parent != null )
                SetFont( Parent.Font );
        }

        protected override void OnClick( EventArgs e ) {
            base.OnClick( e );
            ( Tag as Control ).Focus();
        }

        internal void SetFont( Font font ) {
            lblCaption.Font = new Font( font, ( _isRequired ) ? FontStyle.Bold : FontStyle.Regular );
        }

        private void OnCaptionClick( object sender, EventArgs e ) {
            ( Tag as Control ).Focus();
        }

        private void OnHelpClick( object sender, EventArgs e ) {
            ShowHelp?.Invoke( _parameterName );
            ( Tag as Control ).Focus();
        }

        private void OnFormulaClick( object sender, EventArgs e ) {
            SetFormula?.Invoke( _parameterName );
            ( Tag as Control ).Focus();
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );

            btnFormula.Top = 0;
            btnFormula.Height = Height;
            btnFormula.Width = btnFormula.Height;
            btnFormula.Left = Width - btnFormula.Width;

            btnHelp.Top = btnFormula.Top;
            btnHelp.Height = btnFormula.Height;
            btnHelp.Width = btnFormula.Width;
            btnHelp.Left = btnFormula.Left - _margin  - btnHelp.Width;

            lblCaption.Top = ( Height - lblCaption.Height ) / 2;
            lblCaption.Height = btnFormula.Height;
            lblCaption.MaximumSize = new Size( btnHelp.Left - _margin, lblCaption.Height );
            lblCaption.Left = btnHelp.Left - _margin - lblCaption.Width;

        }




    }
}
