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

namespace POC_Spliiter {
    public partial class ParameterContainer : UserControl {

        const int _margin = 4;

        public ParameterContainer() {
            InitializeComponent();
        }

        protected override void DestroyHandle() {
            //make sure the control cross referencing tags are set to null to avoid memory leaks.
            foreach ( Control control in splitContainer1.Panel1.Controls )
                control.Tag = null;
            foreach ( Control control in splitContainer1.Panel2.Controls )
                control.Tag = null;
            base.DestroyHandle();
        }

        private Point _prevPan2Pos = new Point();

        void OnPanelPaint( object sender, System.Windows.Forms.PaintEventArgs e ) {
            SynchronizeLabelPositions();
            ResizeDataEntryControls();
        }

        private void SynchronizeLabelPositions() {
            if ( splitContainer1.Panel2.AutoScrollPosition != _prevPan2Pos ) {

                int maxheight = GetMaxHeight() + ( _margin * 2 );

                int index =0;
                foreach ( Control control in splitContainer1.Panel2.Controls ) {
                    TextBox textbox = control as TextBox;
                    ParameterLabel parameterLabel = textbox.Tag as ParameterLabel;
                    parameterLabel.Top = splitContainer1.Panel2.AutoScrollPosition.Y + index++ * ( maxheight + 1 ) + ( maxheight - parameterLabel.Height ) / 2;
                }
                _prevPan2Pos = splitContainer1.Panel2.AutoScrollPosition;
            }
        }

        private void ResizeDataEntryControls() {
            int margin = 4;

            int scrollbarOffset = 0;
            if ( splitContainer1.Panel2.VerticalScroll.Visible )
                scrollbarOffset = SystemInformation.VerticalScrollBarWidth;

            foreach ( Control control in splitContainer1.Panel2.Controls ) {
                TextBox textbox = ( control as TextBox );
                textbox.Width = splitContainer1.Panel2.Width - margin - scrollbarOffset;
            }
        }

        protected override void OnCreateControl() {
          
            base.OnCreateControl();

            splitContainer1.SplitterDistance = Width / 2;

            splitContainer1.Panel2.Paint += OnPanelPaint;
            splitContainer1.Panel2.Scroll += ( obj, scrollEventArgs ) => SynchronizeLabelPositions();

            splitContainer1.Panel1.AutoScroll = false;
            splitContainer1.Panel2.AutoScroll = true;

            splitContainer1.SplitterMoved += OnSplitterMoved;

            int margin = 4;


            for( int i = 0; i < 20; i++ ) {
                TextBox textbox = new TextBox();
                ParameterLabel label = new ParameterLabel( $"Param{i}", $"Parameter Label #{i}", ( i % 2) == 0, textbox.Height, 4 );

                label.Tag = textbox;
                splitContainer1.Panel1.Controls.Add( label );
                label.ShowHelp += OnShowHelp;
                label.SetFormula += OnSetFormula;

                textbox.Text = $"Textbox #{i}";
                textbox.Tag = label;
                splitContainer1.Panel2.Controls.Add( textbox );
            }

            

            int maxheight = GetMaxHeight() + ( margin * 2 );
            int index = 0;
            foreach ( Control control in splitContainer1.Panel2.Controls ) {
                TextBox textbox = control as TextBox;
                ParameterLabel label = textbox.Tag as ParameterLabel;

                textbox.Top = index * ( maxheight + 1 ) + margin;
                textbox.Left = 0;
                textbox.Width = splitContainer1.Panel2.Width - margin;

                label.Top = index * ( maxheight + 1 ) + ( maxheight - label.Height ) / 2;
                label.Left = margin;
                label.Width = splitContainer1.Panel1.Width - margin;

                index++;

            }
        }

        private void OnSetFormula( string Name ) {
            MessageBox.Show( $"Show formula dialog for {Name}" );
        }

        private void OnShowHelp( string Name ) {
            MessageBox.Show( $"Show help for {Name}" );
        }

        private void OnSplitterMoved( object sender, SplitterEventArgs e ) {
            int margin = 4;
            foreach ( Control control in splitContainer1.Panel2.Controls ) {
                TextBox textbox = control as TextBox;
                ParameterLabel label = textbox.Tag as ParameterLabel;

                int scrollbarOffset = 0;
                if ( splitContainer1.Panel2.VerticalScroll.Visible )
                    scrollbarOffset = SystemInformation.VerticalScrollBarWidth;
                
                textbox.Width = splitContainer1.Panel2.Width - margin - scrollbarOffset;
                label.Left = margin;
                label.Width = splitContainer1.Panel1.Width - margin;
            }
        }

        int GetMaxHeight() {
            int result = 0;

            foreach ( Control control in splitContainer1.Panel2.Controls ) {
                TextBox textbox = control as TextBox;
                if ( textbox.Height > result )
                    result = textbox.Height;
            }
            return result;
        }

    }
}
