using System;
using System.Collections.Generic;

public static class QueryHelper
{
    public static Dictionary<string, string?> CreateQueryParams(DateOnly fromDate, DateOnly toDate, string currency = "eur")
    {
        var from = fromDate.ToUnixTimestamp().ToString();
        var to = toDate.ToUnixTimestamp().ToString();
        var parameters = new Dictionary<string, string?>
        {
            { "vs_currency", currency },
            { "from", from },
            { "to", to }
        };
        return parameters;
    }
}