using System;

namespace Vigeo {
    
    public static class ScopeExtensionsStaging {

        public static T Or<T>(this T self, T other) {
            if (self == null) {
                return other;
            }
            return self;
        }
    }
}
