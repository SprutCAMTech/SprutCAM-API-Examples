using System.Dynamic;
using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Parameters;

/// <summary>
/// Описание значения для поля авторизации в PLM
/// </summary>
public class PLMLoginParamValue : IPLMLoginParamValue
{
    public string Value { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;   
}
