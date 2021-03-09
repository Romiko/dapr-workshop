package loyalty

import (

	"context"
	"encoding/json"
	"fmt"
	"loyalty-service/models"

	cloudevents "github.com/cloudevents/sdk-go/v2"
)

func init() {
	fmt.Println("Initialize - loyalty package")
}

// UpdateLoyalty function
func UpdateLoyalty(ctx context.Context, event cloudevents.Event) {

	// deserialize incoming order summary
	var orderSummary models.OrderSummary
	json.Unmarshal(event.Data(), &orderSummary)

	fmt.Printf("\nreceived order: %v", string(event.Data()))

	// TODO: Challenge 3 - Retrieve and update customer loyalty points
}

