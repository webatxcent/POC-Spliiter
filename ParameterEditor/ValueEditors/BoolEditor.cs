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

        public BoolEditor(): base() {
            InitializeComponent();
            BackColor = SystemColors.Control;
            Padding = new Padding( 2, 0, 0, 0 );

        }

        protected override void OnEnter( EventArgs e ) {
            base.OnEnter( e );
            ValueEditorContainer.ChildHasFocus(true);
        }

        protected override void OnLeave( EventArgs e ) {
            base.OnLeave( e );
            ValueEditorContainer.ChildHasFocus( false );
        }

        #region IValueEditor implementation

        public IValueEditorContainer ValueEditorContainer {
            get;
            set;
        }

        public Control Control {
            get {
                return this as Control;
            }

        }

        public string Value {
            get {
                if ( CheckState == CheckState.Checked )
                    return "true";
                else if ( CheckState == CheckState.Unchecked )
                    return "false";
                else 
                    return null;
            }
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

        public bool RequiresFocusRectangle => true;

        public bool SuppressUpDownHandling => false;

        #endregion
    }
}
