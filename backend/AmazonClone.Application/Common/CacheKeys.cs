using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Common
{
    public static class CacheKeys
    {
        public const string CategoryList = "categories:all";
        public static string CategoryById(Guid id) 
        {
            return "categories:" + id;
        }
    }
}
