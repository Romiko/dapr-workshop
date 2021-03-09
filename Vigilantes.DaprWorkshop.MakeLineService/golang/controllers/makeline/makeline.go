package makeline

import (

	"bytes"
	"context"
	"encoding/json"
	"fmt"
	"make-line-service/models"
	"net/http"
	"strings"
	"time"

	cloudevents "github.com/cloudevents/sdk-go/v2"
	"github.com/dgrijalva/jwt-go"
)

func init() {
	fmt.Println("Initialize - makeline package")
}

const (
	signalRHubName = "orders"
)

// MakeOrder function
func MakeOrder(ctx context.Context, event cloudevents.Event) {

	// deserialize incoming order summary
	var orderSummary models.OrderSummary
	json.Unmarshal(event.Data(), &orderSummary)

	fmt.Printf("\nreceived order: %v", orderSummary)

	// TODO: Challenge 4 - Load current list of orders to be made from state store
	//                   - Add incoming order to the list
	//                   - Save modified list to state store

	// TODO: Challenge 6 - Call the SignalR output binding with the incoming order summary
}

// TODO: Challenge 4 - Implement a method to get all orders
//                   - Implement a method to delete an order
func Negotiate(w http.ResponseWriter, r *http.Request) {

	// this method will be called by the signalr client when connecting to
	// the azure signalr service. It will return an access token and the
	// endpoint details for the client to use when sending and receiving events.

	// get signalr connection string
	connectionString, connectionStringErr := getSignalRConnectionString()

	if connectionStringErr != nil {

		// set error response
		fmt.Printf("\nerror: %v", connectionStringErr.Error())
		return
	} else {

		fmt.Printf("\nconnection string: %v", connectionString)
	}

	// get map from connection string
	m := getMapFromConnectionString(connectionString)

	// get signalr hub url
	url := fmt.Sprintf("%v/client/?hub=%v", m["Endpoint"], signalRHubName)

	// generate token with claims
	token, tokenErr := generateJWT("vigilantes.ui", url, m["AccessKey"])

	if tokenErr != nil {
		fmt.Printf("\nerror: %v", tokenErr.Error())
	}

	// print token
	fmt.Printf("\ntoken: %v", token)

	// create returned object
	result := make(map[string]interface{})
	result["url"] = url
	result["accessToken"] = token

	// serialize data
	j, _ := json.Marshal(result)

	// set response
	w.Header().Set("Content-Type", "application/json")
	w.Write(j)
}

func generateJWT(nameID string, aud string, signature string) (string, error) {

	// create token with claims
	expirationTime := time.Now().Add(168 * time.Hour) // 7 days

	// create the JWT claims, which includes the username and expiry time
	claims := &models.Claims{
		NameID: nameID,
		StandardClaims: jwt.StandardClaims{
			// in JWT, the expiry time is expressed as unix milliseconds
			ExpiresAt: expirationTime.Unix(),
			IssuedAt:  expirationTime.Unix(),
			NotBefore: expirationTime.Unix(),
			Audience:  aud,
		},
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	token.Header["kid"] = signature

	// sign token
	var bsignature = []byte(signature)
	tokenString, tokenStringErr := token.SignedString(bsignature)

	if tokenStringErr != nil {
		return tokenString, tokenStringErr
	}

	return tokenString, nil
}

func getMapFromConnectionString(connectionString string) map[string]string {

	// create array
	arr := strings.Split(connectionString, ";")

	// create map of strings
	m := make(map[string]string)
	value := false
	var b bytes.Buffer
	prev := ""

	// convert to map
	for _, e := range arr {
		for i := 0; i < len(e); i++ {

			if value {
				b.WriteString(string(e[i]))

				if i == len(e)-1 {
					value = false
					m[prev] = b.String()
					b.Reset()
				}
			} else {
				if string(e[i]) != "=" {
					b.WriteString(string(e[i]))
				} else {
					value = true
					prev = b.String()
					b.Reset()
				}
			}
		}
	}

	// print endpoint, access and version from connection string
	fmt.Printf("\nendpoint: %v", m["Endpoint"])
	fmt.Printf("\naccesskey: %v", m["AccessKey"])
	fmt.Printf("\nversion: %v", m["Version"])

	return m
}

func sendOrderUpdate(eventName string, orderSummary models.OrderSummary) error {

	// TODO: Challenge 6 - Send event to SignalR output binding
	// References:
	//    https://github.com/dapr/docs/tree/master/howto/send-events-with-output-bindings
	//    https://github.com/dapr/docs/blob/master/reference/specs/bindings/signalr.md
	//    Option: use the OrderSummaryUpdateData object

	return nil
}

func getSignalRConnectionString() (string, error) {

	// TODO: Challenge 6 - Call the secrets component to retrieve the connection string
	// Reference: https://github.com/dapr/docs/blob/master/reference/api/secrets_api.md

	return "", nil
}
