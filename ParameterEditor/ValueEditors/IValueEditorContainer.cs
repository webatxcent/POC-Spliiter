using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Splitter
{
    public interface IValueEditorContainer
    {
        //indicate to the conainer that the child control has focus.
        void ChildHasFocus( bool hasFocus );
        void ChangeFocus( MoveFocus moveFocus );
    }
}
