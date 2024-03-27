using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Test.Common;

public class PLMResult : IPLMResult
{
    public int Code { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public string WarningMessage { get; set; } = string.Empty;

    public void SetSuccessful(string warningMsg = "")
    {
        Code = 0;
        ErrorMessage = string.Empty;
        WarningMessage = warningMsg;
    }   
}
