# Image to MBIF

This is a little C# Tool to create MBIF files based on normal images.

## Usage 
Either drag a single image or a whole format into the executable and it will ask you to enter the position offset. 

(You can just enter 0 for the offsets if you dont want it to be centered)

After that it should create a file or a folder with the converted mbif files.



## Format

The format is a very simple bitmap-ish format.

It has a small header and then the raw image data as 32 bit ARGB.

The file is structured like this:
```
Byte 00-03 - Image Width      (int32)
Byte 04-07 - Image Height     (int32)
Byte 08-13 - Image X Offset   (int32)
Byte 12-15 - Image Y Offset   (int32)
Byte 16-23 - Image Data Size  (uint64)
Byte 24-.. - Image Data
```
