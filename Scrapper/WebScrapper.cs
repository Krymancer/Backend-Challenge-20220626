using System;
using System.Xml.Linq;
using Application.Products;
using Domain.Products;
using HtmlAgilityPack;

namespace Scrapper
{
    public class WebScrapper
    {
        private readonly string _url = "https://world.openfoodfacts.org/";

        private readonly Dictionary<string, string> _xpaths  =  new Dictionary<string, string>(){
            {"code","//*[@id=\"barcode\"]"},
            {"barcode","//*[@id=\"barcode_paragraph\"]/text()[2]"},
            {"name","//*[@id=\"prodHead\"]/div/div/h1"},
            {"quantity","//*[@id=\"field_quantity_value\"]"},
            {"categories","//*[@id=\"field_categories_value\"]"},
            {"pakaging","//*[@id=\"field_packaging_value\"]"},
            {"brands","//*[@id=\"field_brands_value\"]"},
            {"og_image","//*[@id=\"og_image\"]"},
        };

        private readonly IProductService _productService;

        public WebScrapper(IProductService productService) 
        {
            _productService = productService;
        }

        public async Task Scrap() {
            var links = GetProductLinks();
            var products = new List<Product>();

            foreach (var link in links.Take(100)) {
                products.Add(GetProduct(link));
            }

            await UpdateDatabase(products);
        }

        private async Task UpdateDatabase(IEnumerable<Product> products)
        {
            var dbProducts = await _productService.GetAsync();

            var toInsert = products.Where(x => !dbProducts.Any(y => y.Code == x.Code)).ToList();

            foreach(var product in products) 
            {
                var prod = await _productService.GetByCodeAsync(product.Code!);
                if (prod is null) continue;
                await _productService.UpdateAsync(prod.Id!, product);
            }

            await _productService.CreateRangeAsync(toInsert);
        }

        private string? GetElementsInnerText(HtmlDocument documment, string xpath)
        {
            var innerTexts = documment.DocumentNode.SelectSingleNode(xpath).ChildNodes.Select(a => a.InnerText);
            return string.Join("", innerTexts);
        }
        private string? GetElementText(HtmlDocument documment, string xpath)
        {
           return documment.DocumentNode.SelectSingleNode(xpath)?.InnerText;
        }

        private string? GetElementAttribute(HtmlDocument documment, string xpath, string attribute) {
            return documment.DocumentNode.SelectSingleNode(xpath)?.Attributes[attribute]?.Value;
        }

        private string? GetImageUrl(HtmlDocument documment, string xpath) => GetElementAttribute(documment, xpath, "src");

        private Product GetProduct(string uri)
        {
            var product = new Product();

            var documment = GetPage(uri);

            product.Code = GetElementText(documment,_xpaths["code"]);
            product.Barcode = product.Code + GetElementText(documment,_xpaths["barcode"])?.Trim();
            product.ProductName = GetElementText(documment,_xpaths["name"]);
            product.Quantity = GetElementText(documment,_xpaths["quantity"]);
            product.Categories = GetElementsInnerText(documment,_xpaths["categories"]);
            product.Packaging = GetElementText(documment,_xpaths["pakaging"]);
            product.Brands = GetElementText(documment,_xpaths["brands"]);
            product.ImageUrl = GetImageUrl(documment,_xpaths["og_image"]);
            product.Url = uri;
            product.Imported = DateTime.Now;
            product.status = Domain.SeedWork.Enums.EnumProductStatus.imported;

            return product;
        }

        private List<string> GetProductLinks()
        {
            var htmlDoc = GetPage(_url);

            var productsXPATH = "//body/div/div[2]/div[2]/div/div/div/div[6]/div/div/ul/li/a";

            var productList = htmlDoc.DocumentNode.SelectNodes(productsXPATH);

            var links = new List<string>();

            var baseUri = new Uri(_url);

            foreach ( var product in productList ) {
                var link = product.Attributes["href"].Value;
                link = new Uri(baseUri, link).AbsoluteUri;
                links.Add(link);
            }

            return links;
        }
        
        private static HtmlDocument GetPage(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDocument = web.Load(url);
            return htmlDocument;
        }
    }
}