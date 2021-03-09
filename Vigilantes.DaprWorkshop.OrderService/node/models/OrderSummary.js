const uuid = require('uuid').v4;
const httpClient = require('axios');
const MenuItem = require('../models/MenuItem');
const DaprHttpPort = process.env.DAPR_HTTP_PORT || "3500";
const DaprPubSubURI = `http://localhost:${DaprHttpPort}/v1.0/publish`

const create = async function (order) {
    const orderSummary = {};
    return new Promise(async function(resolve, reject){
        try {
            const orderItems = await itemSummaries(order.orderItems);

            orderSummary.orderId = uuid();
            orderSummary.storeId = order.storeId;
            orderSummary.customerName = order.customerName;
            orderSummary.loyaltyId = order.loyaltyId;
            orderSummary.orderDate = new Date().toISOString();
            orderSummary.orderItems = orderItems.items;
            orderSummary.orderTotal = orderItems.orderTotal;
            
            resolve(orderSummary);
        }
        catch (err) {
            reject(err);
        }
    });
}

const publish = function(order) {
    return new Promise(async function(resolve, reject){
    });
}

const itemSummaries = async function (orderItems) {
    const itemSummary = {
        items: [],
        orderTotal: 0
    }

    for (const orderItem of orderItems) {
        let menuItem = await MenuItem.findById(orderItem.menuItemId);

        itemSummary.items.push({
            quantity: orderItem.quantity,
            menuItemId: orderItem.menuItemId,
            menuItemName: menuItem.name
        });       
        itemSummary.orderTotal+=(menuItem.price * orderItem.quantity);
    }

    return Promise.resolve(itemSummary);
}

exports.create = create;
exports.publish = publish;