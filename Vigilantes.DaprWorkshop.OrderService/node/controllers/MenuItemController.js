var MenuItem = require('../models/MenuItem');

exports.index = async function(req, res) {
    const menuItems = await MenuItem.findAll();
    console.log("Retrieved Menu Items");

    res.json(menuItems);
};