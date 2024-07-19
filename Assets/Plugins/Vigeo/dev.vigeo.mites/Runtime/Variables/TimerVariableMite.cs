using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-lush")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Variables/Timer")]
    [HideMonoScript]
    public class TimerVariableMite : DoubleVariableMite {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Holds value of passed gameplay time."
        );
    }
}
