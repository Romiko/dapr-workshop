const express = require('express');
const app = express();
const httpClient = require('axios');
const port = normalizePort(process.env.PORT || '5500');
const DaprHttpPort = process.env.DAPR_HTTP_PORT || "3500";
const storeId = process.env.StoreId || "Redmond";

app.post ('/orders', async function(req, res){    
        
    console.log("schedule trigger invoked: " + new Date().toISOString());
    
    const orders = await getOrders(storeId);
    
    if (orders.length > 0){
        const order = orders[0];        
        console.log(`The VirtualBarista is making an order for ${order.customerName} ...`);
        await completeOrder(order.storeId, order.orderId);
        console.log(`${order.customerName}, your drinks are ready!`);        
    } else {
        console.log('The make line is empty! Time to drum up some customers!');
    }

   res.sendStatus(200);
});

const getOrders = async function(storeId) {
    return new Promise(async function(resolve, reject){
    });
}

const completeOrder = async function(storeId, orderId){
    return new Promise(async function(resolve, reject){
    });
    
}

function normalizePort(val) {
    var port = parseInt(val, 10);
  
    if (isNaN(port)) {
      return val;
    }
  
    if (port >= 0) {
      return port;
    }
  
    return false;
}

app.listen(port, '127.0.0.1', () => console.log(`Virtual Barista listening on port ${port}`))
