using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using Animator;

namespace Animator
{
    public enum EasingStyle
    {
        LinearTweenStyle,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce
    };


    public enum AnimationSmoothness
    {
        None , // 0
        BelowNormal, // 0.5
        Normal, // 1
        Smooth, // 2
        VerySmooth // 3
    }

    public sealed class AnimationEngine
    {
        private static Timer dispatcher;
        private static List<Effect> DispatchListEffects;
        private static List<Effect> DisposeListEffects;
        private static List<Effect> QueueListEffects;
        private static float DELTATIME;
        private static long DELTA_START;
        private static long DELTA_END;
        private static long DELTA_ELAPSE;
        private static float interval;
        private static int FPS = 60;
        private static int GLOBAL_ACCU_FPS;
        private static int GLOBAL_CURR_FPS;
        private static int GLOBAL_DELTA_COUNT;


        public static Wrapper wrapper;

        public static float GetDelta() { return DELTATIME; }
        public static float GetInterval()
        {
            return interval;
        }



        public AnimationEngine() 
        { 
            
        }


        public static int GetFPS() { return FPS; }


        public static void SetFPS(int fps) 
        {
            FPS = fps;
            if (dispatcher != null) dispatcher.Interval = 1;
            interval = (1000 / FPS);
        }

        private static void InitializeDispatcher() 
        {
            dispatcher = new Timer();
            dispatcher.Interval = 1;
            interval = (1000 / FPS);

            DispatchListEffects = new List<Effect>();
            DisposeListEffects = new List<Effect>();
            QueueListEffects = new List<Effect>();
            
            dispatcher.Tick += (Object sender, System.EventArgs args) =>
            {
                DELTA_START = System.Diagnostics.Stopwatch.GetTimestamp();
                
                // Dispose all the Effects that is Finished
                if (DisposeListEffects.Count > 0) 
                {
                    foreach (Effect e in DisposeListEffects) 
                    {
                        e.Dispose();
                        DispatchListEffects.Remove(e);
                    }

                    // Clear the dispose list
                    DisposeListEffects.Clear();
                }

                // Add all the Queued Effect
                if (QueueListEffects.Count > 0)
                {
                    foreach (Effect e in QueueListEffects)
                    {
                        DispatchListEffects.Add(e);
                    }

                    // Clear the Queued list
                    QueueListEffects.Clear();
                }

                // Dispatch all the Effects in the DispatchList
                if (DispatchListEffects.Count > 0)
                {
                    foreach (Effect e in DispatchListEffects)
                    {
                        if (e.isNew()) 
                        {
                            e.AnimationStart();
                        }
                        else
                        {
                            e.AnimationUpdate();
                        }
                        
                        // Check if stopped
                        if (e.isEnded()) 
                        { 
                            // Add it to disposeList for disposal
                            DisposeListEffects.Add(e);
                        }
                    }
                }

                GLOBAL_ACCU_FPS++;
                DELTA_END = System.Diagnostics.Stopwatch.GetTimestamp();
                DELTA_ELAPSE = DELTA_END - DELTA_START;
                DELTATIME = (((float)DELTA_ELAPSE) / 10000F);
                GLOBAL_DELTA_COUNT += (int)DELTATIME;

                if (GLOBAL_DELTA_COUNT >= 1000)
                {
                    GLOBAL_CURR_FPS = GLOBAL_ACCU_FPS;
                    GLOBAL_DELTA_COUNT = 0;
                    GLOBAL_ACCU_FPS = 0;
                }
            };

            // Start the Engine
            dispatcher.Start();
        }



        public static void StartUIEngine() 
        {
            wrapper = new  Wrapper();
            InitializeDispatcher();
        }

        public static void SetValue(Object objInstance, string propertyName, Object value, Type propertyType)
        {
            wrapper[objInstance, propertyName] = Convert.ChangeType(value, propertyType);
        }

        public static Object GetValue(Object objInstance, string propertyName)
        {
            return wrapper[objInstance, propertyName];
        }

        /// <summary>
        /// Animate object(s) by manipulating specific variable/property of an Object.
        /// </summary>
        /// <param name="variable">An instance of any Reference Type.</param>
        /// <param name="property">The name of the property to manipulate.</param>
        /// <param name="endValue">The final value of the property after the animation/easing.</param>
        /// <param name="style">The style of animaiton.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="reflect">if TRUE , the animation will be reverse after the execution of the animation. (Same duration and same style)</param>
        /// <param name="repeatCount">The number of time to repeat the animation (Reflect should be set to TRUE to use repeatCount) [Note : if repeat count == -1  then number of repetition will be set to INFINITE]</param>
        /// <returns>return an instance of the created Effect [Use to manipulate animation state, other objects/variables using pre-defined events]</returns>
        public static Effect AnimateEasing(Object variable, String property, int endValue, EasingStyle style, float duration , bool reflect , int repeatCount)
        {
            EasingEffect _ef = new EasingEffect(ref variable, property, endValue, style);
            Effect _effect = Effect.ApplyEffect(_ef, duration, reflect, repeatCount);
            AddEffect(_effect);
            return _effect;
        }

        /// <summary>
        /// Animate object(s) by manipulating variables, using a GET and SET callback.
        /// </summary>
        /// <param name="setCallback">The SET callback for the non-contant/non-readonly variable to manipulate.</param>
        /// <param name="getCallback">The GET callback for the variable to manipulate [Should be the same variable from SET parameter]</param>
        /// <param name="endValue">The final value of the variable after the animation.</param>
        /// <param name="style">The style of animation/easing</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="reflect">if TRUE , the animation will be reverse after the execution of the animation. (Same duration and same style)</param>
        /// <param name="repeatCount">The number of time to repeat the animation (Reflect should be set to TRUE to use repeatCount) [Note : if repeat count == -1  then number of repetition will be set to INFINITE]</param>
        /// <returns>return an instance of the created Effect [Use to manipulate animation state, other objects/variables using pre-defined events]</returns>
        public static Effect Animate(AnimationAction<Object> setCallback , AnimationAction<Object> getCallback ,  int endValue, EasingStyle style, float duration , bool reflect , int repeatCount)
        {
            EasingEffect _ef = new EasingEffect(setCallback , getCallback ,  endValue, style);
            Effect _effect = Effect.ApplyEffect(_ef, duration, reflect, repeatCount);
            AddEffect(_effect);
            return _effect;
        }

        /// <summary>
        /// Add an Effect to the QueueList
        /// </summary>
        /// <param name="effect">The Effect to add.</param>
        private static void AddEffect(Effect effect) 
        {
            if (dispatcher == null)
            {
                StartUIEngine();
            }

            QueueListEffects.Add(effect);
            
        }

        public static int DispatchCount { get { if (DispatchListEffects != null) return DispatchListEffects.Count; else return 0; } }
        public static int QueueCount { get { if (QueueListEffects != null) return QueueListEffects.Count; else return 0; } }
        public static int DisposeCount { get { if (DisposeListEffects != null) return DisposeListEffects.Count; else return 0; } }

        /// <summary>
        /// Force Stop a specific effect.
        /// </summary>
        /// <param name="effect">The object instance of type Effect to Stop.</param>
        /// <returns>return true if success</returns>
        public static bool Stop(Effect effect) 
        {
            if (DispatchListEffects.Contains(effect))
            {
                effect.Stop();
                DispatchListEffects.Remove(effect);
                return true;
            }

            if (QueueListEffects.Contains(effect))
            {
                effect.Stop();
                QueueListEffects.Remove(effect);
                return true;
            }


            if (DisposeListEffects.Contains(effect))
            {
                effect.Stop();
                DisposeListEffects.Remove(effect);
                return true;
            }

            return false;
        }

        public static void StopAllAnimations() 
        {
            DispatchListEffects.Clear();
            DisposeListEffects.Clear();
            QueueListEffects.Clear();
            System.GC.Collect();
        }

    }
}
