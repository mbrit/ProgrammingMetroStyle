using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.Storage.Streams;

namespace StreetFoo.Client
{
    public static class SearchInteractionHelper
    {
        public static async Task PopulateSuggestionsAsync(string queryText, SearchSuggestionCollection results)
        {
            // if we don't have at least three characters to work with, do nothing...
            if(queryText.Length < 3)
                return;

            // how many?
            int maxSuggestions = 5;

            // get the list...
            var suggestions = await ReportItem.GetSearchSuggestionsAsync(queryText);

            // sort the suggestions...
            var titles = new List<string>();
            foreach (var title in suggestions.Keys)
                titles.Add(title);
            titles.Sort();

            // do we have one that we can use as a recommendation?
            ReportItem recommendation = null;
            foreach (var title in titles)
            {
                if (suggestions[title].Count == 1)
                {
                    recommendation = suggestions[title][0];
                    break;
                }
            }

            // if we have a recommendation only show three suggestions...
            if (recommendation != null)
                maxSuggestions -= 2;

            // add the suggestions...
            foreach (var title in titles)
            {
                results.AppendQuerySuggestion(title);

                // enough?
                if (results.Size == maxSuggestions)
                    break;
            }

            // add the recommendation...
            if (recommendation != null)
            {
                // we need an image...
                var viewItem = new ReportViewItem(recommendation);
                var imageUri = await new ReportImageCacheManager().GetLocalImageUriAsync(viewItem);

                // add the suggestion...
                results.AppendSearchSeparator("Recommendation");
                results.AppendResultSuggestion(recommendation.Title, recommendation.Description, recommendation.Id.ToString(),
                    RandomAccessStreamReference.CreateFromUri(new Uri(imageUri)), recommendation.Title);
            }
        }
    }
}
