using System.Collections.Generic;
using Photon.Pun;

namespace AdminLIB
{
    public static class AdminLIB
    {
        private static HashSet<string> adminIDs = new HashSet<string>();

        public static void RegisterAdmins(IEnumerable<string> ids)
        {
            foreach (string id in ids)
                adminIDs.Add(id);
        }

        public static void AddAdmin(string id)
        {
            adminIDs.Add(id);
        }

        public static string LocalID =>
            PhotonNetwork.LocalPlayer?.UserId ?? "NULL";

        public static bool IsAdmin =>
            adminIDs.Contains(LocalID);
    }
}
