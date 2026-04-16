using UnityEngine;

namespace BallRunner.Core
{
    [CreateAssetMenu(fileName = "BuildMetadata", menuName = "BallRunner/Config/Build Metadata")]
    public sealed class BuildMetadata : ScriptableObject
    {
        [SerializeField] private string versionName = "0.1.2-internal";
        [SerializeField] private int versionCode = 3;
        [SerializeField] private string channel = "internal";
        [SerializeField] private string buildTimestampUtc = "";

        public string VersionName => versionName;
        public int VersionCode => versionCode;
        public string Channel => channel;
        public string BuildTimestampUtc => buildTimestampUtc;

        [ContextMenu("Stamp Build Timestamp (UTC)")]
        private void StampBuildTimestamp()
        {
            buildTimestampUtc = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss 'UTC'");
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}


