using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

namespace Script.Runtime.Framework.System
{
    public class YooSystem : BaseSystem
    {
        private ResourcePackage _defaultPackage;
        private ResourcePackage _rawPackage;
        private bool _waitForClose;

        public override async UniTask Initialize()
        {
            YooAssets.Initialize();

            _defaultPackage = YooAssets.CreatePackage("DefaultPackage");
            YooAssets.SetDefaultPackage(_defaultPackage);

            _rawPackage = YooAssets.CreatePackage("RawPackage");

            var mode = SystemDriver.Instance.YooAssetsMode;

            await InitPackage(_defaultPackage, mode);
            await InitPackage(_rawPackage, mode);
        }

        public override void Destroy()
        {
            _waitForClose = true;
            InternalClose().Forget();
            while (_waitForClose)
            {
                return;
            }
        }

        private async UniTaskVoid InternalClose()
        {
            var o1 = _defaultPackage.DestroyAsync();
            await o1.ToUniTask();
            var o2 = _rawPackage.DestroyAsync();
            await o2.ToUniTask();
            _waitForClose = false;
        }


        private async UniTask InitPackage(ResourcePackage package, EYooAssetsMode mode)
        {
            InitializeParameters createParameters = null;
            if (mode == EYooAssetsMode.Editor)
            {
                var buildResult = EditorSimulateModeHelper.SimulateBuild(package.PackageName);
                var packageRoot = buildResult.PackageRootDirectory;
                var p = new EditorSimulateModeParameters();
                p.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
                createParameters = p;
            }

            if (mode == EYooAssetsMode.Offline)
            {
                var p = new OfflinePlayModeParameters();
                p.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                createParameters = p;
            }

            var opt1 = package.InitializeAsync(createParameters);
            await opt1.ToUniTask();
            if (opt1.Status == EOperationStatus.Succeed)
            {
                var opt2 = package.RequestPackageVersionAsync();
                await opt2.ToUniTask();
                if (opt2.Status == EOperationStatus.Succeed)
                {
                    var opt3 = package.UpdatePackageManifestAsync(opt2.PackageVersion);
                    await opt3.ToUniTask();
                    if (opt3.Status == EOperationStatus.Succeed)
                    {
                        return;
                    }
                    throw new NullReferenceException(opt3.Error);
                }
                throw new NullReferenceException(opt2.Error);
            }

            throw new NullReferenceException(opt1.Error);
        }


        public RawFileHandle LoadRawSync(string path)
        {
            var handle = _rawPackage.LoadRawFileSync(path);
            if (handle.Status != EOperationStatus.Succeed)
            {
            }
            return handle;
        }

        public async UniTask<AssetHandle> LoadAssetAsync<T>(string path) where T : Object
        {
            var handle = _defaultPackage.LoadAssetAsync<T>(path);
            await handle.ToUniTask();
            if (handle.Status == EOperationStatus.Succeed && handle.AssetObject is T)
            {
                return handle;
            }
            handle.Dispose();
            return null;
        }
    
        public AssetHandle LoadAssetSync<T>(string path) where T : Object
        {
            var handle = _defaultPackage.LoadAssetSync<T>(path);
            if (handle.Status == EOperationStatus.Succeed && handle.AssetObject is T)
            {
                return handle;
            }
            handle.Dispose();
            return null;
        }

        public async UniTask<UnityEngine.GameObject> InitializeGameObjectAsync(Transform parent, string path)
        {
            var handle = _defaultPackage.LoadAssetAsync<UnityEngine.GameObject>(path);
            await handle.ToUniTask();
            if (handle.Status == EOperationStatus.Succeed)
            {
                var go = (UnityEngine.GameObject)Object.Instantiate(handle.AssetObject, parent);
                var handler = go.AddComponent<YooGameObjectHandler>();
                handler.Handle = handle;
                return go;
            }
            return null;
        }

        public UnityEngine.GameObject InitializeGameObjectSync(Transform parent, string path)
        {
            var handle = _defaultPackage.LoadAssetSync<UnityEngine.GameObject>(path);
            if (handle.Status == EOperationStatus.Succeed)
            {
                var go = (UnityEngine.GameObject)Object.Instantiate(handle.AssetObject, parent, false);
                var handler = go.AddComponent<YooGameObjectHandler>();
                handler.Handle = handle;
                return go;
            }
            return null;
        }
    }
}