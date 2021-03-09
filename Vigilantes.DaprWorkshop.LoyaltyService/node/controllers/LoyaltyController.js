const LoyaltySummary = require('../models/LoyaltySummary');

// POST /orders
exports.orders = async function(req, res) {
  const orderSummary = req.body;
  console.log(`OrderId = ${orderSummary.orderId}, StoreId = ${orderSummary.storeId}, CustomerName = ${orderSummary.customerName}, LoyaltyId = ${orderSummary.loyaltyId}, OrderItemCount = ${orderSummary.orderItems.length}, OrderTotal = ${orderSummary.orderTotal}`);
  
  // TODO: Challenge 3 - Retrieve and update customer loyalty points
  res.json();
};