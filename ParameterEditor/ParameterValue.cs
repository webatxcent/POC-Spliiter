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
    public delegate string ResolveVariable( string variable );

    public partial class ParameterValue : UserControl
    {
        public event MoveFocusHandler FocusChange;
        public event SyncLabelsHandler SyncLabels;

        IValueEditor _editor;
        TextBox _proxy; //stand-in for ideal height of the control
        ResolveVariable _resolveVariable;
        AppFonts _fonts;
        int _margin;
        bool _hasFocus;

        public ParameterDef ParameterDef { get; internal set; }

        public ParameterValue( ParameterDef parameterDef, string value, ResolveVariable resolveVariable, int margin ) {
            InitializeComponent();

            _resolveVariable = resolveVariable;
            _margin = margin;

            btnClear.Text = "\uf057";
            btnClear.Click += OnClearClick;
            btnClear.ForeColor = Color.Red;

            this.BackColor = SystemColors.Control;

            ParameterDef = parameterDef;
            //this is used for locating controls.
            Name = parameterDef.Name;

            //establish minimum height for the control which is based on the textbox. This can always be overridden when implementing PreferredHeight in the Value editors.
            _proxy = new TextBox();
            _proxy.Location = new Point( -1000, -1000 ) ;
            _proxy.TabStop = false;
            _proxy.Enter += _proxy_Enter;
            _proxy.Leave += _proxy_Leave;
            _proxy.KeyDown += _proxy_KeyDown;
            Controls.Add( _proxy );


            if ( parameterDef.ModuleParameterType == ModuleParameterType.String ) {
                _editor = new StringEditor();
            }
            else if ( parameterDef.ModuleParameterType == ModuleParameterType.Bool ) {
                _editor = new BoolEditor();
            }
            else if ( parameterDef.ModuleParameterType == ModuleParameterType.Date ) {
                _editor = new DateEditor();
            }
            else if ( parameterDef.ModuleParameterType == ModuleParameterType.Number ) {
                _editor = new NumberEditor();
            }
            else if ( parameterDef.ModuleParameterType == ModuleParameterType.Choice ) {
                if ( parameterDef.AllowMultipleSelections ) {
                    _editor = new MultiChoiceEditor();
                }
                else { //this this is a single select situation 
                    _editor = new SingleChoiceEditor();
                }
            }
            else if ( parameterDef.ModuleParameterType == ModuleParameterType.ListString ) {
                _editor = new StringEditor();
            }

            _editor.Control.Name = parameterDef.Name;
            Controls.Add( _editor.Control );
            _editor.Control.KeyDown += OnValueControlKeyDown;
            _editor.SetMoveFocusHandler( MoveFocusHandler );


            Value = value;
        }

        private void _proxy_KeyDown( object sender, KeyEventArgs e ) {
            MoveFocus action = ParameterValue.EvaluateKey( e );

            if ( action != MoveFocus.None ) {
                e.Handled = true;
                FocusChange?.Invoke( this, action );
            }
        }

        /// <summary>
        /// this is a special delegate that is called from editor implementations that may do something with the up and down keys such as the NumberBox which swallows those values
        /// because the base TextBox will use them to navigate left and right.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        private void MoveFocusHandler( Control control, MoveFocus action ) {
            FocusChange?.Invoke( this, action );
        }

        //DEBUG PURPOSES - can be removed
        private void _proxy_Leave( object sender, EventArgs e ) {
            _hasFocus = false;
            Debug.Print( "left _proxy." );
            Invalidate();
        }

        //DEBUG PURPOSES - can be removed
        private void _proxy_Enter( object sender, EventArgs e ) {
            _hasFocus = true;
            Debug.Print( "entered _proxy." );
            Invalidate();
        }

        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint( e );
            if ( _hasFocus || _editor.RequiresFocusRectangle  )
                ControlPaint.DrawFocusRectangle( e.Graphics, this.ClientRectangle, SystemColors.ActiveBorder, SystemColors.Control );
        }

        protected override void OnParentChanged( EventArgs e ) {
            if ( Parent != null )
                SetFont( Parent.Font );
        }

        internal void SetFont( Font font ) {
            lblVariable.Font = new Font( font, FontStyle.Regular );

            if ( _fonts == null )
                _fonts = new AppFonts( Parent.Font.Size * 1.1f );

            if ( _fonts.Size != Parent.Font.Size * 1.1f ) {
                _fonts.Dispose();
                _fonts = new AppFonts( Parent.Font.Size );
            }

            btnClear.Font = new Font( _fonts.FARegular, FontStyle.Regular );
        }

        private void OnClearClick( object sender, EventArgs e ) {
            _editor.Control.Text = "";
            ConfigureControls();
        }

        void ConfigureControls( string value = null ) {
            bool isVariableRef;

            if ( value == null )
                isVariableRef = false;
            else
                isVariableRef = value.StartsWith( "{" ) && value.EndsWith( "}" );

            _editor.Control.Visible = !isVariableRef;
            lblVariable.Visible = isVariableRef;
            btnClear.Visible = isVariableRef;

            if ( isVariableRef )
                lblVariable.Text = _resolveVariable( value );
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
            MoveFocus action = ParameterValue.EvaluateKey( e );

            if ( action != MoveFocus.None ) {
                e.Handled = true;
                FocusChange?.Invoke( this, action );
            }
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );

            if ( _proxy == null )
                return;

            var minHeight = _proxy.PreferredHeight;

            if ( _editor.PreferredHeight < minHeight ) {
                Height = minHeight;
                _editor.Control.Height = minHeight;
            }
            else
                Height = _editor.PreferredHeight;

            _editor.Control.Width = Width;

            if ( _editor.RequiresFocusRectangle ) {
                var rc = _editor.Control.DisplayRectangle;
                rc.Inflate( -1, -1 );
                _editor.Control.Size = rc.Size;
                _editor.Control.Location = rc.Location;
            }

            lblVariable.Top = ( Height - lblVariable.PreferredHeight ) / 2;
            lblVariable.Left = 1;
            lblVariable.Height = lblVariable.PreferredHeight;
            lblVariable.Width = Width - ( _proxy.PreferredHeight - 2 )- _margin;
            lblVariable.MaximumSize = new Size( Width - _proxy.PreferredHeight - _margin, lblVariable.Height );

            btnClear.Top = 1;
            btnClear.Left = lblVariable.Left + lblVariable.Width + _margin;
            btnClear.Height = _proxy.PreferredHeight - 2 ;
            btnClear.Width = _proxy.PreferredHeight - 2;
        }

        public string Value {
            get {
                return _editor.Value;
            }
            set {
                _editor.Configure( ParameterDef, value );
                ConfigureControls( value );
            }
        }

        private void lblVariable_MouseDown( object sender, MouseEventArgs e ) {
            _proxy.Focus();
        }

        internal static MoveFocus EvaluateKey( KeyEventArgs e ) {
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

            return action;
        }
    }
}
