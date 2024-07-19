using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-lush")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Variables/Vector3")]
    [HideMonoScript]
    public class Vector3VariableMite : VariableMite<Vector3> {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Holds a Vector3 value."
        );
    }
}
