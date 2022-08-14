package main

import (
	"errors"
	"fmt"
	"os"
)

func main() {
	if len(os.Args) == 1 {
		fmt.Println("usage: intro <filename>")
		return
	}

	err := readFiles(os.Args[1])
	if errors.Is(err, os.ErrNotExist) {
		fmt.Printf("File %q does not exist\n", os.Args[1])
		return
	} else if err != nil {
		fmt.Printf("Ran into an unexpected error: %s\n", err)
		return
	}
}

func readFiles(filename string) error {
	content, err := pass(filename)
	if err != nil {
		return err
	}

	fmt.Printf("Content from pass: %s\n", string(content))

	content, err = filter(filename)
	if err != nil {
		return err
	}

	fmt.Printf("Content from filter: %s\n", string(content))

	content, err = wrap(filename)
	if err != nil {
		return err
	}

	fmt.Printf("Content from wrap: %s\n", string(content))

	content = suppress(filename)
	fmt.Printf("Content from suppress: %s\n", string(content))

	return nil
}

func pass(filename string) ([]byte, error) {
	content, err := os.ReadFile(filename)
	if err != nil {
		return nil, err
	}

	return content, nil
}

func filter(filename string) ([]byte, error) {
	content, err := os.ReadFile(filename)
	if errors.Is(err, os.ErrNotExist) {
		return nil, nil
	} else if err != nil {
		return nil, err
	}

	return content, nil
}

func wrap(filename string) ([]byte, error) {
	content, err := os.ReadFile(filename)
	if err != nil {
		return nil, fmt.Errorf("there was an error reading %s: %w", filename, err)
	}

	return content, nil
}

func suppress(filename string) []byte {
	content, _ := os.ReadFile(filename)
	return content
}
