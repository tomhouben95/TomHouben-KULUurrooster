using System;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Helpers;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.HTMLParserServices
{
    public class EffectiveTimeTableParser: IEffectiveTimeTableParser
    {
        public EffectiveTimeTableParser()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("nl-BE");
        }


        public IEnumerable<TimeTableEntry> Parse(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var dates = doc.DocumentNode.SelectNodes(XPathConstants.DatePath);

            var result = new List<TimeTableEntry>();

            foreach(var date in dates)
            {

                try
                {
                    var parsedDate = HtmlParserHelper.ParseDateRegion(date.InnerText);

                    result.AddRange(HtmlParserHelper.ParseTable(date.ParentNode.NextSibling.NextSibling, parsedDate));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    continue;
                }
            }

            return result;
        }
    }
}
