# Summary

- How does event callback work? (Index -> TodoEditor)
- How does @bind work?
- How does child content work?
- IR is the best way to show these

## Code

### Index.razor

```razor

<h1>Todos</h1>

<ul>
@foreach (var item in items)
{
    <li>@item</li>
}
</ul>

<hr />

<TodoEditor OnSubmit="OnSubmit" />

@code {
    List<string> items = new List<string>();

    void OnSubmit(string title)
    {
        items.Add(title);
    }
}
```

### TodoEditor.razor

```razor
<EditForm Model="item">
    <InputText @bind-Value="@input.Title" />
    <button type="submit">Add</button>
</EditForm>

@code {
    TodoItem item = new TodoItem();

    [Parameter] public EventCallback<string> OnSubmit { get; set; }

    async Task OnSubmit()
    {
        await OnSubmit.InvokeAsync(item);
        item = new TodoItem();
    }

    private class TodoItem
    {
        [Required]
        public string Title { get; set; }
    }
}
```
