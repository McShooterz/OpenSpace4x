using UnityEngine;
using System.Collections.Generic;

public class Credits
{
    public List<CreditsGroup> CreditGroups = new List<CreditsGroup>();

    public class CreditsGroup
    {
        public string GroupName;
        public List<string> GroupItems = new List<string>();
    }
}
