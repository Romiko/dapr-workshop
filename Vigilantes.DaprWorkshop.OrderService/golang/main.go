package main

import (
	"log"
	"net/http"
	"order-service/controllers/menu"
	"order-service/controllers/order"
	"strings"

	"github.com/gorilla/mux"
	"github.com/rs/cors"
)

const (
	serverPort = "127.0.0.1:5100"
)

func main() {

	// router implementation
	router := mux.NewRouter()

	// adding cors
	c := cors.AllowAll()
	handler := c.Handler(router)

	// router handlers
	router.HandleFunc("/menuitem", menu.Get).Methods("GET")
	router.HandleFunc("/order", order.NewOrder).Methods("POST")

	// listen and serve
	log.Printf("will listen on %v\n", serverPort)
	if err := http.ListenAndServe(serverPort, trailingSlashHandler(handler)); err != nil {
		log.Fatalf("unable to start http server, %s", err)
	}
}

func trailingSlashHandler(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		if r.URL.Path != "/" {
			r.URL.Path = strings.TrimSuffix(r.URL.Path, "/")
		}
		next.ServeHTTP(w, r)
	})
}
