# Facebook Export Viewer

[![CI](https://github.com/marcin-przywoski/Facebook-Export-Viewer/actions/workflows/CI.yml/badge.svg)](https://github.com/marcin-przywoski/Facebook-Export-Viewer/actions/workflows/CI.yml)
[![Release](https://img.shields.io/github/release/marcin-przywoski/Facebook-Export-Viewer.svg)](https://github.com/marcin-przywoski/Facebook-Export-Viewer/releases)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/marcin-przywoski/Facebook-Export-Viewer)
[![Downloads](https://img.shields.io/github/downloads/marcin-przywoski/Facebook-Export-Viewer/total)](https://github.com/marcin-przywoski/Facebook-Export-Viewer/releases)

This application is used to embed dates (Creation Time, Last Access Time, Last Write Time) back into the media contained in the Facebook Export as by default these are stripped. Currently it works for both HTML and JSON exports and selected media formats (GIF / JPG / MP4 / PNG).
I've created it as there was no tool available for easy use that allowed to instantly embed dates from Messenger HTML files back into saved media and copy them to another location to keep original files in place.  

Down the line I'll change this project into full-feature blown export viewer

## Installation

Download the repository and compile it by yourself in VS Code or VS or download the compiled package from the Packages section
I've added pre-compiled self contained packages if you don't have .NET core runtime installed

## Requirements

- .NET 3.1 runtime if you want to compile it for yourself

## Road map

- Make program compatible with different versions of exports including older ones (facebook changes some things over time).

- Improve UI and clarity of the program

- Add more customization options

- Add an option to choose only selected media types

- Add an option to choose only selected conversations (both via a direct select of desired folder as well as after inital scanning of export folder)

- Add ability to embed date into EXIF meta-data

- Add ability to change filenames to date of being sent for ease of search

- Add an option to put everything in one folder instead of creating replication of original folder structure

- Add ability to overwrite existing files in source directory

- Make program working on MacOS and Linux

## Acknowledgments

- [AngleSharp](https://github.com/AngleSharp/AngleSharp)
- [AngleSharp.XPath](https://github.com/AngleSharp/AngleSharp.XPath)
