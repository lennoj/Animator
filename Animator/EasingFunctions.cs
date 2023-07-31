using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator
{
    public sealed class EasingFunctions
     {
        
                
        // simple linear tweening - no eaSing, no acceleration
        public static double LinearTween(double t, double b, double c, double d) {
                return c*t/d + b;
        }
		
        
        // quadratic eaSing in - accelerating from zero velocity
        public static double EaseInQuad (double t, double b, double c, double d) {
	        t /= d;
	        return c*t*t + b;
        }
		

        // quadratic eaSing out - decelerating to zero velocity
        public static double EaseOutQuad(double t, double b, double c, double d) {
	        t /= d;
	        return -c * t*(t-2) + b;
        }

		
        // quadratic eaSing in/out - acceleration until halfway, then deceleration
        public static double  EaseInOutQuad(double t, double b, double c, double d) {
	        t /= d/2;
	        if (t < 1) return c/2*t*t + b;
	        t--;
	        return -c/2 * (t*(t-2) - 1) + b;
        }


        // cubic eaSing in - accelerating from zero velocity
        public static double  EaseInCubic(double t, double b, double c, double d) {
	        t /= d;
	        return c*t*t*t + b;
        }

	
        // cubic eaSing out - decelerating to zero velocity
        public static double  EaseOutCubic(double t, double b, double c, double d) {
	        t /= d;
	        t--;
	        return c*(t*t*t + 1) + b;
        }

	
        // cubic eaSing in/out - acceleration until halfway, then deceleration
        public static double  EaseInOutCubic(double t, double b, double c, double d) {
	        t /= d/2;
	        if (t < 1) return c/2*t*t*t + b;
	        t -= 2;
	        return c/2*(t*t*t + 2) + b;
        }
	

        // quartic eaSing in - accelerating from zero velocity
        public static double  EaseInQuart(double t, double b, double c, double d) {
	        t /= d;
	        return c*t*t*t*t + b;
        }


        // quartic eaSing out - decelerating to zero velocity
        public static double  EaseOutQuart(double t, double b, double c, double d) {
        	t /= d;
	        t--;
	        return -c * (t*t*t*t - 1) + b;
        }
	

        // quartic eaSing in/out - acceleration until halfway, then deceleration
        public static double  EaseInOutQuart(double t, double b, double c, double d) {
        	t /= d/2;
        	if (t < 1) return c/2*t*t*t*t + b;
        	t -= 2;
	        return -c/2 * (t*t*t*t - 2) + b;
        }


        // quintic eaSing in - accelerating from zero velocity
        public static double  EaseInQuint(double t, double b, double c, double d) {
	        t /= d;
	        return c*t*t*t*t*t + b;
        }

		

        // quintic eaSing out - decelerating to zero velocity
        public static double  EaseOutQuint(double t, double b, double c, double d) {
        	t /= d;
        	t--;
	        return c*(t*t*t*t*t + 1) + b;
        }	

        // quintic eaSing in/out - acceleration until halfway, then deceleration
        public static double  EaseInOutQuint(double t, double b, double c, double d) {
        	t /= d/2;
        	if (t < 1) return c/2*t*t*t*t*t + b;
	        t -= 2;
	        return c/2*(t*t*t*t*t + 2) + b;
        }
		

        // Sinusoidal eaSing in - accelerating from zero velocity
        public static double  EaseInSine(double t, double b, double c, double d) {
        	return -c * Math.Cos(t/d * (Math.PI/2)) + c + b;
        }

		

        // Sinusoidal eaSing out - decelerating to zero velocity
        public static double  EaseOutSine(double t, double b, double c, double d) {
        	return c * Math.Sin(t/d * (Math.PI/2)) + b;
        }

		

        // Sinusoidal eaSing in/out - accelerating until halfway, then decelerating
        public static double  EaseInOutSine(double t, double b, double c, double d) {
        	return -c/2 * (Math.Cos(Math.PI*t/d) - 1) + b;
        }

		

        // exponential eaSing in - accelerating from zero velocity
        public static double  EaseInExpo(double t, double b, double c, double d) {
        	return c * Math.Pow( 2, 10 * (t/d - 1) ) + b;
        }

		

        // exponential eaSing out - decelerating to zero velocity
        public static double  EaseOutExpo(double t, double b, double c, double d) {
	        return c * ( -Math.Pow( 2, -10 * t/d ) + 1 ) + b;
        }

		

        // exponential eaSing in/out - accelerating until halfway, then decelerating
        public static double  EaseInOutExpo(double t, double b, double c, double d) {
	        t /= d/2;
	        if (t < 1) return c/2 * Math.Pow( 2, 10 * (t - 1) ) + b;
	        t--;
	        return c/2 * ( -Math.Pow( 2, -10 * t) + 2 ) + b;
        }
		

        // circular eaSing in - accelerating from zero velocity
        public static double  EaseInCirc(double t, double b, double c, double d) {
	        t /= d;
	        return -c * (Math.Sqrt(1 - t*t) - 1) + b;
        }

		

        // circular eaSing out - decelerating to zero velocity
        public static double  EaseOutCirc(double t, double b, double c, double d) {
        	t /= d;
	        t--;
	        return c * Math.Sqrt(1 - t*t) + b;
        }

		

        // circular eaSing in/out - acceleration until halfway, then deceleration
        public static double  EaseInOutCirc(double t, double b, double c, double d) {
	        t /= d/2;
	        if (t < 1) return -c/2 * (Math.Sqrt(1 - t*t) - 1) + b;
	        t -= 2;
	        return c/2 * (Math.Sqrt(1 - t*t) + 1) + b;
        }

        
        public static double EaseInElastic(double t,double b , double c, double d) {
	       
            if (t==0) return b;  if ((t/=d)==1) return b+c;  
	        double p    =   d*.3f;
	        double a    =   c; 
	        double s    =   p/4;
	        double postFix =    a* Math.Pow(2,10*(t-=1)); 
	        return -(postFix * Math.Sin((t*d-s)*(2*Math.PI)/p )) + b;
        }

        public static double EaseOutElastic(double t, double b, double c, double d)
        {
	        if (t==0) return b;  if ((t/=d)==1) return b+c;
            double p = d * .3f;
            double a = c;
            double s = p / 4;
	        return (a * Math.Pow(2,-10*t) * Math.Sin( (t*d-s)*(2*Math.PI)/p ) + c + b);	
        }


        public static double EaseInOutElastic(double t,double b , double c, double d) {
	        
            if (t==0) return b;  if ((t/=d/2)==2) return b+c; 
	
            double p    =   d*(.3f*1.5f);
	        double a    =   c; 
	        double s    =   p/4;
	 
	        if (t < 1)
            {
	        	double postFix =a * Math.Pow(2,10*(t-=1)); 
		        return -.5f*(postFix* Math.Sin( (t*d-s)*(2*Math.PI)/p )) + b;
	        } 

	        double _postFix =  a* Math.Pow(2,-10*(t-=1)); 
	        return _postFix * Math.Sin( (t*d-s)*(2*Math.PI)/p )*.5f + c + b;
        }



        public static double  EaseInBounce(double t,double b , double c, double d) {
	        return c - EaseOutBounce (d-t, 0, c, d) + b;
        }


        public static double  EaseOutBounce(double t,double b , double c, double d) {
        
            if ((t/=d) < (1/2.75f)) {
		        return c * (7.5625f * t * t ) + b;
	        } else if (t < (2/2.75f)) {
		        double postFix = t-=(1.5f/2.75f);
		        return c * (7.5625f * (postFix) * t + .75f) + b;
	        } else if (t < (2.5/2.75)) {
			    double postFix = t -= (2.25f/2.75f);
		        return c * (7.5625f * (postFix) * t + .9375f) + b;
	        } else {
		        double postFix = t-=(2.625f/2.75f);
		        return c*(7.5625f*(postFix)*t + .984375f) + b;
	        }
        }

        public static double EaseInOutBounce(double t, double b, double c, double d)    {
	        
            if (t < d/2) return EaseInBounce (t*2, 0, c, d) * .5f + b;
	            else return EaseOutBounce (t*2-d, 0, c, d) * .5f + c*.5f + b;
        }           
    }
}
