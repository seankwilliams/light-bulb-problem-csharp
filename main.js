//only run jquery selectors once as the selector process invovles additional processing time
var $lightBulbs = $("input[name=light-bulbs]");
var $people = $("input[name=people]");
var $resultsValues = $(".results__values");
var $totalLightBulbsOn = $("#total-light-bulbs-on");
var $specificLightBulbsOn = $("#specific-light-bulbs-on");
var $body = $("body");
var lightBulbs, people, startTime;

$(function() {
	//hide results when changing values, and format numbers
	$lightBulbs.add($people).change(function(){
		$resultsValues.hide();
	});
	
	//process user input
	$("form").submit(function(e) {
		e.preventDefault();
		
		if ($body.hasClass("loading")) return;
		
		lightBulbs = Number($lightBulbs.val());
		people = Number($people.val());
		startTime = performance.now();
		
		//validate
		$("input.error").removeClass("error");
		var error = false;
		if (lightBulbs.length == 0 || !Number.isInteger(lightBulbs) || lightBulbs < 1) {
			$lightBulbs.addClass("error");
			error = true;
		}
		if (people.length == 0 || !Number.isInteger(people) || people < 1) {
			$people.addClass("error");
			error = true;
		}
		if (error) return;
		
		$body.addClass("loading");
		
		$.post("https://sean-test-functions.azurewebsites.net/api/CalculateLightBulbState?code=FoclIyUdkAMHI5q8KvYINwfXNrZdoPiQx5Kbx/M9VpTeIeN/Ic1Pbw==", {
				"lightBulbs": lightBulbs,
				"people": people
			}, function(results) {
				$body.removeClass("loading");
				//output results
				$totalLightBulbsOn.html(formatNumberWithCommas(results.length));
				$specificLightBulbsOn.html(results.join(', '));
				$resultsValues.show();
				
				//write the execution time to console
				var endTime = performance.now();
				console.log("Calculating " + lightBulbs + " light bulbs and " + people +
							" people took " + (endTime - startTime) + " milliseconds.");
		});
	});
});

//polyfill for isInteger from MDN
Number.isInteger = Number.isInteger || function(value) {
	return typeof value === 'number' && 
		isFinite(value) && 
		Math.floor(value) === value;
};

function formatNumberWithCommas(n) {
	n = String(n);
	n = n.replace(",", "");
	return n.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}