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

namespace POC_Spliiter
{
    public partial class NumberEditor : NumberBox, IValueEditor
    {

        public NumberEditor() {
            InitializeComponent();
        }

        #region IValueEditor implementation

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
            base.Min = (decimal)parameterDef.MinValue;
            base.Max = (decimal)parameterDef.MaxValue;
            base.Decimals = parameterDef.DecimalPlaces;
        }

        public new int PreferredHeight {
            get {
                return PreferredSize.Height;
            }
        }

        #endregion
    }
}
