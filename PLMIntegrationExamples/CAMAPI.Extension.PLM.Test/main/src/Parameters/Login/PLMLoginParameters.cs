using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Parameters;

/// <summary>
/// Поля для авторизации в PLM
/// </summary>
public class PLMLoginParameters : IPLMLoginParameters
{
    public int Count => loginParameters.Count;

    public IPLMLoginParameter this[int Index] => loginParameters[Index];

    public PLMLoginParameters()
    {
        loginParameters = new List<IPLMLoginParameter>();
    }

    private List<IPLMLoginParameter> loginParameters;

    public void Load(IEnumerable<TempLoginParam> loginParams, IEnumerable<TempParamValue>? paramValues)
    {
        foreach (var param in loginParams)
        {
            var loginParam = new PLMLoginParameter {
                Id = param.Id,
                Name = param.Name,
                DefaultValue = param.DefaultValue,
                Order = param.Order,
                Mandatory = param.Mandatory,
                Password = param.Password
            };

            if (loginParam.Id == "Locale")
            {
                var listOfValues = new PLMLoginParamListOfValues();
                listOfValues.Load(paramValues);
                loginParam.LOV = listOfValues;
            }                

            loginParameters.Add(loginParam);
        }
    }    
}
