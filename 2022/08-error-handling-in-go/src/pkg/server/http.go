package server

import (
	"errors"
	"fmt"
	"net/http"

	"github.com/rynowak/presentations/2022/08-error-handling-in-go/src/pkg/store"
)

func Start() {
	http.ListenAndServe("localhost:9000", http.HandlerFunc(ServeHTTP))
}

func ServeHTTP(w http.ResponseWriter, req *http.Request) {
	defer func() {
		if err := recover(); err != nil {
			// Recovered from panic:
			//
			// Log the error
			// return HTTP 500

			fmt.Printf("Recovered from panic %s\n", err)
			w.WriteHeader(500)
		}
	}()

	err := ServeHTTPForReal(w, req)
	if err != nil {
		// Unhandled error:
		//
		// Log the error
		// return HTTP 500
		fmt.Printf("Unexpected error %s\n", err)
		w.WriteHeader(500)
	}
}

func ServeHTTPForReal(w http.ResponseWriter, req *http.Request) error {
	var client store.StorageClient

	_, err := client.Get(req.Context(), req.URL.Path)
	if errors.Is(err, &store.ErrNotFound{}) {
		w.WriteHeader(404)
		return nil
	} else if err != nil {
		return err
	}

	return nil
}
