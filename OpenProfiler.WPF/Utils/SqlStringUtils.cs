namespace OpenProfiler.WPF.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using SqlFormatter;

    public class SqlStringUtils
    {
        private static Regex variableRegex = new Regex(@"(@p\d+)\s+=\s+(.+?)\s\[");

        public static string Format(string str)
        {
            List<string> result = SqlFormatter.SplitQuery(str).ToList();

            string query;

            if (result.Count > 1)
            {
                query = string.Join(string.Empty, result.Take(result.Count - 1));

                MatchCollection variableMatches = variableRegex.Matches(result.Last());
                
                foreach (Match m in variableMatches)
                {
                    query = query.Replace(m.Groups[1].Value, m.Groups[2].Value);
                }
            }
            else
            {
                query = result[0];
            }

            return SqlFormatter.Format(query).Trim();
        }
    }
}
