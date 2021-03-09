const Receipt = require('../models/Receipt');

// POST /orders
exports.generateReceipt = async function(req, res) {
    const orderSummary = req.body;
    console.log(`Created Order Summary: OrderId = ${orderSummary.orderId}, StoreId = ${orderSummary.storeId}, CustomerName = ${orderSummary.customerName}, LoyaltyId = ${orderSummary.loyaltyId}, OrderItemCount = ${orderSummary.orderItems.length}, OrderTotal = ${orderSummary.orderTotal}`);

    // TODO: Challenge 5 - Store receipt via a Dapr Output Binding that can be used as a data sink
    res.json();
}