using static GmailReader.Domain.Resources.GmailValues;

namespace GmailReader.Domain.Constants
{
    public static class GmailValues
    {
        public static string ME { get { return ME_KEY; } }
        public static string FROM { get { return FROM_KEY; } }
        public static char EMAIL_INF_LIMIT { get { return EMAIL_INF_LIMIT_KEY.First(); } }
        public static char EMAIL_SUP_LIMIT { get { return EMAIL_SUP_LIMIT_KEY.First(); } }
        public static string FORMAT_DATE { get { return FORMAT_DATE_KEY; } }
    }
}
