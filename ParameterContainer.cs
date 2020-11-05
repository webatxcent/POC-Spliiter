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

        private Point _prevPan2Pos = new Point();

        void OnValuePanelPaint( object sender, System.Windows.Forms.PaintEventArgs e ) {
            ResizeValuePanelControls();
            ResizeLabelPanelControls();
            SynchronizeLabelPositions();
        }

        private void SynchronizeLabelPositions() {
            if ( splitContainer.Panel2.AutoScrollPosition != _prevPan2Pos ) {

                int nextTop = 0;
                foreach ( Control control in splitContainer.Panel2.Controls ) {
                    TextBox textbox = control as TextBox;
                    ParameterLabel parameterLabel = textbox.Tag as ParameterLabel;
                    parameterLabel.Top = splitContainer.Panel2.AutoScrollPosition.Y + nextTop;

                    nextTop += textbox.Height + _margin;
                }
                _prevPan2Pos = splitContainer.Panel2.AutoScrollPosition;
            }
        }

        private void ResizeValuePanelControls() {

            int scrollbarOffset = 0;
            if ( splitContainer.Panel2.VerticalScroll.Visible )
                scrollbarOffset = SystemInformation.VerticalScrollBarWidth;

            foreach ( Control control in splitContainer.Panel2.Controls ) {
                TextBox textbox = ( control as TextBox );
                textbox.Width = splitContainer.Panel2.Width - scrollbarOffset;
            }
        }

        private void ResizeLabelPanelControls() {

            foreach ( Control control in splitContainer.Panel1.Controls ) {
                ParameterLabel label  = ( control as ParameterLabel );
                TextBox textbox = label.Tag as TextBox;
                label.Height = textbox.Height;
            }
        }

        protected override void OnCreateControl() {

            base.OnCreateControl();

            //these handlers are part of the fix for the focus problem with the splitter container sticking on the splitter bar.
            splitContainer.MouseDown += splitContainer_MouseDown;
            splitContainer.MouseUp += splitContainer_MouseUp;

            //this sets the initial splitter position, this should probably be externalized and saved off so that it is persisted as user preferences.
            splitContainer.SplitterDistance = Width / 3;

            //these events setup reposition and resize activities as a result of scrolling and movement of the splitter bar.
            splitContainer.Panel2.Paint += OnValuePanelPaint;
            splitContainer.SplitterMoved += OnSplitterMoved;

            splitContainer.Panel1.AutoScroll = false; //we will scroll this one by hand since setting this to true will show a redundant scroll bar.
            splitContainer.Panel2.AutoScroll = true;


            //create the required controls
            for ( int i = 0; i < 20; i++ ) {
                TextBox textbox = new TextBox();
                ParameterLabel label = new ParameterLabel( $"Param{i}", $"Parameter Label #{i}", ( i % 2) == 0, 4 );

                label.Tag = textbox;
                splitContainer.Panel1.Controls.Add( label );
                label.ShowHelp += OnShowHelp;
                label.SetFormula += OnSetFormula;
                label.TabStop = false;

                textbox.Tag = label;
                splitContainer.Panel2.Controls.Add( textbox );
                textbox.Text = $"Textbox #{i}";
            }

            //perform an initial positioning on them.
            int nextTop = 0;
            foreach ( Control control in splitContainer.Panel2.Controls ) {
                TextBox textbox = control as TextBox;
                ParameterLabel label = textbox.Tag as ParameterLabel;

                textbox.Top = nextTop;
                textbox.Left = 0;
                textbox.Width = splitContainer.Panel2.Width;

                label.Top = nextTop;
                label.Left = 0;
                label.Width = splitContainer.Panel1.Width;

                nextTop += textbox.Height + _margin;
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
                TextBox textbox = control as TextBox;
                ParameterLabel label = textbox.Tag as ParameterLabel;

                int scrollbarOffset = 0;
                if ( splitContainer.Panel2.VerticalScroll.Visible )
                    scrollbarOffset = SystemInformation.VerticalScrollBarWidth;

                textbox.Width = splitContainer.Panel2.Width - scrollbarOffset;
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
