using System.Collections.Generic;

namespace cfg.Gas
{
    public partial class GameEffect
    {
        public GameTagContainer AssetTagContainer;
        public GameTagContainer GrantedTagContainer;
        public GameTagContainer ApplyRequiredTagContainer;
        public GameTagContainer ImmuneWhenTagContainer;
        public GameTagContainer OnGoingRequiredTagContainer;
        public GameTagContainer RemoveGeWithTagContainer;
        public GameTagContainer BlockGeWithTagContainer;
        
        public void AfterTableInitialize()
        {
            AssetTagContainer = new GameTagContainer(Tags.Assets);
            AssetTagContainer = new GameTagContainer(Tags.Granted);
            AssetTagContainer = new GameTagContainer(Tags.ApplyRequired);
            AssetTagContainer = new GameTagContainer(Tags.ImmuneWhen);
            AssetTagContainer = new GameTagContainer(Tags.OnGoingRequired);
            AssetTagContainer = new GameTagContainer(Tags.RemoveGeWith);
            AssetTagContainer = new GameTagContainer(Tags.BlockGeWith);
        }
    }
}