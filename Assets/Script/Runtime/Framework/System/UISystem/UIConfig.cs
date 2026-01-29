namespace Script.Runtime.Framework.System
{
    public class UIConfig
    {
        // 层级
        public EUILayer Layer;
        // 预制体路径
        public string Path;
        // 全屏界面，会关闭同层级下的其他界面
        public bool FullScreen = true;
        // 能否打开多个
        public bool CanMultiSpawn = false;
    }
}
