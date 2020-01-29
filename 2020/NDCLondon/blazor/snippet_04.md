# Summary

- Show how generics + type inference work

## Code

### Index.razor

```razor
<p>Using list with type inference</p>
<TemplatedList Items="items">
  <ItemContent Context="item">
  @item.Title
  </ItemContent>
</TemplatedList>


<p>Using list without type inference</p>
<TemplatedList T="TodoItem" Items="items">
  <ItemContent Context="item">
  @item.Title
  </ItemContent>
</TemplatedList>

@code {
    List<TodoItem> items = new List<TodoItem>();

    class TodoItem
    {
        public string Title { get; set; }
    }
}
```

### TemplatedList.razor

```razor
@typeparam T

@if (items.Count == 0)
{
    <p>Sorry, nothing here.</p>
}
else
{
    <ul>
    @foreach (var item in items)
    {
        <li>@ItemContent(item)</li>
    }
    </ul>
}

@code {
    [Parameter] IReadOnlyList<T> Items { get; set; }
    [Parameter] RenderFragment<T> ItemContent { get; set; }
}
```
