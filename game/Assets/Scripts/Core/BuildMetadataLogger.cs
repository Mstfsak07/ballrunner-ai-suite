using UnityEngine;

namespace BallRunner.Core
{
    public sealed class BuildMetadataLogger : MonoBehaviour
    {
        [SerializeField] private BuildMetadata metadata;

        private void Start()
        {
            if (metadata == null)
            {
                return;
            }

            Debug.Log($"[BuildMetadata] {metadata.VersionName} ({metadata.VersionCode}) | {metadata.Channel} | {metadata.BuildTimestampUtc}");
        }
    }
}
