using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TwitterSearchApp.Models;
using Tweetinvi;
using OAuthTwitterWrapper;
using Tweetinvi.Models;

namespace TwitterSearchApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //IOAuthTwitterWrapper twit = new OAuthTwitterWrapper.OAuthTwitterWrapper();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var search = "";

            const String consumer_key = "z0v5fL4jhtEppYhENzArQcG6s";
            const String consumer_secret = "yfoogwNT9BtbpR4RzLIncjbTR7WRamv8UHKNQhBwFoWdeNwmoF";
            //const String access_token = "1296417703-LD7fH3zTkhDNSPTFnPhCRz8z84uftlukKfVL5aZ";
            //const String access_token_secret = "1p5glkIU3FIK4sFd01HjLU6gwzEbXXLflyfPkxiWy2w7b";
            const String bearer_token = "AAAAAAAAAAAAAAAAAAAAADMEmgEAAAAAfbgsf%2F9MWZIbt2D%2BjFwroCKyzuY%3D4rzgFoKxgZtzyhTqMM5RIcbmiMWd9EZpfBkWLrJkLqelaTVaQl";
            //TwitterClient userClient = new TwitterClient(consumer_key, consumer_secret, access_token, access_token_secret);
            ConsumerOnlyCredentials appCredentials = new ConsumerOnlyCredentials(consumer_key, consumer_secret)
            {
                BearerToken = bearer_token
            };
            TwitterClient appClient = new TwitterClient(appCredentials);

            //var user = await userClient.Users.GetAuthenticatedUserAsync();
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();
                var tweets = await appClient.Search.SearchTweetsAsync(search);
                ViewBag.Tweets = tweets;
            }

            ViewBag.SearchString = search;

            if (search != "")
            {
                return RedirectToAction("SearchResult", "Home", new { @search = search });
            }
            return View();

        }

        public async Task<IActionResult> SearchResultAsync(string search, bool? image)
        {
            const String consumer_key = "z0v5fL4jhtEppYhENzArQcG6s";
            const String consumer_secret = "yfoogwNT9BtbpR4RzLIncjbTR7WRamv8UHKNQhBwFoWdeNwmoF";
            //const String access_token = "1296417703-LD7fH3zTkhDNSPTFnPhCRz8z84uftlukKfVL5aZ";
            //const String access_token_secret = "1p5glkIU3FIK4sFd01HjLU6gwzEbXXLflyfPkxiWy2w7b";
            const String bearer_token = "AAAAAAAAAAAAAAAAAAAAADMEmgEAAAAAfbgsf%2F9MWZIbt2D%2BjFwroCKyzuY%3D4rzgFoKxgZtzyhTqMM5RIcbmiMWd9EZpfBkWLrJkLqelaTVaQl";
            //TwitterClient userClient = new TwitterClient(consumer_key, consumer_secret, access_token, access_token_secret);
            ConsumerOnlyCredentials appCredentials = new ConsumerOnlyCredentials(consumer_key, consumer_secret)
            {
                BearerToken = bearer_token
            };
            TwitterClient appClient = new TwitterClient(appCredentials);




            var tweets = await appClient.Search.SearchTweetsAsync(search);
            var tweetsWithImages = tweets.Where(tweet =>
                tweet.Entities.Medias.Any(media => media.MediaType == "photo"));
            if (image.HasValue && image.Value)
            {
                ViewBag.Tweets = tweetsWithImages;
                ViewBag.Image = true;

            }
            else
            {
                ViewBag.Tweets = tweets;
                ViewBag.Image = false;
            }
            return View("Search");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}