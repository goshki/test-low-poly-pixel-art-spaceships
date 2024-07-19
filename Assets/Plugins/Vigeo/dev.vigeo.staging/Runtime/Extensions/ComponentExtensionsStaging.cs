using UnityEngine;

namespace Vigeo {

    public static class ComponentExtensionsStaging {
        
        public static Vector3 Position(this GameObject @this) =>
            @this.transform.position;
        
        public static Vector3 LocalPosition(this GameObject @this) =>
            @this.transform.localPosition;
        
        public static Quaternion Rotation(this GameObject @this) =>
            @this.transform.rotation;
        
        public static (Vector3, Quaternion) Transformation(this GameObject @this) =>
            (@this.Position(), @this.Rotation());

        public static Vector3 Position(this Component @this) =>
            @this.gameObject.Position();
        
        public static Vector3 LocalPosition(this Component @this) =>
            @this.gameObject.LocalPosition();

        public static Quaternion Rotation(this Component @this) =>
            @this.gameObject.Rotation();

        public static (Vector3, Quaternion) Transformation(this Component @this) =>
            (@this.Position(), @this.Rotation());
    }
}
