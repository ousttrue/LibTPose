using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace TPose
{
    [ScriptedImporter(version: 1, ext: "bvh")]
    public class BvhScriptedImporter : ScriptedImporter
    {
        void Build(Transform t, VrmLib.Bvh.BvhNode joint)
        {
            foreach (var childJoint in joint.Children)
            {
                var child = new GameObject(childJoint.Name).transform;
                child.SetParent(t);
                child.localPosition = new Vector3(childJoint.Offset.X, childJoint.Offset.Y, childJoint.Offset.Z);

                Build(child, childJoint);
            }
        }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            // parser bvh
            var path = Path.Combine(Application.dataPath + "/..", ctx.assetPath);
            Debug.LogFormat("load {0}", path);
            var bvh = VrmLib.Bvh.BvhParser.FromPath(path);

            var root = new GameObject("bvh");
            Build(root.transform, bvh.Root);

            ctx.AddObjectToAsset("root", root);
            ctx.SetMainObject(root);
        }
    }
}
