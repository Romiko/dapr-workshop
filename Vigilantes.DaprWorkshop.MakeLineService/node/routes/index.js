var express = require('express');
var router = express.Router();
const makelineController = require('../controllers/MakelineController');
const daprController = require('../controllers/DaprController');

// Root/Index Routes
router.get('/', function(req, res, next) {
  res.json({
    message: "You've reached the makeline service...now what?"
  })
});

// Order Routes
router.post('/orders', makelineController.createOrder);
router.get('/orders/:storeId', makelineController.getStoreOrders);
router.delete('/orders/:storeId/:orderId', makelineController.deleteStoreOrder);

// Negotiate Routes
router.post('/negotiate', makelineController.negotiate)

// Dapr Routes
router.get('/dapr/subscribe', daprController.index);
router.get('/dapr/config', daprController.config);

module.exports = router;