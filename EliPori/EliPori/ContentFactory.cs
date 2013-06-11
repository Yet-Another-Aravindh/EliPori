﻿using System.Collections.Generic;

namespace InfiniteBoard
{
    public static class ContentFactory
    {
        private static int servedContentCount;
        private static List<string> contentCollection = new List<string>

        public static string GetRandomButtonContent()
        {
            return contentCollection[servedContentCount++];
        }
    }
}