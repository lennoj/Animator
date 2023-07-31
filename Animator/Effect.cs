using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator
{

    public sealed class  Effect
    {
        private IEffect _iEffect;
        private float _duration;
        private float _currentTime;
        private bool _started;
        private bool _paused;
        private bool _stopped;
        private bool _new;
        private bool _reflect;
        private float _repeatCount;
        
        public delegate void EffectReaction<Effect>(Effect effect);

        public event EffectReaction<Effect> OnStart;
        public event EffectReaction<Effect> OnEnd;
        public event EffectReaction<Effect> OnUpdate;
        public event EffectReaction<Effect> OnPause;
        public event EffectReaction<Effect> OnResume;
        public event EffectReaction<Effect> OnReflect;


        private Effect(IEffect effect , float duration, bool reflect, int repeat) 
        {
            this.OnStart = null;
            this.OnEnd = null;
            this.OnUpdate = null;
            this.OnPause = null;
            this.OnResume = null;
            _iEffect = effect;
            _duration = duration;
            _started = false;
            _paused = false;
            _stopped = false;
            _new = true;
            _repeatCount = repeat;
            _reflect = reflect;

            //switch (AnimationEngine.GetAnimationSmoothness())
            //{
            //    case AnimationSmoothness.None: _smoothness = 0.0F; break;
            //    case AnimationSmoothness.BelowNormal: _smoothness = 0.5F; break;
            //    case AnimationSmoothness.Normal: _smoothness = 1; break;
            //    case AnimationSmoothness.Smooth: _smoothness = 2; break;
            //    case AnimationSmoothness.VerySmooth: _smoothness = 3; break;

            //}

        }

        public static Effect ApplyEffect(IEffect effect, float duration, bool reflect, int repeat)
        {
            if (duration <= 0F) duration = 1F;  // default = 1 Second;
            return new Effect(effect, duration, reflect, repeat);
        }


        public bool isNew() 
        {
            return this._new;
        }

        public bool isStarted() 
        {
            return this._started;
        }

        public bool isPaused()
        {
            return this._paused;
        }

        public bool isEnded() 
        {
            return this._stopped;
        }

        /// <summary>
        /// Stop the Animation/Effect.
        /// </summary>
        public void Stop() 
        {
            this._stopped = true;
        }

        /// <summary>
        /// Pause the Animation/Effect.
        /// </summary>
        public void Pause() 
        {
            this._paused = true;
        }

        /// <summary>
        /// Resume the Animation/Effect.
        /// </summary>
        public void Resume() 
        {
            this._paused = false;
        }

        /// <summary>
        /// Skip the Animation/Effect.
        /// </summary>
        public void SkipAnimation()
        {
            this._currentTime = this._duration;
        }

        public float GetCurrentTime() 
        {
            return this._currentTime;
        }

        public float GetDuration() 
        {
            return this._duration;
        }

        public void AnimationStart()
        {
            this._started = true;
            this._new = false;

            if (this.OnStart != null) this.OnStart(this);

            this._iEffect.EffectStart(this);
        }

        public void AnimationEnd()
        {
            this._started = true;

            if (this.OnEnd != null) this.OnEnd(this);
            this._iEffect.EffectEnd(this);
            this._stopped = true;
        }

        private float CalculateSpeed(float time, float duration, float delta)
        {
            float ret = (duration / AnimationEngine.GetFPS()); // * (duration * delta);
            return ret;
        }
    

        public void AnimationUpdate() 
        {
           
            if (!(this._paused) && !(this._stopped))
            {
                if (this.OnUpdate != null) this.OnUpdate(this);

                this._iEffect.EffectUpdate(this);
                this._currentTime += CalculateSpeed(this._currentTime, this._duration, AnimationEngine.GetDelta());
               
                if (this._currentTime >= this._duration)
                {
                     if(_reflect && _repeatCount <= -1)    {
                         
                         this._currentTime = 0.0F;
                         _iEffect.EffectReflect(this);
                         if (this.OnReflect != null) this.OnReflect(this);

                    }   else if (_reflect && _repeatCount - 0.5F > 0)   {

                        this._currentTime = 0.0F;
                        _repeatCount -= 0.5F;
                        _iEffect.EffectReflect(this);
                        if (this.OnReflect != null) this.OnReflect(this);

                    }   else {
                        AnimationEnd();
                        this._currentTime = this._duration;
                    }
                   
                }
            }
            else if (this._stopped) 
            {
                AnimationEnd();
                this._currentTime = this._duration;
            }

        }

        public void AnimationPaused() 
        {
            if (this.OnPause != null) this.OnPause(this);

            this._iEffect.EffectPaused(this);
        }

        public void AnimationResume() 
        {
            if (this.OnResume != null) this.OnResume(this);

            this._iEffect.EffectResume(this);
        }

        /// <summary>
        /// Dispose the Effect.
        /// </summary>
        public void Dispose() 
        {
            _iEffect = null;
            OnStart = null;
            OnEnd = null;
            OnUpdate = null;
            OnPause = null;
            OnResume = null;
            OnReflect = null;
        }
    }
}
