using UnityEngine;
using UnityEditor;

namespace BobotGame
{
    // Adds a mesh collider to each game object that contains collider in its name
    public class FBXAnimationPostProcessor_Constant : AssetPostprocessor
    {
        // Increment the version number, when the AssetPostprocessors code/behavior is changed
        static readonly uint k_Version = 3;
        public override uint GetVersion() { return k_Version; }

        private void OnPostprocessAnimation(GameObject root, AnimationClip clip)
        {
            if (!root.name.Contains("Walker"))
                return;

            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
            foreach (var binding in bindings)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                for (int i = 0; i < curve.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                }
                AnimationUtility.SetEditorCurve(clip, binding, curve);
            }
        }
    }
}