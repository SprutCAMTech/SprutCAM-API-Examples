using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Parameters;

/// <summary>
/// Параметры отображения проекта PLM
/// </summary>
public class PLMProjectPreview : IPLMProjectPreview
{
    public int Width { get; set; }

    public int Height { get; set; }
}