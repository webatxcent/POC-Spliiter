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
    public partial class NumberEditor : NumberBox, IValueEditor
    {
        ControlMoveFocusHandler _controlMoveFocusHandler;


        public NumberEditor() {
            InitializeComponent();
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

        public new string Value {
            get { return base.Text; }
        }

        public void Configure( ParameterDef parameterDef, string value ) {

            if ( value != null ) {
                decimal val;
                if ( !Decimal.TryParse( value, out val ) )
                    val = 0;
                base.Number = val;
            }
            else
                base.Number = null;
            base.AllowBlanks = !parameterDef.IsRequired;
            //TODO: WEB fix this after the underlying parameterDef is fixed.
            if ( parameterDef.MinValue != 0 && parameterDef.MaxValue != 0 ) {
                base.Min = (decimal)parameterDef.MinValue;
                base.Max = (decimal)parameterDef.MaxValue;
            }
            base.Decimals = parameterDef.DecimalPlaces;
        }

        public new int PreferredHeight {
            get {
                return PreferredSize.Height;
            }
        }

        public void SetMoveFocusHandler( ControlMoveFocusHandler controlMoveFocusHandler ) {
            _controlMoveFocusHandler = controlMoveFocusHandler;
        }

        public bool RequiresFocusRectangle => false;

        public bool SuppressUpDownHandling => false;
        #endregion
    }
}
