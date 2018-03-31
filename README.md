# FreeTale.Pack
c# serialize library support read/write/convert to other format

# Current readable
* JSON
* INI

# Used

replace method for your format
```c#
using FreeTale.Pack;
using FreeTale.Pack.Json;
```

Read
```c#
string data = File.ReadAllText("data.json");
Unpacker unpacker = new Unpacker(data);
INode node = unpacker.JsonDocument();
```

Write
```c#
INode node = new Node();
node.Add("Hello","World");
string data = node.JsonPack(false);
```

# Extend

for support your format create new Extension method

```c#
public static class JsonExtension
{
	public static string JsonPack(this INode node){
		//pack here
	}
	public static INode JsonDocument(this Unpacker unpacker){
		//unpack here	
	}
}
```
extend from `INode` `IDocument` `Unpacker` choose best for your format


# Note

Code is not well test

Default value is `null` use carefully

TODO

support for XML
