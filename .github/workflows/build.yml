name: Build EXE

on:
  push:
    branches: [ "main" ]

jobs:
  build:
    runs-on: windows-latest

    steps:
    - uses: actions/checkout@v3

    - uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 6.0.x

    - run: dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

    - uses: actions/upload-artifact@v4
      with:
        name: nso-tool
        path: bin/Release/net6.0-windows/win-x64/publish/
