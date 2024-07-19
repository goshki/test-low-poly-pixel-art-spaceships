using System;
using UnityEngine;

namespace Vigeo {

    public static class ColorExtensions {
        
        public static Color Color(this int @this) =>
            @this.Color32();

        public static Color32 Color32(this int @this) =>
            new Color32((byte) ((@this >> 16) & 0xff), (byte) ((@this >> 8) & 0xff), (byte) ((@this >> 0) & 0xff), 0xff);

        public static Color R(this Color @this, float r) {
            @this.r = r;
            return @this;
        }

        public static Color G(this Color @this, float g) {
            @this.g = g;
            return @this;
        }

        public static Color B(this Color @this, float b) {
            @this.b = b;
            return @this;
        }

        public static Color A(this Color @this, float a) {
            @this.a = a;
            return @this;
        }

        public static Color R(this Color @this, Func<float, float> block) {
            @this.r = block(@this.r);
            return @this;
        }

        public static Color G(this Color @this, Func<float, float> block) {
            @this.g = block(@this.g);
            return @this;
        }

        public static Color B(this Color @this, Func<float, float> block) {
            @this.b = block(@this.b);
            return @this;
        }

        public static Color A(this Color @this, Func<float, float> block) {
            @this.a = block(@this.a);
            return @this;
        }
        
        public static Color WithAlpha(this Color c, float alpha) {
            return new Color(c.r, c.g, c.b, alpha);
        }

        public static HSV HSV(this Color @this) =>
            (HSV) @this;

        public static Color Color(this HSV @this) =>
            (Color) @this;

        public static HSV H(this HSV @this, float h) {
            @this.h = h;
            return @this;
        }

        public static HSV S(this HSV @this, float s) {
            @this.s = s;
            return @this;
        }

        public static HSV V(this HSV @this, float v) {
            @this.v = v;
            return @this;
        }

        public static HSV A(this HSV @this, float a) {
            @this.a = a;
            return @this;
        }

        public static HSV H(this HSV @this, Func<float, float> block) {
            @this.h = block(@this.h);
            return @this;
        }

        public static HSV S(this HSV @this, Func<float, float> block) {
            @this.s = block(@this.s);
            return @this;
        }

        public static HSV V(this HSV @this, Func<float, float> block) {
            @this.v = block(@this.v);
            return @this;
        }

        public static HSV A(this HSV @this, Func<float, float> block) {
            @this.a = block(@this.a);
            return @this;
        }
    }

    public struct HSV {
        
        public float h;
        
        public float s;
        
        public float v;
        
        public float a;

        public static explicit operator HSV(Color color) {
            var hsv = new HSV();
            Color.RGBToHSV(color, out hsv.h, out hsv.s, out hsv.v);
            hsv.a = color.a;
            return hsv;
        }

        public static explicit operator Color(HSV hsv) {
            var c = Color.HSVToRGB(hsv.h, hsv.s, hsv.v);
            c.a = hsv.a;
            return c;
        }
    }
}
