using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Items;

/// <summary>
/// Структура проекта PLM
/// </summary>
public class PLMProjectStructItems : IPLMProjectStructItems
{
    public int Count => projectStructItems.Count;

    public IPLMProjectStructItem this[int Index] => projectStructItems[Index];

    public PLMProjectStructItems()
    {
        projectStructItems = new List<IPLMProjectStructItem>();
    }

    private List<IPLMProjectStructItem> projectStructItems;
}
