namespace U_GAS
{
    public class UAssetInfo
    {
        public string selfKey;
        public string ownerType;
        public string deSerializeField;
    }
    
    
    public interface IUAsset
    {
        UAssetInfo GetUAssetInfo();
    }
}