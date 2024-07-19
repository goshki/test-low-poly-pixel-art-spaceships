using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {
    
    public abstract class ScriptableMite : ScriptableObject {
        
        protected virtual string TypeInfo => $"<b>TYPE: [{GetType().Name}]</b>";

        protected virtual string TypeDescription => TypeInfo;

#if UNITY_EDITOR

        [DetailedInfoBox("$TypeInfo", "$TypeDescription")]
        [ShowInInspector, DisplayAsString, LabelText("»"), LabelWidth(8), PropertyOrder(-10000), PropertySpace(SpaceAfter = 10), Tooltip("$ScriptableObjectName")]
        private string ScriptableObjectName => name;
        
        [ShowInInspector, ShowIf("@this.developerOptions.ShouldDisplayWarning == true"), DisplayAsString, PropertyOrder(-1100), BoxGroup]
        [LabelText("", SdfIconType.ExclamationTriangleFill), LabelWidth(15)]
        internal string DeveloperWarning => developerOptions.warning;

        [VerticalGroup("", GroupID = "DEV"), FoldoutGroup("@this.developerOptions.Title", GroupID = "DEV/O")]
        [SerializeField, HideLabel]
        internal DeveloperOptions developerOptions;

#endif
        
    }
}
