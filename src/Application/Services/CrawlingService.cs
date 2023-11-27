using System.Text;
using System.Text.RegularExpressions;
using Application.Interfaces;
using Domain.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CrawlingService : ICrawlingService
{
    private readonly ILogger<CrawlingService> _logger;
    private readonly HtmlWeb _webClient;

    public CrawlingService(ILogger<CrawlingService> logger, HtmlWeb htmlWeb)
    {
        _logger = logger;
        _webClient = htmlWeb;
    }

    public async Task<CrawlResult> CrawlAsync(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url), "Parameter url was null or empty in CrawlAsync.");

        try
        {
            _logger.LogInformation("Tries to crawl the url: {url} .", url);
            HtmlDocument htmlDocument = await _webClient.LoadFromWebAsync(url, cancellationToken);

            return new CrawlResult()
            {
                ImageUrls = GetImageUrls(url, htmlDocument),
                Words = GetAllWordsFromPage(url, htmlDocument)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("CrawlAsync finalized with unhandeled exception. Exception message:{message}", ex.Message);
            throw;
        }
    }

    private List<string> GetImageUrls(string url, HtmlDocument document)
    {
        List<string> imageUrls = new();

        var imageNodes = document.DocumentNode.SelectNodes("//img");
        if (imageNodes == null || imageNodes.Count <= 0)
        {
            _logger.LogInformation("No image nodes found for url {url}.", url);
            return imageUrls;
        }

        var outputUrl = CleanUrlForOutput(url);

        foreach (HtmlNode img in imageNodes)
        {
            var srcValue = img.GetAttributeValue("src", null);
            if (string.IsNullOrEmpty(srcValue))
                continue;

            var srcUrl = GetImageSrcUrl(outputUrl, srcValue);
            imageUrls.Add(srcUrl);
        }

        _logger.LogInformation("Was able to load {imageCount} images from {url} .", imageUrls.Count, url);
        return imageUrls;
    }

    private static string CleanUrlForOutput(string url)
    {
        if (url.EndsWith("/"))
        {
            var sb = new StringBuilder(url);
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        return url;
    }

    private static string GetImageSrcUrl(string url, string srcValue)
    {
        return srcValue.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) ?
            srcValue :
            $"{url}{srcValue}";
    }

    private List<string> GetAllWordsFromPage(string url, HtmlDocument document)
    {
        List<string> wordList = new();

        var textNodes = document.DocumentNode.SelectNodes("//text()[not(parent::script) and not(parent::style)]");
        if (textNodes == null || textNodes.Count <= 0)
        {
            _logger.LogInformation("No text nodes found for url {url}.", url);
            return wordList;
        }

        foreach (HtmlNode node in textNodes)
        {
            if (string.IsNullOrWhiteSpace(node.InnerText))
                continue;

            wordList.AddRange(GetWordsFromHtmlNode(node));
        }

        _logger.LogInformation("Was able to load {wordCound} words from {url} .", wordList.Count, url);
        return wordList;
    }

    private static List<string> GetWordsFromHtmlNode(HtmlNode node)
    {
        return Regex.Matches(node.InnerText, @"\b[a-zA-Z]{2,}\b")
            .Cast<Match>()
            .Select(m => m.Value)
            .Where(w => !w.ToLower().Contains("nbsp") && !Regex.IsMatch(w, @"\d"))
            .ToList();
    }
}
