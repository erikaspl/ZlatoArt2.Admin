﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Web.Tests.Infrastructure
{
    public class JsonContent : StringContent
    {
        public JsonContent(object value)
            : base(SerializeToJson(value))
        {
        }

        private static string SerializeToJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
