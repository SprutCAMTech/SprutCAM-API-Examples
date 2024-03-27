using SprutCAMTech.CAMAPI.Extensions;
using SprutCAMTech.CAMAPI.ResultStatus;

namespace CAMAPI.Extension.PLM.Test;

/// <summary>
/// Создает и передает расширение в SprutCAM
/// </summary>
public class ExtensionFactory : IExtensionFactory
{
    public IExtension Create(string ExtensionIdent, out TResultStatus ret)
    {
        ret = new TResultStatus
        {
            Code = 0            
        };
        
        var ext = new CSharpPLMExtension();
        return ext;
    }
}
