using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.FunctionHandler.Models;

public class SaveCrawlRequest
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public IEnumerable<string> Images { get; set; } = [];
    public IDictionary<string, int> TopWords { get; set; } = new Dictionary<string, int>();
    public int PageWordsCount { get; set; }
    public DateTime CapturedAt { get; set; }
}
