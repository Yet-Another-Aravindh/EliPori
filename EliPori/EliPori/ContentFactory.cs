using System.Threading.Tasks;

namespace Elipori
{
    public static class ContentFactory
    {
        private const int MAX_ACCESSOR_COUNT = 26*26;
        private static int servedContentCount;
        private static readonly string[] ContentCollection;

        static ContentFactory()
        {
            ContentCollection = new string[MAX_ACCESSOR_COUNT];

            Parallel.For(0, MAX_ACCESSOR_COUNT, i => { ContentCollection[i] = CreateString(i); });
        }

        //can be inlined  - might improve performance?
        private static string CreateString(int i)
        {
            var c1 = (char) ((i/26) + 'A');
            var c2 = (char) ((i%26) + 'A');
            return c1.ToString() + c2;
        }

        public static string GetRandomButtonContent()
        {
            return ContentCollection[servedContentCount++];
        }
    }
}