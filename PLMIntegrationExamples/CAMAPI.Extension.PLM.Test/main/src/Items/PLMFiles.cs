using SprutCAMTech.CAMAPI.Extension.PLM;

namespace CAMAPI.Extension.PLM.Items;

/// <summary>
/// Список файлов из PLM
/// </summary>
public class PLMFiles : IPLMFiles
{
    public int Count => filePaths.Count;

    public string this[int Index] => filePaths[Index];

    public PLMFiles()
    {
        filePaths = new List<string>();
    }

    private List<string> filePaths;

    public void AddFiles(List<string> newFilePaths)
    {
        filePaths.AddRange(newFilePaths);
    }    
}
