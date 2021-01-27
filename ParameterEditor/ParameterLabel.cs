using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XCENT.JobServer.Manager.App
{
    public delegate void ShowHelpHandler( string name );
    public delegate void SetFormulaHandler( ParameterValue parameterValue, string name );

    public partial class ParameterLabel : UserControl
    {
        string _parameterName;
        int _margin;
        bool _isRequired;

        AppFonts _fonts;

        public event ShowHelpHandler ShowHelp;
        public event SetFormulaHandler SetFormula;

        public ParameterLabel( string name, string caption, bool isRequired, bool hasInfo, bool canAssignVariable, int margin ) {
            InitializeComponent();

            lblCaption.Text = caption;

            btnHelp.Text = FontAwesome.InfoSquare;
            btnHelp.Click += OnHelpClick;
            btnHelp.ForeColor = Color.CornflowerBlue;
            btnHelp.Enabled = hasInfo;

            btnFormula.Text = FontAwesome.Equals; // was originally FontAwesome.Function
            btnFormula.Padding = new Padding( 0 );
            btnFormula.Click += OnFormulaClick;
            btnFormula.ForeColor = Color.CornflowerBlue;
            btnFormula.Enabled = canAssignVariable;

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

            if ( _fonts == null )
                _fonts = new AppFonts( Parent.Font.Size * 1.2f);

            if ( _fonts.Size != Parent.Font.Size * 1.2f ) {
                _fonts.Dispose();
                _fonts = new AppFonts( Parent.Font.Size );
            }

            btnHelp.Font = new Font( _fonts.FARegular, FontStyle.Regular );
            btnFormula.Font = new Font( _fonts.FARegular, FontStyle.Regular );
        }

        private void OnCaptionClick( object sender, EventArgs e ) {
            ( Tag as Control ).Focus();
        }

        private void OnHelpClick( object sender, EventArgs e ) {
            ShowHelp?.Invoke( _parameterName );
            ( Tag as Control ).Focus();
        }

        private void OnFormulaClick( object sender, EventArgs e ) {
            SetFormula?.Invoke( this.Tag as ParameterValue, _parameterName );
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
            btnHelp.Left = btnFormula.Left - _margin - btnHelp.Width;

            lblCaption.Top = ( Height - lblCaption.Height ) / 2;
            lblCaption.Height = btnFormula.Height;
            lblCaption.MaximumSize = new Size( btnHelp.Left - _margin, lblCaption.Height );
            lblCaption.Left = btnHelp.Left - _margin - lblCaption.Width;

        }

        protected override void DestroyHandle() {
            _fonts.Dispose();
            base.DestroyHandle();
        }


    }
}
