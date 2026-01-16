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
            GrantedTagContainer = new GameTagContainer(Tags.Granted);
            ApplyRequiredTagContainer = new GameTagContainer(Tags.ApplyRequired);
            ImmuneWhenTagContainer = new GameTagContainer(Tags.ImmuneWhen);
            OnGoingRequiredTagContainer = new GameTagContainer(Tags.OnGoingRequired);
            RemoveGeWithTagContainer = new GameTagContainer(Tags.RemoveGeWith);
            BlockGeWithTagContainer = new GameTagContainer(Tags.BlockGeWith);
        }
    }
}