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

namespace POC_Spliiter
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

        public ParameterValue() {
            InitializeComponent();
            txtValue.KeyDown += OnValueControlKeyDown;

        }

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
            Height = txtValue.Height;
            txtValue.Width = Width;
        }

        protected override void OnTextChanged( EventArgs e ) {
            base.OnTextChanged( e );
            txtValue.Text = Text;
        }




    }
}
