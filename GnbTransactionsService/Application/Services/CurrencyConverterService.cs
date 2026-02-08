using GnbTransactionsService.Domain.Exceptions;
using GnbTransactionsService.Domain.Models;
using System.Collections.Generic;

namespace GnbTransactionsService.Application.Services
{
    public class CurrencyConverterService
    {
        private readonly Dictionary<string, List<Rate>> convertGraph;
        private readonly Dictionary<(string , string ), decimal> rateCache = new();

        /// <summary>
        /// Initializes a new instance of the CurrencyConverterService class using the specified collection of currency
        /// rates.
        /// </summary>
        public CurrencyConverterService(IEnumerable<Rate> rates)
        {
            convertGraph = BuildGraph(rates);
        }

        /// <summary>
        /// Converts the given amount from one currency to another using the provided rates.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <returns></returns>
        public decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
                return amount;

            //use cahe
            var key = (fromCurrency, toCurrency);

            if (!rateCache.TryGetValue(key, out var rate))
            {
                rate = FindConversionRate(fromCurrency, toCurrency);
                rateCache[key] = rate;
            }

            return amount * rate;
        }

        /// <summary>
        /// Uses BFS to find the conversion path and calculate the accumulated rate.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="CostumException"></exception>
        private decimal FindConversionRate(string from, string to)
        {
            //BFS to find the conversion path and calculate the accumulated rate

            HashSet<string> visited = new HashSet<string>();
            var queue = new Queue<(string, decimal)>();

            queue.Enqueue((from, 1m));

            while (queue.Count > 0)
            {
                var (current, accumulatedRate) = queue.Dequeue();

                //visited? ignore!
                if (!visited.Add(current))
                    continue;

                //no conversion from current? ignore!
                if (!convertGraph.TryGetValue(current, out var edges))
                    continue;
                
                foreach (var rate in edges)
                {
                    decimal nextRate = accumulatedRate * rate.Value;

                    //if we reach the target currency we return the total rate
                    if (rate.To == to)
                        return nextRate;

                    //if not we add that currency to the queue to continue searching
                    queue.Enqueue((rate.To, nextRate));
                }
            }

            //if the queue empties and the currency is not found
            throw new CurrencyConversionException(
                $"No conversion path found from {from} to {to}"
            );
        }

        /// <summary>
        /// builds a graph from the list of rates.
        /// </summary>
        /// <param name="rates"></param>
        /// <returns></returns>
        private static Dictionary<string, List<Rate>> BuildGraph(IEnumerable<Rate> rates)
        {
            Dictionary<string, List<Rate>> graph = new();

            foreach (var rate in rates)
            {
                if (!graph.ContainsKey(rate.From))
                    graph[rate.From] = new List<Rate>();

                graph[rate.From].Add(rate);
            }

            return graph;
        }
    }
}
