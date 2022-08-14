package store

import "context"

type StorageClient interface {
	// Get gets an item from the store.
	//
	// Callers should be prepared to handle:
	//
	// - ErrNotFound: for items that are missing
	//
	Get(ctx context.Context, id string) (interface{}, error)

	// Update writes an item to the store.
	//
	// Callers should be prepared to handle:
	//
	// - ErrNotFound: for items that are missing
	//
	// - ErrConcurrency: for cases where multiple writers attempt to write the same item
	Update(ctx context.Context, id string, value interface{}) error
}
