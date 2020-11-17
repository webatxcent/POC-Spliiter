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

namespace POC_Splitter
{
    public partial class BoolEditor : CheckBox, IValueEditor
    {

        public BoolEditor(): base() {
            InitializeComponent();
            BackColor = SystemColors.Control;
            Padding = new Padding( 2, 0, 0, 0 );
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


        #endregion
    }
}
