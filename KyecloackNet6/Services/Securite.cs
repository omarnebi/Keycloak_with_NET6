using Newtonsoft.Json.Linq;
using System;

namespace KyecloackNet6.Services
{
    public class Securite
    {
        public const string  password = "password";
        public const string ClientId = "NET6";
        public const string ClientSecret = "sMcTBVU6rWIMAGdv6h3ILS5Ytc4JCrV0";

        public IEnumerable<string> GetRolesFromRealmAccess(string realmAccessRoles)
        {
            // Analyser la chaîne JSON pour extraire les rôles
            // Vous pouvez utiliser un analyseur JSON ou un sérialiseur/désérialiseur pour cela
            // Dans cet exemple, nous supposons que la chaîne est un tableau JSON
            var json = JObject.Parse(realmAccessRoles);
            var rolesArray = json["roles"] as JArray;
            if (rolesArray != null)
            {
                return rolesArray.Select(r => r.ToString());
            }
            return Enumerable.Empty<string>();
        }

    }


}
