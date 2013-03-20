Coding Conventions
===

Casing
---

|Member            |Case    |
|:-----------------|:-------|
|Variables         |camel   |
|Parameters        |camel   |
|Fields            |camel   |
|Properties        |Pascal  |
|Methods           |Pascal  |
|Events            |Pascal  |
|Generic Parameters|TPascal |

No prefix, including underscore.

Constructors
---

There is only one constructor per class. It initializes read-only fields. Because parameter names
and field names share a casing convention, prefix the field with "this." to disambiguate.
