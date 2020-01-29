# Summary

- Demonstrates that parsing knows about components + component parameters
- Demonstrates that parsing knows about string parameters vs non-string

## Code

### Index.razor

```razor
<img alt="A sad crying clown" />

<Banner Message="A sad crying clown" Loudness="11" />
```

### Banner.razor

```razor
@code {
    [Parameter] public string Message { get; set; }
    [Parameter] public int Loudness { get; set; }
}
```
