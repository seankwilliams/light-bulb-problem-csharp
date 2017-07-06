using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("CalculateLightBulbState C# HTTP trigger function processed a request.");

    int totalLightBulbs = int.Parse(GetQueryString(req, "lightBulbs"));
    int people = int.Parse(GetQueryString(req, "people"));

    log.Info("totalLightBulbs: " + totalLightBulbs.ToString() + " | people: " + people.ToString());

    //determine which light bulbs are on
    bool[] lightBulbStatus = new bool[totalLightBulbs];
    log.Info(lightBulbStatus.Count().ToString());
    for (int currentPerson = 1; currentPerson <= people; currentPerson++) {
        for (int j = currentPerson; j <= totalLightBulbs; j += currentPerson) {
            //toggle the light bulb on/off
            lightBulbStatus[j-1] = !lightBulbStatus[j-1];
        }
    }

    //get the light bulb numbers that are on (where 1 is the first light bulb)
    List<int> lightBulbsOn = lightBulbStatus.Select((isOn, index) => new {index, isOn})
                                            .Where(t => t.isOn)
                                            .Select(t => t.index + 1)
                                            .ToList();

    return req.CreateResponse(HttpStatusCode.OK, lightBulbsOn, "application/json");
    
}

public static string GetQueryString(HttpRequestMessage request, string key)
{      
    var queryStrings = request.GetQueryNameValuePairs();
    if (queryStrings == null)
        return null;

    var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
    if (string.IsNullOrEmpty(match.Value))
        return null;

    return match.Value;
}