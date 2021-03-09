const httpClient = require('axios');
const makeOrder = async function(order) {
    return new Promise(async function(resolve, reject){
        // TODO: Challenge 4 - Load current list of orders to be made from state store
        //                   - Add incoming order to the list
        //                   - Save modified list to state store 
        
        // TODO: Challenge 6 - Call the SignalR output binding with the incoming order summary
    });
}

const sendOrderUpdate = async function(updateType, order) {
    return new Promise(async function(resolve, reject){
        // TODO: Challenge 6 - Send event to SignalR output binding
        // References: 
        //    https://github.com/dapr/docs/tree/master/howto/send-events-with-output-bindings
        //    https://github.com/dapr/docs/blob/master/reference/specs/bindings/signalr.md
    });
}

const getAllOrders = async function(storeId) {
    return new Promise(async function(resolve, reject){
    });
}

const deleteOrder = async function(storeId, orderId) {
    return new Promise(async function(resolve, reject){
    });
}

const getSignalrConnectionString = async function(){
    return new Promise(async function(resolve, reject){
        // TODO: Challenge 6 - Call the secrets component to retrieve the connection string
        // Reference: https://github.com/dapr/docs/blob/master/reference/api/secrets_api.md
    });
}

exports.makeOrder = makeOrder;
exports.getAllOrders = getAllOrders;
exports.deleteOrder = deleteOrder;
exports.getSignalrConnectionString = getSignalrConnectionString;