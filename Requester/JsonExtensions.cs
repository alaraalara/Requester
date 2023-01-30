using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Requester
{
    public static class JsonExtensions
    {
        public static void SetByPath(this JToken obj, string path, JToken value)
        {
            Console.WriteLine(path);
            Console.WriteLine(obj);   
            JToken? token = obj.SelectToken(path);
            Console.WriteLine(token);
            token?.Replace(value);
        }

        
        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, name, matches);       
            return matches;
        }

       

        private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches);
                }
            }
        }

        private static void FindTokensWithCount(JToken containerToken, string name, Dictionary<JToken, int> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                int count = -1;  
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        count++;
                        matches.Add(child.Value, count);
                    }
                    FindTokensWithCount(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokensWithCount(child, name, matches);
                }
            }
        }

        public static JToken FindAndReplace(JToken jToken, string key, JToken value, int? occurence)
        {
            var searchedTokens = jToken.FindTokens(key);
            int count = searchedTokens.Count;

            if (count == 0)
                return $"The key you have to search is not present in json, Key: {key}";

            foreach (JToken token in searchedTokens)
            {
                if (!occurence.HasValue)
                    jToken.SetByPath(token.Path, value);
                else
                if (occurence.Value == searchedTokens.IndexOf(token))
                    jToken.SetByPath(token.Path, value);
            }

            return jToken;
        }


    }

}
