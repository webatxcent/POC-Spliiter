using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XCENT.JobServer.Manager.App
{
    public partial class PESplitContainer : SplitContainer
    {
        //private bool _isMouseDown = false;
        //private Control _focused = null;

        public PESplitContainer() {

            //this triggers a focus to the first control after Panel2 gets focus, which is done by the overridden OnGotFocus
            Panel2.GotFocus += ( s, e ) => {
                //if ( _focused != null && _focused.Parent is ParameterValue ) {
                //    Debug.Print( $"Setting focus to {( _focused.Parent as ParameterValue ).Name}" );
                //    _focused.Parent.Focus();
                //    _focused = null;
                //}
                //else
                Panel2.Controls[ 0 ].Focus();
            };
        }


        //[DllImport( "user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi )]
        //internal static extern IntPtr GetFocus();

        //private Control GetFocusedControl() {
        //    Control focusedControl = null;
        //    // To get hold of the focused control:
        //    IntPtr focusedHandle = GetFocus();
        //    if ( focusedHandle != IntPtr.Zero )
        //        // Note that if the focused Control is not a .Net control, then this will return null.
        //        focusedControl = Control.FromHandle( focusedHandle );
        //    return focusedControl;
        //}

        ////it seems as if the only time that mouse down can trigger is when it is not one of the two panels hosted by this control,
        ////thus guaranteeing that we are in the "splitter" which is just a rectange that is rendered.
        //protected override void OnMouseDown( MouseEventArgs e ) {
        //    _isMouseDown = true;
        //    _focused = GetFocusedControl();
        //    base.OnMouseDown( e );
        //}

        //protected override void OnMouseUp( MouseEventArgs e ) {
        //    base.OnMouseUp( e );
        //    if ( _focused != null ) {
        //        // Return focus and clear the temp variable for 
        //        // garbage collection

        //        if ( _focused.Parent is ParameterValue || _focused is ParameterValue) {
        //            Panel2.Focus();
        //        }
        //        else {
        //            _focused.Focus();
        //            _focused = null;
        //        }
        //    }
        //    _isMouseDown = false;
        //}

        //When focus is shifted to the SplitContainer, we set focus to Panel2, 
        //note that because of internal workings of the SplitContainer, attaching to the event does not work.
        protected override void OnGotFocus( EventArgs e ) {
            //Debug.Print( "SplitContainer got focus." );
            //if ( !_isMouseDown )
            Panel2.Focus();
        }
    }
}
