using System.Collections.Generic;

namespace OptifineDownloader.Utils.StringManager
{
    abstract class StringManager
    {
        public static string[] RemoveExcess(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                ref string link = ref array[i];
                int startPos = link.IndexOf("OptiFine");  
                link = link[startPos..];
                int endPos = link.IndexOf(".jar&");
                link = link[..endPos];
                link = link.Replace('_', ' ');
            }
            return array;
        }
        
        public static Dictionary<string, List<string>> GroupingByVersions(string[] array)
        {
            Dictionary<string, List<string>> versions = new();
            foreach (var element in array)
            {
                int pos;
                pos = element.IndexOf(" HD");
                string mcVersion = element[..pos];
                string OptifineVersion = element.Substring(pos+1);
                OptifineVersion = "OptiFine " + OptifineVersion;
                mcVersion = mcVersion.Replace(".0", "");
                mcVersion = mcVersion.Replace("OptiFine", "Minecraft");
                if (versions.ContainsKey(mcVersion))
                {
                    versions[mcVersion].Add(OptifineVersion);
                } else
                {
                    versions[mcVersion] = new List<string>
                    {
                        OptifineVersion
                    };
                }
            }
            return versions;
        }

        public static string GetDownloadPageUrl(string mcVersion,string optiVersion)
        {
            mcVersion = mcVersion.Replace("Minecraft", "OptiFine");
            optiVersion = optiVersion.Replace("OptiFine", "");
            if(optiVersion.Split('.').Length==2)
            {
                optiVersion += ".0";
            }
            string version = mcVersion + optiVersion + ".jar";
            version = version.Replace(' ', '_');
            if(version.IndexOf("pre")>-1)
            {
                version = "preview_" + version;
            }
            string link = "https://optifine.net/adloadx?f=" + version;
            return link;
        }

        public static string GetDownloadName(string mcVersion, string optiVersion)
        {
            int pos = mcVersion.IndexOf('1');
            mcVersion = mcVersion[pos..];
            string name = optiVersion.Insert(8,' ' + mcVersion);
            return name;
        }
    }
}
