The CalculateLightBulbStatus endpoint is set up in Azure Functions and can be called with this endpoint:

	POST https://sean-test-functions.azurewebsites.net/api/CalculateLightBulbState?code=FoclIyUdkAMHI5q8KvYINwfXNrZdoPiQx5Kbx/M9VpTeIeN/Ic1Pbw==
	{
		lightBulbs: [value],
		people: [value]
	}

Where the lightBulbs and people parameters are the number of light bulbs and people to pass into the calculation.

*CSS Generation*

CSS is written with LESS, and can be generated with this in command line:

	lessc styles.less styles.css -compress

Place the contents of styles.css into the inline style tag in index.html (at this amount of CSS, we'll inline CSS to avoid a render-blocking call to an external stylesheet)