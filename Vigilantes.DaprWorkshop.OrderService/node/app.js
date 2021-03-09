var express = require('express');
var path = require('path');
var logger = require('morgan');

var router = require('./routes/index');
var app = express();

// enable cors
app.use(function(req, res, next){
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
})
app.use(logger('dev'));
app.use(express.json({type: ["application/json", "application/cloudevents+json"]}));
app.use(function(req, res, next){
    if(req.headers["content-type"] == "application/cloudevents+json") {
        req.body = req.body.data
    }
    next();
});
app.use(express.urlencoded({ extended: false }));
app.use('/', router);

console.log("Welcome to the Order Service (Node)!");
module.exports = app;