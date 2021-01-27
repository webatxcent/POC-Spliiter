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
using XCENT.JobServer.Model;

namespace XCENT.JobServer.Manager.App
{
    public partial class ParameterEditor : UserControl
    {

        const int _margin = 4;
        List<GlobalDtoForView> _globals;
        List<Variable> _variables;
        List<ParameterDef> _parameterDefs;
        Parameters _parameters;

        public Parameters Parameters {
            get {
                Parameters parameters = new Parameters();

                foreach ( Control c in this.splitContainer.Panel2.Controls ) {
                    ParameterValue parameterValue = c as ParameterValue;
                    parameters.Add( new Parameter { Name = parameterValue.Name, Value = String.IsNullOrEmpty(parameterValue.Value) ? null: parameterValue.Value  } );
                }

                return parameters;
            }
        }

        public int PreferredHeight {
            get {
                var controls = splitContainer.Panel2.Controls;


                if ( controls.Count > 0 ) {
                    Control firstControl = controls[0];
                    Control lastControl = controls[controls.Count - 1];
                    return ( lastControl.Top + lastControl.Height ) - firstControl.Top + _margin;
                }
                else
                    return splitContainer.Panel2.Height;
            }
        }


        /// <summary>
        /// Returns false if any of the entered values violates the limits set on the data entry, such as being required.
        /// </summary>
        public bool IsValid {
            get {
                Parameters parameters = this.Parameters;

                if ( _parameterDefs != null ) {
                    foreach ( ParameterDef def in _parameterDefs ) {
                        if ( !def.IsRequired )
                            continue;
                        Parameter parameter = parameters.Find( m=>m.Name == def.Name);
                        if ( parameter == null )
                            return false;
                        if ( String.IsNullOrEmpty( parameter.Value ) )
                            return false;
                    }
                }

                return true;
            }
        }
        /// <summary>
        /// Returns true if any of the editor fields have been modified, and thus differing from the original.
        /// </summary>
        public bool HasChanged {
            get {
                foreach ( Control c in this.splitContainer.Panel2.Controls ) {
                    ParameterValue parameterValue = c as ParameterValue;
                    var originalParameterValue = _parameters.Find( m=>m.Name == parameterValue.Name );

                    //if there is no original parameter value for this particular named parameter, then there is definitely a change.
                    if ( originalParameterValue == null )
                        return true;

                    //if the original parameter value does not match the current parameter value in the editor, then there is a change.
                    if ( parameterValue.Value != originalParameterValue.Value )
                        return true;
                }
                return false;
            }
        }


        public ParameterEditor() {
            InitializeComponent();
            splitContainer.GotFocus += SplitContainer_GotFocus;

        }

        private void SplitContainer_GotFocus( object sender, EventArgs e ) {
            if ( splitContainer.Panel2.Controls.Count > 0 )
                splitContainer.Panel2.Controls[ 0 ].Focus();
        }

        public void LoadParameterData( List<ParameterDef> parameterDefs, Parameters parameters, List<Variable> variables, List<GlobalDtoForView> globals ) {

            SuspendLayout();

            //clean out any controls first.
            while ( splitContainer.Panel2.Controls.Count > 0 ) {
                Control value = splitContainer.Panel2.Controls[0];
                Control label = splitContainer.Panel1.Controls[0];
                value.Tag = null;
                label.Tag = null;
                splitContainer.Panel2.Controls.RemoveAt( 0 );
                splitContainer.Panel1.Controls.RemoveAt( 0 );
            }


            _variables = variables;
            _globals = globals;
            _parameterDefs = parameterDefs;
            _parameters = parameters;

            //clean up any possible mismatch between defs and actual values. 
            //start by removing any parameters that do not have a corresponding ParameterDef
            //TODO: WEB This should be replaced with a call to a new proposed member on Parameters to do this cleanup 
            List<Parameter> paramsCopy = new Parameters( parameters ); //create a deep copy for enumeration purposes.
            foreach ( Parameter param in paramsCopy ) {
                if ( parameterDefs.Find( m => m.Name == param.Name ) == null )
                    parameters.Remove( param );
            }
            foreach ( ParameterDef def in parameterDefs ) {
                if ( parameters.Find( m => m.Name == def.Name ) == null )
                    parameters.Add( new Parameter { Name = def.Name, Value = null } );
            }


            foreach ( ParameterDef def in parameterDefs ) {
                if ( def.ModuleParameterDirection == ModuleParameterDirection.Out )
                    continue;

                Parameter param = parameters.Find( m => m.Name == def.Name );

                ParameterValue parameterValue = new ParameterValue( def, param.Value, new ResolveVariable( Resolver ), 2 );
                ParameterLabel parameterLabel = new ParameterLabel( def.Name, def.Caption, def.IsRequired, !String.IsNullOrEmpty( def.Description ), def.ModuleParameterType == ModuleParameterType.String, 2 );

                AddParameter( parameterLabel, parameterValue );
            }
            LayoutControls();
            ResumeLayout();
        }

        string Resolver( string variableName ) {
            if ( variableName.StartsWith( "{{::" ) ) {
                var name = variableName.Replace("{{::", "").Replace("}}", "");
                var global = _globals.Find( m => m.Symbol == name );
                if ( global == null )
                    return "<undefined>";
                else
                    return global.Value;
            }
            else {
                var name = variableName.Replace( "{{", "").Replace("}}", "");
                var variable = _variables.Find( m=>m.Name == name);
                if ( variable == null )
                    return "<undefined>";
                else
                    return variable.Name;
            }
        }

        void AddParameter( ParameterLabel parameterLabel, ParameterValue parameterValue ) {
            parameterLabel.Tag = parameterValue;
            splitContainer.Panel1.Controls.Add( parameterLabel );
            parameterLabel.ShowHelp += OnShowHelp;
            parameterLabel.SetFormula += OnSetFormula;
            parameterLabel.TabStop = false;

            parameterValue.Tag = parameterLabel;
            splitContainer.Panel2.Controls.Add( parameterValue );
            parameterValue.FocusChange += OnFocusChange;
            parameterValue.SyncLabels += OnSyncLabels;
            parameterValue.SetFormula += OnSetFormula;
        }

        protected override void DestroyHandle() {
            //make sure the control cross referencing tags are set to null to avoid memory leaks.
            foreach ( Control control in splitContainer.Panel1.Controls )
                control.Tag = null;
            foreach ( Control control in splitContainer.Panel2.Controls )
                control.Tag = null;
            base.DestroyHandle();
        }

        void OnValuePanelPaint( object sender, System.Windows.Forms.PaintEventArgs e ) {
            ResizeValuePanelControls();
            ResizeLabelPanelControls();
            SynchronizeLabelPositions();
        }

        private Point _prevPan2Pos = new Point();

        private void SynchronizeLabelPositions() {
            if ( splitContainer.Panel2.AutoScrollPosition != _prevPan2Pos ) {

                int nextTop = 0;
                foreach ( Control control in splitContainer.Panel2.Controls ) {
                    ParameterValue parameterValue = control as ParameterValue;
                    ParameterLabel parameterLabel = parameterValue.Tag as ParameterLabel;
                    parameterLabel.Top = splitContainer.Panel2.AutoScrollPosition.Y + nextTop;

                    nextTop += parameterValue.Height + _margin;
                }
                _prevPan2Pos = splitContainer.Panel2.AutoScrollPosition;
            }
        }


        /// <summary>
        /// this method is called when some action causes the right panel(2) to change in size
        /// where the edit controls need to be resized in width. Mainly this will be due to 
        /// the user resize the container, or move the splitter bar.
        /// </summary>
        private void ResizeValuePanelControls() {

            int scrollbarOffset = 0;
            if ( splitContainer.Panel2.VerticalScroll.Visible )
                scrollbarOffset = SystemInformation.VerticalScrollBarWidth;

            foreach ( Control control in splitContainer.Panel2.Controls ) {
                ParameterValue parameterValue = ( control as ParameterValue );
                parameterValue.Width = splitContainer.Panel2.Width - scrollbarOffset;
            }
        }

        /// <summary>
        /// this method is called when some motion (or repaint) is triggered in the right panel(2)
        /// that would cause the controls for editing are repositioned, and the labels are now 
        /// out of alignment in the vertical and need to be realigned.
        /// </summary>
        private void ResizeLabelPanelControls() {

            foreach ( Control control in splitContainer.Panel1.Controls ) {
                ParameterLabel parameterLabel  = ( control as ParameterLabel );
                ParameterValue parameterValue = parameterLabel.Tag as ParameterValue;
                parameterLabel.Height = parameterValue.Height;
            }
        }

        protected override void OnCreateControl() {

            base.OnCreateControl();

            //these handlers are part of the fix for the focus problem with the splitter container sticking on the splitter bar.
            splitContainer.MouseDown += splitContainer_MouseDown;
            splitContainer.MouseUp += splitContainer_MouseUp;

            //this sets the initial splitter position, this should probably be externalized and saved off so that it is persisted as user preferences.
            splitContainer.SplitterDistance = (int)(Width * 0.4);
            splitContainer.SplitterWidth = _margin;

            //these events setup reposition and resize activities as a result of scrolling and movement of the splitter bar.
            splitContainer.Panel2.Paint += OnValuePanelPaint;
            splitContainer.SplitterMoved += OnSplitterMoved;

            splitContainer.Panel1.AutoScroll = false; //we will scroll this one by hand since setting this to true will show a redundant scroll bar.
            splitContainer.Panel2.AutoScroll = true;

            #region Test control layout - for dev use only

            /*
                        int i;
                        //create the required controls
                        for ( i = 0; i < 20; i++ ) {

                            ParameterDef defTextBox = new ParameterDef() { Name = $"Param{i}", ModuleParameterType = ModuleParameterType.String };
                            ParameterValue parameterValue = new ParameterValue(  defTextBox, $"Parameter Value #{i}" );
                            ParameterLabel parameterLabel = new ParameterLabel( $"Param{i}", $"Parameter Label #{i}", ( i % 2 ) == 0, 2 );

                            parameterLabel.Tag = parameterValue;
                            splitContainer.Panel1.Controls.Add( parameterLabel );
                            parameterLabel.ShowHelp += OnShowHelp;
                            parameterLabel.SetFormula += OnSetFormula;
                            parameterLabel.TabStop = false;

                            parameterValue.Tag = parameterLabel;
                            splitContainer.Panel2.Controls.Add( parameterValue );
                            parameterValue.FocusChange += OnFocusChange;
                            parameterValue.SyncLabels += OnSyncLabels;
                        }

                        i++;
                        ParameterDef defUnReqCheckbox = new ParameterDef() { Name = $"Param{i}", IsRequired = false, ModuleParameterType = ModuleParameterType.Bool };
                        ParameterValue parameterUnReqCheckBoxValue = new ParameterValue(  defUnReqCheckbox, null );
                        ParameterLabel parameterUnReqCheckBoxLabel = new ParameterLabel( $"Param{i}", $"Checkbox {i}", defUnReqCheckbox.IsRequired, 2 );

                        parameterUnReqCheckBoxLabel.Tag = parameterUnReqCheckBoxValue;
                        splitContainer.Panel1.Controls.Add( parameterUnReqCheckBoxLabel );
                        parameterUnReqCheckBoxLabel.ShowHelp += OnShowHelp;
                        parameterUnReqCheckBoxLabel.SetFormula += OnSetFormula;
                        parameterUnReqCheckBoxLabel.TabStop = false;

                        parameterUnReqCheckBoxValue.Tag = parameterUnReqCheckBoxLabel;
                        splitContainer.Panel2.Controls.Add( parameterUnReqCheckBoxValue );
                        parameterUnReqCheckBoxValue.FocusChange += OnFocusChange;
                        parameterUnReqCheckBoxValue.SyncLabels += OnSyncLabels;


                        i++;
                        ParameterDef defReqCheckbox = new ParameterDef() { Name = $"Param{i}", IsRequired = true, ModuleParameterType = ModuleParameterType.Bool };
                        ParameterValue parameterReqCheckBoxValue = new ParameterValue(  defReqCheckbox, null );
                        ParameterLabel parameterReqCheckBoxLabel = new ParameterLabel( $"Param{i}", $"Checkbox {i}", defReqCheckbox.IsRequired, 2 );

                        parameterReqCheckBoxLabel.Tag = parameterReqCheckBoxValue;
                        splitContainer.Panel1.Controls.Add( parameterReqCheckBoxLabel );
                        parameterReqCheckBoxLabel.ShowHelp += OnShowHelp;
                        parameterReqCheckBoxLabel.SetFormula += OnSetFormula;
                        parameterReqCheckBoxLabel.TabStop = false;

                        parameterReqCheckBoxValue.Tag = parameterReqCheckBoxLabel;
                        splitContainer.Panel2.Controls.Add( parameterReqCheckBoxValue );
                        parameterReqCheckBoxValue.FocusChange += OnFocusChange;
                        parameterReqCheckBoxValue.SyncLabels += OnSyncLabels;

                        i++;
                        ParameterDef defReqDate = new ParameterDef() { Name = $"Param{i}", IsRequired = true, ModuleParameterType = ModuleParameterType.Date };
                        ParameterValue parameterReqDateValue = new ParameterValue(  defReqDate, null );
                        ParameterLabel parameterReqDateLabel = new ParameterLabel( $"Param{i}", $"Date {i}", defReqDate.IsRequired, 2 );

                        parameterReqDateLabel.Tag = parameterReqDateValue;
                        splitContainer.Panel1.Controls.Add( parameterReqDateLabel );
                        parameterReqDateLabel.ShowHelp += OnShowHelp;
                        parameterReqDateLabel.SetFormula += OnSetFormula;
                        parameterReqDateLabel.TabStop = false;

                        parameterReqDateValue.Tag = parameterReqDateLabel;
                        splitContainer.Panel2.Controls.Add( parameterReqDateValue );
                        parameterReqDateValue.FocusChange += OnFocusChange;
                        parameterReqDateValue.SyncLabels += OnSyncLabels;

                        i++;
                        ParameterDef defUnReqDate = new ParameterDef() { Name = $"Param{i}", IsRequired = false, ModuleParameterType = ModuleParameterType.Date };
                        ParameterValue parameterUnReqDateValue = new ParameterValue(  defUnReqDate, null );
                        ParameterLabel parameterUnReqDateLabel = new ParameterLabel( $"Param{i}", $"Date {i}", defUnReqDate.IsRequired, 2 );

                        parameterUnReqDateLabel.Tag = parameterUnReqDateValue;
                        splitContainer.Panel1.Controls.Add( parameterUnReqDateLabel );
                        parameterUnReqDateLabel.ShowHelp += OnShowHelp;
                        parameterUnReqDateLabel.SetFormula += OnSetFormula;
                        parameterUnReqDateLabel.TabStop = false;

                        parameterUnReqDateValue.Tag = parameterUnReqDateLabel;
                        splitContainer.Panel2.Controls.Add( parameterUnReqDateValue );
                        parameterUnReqDateValue.FocusChange += OnFocusChange;
                        parameterUnReqDateValue.SyncLabels += OnSyncLabels;

                        i++;
                        int j = i;
                        for( ; j <= i + 1; j++ ) {

                            string labelText ="";
                            ParameterDef defNumber = null;
                            ParameterValue parameterNumberValue = null;

                            if ( j == i ) {
                                defNumber = new ParameterDef() { Name = $"Param{j}", IsRequired = false, ModuleParameterType = ModuleParameterType.Number, DecimalPlaces = 3, MinValue= 0, MaxValue = 100 };
                                parameterNumberValue = new ParameterValue(  defNumber, "50" );
                                labelText = $"Number d3 0<=x<=100 {j}";
                            }
                            else if ( j == i + 1 ) {
                                defNumber = new ParameterDef() { Name = $"Param{j}", IsRequired = true, ModuleParameterType = ModuleParameterType.Number, DecimalPlaces = 2, MinValue = 0, MaxValue = 1000 };
                                parameterNumberValue = new ParameterValue( defNumber, "500" );
                                labelText = $"Number d2 0<=x<=1000 {j}";
                            }

                            ParameterLabel parameterNumberLabel = new ParameterLabel( $"Param{j}", labelText, defNumber.IsRequired, 2 );

                            parameterNumberLabel.Tag = parameterNumberValue;
                            splitContainer.Panel1.Controls.Add( parameterNumberLabel );
                            parameterNumberLabel.ShowHelp += OnShowHelp;
                            parameterNumberLabel.SetFormula += OnSetFormula;
                            parameterNumberLabel.TabStop = false;

                            parameterNumberValue.Tag = parameterNumberLabel;
                            splitContainer.Panel2.Controls.Add( parameterNumberValue );
                            parameterNumberValue.FocusChange += OnFocusChange;
                            parameterNumberValue.SyncLabels += OnSyncLabels;
                        }

                        i = j;

                        List<string> choices = new List<string>();
                        choices.Add( "Before" );
                        choices.Add( "During" );
                        choices.Add( "After" );
                        ParameterDef defSingleChoice = new ParameterDef() { Name = $"Param{i}", IsRequired = false, ModuleParameterType = ModuleParameterType.Choice, Choices = choices };
                        ParameterValue parameterSingleChoiceValue = new ParameterValue(  defSingleChoice, "During" );
                        ParameterLabel parameterSingleChoiceLabel = new ParameterLabel( $"Param{i}", $"Single Choice {i}", defSingleChoice.IsRequired, 2 );

                        parameterSingleChoiceLabel.Tag = parameterSingleChoiceValue;
                        splitContainer.Panel1.Controls.Add( parameterSingleChoiceLabel );
                        parameterSingleChoiceLabel.ShowHelp += OnShowHelp;
                        parameterSingleChoiceLabel.SetFormula += OnSetFormula;
                        parameterSingleChoiceLabel.TabStop = false;

                        parameterSingleChoiceValue.Tag = parameterSingleChoiceLabel;
                        splitContainer.Panel2.Controls.Add( parameterSingleChoiceValue );
                        parameterSingleChoiceValue.FocusChange += OnFocusChange;
                        parameterSingleChoiceValue.SyncLabels += OnSyncLabels;

                        i++;
                        choices = new List<string>();
                        choices.Add( "Create" );
                        choices.Add( "Modify" );
                        choices.Add( "Delete" );
                        choices.Add( "Rename" );
                        ParameterDef defMultiChoice = new ParameterDef() { Name = $"Param{i}", IsRequired = false, ModuleParameterType = ModuleParameterType.Choice, Choices = choices, AllowMultipleSelections = true };
                        ParameterValue parameterMultiChoiceValue = new ParameterValue(  defMultiChoice, "Create, Modify" );
                        ParameterLabel parameterMultiChoiceLabel = new ParameterLabel( $"Param{i}", $"Multi Choice {i}", defMultiChoice.IsRequired, 2 );

                        parameterMultiChoiceLabel.Tag = parameterMultiChoiceValue;
                        splitContainer.Panel1.Controls.Add( parameterMultiChoiceLabel );
                        parameterMultiChoiceLabel.ShowHelp += OnShowHelp;
                        parameterMultiChoiceLabel.SetFormula += OnSetFormula;
                        parameterMultiChoiceLabel.TabStop = false;

                        parameterMultiChoiceValue.Tag = parameterMultiChoiceLabel;
                        splitContainer.Panel2.Controls.Add( parameterMultiChoiceValue );
                        parameterMultiChoiceValue.FocusChange += OnFocusChange;
                        parameterMultiChoiceValue.SyncLabels += OnSyncLabels;


                        //perform an initial positioning on them.
                        int nextTop = 0;
                        foreach ( Control control in splitContainer.Panel2.Controls ) {
                            ParameterValue parameterValue = control as ParameterValue;
                            ParameterLabel label = parameterValue.Tag as ParameterLabel;

                            parameterValue.Top = nextTop;
                            parameterValue.Left = 0;
                            parameterValue.Width = splitContainer.Panel2.Width;

                            label.Top = nextTop;
                            label.Left = 0;
                            label.Width = splitContainer.Panel1.Width;

                            nextTop += parameterValue.Height + _margin;
                        }
            */
            #endregion

        }

        protected override void OnEnter( EventArgs e ) {
            base.OnEnter( e );
            if ( splitContainer.Panel2.Controls.Count > 0 )
                splitContainer.Panel2.Controls[ 0 ].Focus();
        }

        private void LayoutControls() {
            int nextTop = 0;
            foreach ( Control control in splitContainer.Panel2.Controls ) {
                ParameterValue parameterValue = control as ParameterValue;
                ParameterLabel label = parameterValue.Tag as ParameterLabel;

                parameterValue.Top = nextTop;
                parameterValue.Left = 0;
                parameterValue.Width = splitContainer.Panel2.Width;

                label.Top = nextTop;
                label.Left = 0;
                label.Width = splitContainer.Panel1.Width;

                nextTop += parameterValue.Height + _margin;
            }
        }


        private void OnSyncLabels() {
            SynchronizeLabelPositions();
        }

        private void OnFocusChange( ParameterValue source, MoveFocus moveFocus ) {

            if ( moveFocus == MoveFocus.First ) {
                splitContainer.Panel2.Controls[ 0 ].Focus();
            }
            else if ( moveFocus == MoveFocus.Last ) {
                splitContainer.Panel2.Controls[ splitContainer.Panel2.Controls.Count - 1 ].Focus();
            }
            else if ( moveFocus == MoveFocus.Next ) {
                for ( int i = 0; i < splitContainer.Panel2.Controls.Count - 1; i++ ) {
                    ParameterValue parameterValue = splitContainer.Panel2.Controls[i] as ParameterValue;
                    if ( parameterValue.Name == source.Name ) {
                        splitContainer.Panel2.Controls[ i + 1 ].Focus();
                    }
                }
            }
            else if ( moveFocus == MoveFocus.Previous ) {
                for ( int i = 1; i < splitContainer.Panel2.Controls.Count; i++ ) {
                    ParameterValue parameterValue = splitContainer.Panel2.Controls[i] as ParameterValue;
                    if ( parameterValue.Name == source.Name ) {
                        splitContainer.Panel2.Controls[ i - 1 ].Focus();
                    }
                }
            }
        }

        private void OnSetFormula( ParameterValue valueControl, string name ) {

            if ( _globals.Count == 0 && _variables.Count == 0 ) {
                MessageBox.Show( "Sorry, there are no globals defined or variables available at this step.", "No Globals or Variables", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }

            ParameterVariables dialog = new ParameterVariables( valueControl.ParameterDef.Caption, valueControl.Value, _globals, _variables );
            dialog.StartPosition = FormStartPosition.CenterScreen;
            DialogResult result = dialog.ShowDialog();

            if ( result == DialogResult.OK ) {
                valueControl.Value = dialog.Reference;
            }
        }

        private void OnShowHelp( string name ) {
            ParameterDef def = _parameterDefs.Find( m=>m.Name == name);
            string message = def.Description;
            MessageBox.Show( message, $"Usage - {def.Caption}" );
        }

        /// <summary>
        /// This event handles resizing both the label controls and the value controls when the splitter bar is moved.
        /// </summary>
        private void OnSplitterMoved( object sender, SplitterEventArgs e ) {

            foreach ( Control control in splitContainer.Panel2.Controls ) {
                ParameterValue parameterValue = control as ParameterValue;
                ParameterLabel label = parameterValue.Tag as ParameterLabel;

                int scrollbarOffset = 0;
                if ( splitContainer.Panel2.VerticalScroll.Visible )
                    scrollbarOffset = SystemInformation.VerticalScrollBarWidth;

                parameterValue.Width = splitContainer.Panel2.Width - scrollbarOffset;
                label.Left = 0;
                label.Width = splitContainer.Panel1.Width;
            }
        }

        #region Fix for focus sticking on the splitter bar.

        private Control focused = null;

        private void splitContainer_MouseDown( object sender, MouseEventArgs e ) {
            // Get the focused control before the splitter is focused
            focused = getFocused( this.Controls );
        }

        private Control getFocused( Control.ControlCollection controls ) {
            foreach ( Control c in controls ) {
                if ( c.Focused ) {
                    // Return the focused control
                    return c;
                }
                else if ( c.ContainsFocus ) {
                    // If the focus is contained inside a control's children
                    // return the child
                    return getFocused( c.Controls );
                }
            }
            // No control on the form has focus
            return null;
        }

        private void splitContainer_MouseUp( object sender, MouseEventArgs e ) {
            // If a previous control had focus
            if ( focused != null ) {
                // Return focus and clear the temp variable for 
                // garbage collection
                focused.Focus();
                focused = null;
            }
        }

        #endregion

    }
}
