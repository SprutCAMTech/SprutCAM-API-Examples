using SprutCAMPlugins;
using SprutTechnology.STLibraryTypes;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PluginsExample
{
    /// <summary> Plugin class example. </summary>
    /// The PluginID attribute must be unique. 
    /// The PluginType attribute must match the interface GUID, implemented in the plugin class. 
    /// All plugin classes must inherit from the TAbstractPlugin base class 
    /// and implement the IST_UtilitiesButtonCAMPlugin interface 
    /// (plugin interfaces may differ in their purpose).
    [SprutCAMPlugin(
        PluginID = "7581E851-9239-4820-A4DF-FBBADCAAD7DD",
        PluginType = "4B74BB21-9F48-4D62-9870-0A831C8AD2DA",
        PluginCaption = "Plugin 1 caption",
        PluginDescription = "Plugin 1 description"
    )]
    public class TPlugin1Class : TAbstractPlugin, IST_UtilitiesButtonCAMPlugin
    {
        /// IST_UtilitiesButtonCAMPlugin
        /// <summary> An event called when the plugin button is clicked in the SprutCAM client. </summary>
        public void OnButtonClick(object SenderApplication)
        {
            //var app = (IST_Application) SenderApplication;
            TUtils.ShowMessage("TPlugin1Class button click!", "PluginsExample", 0);
        }
    }

    /// <summary> Plugin class example. </summary>
    [SprutCAMPlugin(
        PluginID = "462D862C-2095-41DA-B79A-54D424194EA5",
        PluginType = "4B74BB21-9F48-4D62-9870-0A831C8AD2DA",
        PluginCaption = "Plugin 2 caption",
        PluginDescription = "Plugin 2 description"
    )]
    public class TPlugin2Class : TAbstractPlugin, IST_UtilitiesButtonCAMPlugin
    {
        // IST_UtilitiesButtonCAMPlugin
        /// <summary> An event called when the plugin button is clicked in the SprutCAM client. </summary>
        public void OnButtonClick(object SenderApplication)
        {
            //var app = (IST_Application) SenderApplication;
            TUtils.ShowMessage("TPlugin2Class button click!", "PluginsExample", 0);
        }
    }

    /// <summary> For example, not mandatory. </summary>
    public class TUtils {

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        public static int ShowMessage(string message, string caption, int msgType) {
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            int res = MessageBox(handle, message, caption, msgType);
            return res;
        }
    }

}