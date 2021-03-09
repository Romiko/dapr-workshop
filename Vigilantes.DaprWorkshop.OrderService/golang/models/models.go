package models

import (
	"fmt"
	"time"
)

func init() {
	fmt.Println("Initialize - models package")
}

// MenuItem struct
type MenuItem struct {
	MenuItemID  int     `json:"menuItemId"`
	Name        string  `json:"name"`
	Description string  `json:"description"`
	Price       float64 `json:"price"`
	ImageURL    string  `json:"imageUrl"`
}

// CustomerOrder struct
type CustomerOrder struct {
	StoreID      string              `json:"storeId"`
	CustomerName string              `json:"customerName"`
	LoyaltyID    string              `json:"loyaltyId"`
	OrderItems   []CustomerOrderItem `json:"orderItems"`
}

// CustomerOrderItem struct
type CustomerOrderItem struct {
	MenuItemID int `json:"menuItemId"`
	Quantity   int `json:"quantity"`
}

// OrderItemSummary struct
type OrderItemSummary struct {
	MenuItemID   int    `json:"menuItemId"`
	MenuItemName string `json:"menuItemName"`
	Quantity     int    `json:"quantity"`
}

// OrderSummary struct
type OrderSummary struct {
	OrderID      string             `json:"orderId"`
	OrderDate    time.Time          `json:"orderDate"`
	StoreID      string             `json:"storeId"`
	CustomerName string             `json:"customerName"`
	LoyaltyID    string             `json:"loyaltyId"`
	OrderItems   []OrderItemSummary `json:"orderItems"`
	OrderTotal   float64            `json:"orderTotal"`
}

// GetAll function
func GetAll() []MenuItem {

	var list []MenuItem

	item1 := MenuItem{
		MenuItemID:  1,
		Name:        "Americano",
		Description: "Americano",
		Price:       2.99,
		ImageURL:    "https://daprworkshop.blob.core.windows.net/images/americano.jpg",
	}

	// append item to menu
	list = append(list, item1)

	item2 := MenuItem{
		MenuItemID:  2,
		Name:        "Caramel Macchiato",
		Description: "Caramel Macchiato",
		Price:       4.99,
		ImageURL:    "https://daprworkshop.blob.core.windows.net/images/macchiato.jpg",
	}

	// append item to menu
	list = append(list, item2)

	item3 := MenuItem{
		MenuItemID:  3,
		Name:        "Latte",
		Description: "Latte",
		Price:       3.99,
		ImageURL:    "https://daprworkshop.blob.core.windows.net/images/latte.jpg",
	}

	// append item to menu
	list = append(list, item3)

	return list
}
