var express = require('express');
var router = express.Router();
const receiptGenerationController = require('../controllers/ReceiptGenerationController');
const daprController = require('../controllers/DaprController');

// Root/Index Routes
router.get('/', function(req, res, next) {
  res.json({
    message: "You've reached the makeline service...now what?"
  })
});

// Order Routes
router.post('/orders', receiptGenerationController.generateReceipt);

// Dapr Routes
router.get('/dapr/subscribe', daprController.index);

module.exports = router;