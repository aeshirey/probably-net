#!/bin/bash
cargo build --target=x86_64-pc-windows-gnu --release
strip target/x86_64-pc-windows-gnu/release/probably_net.dll

cp target/x86_64-pc-windows-gnu/release/probably_net.dll Probably.NET/Probably.NET/

