using UnityEngine;
using UnityEngine.Playables;

namespace Vigeo {

    public static class PlayableExtensions {

        public static T ResolveReference<T>(this Playable playable, ExposedReference<T> reference) where T : Object =>
            reference.Resolve(playable.GetGraph().GetResolver());
    }
}
