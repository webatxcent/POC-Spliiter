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

    //delegate for event that will be raised to indicate that the focus needs to change to another parameter value.
    public delegate void MoveFocusHandler( ParameterValue source, MoveFocus moveFocus );
    //delegate for event that will be raised to indicate that labels need to be adjusted; this is a hack because events indicating movement don't always work
    public delegate void SyncLabelsHandler();
    //delegate for function that this class will use to resolve a variable reference for display purposes.
    public delegate string ResolveVariable( string variable );

    public partial class ParameterValue : UserControl, IValueEditorContainer
    {
        public event MoveFocusHandler FocusChange;
        public event SyncLabelsHandler SyncLabels;

        IValueEditor _editor;
        TextBox _proxy; //stand-in for ideal height of the control
        ResolveVariable _resolveVariable;
        AppFonts _fonts;
        int _margin;
        bool _hasFocus;
        bool _isVariableReference;

        public ParameterDef ParameterDef { get; internal set; }

        public string Value {
            get {
                return _editor.Value;
            }
            set {
                _editor.Configure( ParameterDef, value );
                ConfigureControls( value );
            }
        }

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
            _proxy.Location = new Point( -1000, -1000 );
            _proxy.TabStop = false;
            _proxy.Visible = false;
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
                //TODO write the list editor
                _editor = new StringEditor();
            }

            _editor.ValueEditorContainer = this;
            _editor.Control.Name = parameterDef.Name;
            _editor.Control.KeyDown += OnValueEditorControlKeyDown;
            Controls.Add( _editor.Control );
            Value = value;
        }

        private void OnValueEditorControlKeyDown( object sender, KeyEventArgs e ) {
            MoveFocus action =  ParameterValue.EvaluateKey( e );

            if ( action != MoveFocus.None ) {
                e.Handled = true;
                FocusChange?.Invoke( this, action );
            }

        }

        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;

        private bool ctrlKeyDown = false;

        protected override bool ProcessCmdKey( ref Message msg, Keys keyData ) {
            Debug.Print( $"ProcessCmdKey- Msg:{msg.Msg:X} LParam:{msg.LParam.ToInt64():X} WParam:{msg.WParam.ToInt32():X}" );

            if ( msg.Msg == WM_KEYDOWN ) {
                Debug.Print( "\tKey Down" );

                if ( keyData == Keys.Control ) {
                    Debug.Print( "\tCTRL pressed" );
                    ctrlKeyDown = true;
                    return base.ProcessCmdKey( ref msg, keyData );
                }
                if ( keyData == Keys.Up ) {
                    Debug.Print( "\t\tMoving Previous" );
                    FocusChange?.Invoke( this, MoveFocus.Previous );
                    return true;
                }
                if ( keyData == Keys.Down ) {
                    Debug.Print( "\t\tMoving Next" );
                    FocusChange?.Invoke( this, MoveFocus.Next );
                    return true;
                }
                if ( keyData == Keys.Home && ctrlKeyDown ) {
                    Debug.Print( "\t\tMoving First" );
                    FocusChange?.Invoke( this, MoveFocus.First );
                    return true;
                }
                if ( keyData == Keys.End && ctrlKeyDown ) {
                    Debug.Print( "\t\tMoving last" );
                    FocusChange?.Invoke( this, MoveFocus.Last );
                    return true;
                }
            }

            if ( msg.Msg == WM_KEYUP ) {
                Debug.Print( "\tKey Up" );
                if ( keyData == Keys.Control ) {
                    Debug.Print( "\t\tCTRL released" );
                    ctrlKeyDown = false;
                    return base.ProcessCmdKey( ref msg, keyData );
                }
            }

            return base.ProcessCmdKey( ref msg, keyData );
        }

        //draws focus rectangle if needed for currently hosted editor control.
        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint( e );
            if ( _hasFocus && ( _editor.RequiresFocusRectangle || _isVariableReference ) )
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

            if ( value == null )
                _isVariableReference = false;
            else
                _isVariableReference = value.StartsWith( "{" ) && value.EndsWith( "}" );

            _editor.Control.Visible = !_isVariableReference;
            lblVariable.Visible = _isVariableReference;
            btnClear.Visible = _isVariableReference;

            if ( _isVariableReference )
                lblVariable.Text = _resolveVariable( value );
        }


        
        /*
            This event handler is needed to compensate for a problem in the parent container control where small scrollbar movements,
            and control repositions as the result of tabbing/focus do not trigger a scroll event. The consumer uses this event as a 
            signal to execute code to realign the label panel with the value panel
        */
        protected override void OnMove( EventArgs e ) {
            base.OnMove( e );
            SyncLabels?.Invoke();
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

            btnClear.Top = 1;
            btnClear.Left = Width - _proxy.PreferredHeight;
            btnClear.Height = _proxy.PreferredHeight - 2;
            btnClear.Width = _proxy.PreferredHeight - 2;

            lblVariable.Top = ( Height - lblVariable.PreferredHeight ) / 2;
            lblVariable.Left = 1;
            lblVariable.Height = lblVariable.PreferredHeight;
            lblVariable.Width = btnClear.Left - 1;

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

        #region
        public void ChildHasFocus( bool hasFocus ) {
            _hasFocus = hasFocus;
            Invalidate();
        }

        public void ChangeFocus( MoveFocus moveFocus ) {
            FocusChange?.Invoke( this, moveFocus );
        }

        #endregion
    }
}
