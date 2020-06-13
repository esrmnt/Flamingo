namespace Flamingo
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    [Guid("0dc5c4f4-138e-41ff-bb6b-3dfdea5d0844")]
    public class FlamingoProperties : ToolWindowPane
    {        
        public FlamingoProperties() : base(null)
        {
            this.Caption = "Flamingo";
            this.Content = new FlamingoPropertiesControl();
        }
    }
}
