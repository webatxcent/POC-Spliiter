using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCENT.JobServer.Abstract;

namespace POC_Splitter
{
    public delegate void ControlMoveFocusHandler( Control control, MoveFocus action );
    

    public interface IValueEditor
    {
        //provides access to parent functionalities.
        IValueEditorContainer ValueEditorContainer { get; set; }

        /// <summary>
        /// provides access to the base control so that common interactions can be performed such as tying into keyboard handlers and such.
        /// </summary>
        Control Control { get; }
        /// <summary>
        /// provides the means to inject the configuration data and the value into the implemented control.
        /// </summary>
        /// <param name="parameterDef"></param>
        /// <param name="value"></param>
        void Configure( ParameterDef parameterDef, string value );
        /// <summary>
        /// returns the final value of the control after editing is done.
        /// </summary>
        string Value { get; }
        /// <summary>
        /// The implemented control can determine its required height and report it back to the consumer.
        /// </summary>
        int PreferredHeight { get; }

        bool RequiresFocusRectangle { get; }

        bool WillHandleNavigation { get; }

    }
}
