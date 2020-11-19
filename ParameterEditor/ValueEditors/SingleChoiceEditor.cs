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
        bool _suppressUpDownHandling = false;


        public SingleChoiceEditor() : base() {
            InitializeComponent();
            //RequireMatch = true;
            AutoCompleteMode = AutoCompleteMode.None;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
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

        protected override void OnDropDown( EventArgs e ) {
            base.OnDropDown( e );
            _suppressUpDownHandling = true;

        }
        protected override void OnDropDownClosed( EventArgs e ) {
            base.OnDropDownClosed( e );
            _suppressUpDownHandling = false;
        }

        public string Value {
            get { return Text; }
        }

        public void Configure( ParameterDef parameterDef, string value ) {

            this.DisplayMember = "display";
            this.ValueMember = "value";
            foreach ( string choice in parameterDef.Choices ) {
                this.Items.Add( new { value = choice, display = choice } );
            }
            this.SelectedIndex = this.FindStringExact( value );
        }

        public new int PreferredHeight {
            get {
                return PreferredSize.Height;
            }
        }

        public bool RequiresFocusRectangle => false;

        public bool SuppressUpDownHandling => _suppressUpDownHandling;
        #endregion
    }
}
