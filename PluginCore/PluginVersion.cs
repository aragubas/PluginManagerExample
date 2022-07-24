using System.Text.Json;

namespace PluginCore
{
    public struct PluginVersion
    {
        // First digit in version string <b>1</b>.0.0
        public int MajorRelease;
        public int MinorRelease;
        public int BuildNumber;

        public PluginVersion(int majorRelease, int minorRelease, int buildNumber)
        {
            MajorRelease = majorRelease;
            MinorRelease = minorRelease;
            BuildNumber = buildNumber;
        }

        public override string ToString()
        {
            return $"{MajorRelease}.{MinorRelease}.{BuildNumber}";
        }
    }
}