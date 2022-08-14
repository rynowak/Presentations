package cmd

import (
	"errors"
	"fmt"
	"os"

	"github.com/spf13/cobra"
)

var ReadFile = &cobra.Command{
	Use:  "read-file <filename>",
	Args: cobra.ExactArgs(1),
	RunE: func(cmd *cobra.Command, args []string) error {
		content, err := os.ReadFile(args[1])
		if errors.Is(err, os.ErrNotExist) {
			return &FriendlyError{Message: fmt.Sprintf("File %s does not exist", args[1])}
		} else if err != nil {
			return err
		}

		fmt.Printf("Got: %s\n", string(content))
		return nil
	},
}

func init() {
	RootCmd.AddCommand()
}
