package main

func scannability(filename string) ([]byte, error) {
	content, err := pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	content, err = pass(filename)
	if err != nil {
		return nil, err
	}

	return content, nil
}
