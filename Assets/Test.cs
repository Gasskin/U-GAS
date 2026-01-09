using System.IO;
using cfg;
using Luban;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Tables _tables;
    
    void Start()
    {
        _tables = new cfg.Tables(LoadByteBuf);

        foreach (var ge in _tables.TbGameEffect.DataList)
        {
            Debug.Log(ge.Test1.GetType().Name);
        }
    }

    private static ByteBuf LoadByteBuf(string file)
    {
        return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/Design/Data/{file}.bytes"));
    }

}
