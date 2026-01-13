using UnityEngine;

public class SkillSpellComp: EntityComp
{
    public override int Priority => c_SkillSpell;
    public override bool NeedTick => true;
    
    private TimelineDriver _timelineDriver ;

    public override void OnAdd()
    {
        _timelineDriver = new(OnTimelineEnd);
    }

    public override void Tick(float dt)
    {
        if (!_timelineDriver.IsValid) 
        {
            return;
        }
        _timelineDriver.Tick(dt);
    }

    public void SpellSkill(int id)
    {
        // todo 检查消耗等等

        StartTimelineDriver(id);
    }

    public void InterruptSkill()
    {
        if (!_timelineDriver.IsValid)
        {
            return;
        }
        _timelineDriver.Interrupt();
    }

    private void StartTimelineDriver(int id)
    {
        _timelineDriver.Reset(id);
    }

    private void OnTimelineEnd()
    {
    }
}