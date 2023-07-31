using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;

namespace Animator
{
    public delegate Object AnimationAction<T>(T Obj);
    public enum ChangePropertyValueStyle 
    { 
        ObjectAndNamedPropertyStyle,
        GetSetDelegateStyle
    }

    public class EasingEffect : IEffect
    {
        private String propertyName;
        private Object obj;
        private Object startValue;
        private Object endValue;
        private Type propType;

        private delegate double EasingStyleCB(double t, double b, double c, double d);
        private EasingStyleCB _cb;
        private AnimationAction<Effect> _updateCallback;
        private EasingStyle _style;
        private ChangePropertyValueStyle _cstyle;
        private AnimationAction<Object> _getCallBack;
        private AnimationAction<Object> _setCallBack;

        public String PropertyName      { get { return this.propertyName; } }
        public Object StartValue        { get { return this.startValue; } }
        public Object EndValue          { get { return this.endValue; } }
        public Object AnimatedObject    { get { return this.obj; } }
        public Type   PropertyType      { get { return this.propType; } }


        public EasingEffect(ref Object obj, String prop, Object endValue , EasingStyle style)
        {
            this.obj = obj;
            this.propertyName = prop;
            this.startValue = AnimationEngine.wrapper[obj, prop];
            this.endValue = (double)(Convert.ToDouble(endValue) - Convert.ToDouble(startValue));
            this.propType = AnimationEngine.wrapper[obj, prop].GetType();
            this._style = style;
            this._cstyle = ChangePropertyValueStyle.ObjectAndNamedPropertyStyle;

            SetStyle();
        }

        public EasingEffect(AnimationAction<Object> setCallback, AnimationAction<Object> getCallback, Object endValue, EasingStyle style)
        {
            this._setCallBack = setCallback;
            this._getCallBack = getCallback;
            this.startValue = this._getCallBack(0);
            this.endValue = (double)(Convert.ToDouble(endValue) - Convert.ToDouble(startValue));
            this.propType = startValue.GetType();
            this._style = style;
            this._cstyle = ChangePropertyValueStyle.GetSetDelegateStyle;

            SetStyle();
        }

        private void SetStyle() 
        {
            // Set the callback for easing
            switch (this._style) 
            {
                case EasingStyle.LinearTweenStyle:      this._cb = EasingFunctions.LinearTween; break;
                case EasingStyle.EaseOutSine:           this._cb = EasingFunctions.EaseOutSine; break;
                case EasingStyle.EaseOutQuint:          this._cb = EasingFunctions.EaseOutQuint; break;
                case EasingStyle.EaseOutQuart:          this._cb = EasingFunctions.EaseOutQuart; break;
                case EasingStyle.EaseOutQuad:           this._cb = EasingFunctions.EaseOutQuad; break;
                case EasingStyle.EaseOutExpo:           this._cb = EasingFunctions.EaseOutExpo; break;
                case EasingStyle.EaseOutElastic:        this._cb = EasingFunctions.EaseOutElastic; break;
                case EasingStyle.EaseOutCubic:          this._cb = EasingFunctions.EaseOutCubic; break;
                case EasingStyle.EaseOutCirc:           this._cb = EasingFunctions.EaseOutCirc; break;
                case EasingStyle.EaseOutBounce:         this._cb = EasingFunctions.EaseOutBounce; break;
                case EasingStyle.EaseInSine:            this._cb = EasingFunctions.EaseInSine; break;
                case EasingStyle.EaseInQuint:           this._cb = EasingFunctions.EaseInQuint; break;
                case EasingStyle.EaseInQuart:           this._cb = EasingFunctions.EaseInQuart; break;
                case EasingStyle.EaseInQuad:            this._cb = EasingFunctions.EaseInQuad; break;
                case EasingStyle.EaseInOutQuint:        this._cb = EasingFunctions.EaseInOutQuint; break;
                case EasingStyle.EaseInOutQuart:        this._cb = EasingFunctions.EaseInOutQuart; break;
                case EasingStyle.EaseInOutQuad:         this._cb = EasingFunctions.EaseInOutQuad; break;
                case EasingStyle.EaseInOutExpo:         this._cb = EasingFunctions.EaseInOutExpo; break;
                case EasingStyle.EaseInOutElastic:      this._cb = EasingFunctions.EaseInOutElastic; break;
                case EasingStyle.EaseInOutCubic:        this._cb = EasingFunctions.EaseInOutCubic; break;
                case EasingStyle.EaseInOutCirc:         this._cb = EasingFunctions.EaseInOutCirc; break;
                case EasingStyle.EaseInOutBounce:       this._cb = EasingFunctions.EaseInOutBounce; break;
                case EasingStyle.EaseInExpo:            this._cb = EasingFunctions.EaseInExpo; break;
                case EasingStyle.EaseInElastic:         this._cb = EasingFunctions.EaseInElastic; break;
                case EasingStyle.EaseInCubic:           this._cb = EasingFunctions.EaseInCubic; break;
                case EasingStyle.EaseInCirc:            this._cb = EasingFunctions.EaseInCirc; break;
                case EasingStyle.EaseInBounce:          this._cb = EasingFunctions.EaseInBounce; break;
                case EasingStyle.EaseInOutSine:         this._cb = EasingFunctions.EaseInOutSine; break;
            }

            // Set the Update callback
            if (_cstyle == ChangePropertyValueStyle.ObjectAndNamedPropertyStyle) 
            {
                _updateCallback = (Effect effect) =>
                {
                    double newValue = Math.Truncate((_cb(effect.GetCurrentTime(), Convert.ToDouble(this.startValue), Convert.ToDouble(this.endValue), effect.GetDuration())));
                    AnimationEngine.wrapper[obj, propertyName] = Convert.ChangeType(newValue, propType);
                    return newValue;
                };

            }
            else
            {
                _updateCallback = (Effect effect) =>
                {
                    double newValue = Math.Truncate((_cb(effect.GetCurrentTime(), Convert.ToDouble(this.startValue),Convert.ToDouble(this.endValue), effect.GetDuration())));
                    _setCallBack(Convert.ChangeType(newValue, propType));
                    return newValue;
                };

            
            }
            
        }

        public void Reflect()
        {
            this.startValue = Convert.ToDouble(this.startValue) + Convert.ToDouble(this.endValue);
            this.endValue = -Convert.ToDouble(this.endValue);
        }

        public IEffect GetEffectCallBack() 
        {
            return this;
        }


        public void EffectStart(Effect effect)
        {
            this.startValue = AnimationEngine.GetValue(this.AnimatedObject, this.PropertyName);
        }

        public void EffectEnd(Effect effect)
        {
            if (_cstyle == ChangePropertyValueStyle.ObjectAndNamedPropertyStyle)
                AnimationEngine.wrapper[obj, propertyName] = Convert.ToDouble(this.startValue) + Convert.ToDouble(this.endValue);
            else
                _setCallBack(Convert.ChangeType(Convert.ToDouble(this.startValue) + Convert.ToDouble(this.endValue), propType));
            
        }

        public void EffectUpdate(Effect effect)
        {
            _updateCallback(effect);
        }

        public void EffectPaused(Effect effect)
        {
            
        }

        public void EffectResume(Effect effect)
        {
            
        }


        public void EffectReflect(Effect effect)
        {
            Reflect();
        }
    }
}
