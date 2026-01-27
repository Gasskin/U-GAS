using System;
using System.IO;
using cfg;
using Cysharp.Threading.Tasks;
using Luban;
using UnityEngine;

public class ConfigSystem : BaseSystem
{
    public Tables Tables { get; private set; }

    public override async UniTask Initialize()
    {
        Tables = new cfg.Tables(LoadByteBuf);

        Tables.ResolveCustomTables(LoadCustomBytes);

        foreach (var ge in Tables.TbGameEffect.DataList)
        {
            ge.AfterTableInitialize();
        }
        await UniTask.Yield();
    }

    public override void Destroy()
    {
    }

    private static ByteBuf LoadByteBuf(string file)
    {
        var handle = SystemDriver.YooSystem.LoadRawSync($"Assets/Config/Data/{file}.bytes");
        var bytes = handle.GetRawFileData();
        var buf = new ByteBuf(bytes);
        handle.Dispose();
        return buf;
    }

    private static byte[] LoadCustomBytes(string file)
    {
        var handle = SystemDriver.YooSystem.LoadRawSync($"Assets/Config/CustomData/{file}.bytes");
        var bytes = handle.GetRawFileData();
        handle.Dispose();
        return bytes;
    }

#if UNITY_EDITOR
    public void HotReload()
    {
        Tables.ResolveCustomTables(LoadCustomBytes);
    }
#endif
}