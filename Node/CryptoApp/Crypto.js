const crypto = require("crypto");

const numOfRequests = 10;

var base = Date.now();

for (let i = 1; i < numOfRequests + 1; i++) {
    var start = Date.now();

    var result = crypto.pbkdf2("password", "salt", 10000, 512, "sha512",
        () => {
            var duration = Date.now() - start;
            console.log(i, " => ", start - base, " | ", duration);

        });
}