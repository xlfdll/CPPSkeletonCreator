using System;
using System.Text;
using System.IO;

namespace CPPSkeletonCreator
{
    internal static class Helper
    {
        internal static String CreateHeaderContents(String headerFileName)
        {
            StringBuilder sb = new StringBuilder();

            String headerFlag = Path.GetFileName(headerFileName).ToUpperInvariant().Replace(".", "_");

            sb.AppendFormat("#ifndef {0}", headerFlag);
            sb.AppendLine();
            sb.AppendFormat("#define {0}", headerFlag);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("#endif");

            return sb.ToString();
        }

        internal static String CreateCodeContents(String headerFileName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"#include ""{0}""", Path.GetFileName(headerFileName));
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
}