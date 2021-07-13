using System;
using System.Text.RegularExpressions;

namespace SmartDocumentor.GenericPlugin.Demo.Base
{
    public static class Constants
    {
        public static readonly Regex RegexLine = new Regex(@"^(?<Linha>\d{1,3})[ ](?<Codigo>(\w+))$", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        public static class Campos
        {
            public const string CustomTable = "Custom_Table";
            public const string VendorVAT = "VendorVAT";
        }
    }
}