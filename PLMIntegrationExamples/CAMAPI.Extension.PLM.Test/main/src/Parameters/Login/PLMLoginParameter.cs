using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Parameters;

/// <summary>
/// Описание поля для авторизации в PLM
/// </summary>
public class PLMLoginParameter : IPLMLoginParameter
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string DefaultValue { get; set; } = string.Empty;

    public int Order { get; set; }

    public bool Mandatory { get; set; }

    public bool Password { get; set; }

    public IPLMLoginParamListOfValues? LOV { get; set; } 
}
