# AsyncAwaitLite

This project **replicates and extends** the implementation presented in [Writing Async/Await from scratch with Stephen Toub](https://www.youtube.com/watch?v=R-z2Hv-7nxk&ab_channel=dotnet) for educational purposes.

## ✨ Overview

**AsyncAwaitLite** is a minimal implementation of the `async/await` model in .NET. It re-creates the key mechanics of asynchronous task handling from scratch and explores how continuations, scheduling, and execution contexts work internally.

### ✅ Key Features

- 🧠 Reimplements `Task`  behavior from scratch (exactly from the video)
- 🕹️ Reimplements `async`/`await` by leveraging iterators (exactly from the video)
- 🔬 Adds structure and interfaces to the project
- 🧰 Adds support for `Task<?>` **generic task-like types**.

## 🚀 Getting Started

1. **Clone the repository**
   ```bash
   git clone https://your-repo-url.git
   cd AsyncAwaitLite
   ```
2. **Run the project**
   ```bash
   dotnet build
   dotnet run --project .\TaskLite\TaskLite.csproj   
   ```
   
## 🌍 Additional Sources
-  Original source [code](https://gist.github.com/jamesmontemagno/12992547430b85723e997a312f13ddf7).
-  Supplementary [blog post](https://devblogs.microsoft.com/dotnet/how-async-await-really-works/) by Stephen Toub.