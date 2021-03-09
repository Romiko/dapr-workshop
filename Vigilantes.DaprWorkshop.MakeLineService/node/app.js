var express = require('express');
var cors = require('cors')
var path = require('path');
var logger = require('morgan');

var router = require('./routes/index');
var app = express();

var corsOptions = {
    origin: 'http://127.0.0.1:8080',
    credentials: true 
}

app.use(cors(corsOptions));
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

console.log("Welcome to the Make Line Service (Node)!");
module.exports = app;