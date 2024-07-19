using System;

namespace Vigeo {

    public static class BooleanExtensions {
        
        public static bool And(this bool @this, Func<bool> block) =>
            @this && block();

        public static bool And(this bool @this, Func<bool, bool> block) =>
            @this && block(true);
        
        public static bool Or(this bool @this, Func<bool> block) =>
            @this || block();

        public static bool Or(this bool @this, Func<bool, bool> block) =>
            @this || block(false);

        public static void WhenTrue(this bool @this, Action block) {
            if (@this) {
                block();
            }
        }
        
        public static void WhenTrue(this bool @this, Action<bool> block) {
            if (@this) {
                block(true);
            }
        }

        public static void WhenFalse(this bool @this, Action block) {
            if (@this == false) {
                block();
            }
        }
        
        public static void WhenFalse(this bool @this, Action<bool> block) {
            if (@this == false) {
                block(false);
            }
        }
    }
}
