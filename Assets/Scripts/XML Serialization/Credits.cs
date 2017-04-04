using UnityEngine;
using System.Collections.Generic;

public class Credits
{
    public List<CreditsGroup> CreditGroups = new List<CreditsGroup>();

    public class CreditsGroup
    {
        public string GroupName { get; set; }
        public List<string> GroupItems { get; set; }

        public CreditsGroup()
        {
            GroupItems = new List<string>();
        }
    }
}
