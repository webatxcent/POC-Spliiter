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
    public partial class SingleChoiceEditor : ComboBox, IValueEditor
    {

        public SingleChoiceEditor() : base() {
            InitializeComponent();
            //RequireMatch = true;
            AutoCompleteMode = AutoCompleteMode.None;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
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

            this.DisplayMember = "display";
            this.ValueMember = "value";
            foreach ( string choice in parameterDef.Choices ) {
                this.Items.Add(new { value = choice, display = choice } );
            }
        }

        public new int PreferredHeight {
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
