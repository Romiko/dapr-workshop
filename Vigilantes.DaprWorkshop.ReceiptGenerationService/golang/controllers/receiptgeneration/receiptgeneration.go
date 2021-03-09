package receiptgeneration

import (

	"context"
	"encoding/json"
	"fmt"
	"receipt-generation-service/models"

	cloudevents "github.com/cloudevents/sdk-go/v2"
)

func init() {
	fmt.Println("Initialize - receiptgeneration package")
}

// GenerateReceipt function
func GenerateReceipt(ctx context.Context, event cloudevents.Event) {

	// deserialize incoming order summary
	var orderSummary models.OrderSummary
	json.Unmarshal(event.Data(), &orderSummary)

	fmt.Printf("\nwriting order summary (receipt) to storage: %v", orderSummary)

	// TODO: Challenge 5 - Store receipt via a Dapr Output Binding that can be used as a data sink
}

