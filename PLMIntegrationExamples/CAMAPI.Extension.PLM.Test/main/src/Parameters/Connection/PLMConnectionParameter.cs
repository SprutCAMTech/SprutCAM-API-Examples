using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Parameters;

/// <summary>
/// Описание параметра подключения к PLM
/// </summary>
public class PLMConnectionParameter : IPLMConnectionParameter
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string DefaultValue { get; set; } = string.Empty;

    public int Order { get; set; }
}
