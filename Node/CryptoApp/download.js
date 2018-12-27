const https = require('https');

const numOfRequests = 10;

var base = Date.now();

for (let i = 1; i < numOfRequests + 1; i++) {
    var start = Date.now();

    https.request("https://codehaks.com/images/me.jpg",
        (result) => {
            var duration = Date.now() - start;
            console.log(i, " => ", start - base, " | ", duration);

        });
}