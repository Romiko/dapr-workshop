var express = require('express');
var router = express.Router();
const orderController = require('../controllers/OrderController');
const menuItemController = require('../controllers/MenuItemController');

// Root/Index Routes
router.get('/', function(req, res, next) {
  res.json({
    message: "You've reached the order service...now what?"
  })
});

// Order Routes
router.post('/order', orderController.newOrder);

// menuItem Routes
router.get('/menuitem', menuItemController.index);

module.exports = router;