using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Tree;

/// <summary>
/// Элемент дерева PLM
/// </summary>
public class PLMTreeItem : IPLMTreeItem
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public TPLMItemType Type { get; set; }

    public string Comment { get; set; } = string.Empty;

    public bool ChildsLoaded { get; set; }

    public IPLMTree? Childs { get; set; }

}
