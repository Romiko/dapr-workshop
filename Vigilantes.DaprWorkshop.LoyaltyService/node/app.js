var express = require('express');
var path = require('path');
var logger = require('morgan');

var router = require('./routes/index');
var app = express();

app.use(logger('dev'));
app.use(express.json({type: ["application/json", "application/cloudevents+json"]}));
app.use(function(req, res, next){
    if(req.headers["content-type"] == "application/cloudevents+json") {
        req.body = req.body.data
    }
    next();
});
app.use(express.urlencoded({ extended: false }));
app.use(express.static(path.join(__dirname, 'public')));

app.use('/', router);

console.log("Welcome to the Loyalty Service (Node)!");
module.exports = app;