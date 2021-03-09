package order

import (

	"encoding/json"
	"fmt"
	"net/http"
	"order-service/models"
	"time"

	guid "github.com/google/uuid"
)

func init() {
	fmt.Println("Initialize - order package")
}

// NewOrder function
func NewOrder(w http.ResponseWriter, r *http.Request) {

	// get body
	var order models.CustomerOrder
	decodeErr := json.NewDecoder(r.Body).Decode(&order)

	if decodeErr != nil {
		fmt.Printf("\nerror: %v", decodeErr.Error())

		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	jOrder, jOrderErr := json.Marshal(order)

	if jOrderErr != nil {
		fmt.Printf("\nerror: %v", jOrderErr.Error())

		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	fmt.Printf("\ncustomer order received: %v", string(jOrder))

	// create an order summary that will be used as the
	// message body when published
	orderSummary := createOrderSummary(order)
	fmt.Printf("\norder summary: %v", orderSummary)

	// TODO: Challenge 2 - Publish an OrderSummary message via Dapr
	w.Write([]byte("Bummer. Business logic and pub/sub isn't implemented yet but, hey, at least your POST worked and you should see the order in the log! YOINK!"))
}

func createOrderSummary(order models.CustomerOrder) models.OrderSummary {

	// retrieve all the menu items
	menuItems := models.GetAll()

	// iterate through the list of ordered items to calculate
	// the total and compile a list of item summaries.
	orderTotal := 0.0
	var itemSummaries []models.OrderItemSummary

	for i := 0; i < len(order.OrderItems); i++ {

		for j := 0; j < len(menuItems); j++ {
			if order.OrderItems[i].MenuItemID == menuItems[j].MenuItemID {

				menuItem := menuItems[j]

				orderTotal += (menuItem.Price * float64(order.OrderItems[i].Quantity))

				orderItemSummary := models.OrderItemSummary{
					Quantity:     order.OrderItems[i].Quantity,
					MenuItemID:   order.OrderItems[i].MenuItemID,
					MenuItemName: menuItem.Name,
				}

				// append item to menu
				itemSummaries = append(itemSummaries, orderItemSummary)
			}
		}
	}

	id := guid.New()
	orderSummary := models.OrderSummary{
		OrderID:      id.String(),
		CustomerName: order.CustomerName,
		StoreID:      order.StoreID,
		LoyaltyID:    order.LoyaltyID,
		OrderDate:    time.Now(),
		OrderItems:   itemSummaries,
		OrderTotal:   orderTotal,
	}

	return orderSummary
}

