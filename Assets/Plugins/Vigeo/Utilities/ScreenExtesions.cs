using UnityEngine;

namespace Vigeo {

    public static class ScreenExtensions {

        /// <summary>
        /// Default (medium) screen density in DPI (dots per inch).
        /// </summary>
        public const float MDPI = 160f;

        public static float CalculateValueInDp(float value) {
            return Mathf.Max(value, value * Screen.dpi / MDPI);
        }

        public static float InDp(this float value) {
            return CalculateValueInDp(value);
        }

        public static int InDp(this int value) {
            return (int) CalculateValueInDp(value);
        }

        public static float Remap(this float value, float min, float max, float newMin, float newMax) {
            return (value - min) / (max - min) * (newMax - newMin) + newMin;
        }
    }
}
