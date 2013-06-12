namespace Elipori
{
    public static class ContentFactory
    {
        private static int servedContentCount;
        private const int MAX_ACCESSOR_COUNT = 26*26;

        static ContentFactory()
        {
            ContentCollection = new string[MAX_ACCESSOR_COUNT];
            for (int i = 0; i < MAX_ACCESSOR_COUNT; i++)
            {
                ContentCollection[i] = CreateString(i);
            }
        }

        //can be inlined  - might improve performance?
        private static string CreateString(int i)
        {
            char c1 = (char)((i/26) + 'A');
            char c2 = (char)((i%26) + 'A');
            return c1.ToString() + c2;
        }

        private static readonly string[] ContentCollection;

        public static string GetRandomButtonContent()
        {
            return ContentCollection[servedContentCount++];
        }
    }
}
