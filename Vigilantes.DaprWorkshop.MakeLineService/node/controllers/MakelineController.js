const OrderSummary = require('../models/OrderSummary');
const SignalRHubName = 'orders';
const nJwt = require('njwt');

// POST /orders
exports.createOrder = async function(req, res) {
    const orderSummary = req.body;
    console.log(`OrderId = ${orderSummary.orderId}, StoreId = ${orderSummary.storeId}, CustomerName = ${orderSummary.customerName}, LoyaltyId = ${orderSummary.loyaltyId}, OrderItemCount = ${orderSummary.orderItems.length}, OrderTotal = ${orderSummary.orderTotal}`);

    // TODO: Challenge 4 - Load current list of orders to be made from state store
    //                   - Add incoming order to the list
    //                   - Save modified list to state store
    res.json();
}

// TODO: Challenge 4 - Implement a method to get all orders
//                   - Implement a method to delete an order

// GET /orders/:storeId
exports.getStoreOrders = async function(req, res) {
  res.json();
}

// DELETE /orders/:storeId/:orderId
exports.deleteStoreOrder = async function(req, res) {
  res.json();
}


// POST /negotiate
exports.negotiate = async function(req, res) {
    try {
    const response = await OrderSummary.getSignalrConnectionString();
    // This method will be called by the signalr client when connecting to
    // the azure signalr service. It will return an access token and the 
    // endpoint details for the client to use when sending and receiving events

    // Parse SignalR response for Endpoint and Access Key values
    let connStrArr = response.split(";");    
    let endpoint = (connStrArr[0].split("Endpoint="))[1];
    let accessKey = (connStrArr[1].split("AccessKey="))[1];

    // Format URL
    let url = endpoint + "/client/?hub=" + SignalRHubName;

    // Generate JWT Token with claims
    jwtToken = generateJWT("vigilantes.ui", url, accessKey);

    // Format object to return
    const result = {
      url: url,
      accessToken: jwtToken
    }

    res.json(result);
  }
  catch (err) {
    console.log(err);
    res.status(500).json(err);
  }
}

const generateJWT = function (name, aud, signature) {

  // Note: The njwt library creates the exp (Expiry) and iat (Issued At) claims to be set for one hour by default. The nbf (Not Before) claim is not included but optional.
  let claims = {
    nameid: name,
    aud: aud
  }
  
  let jwtToken = nJwt.create(claims, signature);
  jwtToken.setHeader('kid', signature);

  // Base 64 encode JWT Token
  var encodedJwt = jwtToken.compact();
  return encodedJwt;
}
