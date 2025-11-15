using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;

namespace AdminLIB
{
    public static class AdminLIB
    {
        private static HashSet<string> adminIDs = new HashSet<string>();
        private static bool loaded = false;

        // GitHub RAW URL e.g.
        // https://raw.githubusercontent.com/YourName/YourRepo/main/admins.txt
        private static string githubURL = "";

        // Call once on your mod's load
        public static void SetGithubURL(string url)
        {
            githubURL = url;
            _ = LoadAdminsFromGithub();
        }

        public static string LocalID =>
            PhotonNetwork.LocalPlayer?.UserId ?? "NULL";

        public static bool IsAdmin
        {
            get
            {
                if (!loaded) return false;
                return adminIDs.Contains(LocalID);
            }
        }

        // Downloads and loads admin IDs
        private static async Task LoadAdminsFromGithub()
        {
            if (string.IsNullOrEmpty(githubURL))
            {
                Debug.LogError("[AdminLIB] GitHub URL not set!");
                return;
            }

            using (UnityWebRequest req = UnityWebRequest.Get(githubURL))
            {
                req.timeout = 10;
                var op = req.SendWebRequest();

                while (!op.isDone)
                    await Task.Yield();

#if UNITY_2020_1_OR_NEWER
                if (req.result != UnityWebRequest.Result.Success)
#else
                if (req.isNetworkError || req.isHttpError)
#endif
                {
                    Debug.LogError("[AdminLIB] Failed to download admin list: " + req.error);
                    return;
                }

                ParseAdminList(req.downloadHandler.text);
            }

            loaded = true;
            Debug.Log("[AdminLIB] Admin list loaded. " + adminIDs.Count + " admins.");
        }

        private static void ParseAdminList(string text)
        {
            adminIDs.Clear();

            text = text.Trim();

            if (text.StartsWith("["))
            {
                string cleaned = text.Replace("[", "")
                                     .Replace("]", "")
                                     .Replace("\"", "");

                foreach (string id in cleaned.Split(','))
                {
                    adminIDs.Add(id.Trim());
                }
            }
            else
            {
                foreach (string line in text.Split('\n'))
                {
                    string id = line.Trim();
                    if (id.Length > 0)
                        adminIDs.Add(id);
                }
            }
        }
    }
}
