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

namespace XCENT.JobServer.Manager.App
{
    public partial class NumberEditor : NumberBox, IValueEditor
    {
        ControlMoveFocusHandler _controlMoveFocusHandler;


        public NumberEditor() {
            InitializeComponent();
            base.TextAlign = HorizontalAlignment.Left;
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
            base.Min = parameterDef.MinValue.HasValue ? (decimal)parameterDef.MinValue.Value : decimal.MinValue;
            base.Max = parameterDef.MaxValue.HasValue ? (decimal)parameterDef.MaxValue.Value : decimal.MaxValue;
            base.Decimals = parameterDef.DecimalPlaces ?? 0;
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
