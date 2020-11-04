using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POC_Spliiter {
    public partial class ParameterContainer : UserControl {

        bool controlsCreated = false;

        class TagData {
            public Control LinkedControl { get; set; }
            public int Position { get; set; }

            public TagData( Control linkedControl, int position ) {
                LinkedControl = linkedControl;
                Position = position;
            }
        }


        List<Label> labels = new List<Label>();
        List<TextBox> textboxes = new List<TextBox>();

        public ParameterContainer() {
            InitializeComponent();
        }

        private Point _prevPan2Pos = new Point();

        void PanelPaint( object sender, System.Windows.Forms.PaintEventArgs e ) {
            SynchronizeSplitContainerScrollbars();
        }

        private void SynchronizeSplitContainerScrollbars() {
            if ( splitContainer1.Panel2.AutoScrollPosition != _prevPan2Pos ) {
                int margin = 4;
                int maxheight = textboxes.Max( m => m.Height ) + ( margin * 2 );
                foreach ( TextBox textbox in textboxes ) {
                    TagData tagdata = textbox.Tag as TagData;
                    Label label = tagdata.LinkedControl as Label;
                    int position = tagdata.Position;
                    label.Top = splitContainer1.Panel2.AutoScrollPosition.Y + position * ( maxheight + 1 ) + ( maxheight - label.Height ) / 2;
                }


                _prevPan2Pos = splitContainer1.Panel2.AutoScrollPosition;
            }
        }

        protected override void OnCreateControl() {
          
            base.OnCreateControl();

            splitContainer1.SplitterDistance = Width / 2;

            splitContainer1.Panel2.Paint += PanelPaint;
            splitContainer1.Panel2.Scroll += ( obj, scrollEventArgs ) => SynchronizeSplitContainerScrollbars();

            splitContainer1.Panel1.AutoScroll = false;
            splitContainer1.Panel2.AutoScroll = true;

            splitContainer1.SplitterMoved += SplitContainer1_SplitterMoved;


            for( int i = 0; i < 20; i++ ) {
                Label label = new Label();
                TextBox textbox = new TextBox();

                label.Text = $"Label #{i}";
                label.Tag = new TagData( textbox, i );
                splitContainer1.Panel1.Controls.Add( label );
                labels.Add( label );
                label.Click += LabelClick;


                textbox.Text = $"Textbox #{i}";
                textbox.Tag = new TagData( label, i );
                splitContainer1.Panel2.Controls.Add( textbox );
                textboxes.Add( textbox );
            }

            controlsCreated = true;
            LayoutControls();
        }

        private void LabelClick( object sender, EventArgs e ) {
            Label label = sender as Label;
            ( ( label.Tag as TagData ).LinkedControl as TextBox ).Focus();
        }

        private void SplitContainer1_SplitterMoved( object sender, SplitterEventArgs e ) {
            int margin = 4;
            int maxheight = textboxes.Max( m => m.Height ) + ( margin * 2 );
            foreach ( TextBox textbox in textboxes ) {
                TagData tagdata = textbox.Tag as TagData;
                Label label = tagdata.LinkedControl as Label;
                int position = tagdata.Position;

                textbox.Width = splitContainer1.Panel2.Width - margin;

                label.Left = margin;
                label.Width = splitContainer1.Panel1.Width - margin;
            }
        }

        void LayoutControls() {

            int margin = 4;
            int maxheight = textboxes.Max( m => m.Height ) + ( margin * 2 );
            foreach ( TextBox textbox in textboxes ) {
                TagData tagdata = textbox.Tag as TagData;
                Label label = tagdata.LinkedControl as Label;
                int position = tagdata.Position;

                textbox.Top = position * ( maxheight + 1 ) + margin;
                textbox.Left = 0;
                textbox.Width = splitContainer1.Panel2.Width - margin;

                label.Top = position * ( maxheight + 1 ) + ( maxheight - label.Height ) / 2;
                label.Left = margin;
                label.Width = splitContainer1.Panel1.Width - margin;
                label.TextAlign = ContentAlignment.MiddleRight;

           
            }

        }

    }
}
