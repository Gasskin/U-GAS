using System;
using System.Collections.Generic;
using cfg.Gas;

public class TimelineDriver
{
    public const int c_TimelineFrame = 60;

    public bool IsValid => _skillId > 0;

    private float _frameTime = 1f / c_TimelineFrame;
    private float _frameTimeTick = 0f;
    private int _nowFrame = 0;

    private int _skillId;
    private List<SkillTimeline> _skillTimelines = new();

    private Action _onEnd;

    public TimelineDriver(Action onEnd)
    {
        _onEnd = onEnd;
    }

    public void Tick(float dt)
    {
        if (_skillId <= 0)
        {
            return;
        }

        if (dt >= _frameTimeTick)
        {
            while (dt >= _frameTimeTick)
            {
                _nowFrame++;
                _frameTimeTick = _frameTime - (dt - _frameTimeTick);
                if (OneFrame())
                {
                    break;
                }
            }
        }
        else
        {
            _frameTimeTick -= dt;
        }

        bool OneFrame()
        {
            var isAllEnd = true;
            for (int i = 0; i < _skillTimelines.Count; i++)
            {
                var timeline = _skillTimelines[i];
                if (!timeline.Tick(dt, _nowFrame))
                {
                    isAllEnd = false;
                }
            }
            if (isAllEnd)
            {
                _skillId = -1;
                _onEnd?.Invoke();
            }
            return isAllEnd;
        }
    }

    public void Reset(int skillId)
    {
        var tables = SystemDriver.DesignSystem.Tables;
        var skill = tables.TbSkill.GetOrDefault(_skillId);
        if (skill != null)
        {
            _skillTimelines.Clear();
            foreach (var id in skill.SkillTimelinesId)
            {
                var timeline = tables.TbSkillTimeline.GetOrDefault(id);
                if (timeline == null)
                {
                    continue;
                }
                timeline.Reset();
                _skillTimelines.Add(timeline);
            }
        }
        _skillId = skillId;
        _frameTimeTick = _frameTime;
        _nowFrame = 0;
    }


    public void Interrupt()
    {
        if (_skillId <= 0 || _nowFrame <= 0)
        {
            return;
        }

        foreach (var timeline in _skillTimelines)
        {
            timeline.Interrupt();
        }
    }
}