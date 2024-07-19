using UnityEditor;
using UnityEngine;

namespace Vigeo.Unity.Editor {

    public static class EditorContextMenuUtilities {
        
        [MenuItem("CONTEXT/Component/Copy transform to parent and reset", isValidateFunction: true)]
        public static bool CanCopyTransformToParent(MenuCommand command) =>
            (command.context as Transform)?.parent != null;

        [MenuItem("CONTEXT/Component/Copy transform to parent and reset", priority = 10)]
        public static void CopyTransformToParent(MenuCommand command) => (command.context as Transform).Run(transform => {
            Transform parent = transform.parent;
            Undo.RegisterCompleteObjectUndo(parent, "Copy transform to parent and reset");
            parent.localPosition = transform.localPosition;
            parent.localEulerAngles = transform.localEulerAngles;
            parent.localScale = transform.localScale;
            Undo.RegisterCompleteObjectUndo(transform, "Copy transform to parent and reset");
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
            EditorUtility.SetDirty(parent);
            EditorUtility.SetDirty(transform);
        });
        
        [MenuItem("CONTEXT/Component/Move component to parent", isValidateFunction: true)]
        public static bool CanMoveComponentToParent(MenuCommand command) =>
            command.context is not Transform && (command.context as Component)?.transform.parent != null;

        [MenuItem("CONTEXT/Component/Move component to parent", priority = 10)]
        public static void MoveComponentToParent(MenuCommand command) => (command.context as Component).Run(component => {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Move component to parent");
            GameObject parent = component.transform.parent.gameObject;
            var componentClone = Undo.AddComponent(parent, component.GetType());
            EditorUtility.CopySerialized(component, componentClone);
            component.gameObject.Run(gameObject => {
                Undo.DestroyObjectImmediate(component);
                EditorUtility.SetDirty(gameObject);
            });
            EditorUtility.SetDirty(parent);
        });
    }
}
