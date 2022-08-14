package main

import (
	"errors"
	"fmt"
	"io/ioutil"
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
	content, err := usingReadFile(filename)
	if err != nil {
		return err
	}

	fmt.Printf("Content from os.ReadFile: %s\n", string(content))

	content, err = usingReader(filename)
	if err != nil {
		return err
	}

	fmt.Printf("Content from os.OpenFile/ioutil.ReadAll: %s\n", string(content))

	return nil
}

func usingReadFile(filename string) ([]byte, error) {
	content, err := os.ReadFile(filename)
	if err != nil {
		return nil, err
	}

	return content, nil
}

func usingReader(filename string) ([]byte, error) {
	file, err := os.OpenFile(filename, os.O_RDONLY, 0)
	if err != nil {
		return nil, err
	}

	defer file.Close()

	content, err := ioutil.ReadAll(file)
	if err != nil {
		return nil, err
	}

	return content, nil
}
