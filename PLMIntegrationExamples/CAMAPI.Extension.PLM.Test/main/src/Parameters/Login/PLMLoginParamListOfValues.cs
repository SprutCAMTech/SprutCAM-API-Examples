using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Parameters;

/// <summary>
/// Список значений для поля авторизации в PLM
/// </summary>
public class PLMLoginParamListOfValues : IPLMLoginParamListOfValues
{
    public int Count => loginParamValues.Count;

    public IPLMLoginParamValue this[int Index] => loginParamValues[Index];

    public PLMLoginParamListOfValues()
    {
        loginParamValues = new List<IPLMLoginParamValue>();
    }

    private List<IPLMLoginParamValue> loginParamValues;

    public void Load(IEnumerable<TempParamValue>? paramValues)
    {
        if(paramValues is null) return;
        
        foreach (var param in paramValues)
            loginParamValues.Add(new PLMLoginParamValue {
                Value = param.Value,
                DisplayName = param.DisplayName
            });
    }    
}
