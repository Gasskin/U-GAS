using System.IO;
using cfg;
using Cysharp.Threading.Tasks;
using Luban;
using UnityEngine;

public class DesignSystem : BaseSystem
{
    public Tables Tables { get; private set; }
    
    public override async UniTask Initialize()
    {
        Tables = new cfg.Tables(LoadByteBuf);

        foreach (var ge in Tables.TbGameEffect.DataList)
        {
            ge.AfterTableInitialize();
        }
        
        await UniTask.Yield();
    }

    public override void Close()
    {
    }
    
    private static ByteBuf LoadByteBuf(string file)
    {
        var handle = SystemDriver.GetSystem<YooSystem>().LoadRawSync($"Assets/Design/Data/{file}.bytes");
        var bytes = handle.GetRawFileData();
        var buf = new ByteBuf(bytes);
        handle.Dispose();
        return buf;
    }
}