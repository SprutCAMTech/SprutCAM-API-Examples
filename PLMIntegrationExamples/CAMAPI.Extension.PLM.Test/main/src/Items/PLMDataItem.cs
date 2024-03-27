using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Items;

/// <summary>
/// Описание элемента данных из PLM
/// </summary>
public class PLMDataItem : IPLMDataItem
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public TPLMItemType Type { get; set; }

    public string TimeStamp { get; set; } = string.Empty;

    public IPLMFiles? Files { get; set; }
}
