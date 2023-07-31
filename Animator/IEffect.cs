using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator
{
    public interface IEffect
    {
        void EffectStart(Effect effect);
        void EffectEnd(Effect effect);
        void EffectUpdate(Effect effect);
        void EffectPaused(Effect effect);
        void EffectResume(Effect effect);
        void EffectReflect(Effect effect);
    }
}
