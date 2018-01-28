# JpegInfo

Reads header information from JPEG’s such as, width & height dimensions (resolution). It only takes details from the jpeg headers. Making it fast, with low memory usage, because it does no image processing. 

Supports .Net standard 1.0 and above. No unsafe context code. Under MIT licence. Unit tested.

As it only reads the jpeg headers at the start of the Jpeg file format, it does not read the full jpeg stream. This means you can drastically reduce IO/bandwidth usage.

I wrote this because I needed to find out the resolution of lots of images. The only way I could find in .Net Core was to use a fully-fledged image processor that took up too much CPU and memory for the level of task.
