# ef-with-cube

Provides WITH CUBE emulation for entity framework.

**Usage:**
```
var result = dbContext.LineItems
    .WithCube(
        l => l.CustomerName, 
        l => l.Region,
        g => g.Sum(l => l.Total)
    );
```

It can do up to 4 levels of grouping:
```
var result = dbContext.LineItems
    .WithCube(
        l => l.CustomerName, 
        l => l.Country,
        l => l.Region, 
        l => l.Suburb, 
        g => g.Sum(l => l.Total)
    );
```

If multiple aggregations are required:
```
var result = dbContext.LineItems
    .WithCube(
        l => l.CustomerName, 
        l => l.Region, 
        g => new {
                Sum = g.Sum(l => l.Total),
                Average = g.Average(l => l.Total)
        }
    );
```

The result of the operation provides a few ways to view the data:
```
 var withCubeRows = result.AllRows; // The same raw data that would be returned by a WITH CUBE statement.
 var grandTotal = result.GrandTotal; // The aggregates of all the data.
 var customerAggregates = result.Group1; // A dictionary of the aggregated data, keyed by the first group.
 var regionalAggregates = result.Group2; // Same again, but keyed by the second group.
```

The WithCube method can be done on any IQueryable, even after projections, joins & filtering. For example:
```
var query = from l in dbContext.LineItems
            join c in dbContext.Customers on c.CustomerId equals l.CustomerId
            where l.Quantity > 5
            select new
            {
                l.Date,
                c.CustomerName,
                c.Region,
                l.Total
            };
var result = query.WithCube(
                    l => l.CustomerName,
                    l => l.Region,
                    g => g.Sum(l => l.Total)
                );
```


Utilises [LINQKit](http://www.albahari.com/nutshell/linqkit.aspx) to help build the expression tree, ensuring only a single SQL query occurs.

Because this is only an emulation of WITH CUBE - by unioning all the possbile groupings, it is obviously not as performant as a native SQL WITH CUBE query.
