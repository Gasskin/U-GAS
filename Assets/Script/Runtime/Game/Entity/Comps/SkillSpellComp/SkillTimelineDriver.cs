using System;
using System.Collections.Generic;
using cfg.Gas;

public class SkillTimelineDriver
{
    public const int TimelineFrame = 60;

    public bool IsValid => _context.SkillId > 0;

    private float _frameTime = 1f / TimelineFrame;
    private float _frameTimeTick = 0f;
    private int _nowFrame = 0;


    private SkillTimelineContext _context = default;

    private List<SkillTimeline> _skillTimelines = new();

    private Action _onStart;
    private Action _onInterrupt;
    private Action _onEnd;

    public SkillTimelineDriver(Action onStart, Action onInterrupt, Action onEnd)
    {
        _onStart = onStart;
        _onInterrupt = onInterrupt;
        _onEnd = onEnd;
    }

    public void Tick(float dt)
    {
        if (_context.SkillId <= 0)
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
            _frameTimeTick = _frameTime;
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
                _context = default;
                _onEnd?.Invoke();
            }
            return isAllEnd;
        }
    }

    public void Start(SkillTimelineContext context)
    {
        var tables = SystemDriver.ConfigSystem.Tables;
        var skill = tables.TbSkill.GetOrDefault(context.SkillId);
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
                timeline.Reset(context);
                _skillTimelines.Add(timeline);
            }
        }
        _context = context;
        _frameTimeTick = _frameTime;
        _nowFrame = 0;
        _onStart?.Invoke();
    }


    public void Interrupt()
    {
        if (_context.SkillId <= 0 || _nowFrame <= 0)
        {
            return;
        }

        foreach (var timeline in _skillTimelines)
        {
            timeline.Interrupt();
        }
        _context = default;
        _onInterrupt?.Invoke();
    }
}