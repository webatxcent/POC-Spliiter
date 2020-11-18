using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCENT.JobServer.Abstract;
using System.Diagnostics;

namespace POC_Splitter
{
    public partial class BoolEditor : CheckBox, IValueEditor
    {

        ControlMoveFocusHandler _controlMoveFocusHandler;

        public BoolEditor(): base() {
            InitializeComponent();
            BackColor = SystemColors.Control;
            Padding = new Padding( 2, 0, 0, 0 );

        }

        protected override void OnPreviewKeyDown( PreviewKeyDownEventArgs e ) {
            Debug.Print( "OnPreviewKeyDown" );
            MoveFocus action = ParameterValue.EvaluateKey( new KeyEventArgs( e.KeyData ) );
            if ( action != MoveFocus.None )
                _controlMoveFocusHandler( this, action );

            base.OnPreviewKeyDown( e );
        }

        protected override void OnKeyDown( KeyEventArgs e ) {
            Debug.Print( "OnKeyDown" );
            MoveFocus action = ParameterValue.EvaluateKey( e );

            if ( action != MoveFocus.None )
                _controlMoveFocusHandler( this, action );
            else
                base.OnKeyDown( e );
        }


        #region IValueEditor implementation

        public Control Control {
            get {
                return this as Control;
            }

        }

        public string Value {
            get { return Text; }
        }

        public void Configure( ParameterDef parameterDef, string value ) {
            ThreeState = !parameterDef.IsRequired;
            if ( ( string.IsNullOrEmpty( value ) && !parameterDef.IsRequired ) ) {
                CheckState = CheckState.Indeterminate;
            }
            else if ( ( string.IsNullOrEmpty( value ) && parameterDef.IsRequired ) || value.ToLower().Contains( "false" ) ) {
                CheckState = CheckState.Unchecked;
            }
            else
                CheckState = CheckState.Checked;
        }

        public int PreferredHeight {
            get {
                return PreferredSize.Height;
            }
        }

        public void SetMoveFocusHandler( ControlMoveFocusHandler controlMoveFocusHandler ) {
            _controlMoveFocusHandler = controlMoveFocusHandler;
        }

        public bool RequiresFocusRectangle => true;

        #endregion
    }
}
