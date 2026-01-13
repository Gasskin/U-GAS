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

        Tables.ResolveTbSkillTimelineAsset(LoadBytes);

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
        var handle = SystemDriver.Get<YooSystem>().LoadRawSync($"Assets/Design/Data/{file}.bytes");
        var bytes = handle.GetRawFileData();
        var buf = new ByteBuf(bytes);
        handle.Dispose();
        return buf;
    }

    private static byte[] LoadBytes(string file)
    {
        var handle = SystemDriver.Get<YooSystem>().LoadRawSync($"Assets/Design/Data/{file}.bytes");
        var bytes = handle.GetRawFileData();
        handle.Dispose();
        return bytes;
    }
}