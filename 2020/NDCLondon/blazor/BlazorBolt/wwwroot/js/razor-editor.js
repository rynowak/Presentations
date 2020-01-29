function debounceElement(element, event, component, method) {
    element.addEventListener(event, debounce(async function() {
        await component.invokeMethodAsync(method, element.value);
    }, 1000, false));
}