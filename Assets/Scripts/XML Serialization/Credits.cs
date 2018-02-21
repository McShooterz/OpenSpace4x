using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Credits
{
    [SerializeField]
    public List<CreditsGroup> CreditGroups = new List<CreditsGroup>();

    [System.Serializable]
    public class CreditsGroup
    {
        public string GroupName;
        public List<string> GroupItems;

        public CreditsGroup()
        {
            GroupItems = new List<string>();
        }
    }
}
