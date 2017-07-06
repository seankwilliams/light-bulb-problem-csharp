using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("CalculateLightBulbState C# HTTP trigger function processed a request.");

    var data = await req.Content.ReadAsStringAsync();
    log.Info(data);
    var formValues = data.Split('&')
        .Select(value => value.Split('='))
        .ToDictionary(pair => Uri.UnescapeDataString(pair[0]), 
                      pair => Uri.UnescapeDataString(pair[1]));

    if (!formValues.Keys.Contains("lightBulbs") || !formValues.Keys.Contains("people")) {
        return req.CreateResponse(HttpStatusCode.BadRequest, "The lightBulbs and people parameters are required");
    }

    int totalLightBulbs, people;
    if (!int.TryParse(formValues["lightBulbs"], out totalLightBulbs) || !int.TryParse(formValues["people"], out people)) {
        return req.CreateResponse(HttpStatusCode.BadRequest, "lightBulbs and people parameters must be of type int");
    }

    if (totalLightBulbs < 1 || people < 1) {
        return req.CreateResponse(HttpStatusCode.BadRequest, "lightBulbs and people parameters must be >= 1");
    }

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
