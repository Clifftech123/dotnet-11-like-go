# dotnet-11-like-go

This repo exists to show off **one** thing: **.NET 11 file-based apps**.

A real ASP.NET Core + EF Core + PostgreSQL web service — CRUD, DI, Minimal APIs,
OpenAPI, middleware, exception handling — with **no `.csproj`, no `.sln`, no `.slnx`,
no project metadata of any kind**. Just `.cs` files and a few directives at the top.

```
dotnet run src/main.cs       # runs the whole web service
dotnet publish src/main.cs   # one self-contained native binary
./bin/main                   # like shipping a Go binary, except it's C#
```

That's the feature. Everything else in this README is in service of explaining it.

---

## The new thing: directives instead of a project file

File-based apps ship as plain `.cs` files. Project metadata lives inline, at the top of the file, as `#:` directives the compiler understands **before** C# kicks in.

| Directive   | Replaces                       | Example |
|-------------|--------------------------------|---------|
| `#:sdk`     | `<Project Sdk="...">`          | `#:sdk Microsoft.NET.Sdk.Web` |
| `#:package` | `<PackageReference />`         | `#:package Npgsql.EntityFrameworkCore.PostgreSQL@10.0.1` |
| `#:property`| `<PropertyGroup><Foo>`         | `#:property TargetFramework=net11.0` |
| `#:include` | `<Compile Include="..." />`    | `#:include src/entity/Author.cs` |

All four are in this repo. Two of them — `#:include` and the transitive-directive flag that makes it useful — are still **experimental in .NET 11 preview 3** and have to be opted into with properties.

### `packages.cs` — the "go.mod"

```csharp
#:sdk Microsoft.NET.Sdk.Web

#:package Npgsql.EntityFrameworkCore.PostgreSQL@10.0.1
#:package Microsoft.AspNetCore.OpenApi@10.0.0
```

`Microsoft.NET.Sdk.Web` is what unlocks `WebApplication.CreateBuilder` and the rest of ASP.NET Core. The `#:package` lines pin NuGet versions, same syntax the CLI understands for `dotnet package add`.

### `includes.cs` — the "compile list"

```csharp
#:include src/util/StringExtensions.cs
#:include src/config/Config.cs
#:include src/config/JsonContext.cs
#:include src/entity/Author.cs
...
#:include src/middleware/logger.cs
```

`#:include` pulls another `.cs` file into the same compilation. Think `#include` in C, or `package main` auto-picking up every file in a Go folder — except explicit. No glob, no magic: the file compiles only if it's listed here.

Keeping the list flat and in one file means one place to look when something isn't being picked up.

### `src/main.cs` — the entry point

```csharp
#:property TargetFramework=net11.0
#:property LangVersion=preview
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true

#:include ../packages.cs
#:include ../includes.cs
```

Four property lines do the work of an entire `.csproj`:

- `TargetFramework=net11.0` — target the .NET 11 preview runtime.
- `LangVersion=preview` — enable preview C# language features (C# 14 `extension` blocks, etc.).
- `ExperimentalFileBasedProgramEnableIncludeDirective=true` — turn on `#:include` itself.
- `ExperimentalFileBasedProgramEnableTransitiveDirectives=true` — let an included file carry **its own** `#:sdk` / `#:package` / `#:property` directives back up into the compilation. Without this, `#:include packages.cs` would be a no-op for NuGet.

Then `main.cs` `#:include`s `packages.cs` (for the SDK + NuGet pins) and `includes.cs` (for every source file). Because of the transitive flag, all those directives behave as if they were written directly in `main.cs`.

**Result:** the only thing the compiler is ever told about this project is `src/main.cs`. Everything else — SDK, packages, sources, compile order — reaches it through directive transitivity.

---

## Run it like a script

```powershell
dotnet run src/main.cs
```

On Unix, file-based apps also support a shebang, so the file itself is the program:

```bash
chmod +x src/main.cs
./src/main.cs
```

No `dotnet build` step, no `obj/`, no `bin/` to pollute your tree while you're iterating. This is the `go run .` / `python main.py` ergonomic for C#.

---

## Publish as a single native binary

This is the part that makes file-based apps actually interesting in production.

```powershell
dotnet publish src/main.cs -o bin
```

File-based apps publish with **Native AOT by default**. One command, one self-contained native executable at `bin/main`, around **~33 MB** out of the box.

For a leaner binary, pass a few size-oriented flags:

```bash
dotnet publish src/main.cs -o bin \
  -p:OptimizationPreference=Size \
  -p:InvariantGlobalization=true \
  -p:DebuggerSupport=false \
  -p:EventSourceSupport=false \
  -p:HttpActivityPropagationSupport=false \
  -p:StripSymbols=true
```

Result: **~30 MB**. One file. Zero dependencies.

| Out of the box | Size-optimized |
|----------------|----------------|
| ~33 MB         | ~30 MB         |

That's a full ASP.NET Core web service with EF Core, the Npgsql driver, Minimal APIs, and OpenAPI, in a single binary.

### What you don't need on the target machine

- No .NET SDK
- No .NET runtime
- No ASP.NET Core shared framework
- No `dotnet` CLI
- No container, no `glibc` shim, no sidecar

```bash
scp bin/main user@server:/opt/app/main
ssh user@server /opt/app/main
```

Exactly like shipping a Go binary — except you're writing C#.

> **Caveat.** EF Core isn't fully AOT/trim-safe yet. The build warns on `IL2026` / `IL3050` around `DbContext` and `ApplyConfigurationsFromAssembly`. It runs fine; strict NativeAOT production use still needs care around model configuration and JSON (which is why this repo already uses a [source-gen JSON context](src/config/JsonContext.cs)).

---

## IDE caveat

`#:include` and the transitive-directive flag are **experimental**. The C# language server in VS Code / Rider currently doesn't fully understand them, so you'll see red squiggles like:

- `Unrecognized directive 'include'` in [includes.cs](includes.cs) / [packages.cs](packages.cs)
- `The type or namespace name 'EntityFrameworkCore' does not exist in the namespace 'Microsoft'` in files that reach EF Core through a transitive `#:package`

These are tooling false positives. `dotnet build src/main.cs` compiles cleanly — only nullable-reference and EF Core trim/AOT warnings remain. Don't "fix" them by adding a `.csproj`; that defeats the point of the repo.

---

## What the app itself does (short version)

Because you'll want to know: the app inside is a small CRUD service for **Authors**, **Blogs**, and **Categories**, backed by PostgreSQL via EF Core 10, exposed over Minimal APIs. Layered as `handler → service → repository → AppDbContext`, with a `RequestLogger` middleware and an `IExceptionHandler` that turns `ArgumentException` / `InvalidOperationException` into RFC 7807 `ProblemDetails` 400s. OpenAPI doc at `GET /openapi/v1.json`. Connection string is picked up from `ConnectionStrings:Postgres` in `appsettings.json` or from `POSTGRES_CONNECTION_STRING`.

But the point of this repo isn't the CRUD. The point is that all of the above fits — with zero ceremony — inside a pile of `.cs` files and a few `#:` directives.

---

## Files worth opening, in order

1. [packages.cs](packages.cs) — SDK + NuGet, four lines.
2. [includes.cs](includes.cs) — every source file in the build, one line each.
3. [src/main.cs](src/main.cs) — property directives, transitive includes, then a plain `class Program { static async Task Main }`.

After that, everything else is just ordinary ASP.NET Core.
