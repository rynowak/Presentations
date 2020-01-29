# Summary

- Demonstrates how we tranform markup into blocks
- Show generated code and the difference between a markup block with dynamic content and not

## Code

### Index.razor

```razor
<h1>An important announcement about peanut butter</h1>
<p>All peanut butter will be chunky from <em>now on</em></p>

<p>All of <em>@DateTime.Now</em> the international peanut butter council has banned smooth peanut butter</p>
```
