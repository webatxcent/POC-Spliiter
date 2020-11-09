using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using XCENT.JobServer.Abstract;
using POC_Spliiter;

namespace POC_Splitter
{

    public enum MoveFocus
    {
        None,
        First,
        Previous, 
        Next,
        Last
    }

    public delegate void MoveFocusHandler( ParameterValue source, MoveFocus moveFocus );
    public delegate void SyncLabelsHandler();

    public partial class ParameterValue : UserControl
    {
        public event MoveFocusHandler FocusChange;
        public event SyncLabelsHandler SyncLabels;

        IValueEditor _editor;
        TextBox _proxy; //stand-in for ideal height of the control

        public ParameterValue( ParameterDef parameterDef, string value ) {
            InitializeComponent();

            //this is used for locating controls.
            Name = parameterDef.Name;

            //establish minimum height for the control which is based on the textbox. This can always be overridden when implementing PreferredHeight in the Value editors.
            _proxy = new TextBox();
            _proxy.Visible = false;
            Controls.Add( _proxy );


            if ( parameterDef.ModuleParameterType == ModuleParameterType.String ) {
                _editor = new StringEditor();
            }
            else if ( parameterDef.ModuleParameterType == ModuleParameterType.Bool ) {
                _editor = new BoolEditor();
            }

            Controls.Add( _editor.Control );

            _editor.Configure( parameterDef, value );
            _editor.Control.KeyDown += OnValueControlKeyDown;
        }

        /// <summary>
        /// This event handler is needed to compensate for a problem in the parent container control where small scrollbar movements,
        /// and control repositions as the result of tabbing/focus do not trigger a scroll event. The consumer uses this event as a 
        /// signal to execute code to realign the label panel with the value panel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMove( EventArgs e ) {
            base.OnMove( e );
            SyncLabels?.Invoke();
        }


        private void OnValueControlKeyDown( object sender, KeyEventArgs e ) {
            MoveFocus action = MoveFocus.None;

            if ( e.Modifiers == Keys.Control && e.KeyCode == Keys.Home ) {
                action = MoveFocus.First;
            }
            else if ( e.Modifiers == Keys.Control && e.KeyCode == Keys.End ) {
                action = MoveFocus.Last;
            }
            else if ( e.Modifiers == 0 && e.KeyCode == Keys.Up ) {
                action = MoveFocus.Previous;
            }
            else if ( e.Modifiers == 0 && e.KeyCode == Keys.Down ) {
                action = MoveFocus.Next;
            }

            if ( action != MoveFocus.None ) {
                e.Handled = true;
                FocusChange?.Invoke( this, action );
            }
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );
            if ( _editor == null )
                return;
            var minHeight = _proxy.PreferredHeight;
            if ( _editor.PreferredHeight < minHeight ) {
                Height = minHeight;
                _editor.Control.Height = minHeight;
            }
            else
                Height = _editor.PreferredHeight;
            _editor.Control.Width = Width;
        }
    }
}
