package cmd

import (
	"context"
	"errors"
	"fmt"
	"os"

	"github.com/spf13/cobra"
)

var RootCmd = &cobra.Command{
	Use:           "cli",
	Short:         "Fictional CLI",
	SilenceErrors: true,
	SilenceUsage:  true,
}

func Execute() {
	ctx := context.Background()
	err := RootCmd.ExecuteContext(ctx)

	// You could handle panics here too if you want

	// Last chance error handler
	if errors.Is(&FriendlyError{}, err) {
		fmt.Println(err.Error())
		os.Exit(1)
	} else if err != nil {
		fmt.Println("An unexpected error occurred - this may be a bug:", err)
		os.Exit(1)
	}
}
