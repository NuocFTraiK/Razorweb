﻿namespace CS54.Helpers
{
    public class PagingModel
    {
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }

        public Func<int?,string> GenerateUrl { get; set; }

    }
}
