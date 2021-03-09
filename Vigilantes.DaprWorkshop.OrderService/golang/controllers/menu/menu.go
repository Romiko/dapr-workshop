package menu

import (
	"encoding/json"
	"fmt"
	"net/http"
	"order-service/models"
)

func init() {
	fmt.Println("Initialize - menu package")
}

// Get function
func Get(w http.ResponseWriter, r *http.Request) {

	// serialize data
	j, _ := json.Marshal(models.GetAll())

	// set response
	w.Header().Set("Content-Type", "application/json")
	w.Write(j)
}
