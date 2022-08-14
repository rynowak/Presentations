package main

import (
	"encoding/json"
	"os"
)

func causePanic(filename string) []byte {
	content, err := os.ReadFile(filename)
	if err != nil {
		panic(err)
	}

	return content
}

func badPanic(dataFromTheInternet []byte) interface{} {
	return readWidgetData(dataFromTheInternet)
}

func readWidgetData(widgetJSONText []byte) interface{} {
	value := map[string]interface{}{}
	err := json.Unmarshal(widgetJSONText, &value)
	if err != nil {
		panic(err)
	}

	return value
}

func goodPanic() []byte {
	jsondata := marshalOrPanic(map[string]interface{}{
		"message": "hi there",
	})
	return jsondata
}

func marshalOrPanic(in interface{}) []byte {
	b, err := json.Marshal(in)
	if err != nil {
		panic(err)
	}

	return b
}
