# probably-net
FFI &amp; C# pinvokes for the [`probably` Rust crate](http://crates.io/crates/probably). Very much a work-in-progress.

To build the DLL for Windows, add the following to `~/.cargo/config`:

```
[target.x86_64-pc-windows-gnu]
linker = "x86_64-w64-mingw32-gcc"
ar = "x86_64-w64-mingw32-gcc-ar"
```

Then add the 64-bit Windows target and build the project against it:

```bash
sudo apt install gcc-mingw-w64-x86-64
rustup target add x86_64-pc-windows-gnu
cargo build --target=x86_64-pc-windows-gnu
```

Still investigating building for 32-bit:

```bash
rustup target add i686-pc-windows-gnu
cargo build --target=i686-pc-windows-gnu
```

In the C# project's Properties -> Build, set `Platform target: x64`.
