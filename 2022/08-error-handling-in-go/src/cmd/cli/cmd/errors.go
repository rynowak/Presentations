package cmd

type FriendlyError struct {
	Message string
	Err     error
}

func (fe *FriendlyError) Error() string {
	return fe.Message
}
