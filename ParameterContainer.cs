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
    public partial class ParameterContainer : UserControl
    {

        const int _margin = 4;

        public ParameterContainer() {
            InitializeComponent();
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
            Debug.Print( "Paint" );
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

        private void ResizeValuePanelControls() {

            int scrollbarOffset = 0;
            if ( splitContainer.Panel2.VerticalScroll.Visible )
                scrollbarOffset = SystemInformation.VerticalScrollBarWidth;

            foreach ( Control control in splitContainer.Panel2.Controls ) {
                ParameterValue parameterValue = ( control as ParameterValue );
                parameterValue.Width = splitContainer.Panel2.Width - scrollbarOffset;
            }
        }

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
            splitContainer.SplitterDistance = Width / 3;
            splitContainer.SplitterWidth = _margin;

            //these events setup reposition and resize activities as a result of scrolling and movement of the splitter bar.
            splitContainer.Panel2.Paint += OnValuePanelPaint;
            splitContainer.SplitterMoved += OnSplitterMoved;

            splitContainer.Panel1.AutoScroll = false; //we will scroll this one by hand since setting this to true will show a redundant scroll bar.
            splitContainer.Panel2.AutoScroll = true;


            //create the required controls
            for ( int i = 0; i < 20; i++ ) {
                ParameterValue parameterValue = new ParameterValue();
                ParameterLabel parameterLabel = new ParameterLabel( $"Param{i}", $"Parameter Label #{i}", ( i % 2 ) == 0, 2 );

                parameterLabel.Tag = parameterValue;
                splitContainer.Panel1.Controls.Add( parameterLabel );
                parameterLabel.ShowHelp += OnShowHelp;
                parameterLabel.SetFormula += OnSetFormula;
                parameterLabel.TabStop = false;

                parameterValue.Tag = parameterLabel;
                splitContainer.Panel2.Controls.Add( parameterValue );
                parameterValue.Text = $"Parameter Value #{i}";
                parameterValue.Name = $"Param{i}";
                parameterValue.FocusChange += OnFocusChange;
                parameterValue.SyncLabels += OnSyncLabels;
            }

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

        private void OnSetFormula( string Name ) {
            MessageBox.Show( $"Show formula dialog for {Name}" );
        }

        private void OnShowHelp( string Name ) {
            MessageBox.Show( $"Show help for {Name}" );
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
