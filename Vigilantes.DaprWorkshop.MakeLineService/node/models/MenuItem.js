const items = [
    {
        menuItemId: 1,
        name: "Americano",
        Description: "Americano",
        price: "2.99",
        imageUrl: "https://daprworkshop.blob.core.windows.net/images/americano.jpg"
    },
    {
        menuItemId: 2,
        name: "Caramel Macchiato",
        Description: "Caramel Macchiato",
        price: "4.99",
        imageUrl: "https://daprworkshop.blob.core.windows.net/images/macchiato.jpg"
    },
    {
        menuItemId: 3,
        name: "Latte",
        Description: "Latte",
        price: "3.99",
        imageUrl: "https://daprworkshop.blob.core.windows.net/images/latte.jpg"
    }
];

const findAll = async function() {
    return Promise.resolve(items);
};

const findById = async function(id) {
    const result = items.find(function(item){
        return item.menuItemId == id;
    });

    return Promise.resolve(result);
}

exports.findAll = findAll;
exports.findById = findById;