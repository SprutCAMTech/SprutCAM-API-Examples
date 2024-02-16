namespace SprutCAMPlugins; //Do not rename this namespace!

using SprutTechnology.STLibraryTypes;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Plugin class attributes.
/// The PluginID and PluginType attributes are required for plugin class instances.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SprutCAMPluginAttribute: System.Attribute
{
    public string PluginID { get; set; } = Guid.Empty.ToString();
    public string PluginType { get; set; } = Guid.Empty.ToString();
    public string PluginCaption { get; set; } = "";
    public string PluginDescription { get; set; } = "";
}

/// <summary>
/// Base class for registering and calling plugins.
/// </summary>
public class CAMPluginsFactory : IST_CAMPluginsFactory
{
    public CAMPluginsFactory()
    {
        RegAllPlugins();
    }

    private void RegAllPlugins()
    {
        var a = Assembly.GetExecutingAssembly();
        foreach (TypeInfo c in a.DefinedTypes.Where(t => t.IsClass && t.IsPublic)) {
            var atr = c.GetCustomAttribute(typeof(SprutCAMPluginAttribute));
            if (atr != null) {
                var dsc = (SprutCAMPluginAttribute)atr;
                TPluginsEnumerator.Plugins.Add(new PluginDescriptor(
                    new Guid(dsc.PluginType),
                    new Guid(dsc.PluginID),
                    c.AsType(),
                    dsc.PluginCaption,
                    dsc.PluginDescription
                ));
            }
        }
    }

    /// IST_CAMPluginsFactory
    public IST_CAMPluginsEnumerator GetPluginsEnumeratorOfType(Guid PluginInterfaceID)
    {
        return new TPluginsEnumerator(PluginInterfaceID);
    }

    /// IST_CAMPluginsFactory
    public IST_CAMPlugin CreateInstanceOfPlugin(Guid PluginID)
    {
        return TPluginsEnumerator.CreateInstanceOfPlugin(PluginID);
    }
}

/// <summary>
/// The base abstract class of a plugins.
/// </summary>
public abstract class TAbstractPlugin : IST_CAMPlugin
{
    protected PluginDescriptor fDescriptor;

    public TAbstractPlugin()
    {
        foreach(var desc in TPluginsEnumerator.Plugins) {
            if (desc.PluginClass.Equals(this.GetType())) {
                fDescriptor = desc;
                break;
            }
        }
    }

    public Guid PluginID => fDescriptor.PluginID;

    public string PluginCaption => fDescriptor.PluginCaption;

    public string PluginDescription => fDescriptor.PluginDescription;
}

/// <summary>
/// Basic Plugin Attributes Structure.
/// </summary>
public struct PluginDescriptor
{
    public Guid PluginType;
    public Guid PluginID;
    public Type PluginClass;
    public string PluginCaption;
    public string PluginDescription;

    public PluginDescriptor(Guid pluginType, Guid pluginID, Type pluginClass, string pluginCaption, string pluginDescription)
    {
        PluginType = pluginType;
        PluginID = pluginID;
        PluginClass = pluginClass;
        PluginCaption = pluginCaption;
        PluginDescription = pluginDescription;
    }
}

/// <summary>
/// Basic enumerator for working with registered plugins.
/// </summary>
public class TPluginsEnumerator : IST_CAMPluginsEnumerator
{
    int index;
    Guid pluginType;

    public static List<PluginDescriptor> Plugins = new List<PluginDescriptor>();

    public TPluginsEnumerator(Guid PluginType)
    {
        index = -1;
        pluginType = PluginType;
    }

    /// IST_CAMPluginsEnumerator
    public bool MoveNext()
    {
        index++;
        while (index < Plugins.Count)
        {
            PluginDescriptor dsc = Plugins[index];
            if (dsc.PluginType == pluginType)
                break;
            index++;
        }
        return index < Plugins.Count;
    }

    /// IST_CAMPluginsEnumerator
    public Guid GetCurrent()
    {
        if (index < Plugins.Count)
            return Plugins[index].PluginID;
        else
            return Guid.Empty;
    }

    private static int IndexOfPlugin(Guid pluginID)
    {
        for (int i = 0; i < Plugins.Count; i++)
            if (Plugins[i].PluginID == pluginID)
                return i;
        return -1;
    }

    public static IST_CAMPlugin CreateInstanceOfPlugin(Guid PluginID)
    {
        IST_CAMPlugin? result = null;
        int i = IndexOfPlugin(PluginID);
        if (i >= 0)
            result = (IST_CAMPlugin)Activator.CreateInstance(Plugins[i].PluginClass)!;
        return result;
    }
}