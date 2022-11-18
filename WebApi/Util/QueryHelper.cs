using System.Collections.Generic;

public static class QueryHelper
{
    public static Dictionary<string, string?> CreateQueryParams(string fromDate, string toDate, string currency = "eur")
    {
        var datetimeSuffix = "T00:00:00.000";
        var from = DateHelper.DateToUnixTime(fromDate + datetimeSuffix).ToString();
        var to = (DateHelper.DateToUnixTime(toDate + datetimeSuffix) + 3600).ToString();

        var parameters = new Dictionary<string, string?>();
        parameters.Add("vs_currency", currency);
        parameters.Add("from", from);
        parameters.Add("to", to);
        return parameters;
    }
}