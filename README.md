# Data Converter
Data Converter is a command line utility for processing and converting data from one format to another.

## Input data
The input data is read from a file whose path is supplied to the application using the `--inputPath` argument (short form: `-i`). Data Converter will attempt to determine the type of source data by reading the first line of the file. Currently recognised input formats are:
* BOM rainfall data, which is expected to be in CSV format and recognised by having a first line with these field headings - `Product code,Bureau of Meteorology station number,Year,Month,Day,Rainfall amount (millimetres),Period over which rainfall was measured (days),Quality`

## Data processing
As the data in the source file is read, it will be processed according to predefined rules. Currently supported rulesets are:
* BOM rainfall data summation, which is used when the input data is recognised as BOM rainfall data and aggregates rainfall readings by year and month.

## Output conversion
Once the data is processed, it will be converted before being written to output. The output format is defined using the argument `--outputFormat` (short form: `-of`). Currently supported output formats are:
* JSON, using the argument `-of json`, will convert the processed data to a minified JSON string. This is the default value if no output format argument is supplied.
* Formatted JSON, using the argument `-of json-pretty`, will convert the processed data to a formatted and indented JSON string.

## Output type
The converted data string will be written to an output type spcified by the `--outputType` argument (short form `-ot`). Currently supported output types are:
* File, using the argument `-ot file` , which will write the converted output to a file at the path specified by the `--output` argument (short form: `-o`). If no output argument is supplied, the output path will default to the same path and file name as the input path, with a file extension determined by the output format (e.g. `.json` for JSON output formats). This is the default value if no output type argument is supplied.

