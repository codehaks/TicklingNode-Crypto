const crypto = require("crypto");

const numOfRequests = 10;

var base = date.now();
for (let i = 1; i < numOfRequests + 1; i++) {
    var start = date.now();

    crypto.pbkdf2sync("password", "salt", 10000, 512, "sha512");

    var duration = date.now() - start;
    console.log(i, " => ", start - base, " | ", duration);

}