package store

import "fmt"

var _ error = (*ErrInvalid)(nil)

type ErrInvalid struct {
	Message string
}

func (e *ErrInvalid) Error() string {
	return e.Message
}

type ErrNotFound struct {
	ID string
}

func (e *ErrNotFound) Error() string {
	return fmt.Sprintf("the resource %s was not found", e.ID)
}

var _ error = (*ErrNotFound)(nil)

type ErrConcurrency struct {
	ID  string
	Err error
}

func (e *ErrConcurrency) Error() string {
	return fmt.Sprintf("the operation on resource %s failed due to a concurrency conflict: %s", e.ID, e.Err)
}

func (e *ErrConcurrency) Unwrap() error {
	return e.Err
}
