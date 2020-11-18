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
using XCENT.Core.UI.WinForms;

namespace POC_Splitter
{
    public partial class MultiChoiceEditor : MultiSelectDropDown, IValueEditor
    {

        public MultiChoiceEditor() : base() {
            InitializeComponent();
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

            value = value ?? "";
            var myData = parameterDef.Choices.Select( m => new MultiSelectDropDownItem { Display = m, Value = m, IsSelected = ( value.Contains( m ) ) } ).ToList();
            base.Items = myData;

        }

        public int PreferredHeight {
            get {
                return PreferredSize.Height;
            }
        }

        public void SetMoveFocusHandler( ControlMoveFocusHandler controlMoveFocusHandler ) {
            //nothing to do for this control.
        }

        public bool RequiresFocusRectangle => false;

        #endregion
    }
}
