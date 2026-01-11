using System.IO;
using cfg;
using Luban;
using UnityEngine;

public class DesignSystem : BaseSystem
{
    public Tables Tables { get; private set; }
    
    public override void Initialize()
    {
        Tables = new cfg.Tables(LoadByteBuf);

        foreach (var ge in Tables.TbGameEffect.DataList)
        {
            ge.AfterTableInitialize();
        }
    }

    public override void Close()
    {
    }
    
    private static ByteBuf LoadByteBuf(string file)
    {
        return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/Design/Data/{file}.bytes"));
    }
}