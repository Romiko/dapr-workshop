package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"
	"time"
)

// Constants
const (
	port    = "127.0.0.1:5500"
	storeID = "Redmond"
)

// OrderItemSummary struct
type orderItemSummary struct {
	MenuItemID   int    `json:"menuItemId"`
	MenuItemName string `json:"menuItemName"`
	Quantity     int    `json:"quantity"`
}

// OrderSummary struct
type orderSummary struct {
	OrderID      string             `json:"orderId"`
	OrderDate    time.Time          `json:"orderDate"`
	StoreID      string             `json:"storeId"`
	CustomerName string             `json:"customerName"`
	LoyaltyID    string             `json:"loyaltyId"`
	OrderItems   []orderItemSummary `json:"orderItems"`
	OrderTotal   float64            `json:"orderTotal"`
}

func main() {

	http.HandleFunc("/orders", func(w http.ResponseWriter, r *http.Request) {

		fmt.Println("schedule trigger invoked")

		// Get all orders
		orders, ordersErr := getAllOrders(storeID)
		if ordersErr != nil {
			fmt.Printf("error: %v\n", ordersErr.Error())
			return
		}

		// Complete the first order
		if len(orders) > 0 {
			fmt.Printf("The Virtual Barista is making an order for %v\n", orders[0].CustomerName)
			completeOrder(orders[0].StoreID, orders[0].OrderID)
			fmt.Printf("%v, your drinks are ready\n", orders[0].CustomerName)
		}
	})

	fmt.Println("\nVirtual Barista is listening on port", port)
	http.ListenAndServe(port, nil)
}

func completeOrder(storeID string, orderID string) {

	// TODO: Format url for completing an order
	url := ""

	req, err := http.NewRequest(http.MethodDelete, url, nil)
	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		fmt.Println("completeOrder failed:", err)
	}

	defer resp.Body.Close()
}

func getAllOrders(storeID string) ([]orderSummary, error) {

	var list []orderSummary

	// TODO: Format url for retrieving all orders
	url := ""

	resp, respErr := http.Get(url)

	if respErr == nil {

		// defer close
		defer resp.Body.Close()

		// read body bytes
		bodyBytes, bodyBytesErr := ioutil.ReadAll(resp.Body)
		if bodyBytesErr != nil {
			return list, bodyBytesErr
		}

		// evaluate result
		if resp.StatusCode >= 200 && resp.StatusCode <= 299 {
			err := json.Unmarshal(bodyBytes, &list)

			if err != nil {
				return list, err
			}
		} else {
			fmt.Printf("\nlist of orders summary not found")
			fmt.Printf("\nstatus code: %v", resp.StatusCode)
			fmt.Printf("\nstatus: %v", resp.Status)
			fmt.Printf("\nmessage: %v", string(bodyBytes))
		}
	} else {
		return list, respErr
	}

	return list, nil
}
