using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Parameters;

/// <summary>
/// Параметры подключения к PLM
/// </summary>
public class PLMConnectionParameters : IPLMConnectionParameters
{
    public int Count => connectionParameters.Count;

    public IPLMConnectionParameter this[int Index] => connectionParameters[Index];

    public PLMConnectionParameters()
    {
        connectionParameters = new List<IPLMConnectionParameter>();
    }

    private List<IPLMConnectionParameter> connectionParameters;

    public void Load(IEnumerable<TempParam> connectionParams)
    {
        foreach (var param in connectionParams)
            connectionParameters.Add(new PLMConnectionParameter {
                Id = param.Id,
                Name = param.Name,
                DefaultValue = param.DefaultValue,
                Order = param.Order
            });
    }   
}
