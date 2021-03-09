var express = require('express');
var router = express.Router();
const loyaltyController = require('../controllers/LoyaltyController');
const daprController = require('../controllers/DaprController');

// Root/Index Routes
router.get('/', function(req, res, next) {
  res.json({
    message: "You've reached the Loyalty service...now what?"
  })
});

// Order Route

router.post('/orders', loyaltyController.orders);

// Dapr Routes
router.get('/dapr/subscribe', daprController.subscribe);

module.exports = router;