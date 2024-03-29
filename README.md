# probably-net
FFI &amp; C# pinvokes for the [`probably` Rust crate](http://crates.io/crates/probably). Very much a work-in-progress. Currently, this crate provides .NET access to:

* HyperLogLog
* Greenwald-Khanna quantile estimator

'Official' release forthcoming, but in the meantime, [`probably_net.dll`](https://github.com/aeshirey/probably-net/blob/main/Probably.NET/Probably.NET/probably_net.dll) is a stripped release build you can use with this project. To build this DLL yourself, add the following to `~/.cargo/config`:

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

## License

Licensed under either of

 * Apache License, Version 2.0, ([LICENSE-APACHE](LICENSE-APACHE) or http://www.apache.org/licenses/LICENSE-2.0)
 * MIT license ([LICENSE-MIT](LICENSE-MIT) or http://opensource.org/licenses/MIT)

at your option.

### Contribution

Unless you explicitly state otherwise, any contribution intentionally submitted
for inclusion in the work by you, as defined in the Apache-2.0 license, shall be dual licensed as above, without any
additional terms or conditions.
