const OrderSummary = require('../models/OrderSummary');

exports.newOrder = async function(req, res) {
    const order = req.body;

    // Create an order summary that will be used as the 
    // message body when published
    try {
        const orderSummary = await OrderSummary.create(order);
        console.log(`Created Order Summary: OrderId = ${orderSummary.orderId}, StoreId = ${orderSummary.storeId}, CustomerName = ${orderSummary.customerName}, LoyaltyId = ${orderSummary.loyaltyId}, OrderItemCount = ${orderSummary.orderItems.length}, OrderTotal = ${orderSummary.orderTotal}`);

        // TODO: Challenge 2 - Publish an OrderSummary message via Dapr
        res.send("Bummer. Business logic and pub/sub isn't implemented yet but, hey, at least your POST worked and you should see the order in the log! YOINK!");
    }
    catch (err) {
        console.log(err);
        res.status(500).json(err);
    }
};